using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.VisualStudio.Threading;
using System.Text.Json.Serialization;
using System.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NSwag.CodeGeneration.CSharp;
using Serilog.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ParserLibrary
{
    public class HTTPReceiverSwagger : Receiver
    {
        public int port = 8080;

        public string swaggerSpecPath = null;        

        IHostBuilder _hostBuilder;
        
        public string ResponseType = "application/json";
        public HTTPReceiverSwagger()
        {
            Logger.log("HTTPReceiverSwagger: Creating a host to listen on the port " + port);
            
            // Create a new host listening on the given port
            _hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel()
                        .ConfigureServices(services =>
                        {
                            services.AddSwaggerGen(c =>
                            {
                                c.SwaggerDoc("v1",
                                    new OpenApiInfo
                                    {
                                        Title = $"HTTPReceiverSwagger API based on {this.swaggerSpecPath}",
                                        Version = "v1"
                                    });
                            });
                        })
                        .Configure(app =>
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI(c =>
                            {
                                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Integration Utility v1");
                            });
                            app.UseRouting();
                            app.UseEndpoints(ep => ep.MapControllers());
                        });

                });
        }

        /// <summary>
        /// Compiles the given Swagger specification into an assembly and returns it as an ApplicationPart
        /// </summary>
        /// <returns></returns>
        private async Task<AssemblyPart> CompileControllerAssemblyPartAsync()
        {
            var doc = await NSwag.OpenApiDocument.FromFileAsync(this.swaggerSpecPath);
            
            if (doc == null)
                throw new Exception("Failed to load swagger spec");
            
            var serverGen = new CSharpControllerGenerator(doc, new CSharpControllerGeneratorSettings());
            var serverCode = serverGen.GenerateFile();

            if (string.IsNullOrWhiteSpace(serverCode))
                throw new Exception("Failed to generate server code");

            // Compile the code using Roslyn
            var syntaxTree = CSharpSyntaxTree.ParseText(serverCode);

            // Rewrite the above list with all variables inlined
            var references = new List<Assembly>
            {
                Assembly.Load("Newtonsoft.Json"),
                Assembly.Load("System.Runtime"),
                Assembly.Load("System.Private.CoreLib"),
                Assembly.Load("System.Runtime.Serialization.Primitives"),
                Assembly.Load("System.ComponentModel.Annotations"),
                Assembly.Load("netstandard"),
                
                // AspNetCore assemblies have to be listed explicitly
                Assembly.Load("Microsoft.AspNetCore.Mvc"),
                Assembly.Load("Microsoft.AspNetCore.Mvc.Core"),
            };

            var compilation = CSharpCompilation.Create("ParserLibrary")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references.Select(r => MetadataReference.CreateFromFile(r.Location)))
                .AddSyntaxTrees(syntaxTree);

            // Emit the code to a byte array
            using var stream = new MemoryStream();
            var result = compilation.Emit(stream);
            
            // Log diagnostics if the compilation failed
            if (!result.Success)
            {
                foreach (var diagnostic in result.Diagnostics)
                    Logger.log(diagnostic.ToString());
                
                throw new Exception("Failed to compile the generated server code");
            }
            
            // Load the assembly from the byte array
            var assembly = Assembly.Load(stream.ToArray());
            
            // Check and throw
            if (assembly == null)
                throw new Exception("Failed to load the server assembly");

            return new AssemblyPart(assembly);
        }

        /// <summary>
        /// Class to handle all incoming requests in a generic manner.
        /// The dynamically generated IController implementation forwards calls to ReceiveRequest() in this class.
        /// </summary>
        public class RequestHandler
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
            public async Task<object> ReceiveRequest(MethodInfo controllerAction, Type returnType, IDictionary<string, object> parameters)
            {
                Logger.log(
                    "HandlerImplementation() parameters: " + string.Join(", ", parameters.Select(p => p.Value)),
                    LogEventLevel.Debug);

                // Create a JSON object where names are parameter names and values are JSON representations of the parameters.
                // Use dictionary mapping.
                // This is the format that the AbstrParser expects.
                var json = JsonSerializer.Serialize(parameters);
                Logger.log("HandlerImplementation() parameters as JSON: " + json, LogEventLevel.Debug);

                var item = new SyncroItem();

                Interlocked.Increment(ref CountOpened);
                metricCountOpened.Increment();
                DateTime time1 = DateTime.Now;

                var statusCode = HttpStatusCode.OK;

                try
                {
                    // TODO: consider reworking the pipeline to use accept UniEl instead of string
                    _receiver.signal1(json, item).ContinueWith(antecedent =>
                    {
                        metricCountOpened.Decrement();
                        metricErrors.Increment();
                        statusCode = HttpStatusCode.NotFound;
                        item.semaphore.Set();
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
                catch (Exception e)
                {
                    // Log the exception
                    Logger.log(e.ToString(), LogEventLevel.Error);

                    metricCountOpened.Decrement();
                    metricErrors.Increment();
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                // Wait for the pipeline to signal the completion
                await item.semaphore.WaitAsync();
                if (statusCode != HttpStatusCode.OK)
                    throw new HttpResponseException(statusCode);

                Interlocked.Increment(ref item.unwait);

                if (_receiver.debugMode)
                {
                    Logger.log("Answer to client step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", _receiver.owner, item.answer);
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
                        
                        // TODO: If it is a dictionary, then parse the answer into a dictionary
                        if (taskType.IsGenericType && taskType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                        {
                            throw new NotImplementedException("IDictionary<,> is not supported yet");
                        }
                        
                        if (taskType == typeof(string))
                            return item.answer;

                        // The task type itself is non-parametric, so create an instance of it
                        if (taskType.IsGenericType == false)
                        {
                            try
                            {
                                var options = new JsonSerializerOptions();
                                options.Converters.Add(new JsonStringEnumConverter());
                                return JsonSerializer.Deserialize(item.answer, taskType, options);
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
                    il.Emit(OpCodes.Call, this.GetType().GetMethod(nameof(ReceiveRequest)));
                    
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
                                il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                                // Print the actual return type to the console.
                                // The actual value is at the top of the stack.
                                il.Emit(OpCodes.Dup);
                                il.Emit(OpCodes.Call, typeof(object).GetMethod("GetType"));
                                il.Emit(OpCodes.Call,
                                    typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }));

                                il.Emit(OpCodes.Ldstr, "Actual object that is wrapped within Task");
                                il.Emit(OpCodes.Call,
                                    typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                                // Print the type of the value wrapped by Task.
                                il.Emit(OpCodes.Dup);
                                var resultMethod = typeof(Task<>).MakeGenericType(methodInfo.ReturnType)
                                    .GetMethod("get_Result");
                                il.Emit(OpCodes.Call, resultMethod);
                                il.Emit(OpCodes.Call, typeof(object).GetMethod("GetType"));
                                il.Emit(OpCodes.Call,
                                    typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }));
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


        public override async Task startInternal()
        {
            // Finish the host configuration
            Logger.log("HTTPReceiverSwagger: finish building the host");
            
            var serverPart = await CompileControllerAssemblyPartAsync();

            // Dynamically implement the IController interface for Controller and add it to the service container
            var requestHandler = new RequestHandler(this);
            var controllerImplAssembly = requestHandler.ImplementController(serverPart.Assembly);

            var host = _hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://localhost:" + port);
                webBuilder.ConfigureServices(services =>
                {
                    services.AddControllers()
                        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                        .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(serverPart));
                    
                    // Register the IController implementation in the service container
                    var implementationClass = controllerImplAssembly.GetType("ControllerImpl");
                    Debug.Assert(implementationClass != null,
                        "ControllerImpl not found in the dynamically generated controller assembly. This is not supposed to happen.");

                    var controllerInterface= serverPart.Assembly.DefinedTypes.First(t => t.Name == "IController");
                    services.AddSingleton(controllerInterface, implementationClass);
                    
                    // Register the RequestHandler in the service container
                    services.AddSingleton(requestHandler);
                });
            }).Build();

            Logger.log("HTTPReceiverSwagger: Starting the host");
            await host.RunAsync();
        }
        //namespace Kestrel;
        public override async Task sendResponseInternal(string response, object context)
        {
            if (debugMode)
            {
                Logger.log("Send response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner, response);
                Logger.log("Response: {response}", Serilog.Events.LogEventLevel.Debug, response);
            }

            if (context is SyncroItem item)
            {
                item.answer = response;
                Interlocked.Increment(ref item.srabot);
                item.semaphore.Set();
            }
        }

        public async Task signal1(string body,SyncroItem semaphoreItem)
        {
            await signal(body, semaphoreItem);
           // semaphoreItem.semaphore.
            if (semaphoreItem.srabot==0)
            {
                if (debugMode)
                {
                    Logger.log("Answer without response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner, "-");

                }
                semaphoreItem.semaphore.Set();
            }
        }
        public class SyncroItem
        {
            public int srabot = 0;
            public int unwait = 0;
            public string answer="";
            public List<KestrelServer.Header> headers = new List<KestrelServer.Header>(); 
            public AsyncAutoResetEvent semaphore = new AsyncAutoResetEvent();
        }
        public class KestrelServer: Kestrel.KestrelServerImplement
        {
            HTTPReceiverSwagger owner;
            public KestrelServer(HTTPReceiverSwagger owner)
            {
                this.owner = owner;
            }


            public static long CountExecuted = 0;
            public static long CountOpened = 0;
            public static Metrics.MetricCount metricCountOpened = new Metrics.MetricCount("HTTPOpenConnectCount", "opened http connection at same time ");
            public static Metrics.MetricCount metricErrors = new Metrics.MetricCount("HTTPErrorCount", "Error http request ");
            public static Metrics.MetricCount metricCountExecuted = new Metrics.MetricCount("HTTPExecutedConnections", "All http executed connection's ");
            public static Metrics.MetricCount metricTimeExecuted = new Metrics.MetricCount("HTTPExecutedTime", "All http executed connection's time");
            public async Task<string> GetMetrics()
            {
                return Metrics.metric.getPrometeusMetric();
                //            return 1;
            }

            public class Header
            {
                public string Key;
                public string? Value;
            }
           // List<Header> headers = new List<Header>();
            public override async Task ReceiveRequest(HttpContext httpContext)
            {
                if (httpContext.Request.Path.Value.Contains("/metrics"))
                {
                    SetResponseType(httpContext, "text/plain");
                    await SetResponseContent(httpContext,await GetMetrics());
                    return;
                }

                    if (httpContext.Request.Path.Value.Contains("/swagger"))
                {
                    string json_body;
                    
                    using (StreamReader sr = new StreamReader(this.owner.swaggerSpecPath))
                    {
                        json_body = sr.ReadToEnd();
                    }

                    string content=GetSwaggerHtmlBody(json_body);
                    SetResponseType(httpContext, "text/html");
                    await SetResponseContent(httpContext, content);
                    return;

                }
                Interlocked.Increment(ref CountOpened);
                metricCountOpened.Increment();
                DateTime time1=DateTime.Now;
                bool iError = false;
                HTTPReceiverSwagger.SyncroItem item = new SyncroItem();
                using (var stream = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                {
                    string str = await stream.ReadToEndAsync();
                    if (owner.debugMode)
                        Logger.log("Stream reading:{o} ", Serilog.Events.LogEventLevel.Debug, "any", Thread.CurrentThread.ManagedThreadId);
                  //  headers.Clear();
                    foreach(var head in httpContext.Request.Headers)
                    {
                        item.headers.Add(new Header() { Key = head.Key, Value = head.Value });   
                      /*  var st = head.Key;
                        var st1 = head.Value;*/
                    }
                    
                    try
                    {
                        owner.signal1(str, item).ContinueWith(antecedent =>
                        {
                            iError = true;
                            metricCountOpened.Decrement();
                            metricErrors.Increment();
                            httpContext.Response.StatusCode = 404;
                            item.semaphore.Set();
                           // Console.WriteLine($"Error {antecedent}!");
                            //Console.WriteLine($"And how are you this fine {antecedent.Result}?");
                        },TaskContinuationOptions.OnlyOnFaulted);
                    }
                    catch(Exception e77)
                    {
                        metricCountOpened.Decrement();
                        metricErrors.Increment();
                        httpContext.Response.StatusCode = 404;
                        return;
                    }
                }

                await item.semaphore.WaitAsync();
                if (iError)
                    return;
                Interlocked.Increment(ref item.unwait);

                if (owner.debugMode)
                {
                    Logger.log("Answer to client step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner.owner, item.answer);

                }
                metricCountExecuted.Increment();
                Interlocked.Increment(ref CountExecuted);
                // await httpContext.Request.Body.
                SetResponseType(httpContext, owner.ResponseType);
                await SetResponseContent(httpContext, item.answer);
                metricCountOpened.Decrement();
                Interlocked.Decrement(ref CountOpened);
                metricTimeExecuted.Add(time1);
                //return base.ReceiveRequest(httpContext);
            }
        }
        static string template = "";


        public static string GetSwaggerHtmlBody(string json_body)
        {
            if(template =="")
            {
                using(StreamReader sr = new StreamReader("SwaggerTemplate.txt"))
                {
                    template = sr.ReadToEnd();
                }
            }
            return template.Replace("{0}", json_body);
        }
    }

}
