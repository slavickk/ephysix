/******************************************************************
 * File: HTTPReceiverSwagger.RequestHandler.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ParserLibrary;
using Serilog.Events;

namespace Plugins;

public partial class HTTPReceiverSwagger
{
    /// <summary>
    /// Class to handle all incoming requests in a generic manner.
    /// The dynamically generated IController implementation forwards calls to ReceiveRequest() in this class.
    /// </summary>
    public partial class RequestHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="receiver">The receiver that will handle the request</param>
        /// <param name="debugOutput">Whether to generate code to output debug information</param>
        public RequestHandler(HTTPReceiverSwagger receiver, bool debugOutput = false)
        {
            this._receiver = receiver;
            this._debugOutput = debugOutput;
        }

        private HTTPReceiverSwagger _receiver;
        public static long CountExecuted = 0;  // Why do we have it when there is metricCountExecuted?
        public static long CountOpened = 0;  // Why do we have it when there is metricCountOpened?
        public static Metrics.MetricCount metricCountOpened = new("HTTPOpenConnectCount", "opened http connection at same time ");
        public static Metrics.MetricCount metricErrors = new("HTTPErrorCount", "Error http request ");
        public static Metrics.MetricCount metricCountExecuted = new("HTTPExecutedConnections", "All http executed connection's ");
        public static Metrics.MetricCount metricTimeExecuted = new("HTTPExecutedTime", "All http executed connection's time");
        private bool _debugOutput;

        /// <summary>
        /// This method is called by the dynamically generated IController implementation to handle the request.
        /// </summary>
        /// <param name="controllerAction">The controller method that was called.</param>
        /// <param name="parameters">Input parameters, each of type defined in the API specification.</param>
        /// <param name="returnType">The actual return type of the controller method.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<object> ReceiveRequestAsync(MethodInfo controllerAction, Type returnType, IDictionary<string, object> parameters)
        {
            Logger.log(
                "HandlerImplementation(), method:"+controllerAction.Name+", parameters: "  + string.Join(", ", parameters.Select(p => p.Value)),
                LogEventLevel.Debug);


            //Add method name(without async)
            parameters.Add("SwaggerMethod", controllerAction.Name.Substring(0, controllerAction.Name.Length - 5));

            // Create a JSON object where names are parameter names and values are JSON representations of the parameters.
            // Use dictionary mapping.
            // This is the format that the AbstrParser expects.
            var json = JsonSerializer.Serialize(parameters);
            Logger.log("HandlerImplementation() parameters as JSON: " + json, LogEventLevel.Debug);

            var item = new SyncroItem();

            Interlocked.Increment(ref CountOpened);
            metricCountOpened.Increment();
            DateTime time1 = DateTime.Now;

            var statusCode = StatusCodes.Status200OK;

            try
            {
                if (!(_receiver._host as Step.ReceiverHost).choosePath(item, _receiver.paths,parameters.Last().Value.ToString()))
                {
                    return Results.StatusCode(StatusCodes.Status404NotFound);

                }

                // TODO: consider reworking the pipeline to use accept UniEl instead of string
                _receiver.signal1(json, item).ContinueWith(antecedent =>
                {
                    metricCountOpened.Decrement();
                    metricErrors.Increment();
                    statusCode = StatusCodes.Status404NotFound;
                    item.semaphore.Set();
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (Exception e)
            {
                // Log the exception
                Logger.log(e.ToString(), LogEventLevel.Error);

                metricCountOpened.Decrement();
                metricErrors.Increment();
                return Results.NotFound();
            }

            // Wait for the pipeline to signal the completion
            await item.semaphore.WaitAsync();
            if (statusCode != StatusCodes.Status200OK)
                return Results.StatusCode(statusCode);

            Interlocked.Increment(ref item.unwait);

            if (_receiver._debugMode)
            {
                Logger.log("Answer to client step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", this._receiver._host.IDStep, item.answer);
            }

            metricCountExecuted.Increment();
            Interlocked.Increment(ref CountExecuted);
            metricCountOpened.Decrement();
            Interlocked.Decrement(ref CountOpened);
            metricTimeExecuted.Add(time1);

            // The pipeline returns the answer as a string in item.answer.
            // Convert it into the actual return type of the controller method.
            if (returnType.IsGenericType)
            {
                if (returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    // The return type is Task<T>
                    var taskType = returnType.GetGenericArguments()[0];
                        
                    if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(ICollection<>))
                    {
                        // Parse item.answer as a List of the given return type.
                        // Construct the resulting type dynamically as List<taskType>
                        var listType = typeof(List<>).MakeGenericType(taskType.GetGenericArguments()[0]);
                        var answer = JsonSerializer.Deserialize(item.answer, listType);
                        return answer;
                    }
                        
                    if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        // Parse item.answer as a Dictionary with string keys and taskType values.
                        // Construct the resulting type dynamically as Dictionary<string, taskType>
                        var dictType = typeof(Dictionary<,>).MakeGenericType(typeof(string), taskType.GetGenericArguments()[1]);
                        var answer = JsonSerializer.Deserialize(item.answer, dictType);
                        return answer;
                    }
                        
                    if (taskType == typeof(string))
                        return item.answer;

                    // The task type itself is non-parametric, so create an instance of it
                    if (taskType.IsGenericType == false)
                    {
                        try
                        {
                            // Deserialize the answer into the task type, using Newtonsoft.Json,
                            // so that data validation attributes are respected.
                            var settings = new Newtonsoft.Json.JsonSerializerSettings
                            {
                                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error
                            };
                            return Newtonsoft.Json.JsonConvert.DeserializeObject(item.answer, taskType, settings);

                        }
                        catch (Exception e)
                        {
                            // Throw a more informative exception
                            throw new Exception("Failed to deserialize the answer into " + taskType, e);
                        }
                    }
                        
                    throw new Exception("Unsupported return type: " + taskType);
                }
                else
                {
                    // If it is a non-Task generic type, throw an exception
                    throw new Exception("Unsupported return type: " + returnType);
                }
            }
            // Handle non-generic Task, which means the actual return type is void
            else if (returnType == typeof(Task))
            {
                return null;
            }
            else
            {
                // The return type is something else, throw an exception
                throw new Exception("Unsupported return type: " + returnType);
            }
        }

        public AssemblyBuilder ImplementController(Assembly assembly)
        {
            // Find the IController interface in the assembly
            var controllerInterface = assembly.DefinedTypes.FirstOrDefault(t => t.Name == "IController");
            if (controllerInterface == null)
                throw new Exception("IController interface not found in the assembly");

            // Dynamically compile an implementation of the IController interface using System.Reflection.Emit.
            // The implementation should forward the call to a single method.

            // Create a dynamic assembly
            var assemblyName = new AssemblyName("ControllerImplAssembly");
            var assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("ControllerImplModule");

            // Enumerate all methods in the IController interface
            var methodInfos = controllerInterface.GetMethods();
                
            // Define a class named ControllerImpl with constructor that takes a RequestHandler as a parameter and stores it in a private field.
            // The class implements all methods of the IController interface.
            // Every method implementation calls the RequestHandler's ReceiveRequest method with the parameters as a string->object dictionary.

            // Create a type that implements the IController interface
            var typeBuilder =
                moduleBuilder.DefineType("ControllerImpl", TypeAttributes.Public, null, new[] { controllerInterface });
            var _rhfld = typeBuilder.DefineField("_requestHandler", typeof(RequestHandler), FieldAttributes.Private);
                
            // Create a constructor that accepts a RequestHandler instance and stores it in a member field
            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(RequestHandler) });
            var cil = constructorBuilder.GetILGenerator();
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)); // Call the base class constructor first
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Ldarg_1);
            cil.Emit(OpCodes.Stfld, _rhfld);
            cil.Emit(OpCodes.Ret);

            // Implement each method of the IController interface.
            // Let's make all methods call HandlerImplementation.
            foreach (var methodInfo in methodInfos)
            {
                // Create a method builder
                var methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual, methodInfo.ReturnType,
                    methodInfo.GetParameters().Select(p => p.ParameterType).ToArray());

                // Build a method that calls HandlerImplementation
                var il = methodBuilder.GetILGenerator();
                    
                // We are going to call a method in the _requestHandler, so push it to the stack
                il.Emit(OpCodes.Ldarg_0);  // Arg 0 is "this" pointing to ControllerImpl
                il.Emit(OpCodes.Ldfld, _rhfld);  // Read the _requestHandler field and push it on the stack
                    
                // PARAMETER: Push the controller method MethodInfo. Use the single-argument form of GetMethodFromHandle().
                il.Emit(OpCodes.Ldtoken, methodInfo);
                il.Emit(OpCodes.Call, typeof(MethodBase).GetMethod("GetMethodFromHandle", new[] { typeof(RuntimeMethodHandle) }));

                // PARAMETER: Push the controller method return type
                il.Emit(OpCodes.Ldtoken, methodInfo.ReturnType);
                il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));

                // PARAMETER: Pack the arguments in a dictionary and push it as another argument
                il.Emit(OpCodes.Newobj, typeof(Dictionary<string, object>).GetConstructor(Type.EmptyTypes));
                for (var i = 0; i < methodInfo.GetParameters().Length; i++)
                {
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldstr, methodInfo.GetParameters()[i].Name);
                    il.Emit(OpCodes.Ldarg, i + 1); // Arg 0 is "this", so we need to skip it
                    il.Emit(OpCodes.Box, methodInfo.GetParameters()[i].ParameterType);
                    il.Emit(OpCodes.Callvirt, typeof(Dictionary<string, object>).GetMethod("Add"));
                }

                // Call the HandleRequest method
                il.Emit(OpCodes.Call, this.GetType().GetMethod(nameof(ReceiveRequestAsync)));
                    
                // RequestHandler returns Task<object> or just Task,
                // depending on whether the method being implemented has a return value.
                // If the method has a return value, create a more specific Task<T> object to wrap the return value.
                // If the method has no return value, return an empty Task.
                if (methodInfo.ReturnType.IsGenericType)
                {
                    if (methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        if (this._debugOutput)
                        {
                            il.Emit(OpCodes.Ldstr, "Actual type on the stack");
                            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new [] { typeof(string) }));

                            // Print the actual return type to the console.
                            // The actual value is at the top of the stack.
                            il.Emit(OpCodes.Dup);
                            il.Emit(OpCodes.Call, typeof(object).GetMethod("GetType"));
                            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new [] { typeof(object) }));

                            il.Emit(OpCodes.Ldstr, "Actual object that is wrapped within Task");
                            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new [] { typeof(string) }));

                            // Print the type of the value wrapped by Task.
                            il.Emit(OpCodes.Dup);
                            var resultMethod = typeof(Task<>).MakeGenericType(methodInfo.ReturnType)
                                .GetMethod("get_Result");
                            il.Emit(OpCodes.Call, resultMethod);
                            il.Emit(OpCodes.Call, typeof(object).GetMethod("GetType"));
                            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new [] { typeof(object) }));
                        }
                            
                        // Get the type of the value wrapped by the Task<T> returned by RequestHandler
                        var taskType = methodInfo.ReturnType.GetGenericArguments()[0];

                        // Create a new Task<T> that wraps the value returned by RequestHandler
                        var taskResult = typeof(Task<>).MakeGenericType(taskType).GetMethod("get_Result");
                        il.Emit(OpCodes.Call, taskResult);
                        il.Emit(OpCodes.Call, typeof(Task).GetMethod("FromResult").MakeGenericMethod(taskType));
                    }
                    else
                    {
                        throw new Exception("Unsupported return type: " + methodInfo.ReturnType.Name);
                    }
                }
                // However, if the method return type is void (i.e. just Task),
                // then create a completed task to be the actual return value.
                else
                {
                    // Pop the value first, it is a Task<object> containing null. We don't need it.
                    il.Emit(OpCodes.Pop);
                    // Return a completed task
                    il.Emit(OpCodes.Call, typeof(Task).GetProperty("CompletedTask").GetGetMethod());
                }

                il.Emit(OpCodes.Ret);

                // Mark the method as an override
                typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
            }

            typeBuilder.CreateType(); // Create the type so that it can be found using reflection later

            return assemblyBuilder;
        }
    }
}