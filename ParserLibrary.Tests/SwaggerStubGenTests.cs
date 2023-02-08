using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using CSScriptLib;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.CSharp;
using NSwag.CodeGeneration.CSharp;

namespace ParserLibrary.Tests;

[TestFixture]
public class SwaggerStubGenTests
{
    public static object HandlerImplementation(params object[] parameters)
    {
        Console.WriteLine("HandlerImplementation:");

        foreach (var parameter in parameters)
            Console.WriteLine(parameter);

        return null;
    }

    public static object HandlerImplementation_2args(object a, object b)
    {
        Console.WriteLine("HandlerImplementation:");
        Console.WriteLine(a);
        Console.WriteLine(b);

        return null;
    }

    
    // Test the compilation of the generated server code
    [Test]
    public async Task TestSwaggerStubGenAsync()
    {
        var swaggerSpecPath = "TestData/swagger.json";
        var doc = await NSwag.OpenApiDocument.FromFileAsync(swaggerSpecPath);
        
        if (doc == null)
            throw new Exception("Failed to load swagger spec");
        
        var serverGen = new CSharpControllerGenerator(doc, new CSharpControllerGeneratorSettings());
        var serverCode = serverGen.GenerateFile();

        if (string.IsNullOrWhiteSpace(serverCode))
            throw new Exception("Failed to generate server code");

        // Compile the code using Roslyn
        var syntaxTree = CSharpSyntaxTree.ParseText(serverCode);

        var references = new []
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

        // Dump diagnostics to file if the compilation failed
        if (!result.Success)
            File.WriteAllLines("diagnostics.txt", result.Diagnostics.Select(d => d.ToString()));

        Assert.True(result.Success);
        
        // Load the assembly from the byte array
        var controllerAssembly = Assembly.Load(stream.ToArray());
        Assert.NotNull(controllerAssembly);

        var parentReceiver = new HTTPReceiverSwagger();
        parentReceiver.Init(null);
        parentReceiver.stringReceived = async (input, context) =>
        {
            Console.WriteLine("(test) stringReceived:");
            Console.WriteLine(input);
            await parentReceiver.sendResponse(DummyPetAnswer, context);
        };
        var requestHandler = new HTTPReceiverSwagger.RequestHandler(parentReceiver);

        var controllerImplAssembly = requestHandler.ImplementController(controllerAssembly);

        // Create the type
        var type = controllerImplAssembly.GetType("ControllerImpl");

        // Create an instance of the type
        var instance = Activator.CreateInstance(type, requestHandler);

        // // Invoke GetPetByIdAsync()
        var GetPetByIdAsync = type.GetMethod("GetPetByIdAsync");
        Assert.NotNull(GetPetByIdAsync);
        var res_GetPetByIdAsync = GetPetByIdAsync.Invoke(instance, new object[] { 123 });

        // Invoke updatePetWithForm. Corresponds to POSTing to /pet/{petId}
        var UpdatePetWithFormAsync = type.GetMethod("UpdatePetWithFormAsync");
        Assert.NotNull(UpdatePetWithFormAsync);
        var res_UpdatePetWithFormAsync = UpdatePetWithFormAsync.Invoke(instance, new object[] { 333, "name", "Available" });

        // The test passes if the code compiles and the method can be invoked
        Assert.Pass();
    }

    private static string DummyPetAnswer
    {
        get
        {
            var answer = @"{
                ""Id"": 1,
                ""Category"": {
                    ""id"": 2,
                    ""Name"": ""category1""
                },
                ""Name"": ""Doggie"",
                ""PhotoUrls"": [
                    ""url1""
                ],
                ""Tags"": [
                    {
                        ""Id"": 0,
                        ""Name"": ""tag1""
                    }
                ],
                ""Status"": ""Available""
            }";
            return answer;
        }
    }

    [Test]
    public void TestSimpleDynamicMethod()
    {
        // Dynamically generate a method that adds two integer arguments and returns the result.
        // The method is compiled using System.Reflection.Emit.

        // Create a dynamic assembly
        var assemblyName = new AssemblyName("TestAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("TestModule");

        // Create a new type
        var typeBuilder = moduleBuilder.DefineType("TestType", TypeAttributes.Public);

        // Create a method builder
        var methodBuilder = typeBuilder.DefineMethod("Add", MethodAttributes.Public | MethodAttributes.Static,
            typeof(int), new[] { typeof(int), typeof(int) });

        // Build the method
        var il = methodBuilder.GetILGenerator();

        // Load the first argument onto the stack
        il.Emit(OpCodes.Ldarg_0);

        // Load the second argument onto the stack
        il.Emit(OpCodes.Ldarg_1);

        // Add the two arguments
        il.Emit(OpCodes.Add);

        // Return the result
        il.Emit(OpCodes.Ret);

        // Create the type
        var type = typeBuilder.CreateType();

        // Invoke the method
        var method = type.GetMethod("Add");

        var result = method.Invoke(null, new object[] { 10, 20 });

        // Check the result, it should be 30
        Assert.AreEqual(30, result);
    }
    
    public static int AddImpl(int a, int b)
    {
        return a + b;
    }

    [Test]
    public void TestSimpleDynamicMethod_Add()
    {
        // Dynamically generate a method that calls HandlerImplementation and returns the result.
        // The method is compiled using System.Reflection.Emit.

        // Create a dynamic assembly
        var assemblyName = new AssemblyName("TestAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("TestModule");

        // Create a new type
        var typeBuilder = moduleBuilder.DefineType("TestType", TypeAttributes.Public);

        // Create a method builder
        var methodBuilder = typeBuilder.DefineMethod("Handle", MethodAttributes.Public | MethodAttributes.Static,
            typeof(int), new[] { typeof(int), typeof(int) });
        
        // Build the method
        var il = methodBuilder.GetILGenerator();
        
        // For each method parameter, emit a Ldarg instruction to load the parameter onto the stack
        for (var i = 0; i < 2; i++)
            il.Emit(OpCodes.Ldarg, i);
        
        // Call AddImpl
        var targetMethod = typeof(SwaggerStubGenTests).GetMethod(nameof(AddImpl));
        il.Emit(OpCodes.Call, targetMethod);
        
        // Return the result
        il.Emit(OpCodes.Ret);
        
        // Create the type
        var type = typeBuilder.CreateType();
        
        // Invoke the method
        var method = type.GetMethod("Handle");
        
        var result = method.Invoke(null, new object[] { 10, 20 });
 
        // The result should be 30
        Assert.AreEqual(30, result);
    }
    
    public static object AddImpl_Boxed(object a, object b)
    {
        return (int)a + (int)b;
    }

    [Test]
    public void TestSimpleDynamicMethod_Add_Boxed()
    {
        // Dynamically generate a method that calls HandlerImplementation and returns the result.
        // The method is compiled using System.Reflection.Emit.

        // Create a dynamic assembly
        var assemblyName = new AssemblyName("TestAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("TestModule");

        // Create a new type
        var typeBuilder = moduleBuilder.DefineType("TestType", TypeAttributes.Public);

        // Create a method builder
        var methodBuilder = typeBuilder.DefineMethod("Handle", MethodAttributes.Public | MethodAttributes.Static,
            typeof(int), new[] { typeof(object), typeof(object) });
        
        // Build the method
        var il = methodBuilder.GetILGenerator();
        
        // For each method parameter, emit a Ldarg instruction to load the parameter onto the stack
        for (var i = 0; i < 2; i++)
            il.Emit(OpCodes.Ldarg, i);
        
        // Call AddImpl_Boxed
        var targetMethod = typeof(SwaggerStubGenTests).GetMethod(nameof(AddImpl_Boxed));
        il.Emit(OpCodes.Call, targetMethod);
        
        // Unbox the return value into a integer
        il.Emit(OpCodes.Unbox_Any, typeof(int));
        
        // Return the result
        il.Emit(OpCodes.Ret);
        
        // Create the type
        var type = typeBuilder.CreateType();
        
        // Invoke the method
        var method = type.GetMethod("Handle");
        
        var result = method.Invoke(null, new object[] { 10, 20 });
 
        // The result should be 30
        Assert.AreEqual(30, result);
    }
    
    [Test]
    public void TestDynamicMethod_CallHandlerWithVarArgs()
    {
        // Dynamically generate a method that calls HandlerImplementation packing all arguments into params.
        // The method is compiled using System.Reflection.Emit.

        // Create a dynamic assembly
        var assemblyName = new AssemblyName("TestAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("TestModule");

        // Create a new type
        var typeBuilder = moduleBuilder.DefineType("TestType", TypeAttributes.Public);

        var argTypes = new[]
        {
            typeof(object),
            typeof(int),
            typeof(object)
        };

        // Create a method builder
        var methodBuilder = typeBuilder.DefineMethod("Handle", MethodAttributes.Public | MethodAttributes.Static,
            typeof(object), argTypes);
        
        // Build the method
        var il = methodBuilder.GetILGenerator();
        
        // Pack the arguments in an object array
        il.Emit(OpCodes.Ldc_I4, argTypes.Length);
        il.Emit(OpCodes.Newarr, typeof(object));
        for (var i = 0; i < argTypes.Length; i++)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldc_I4, i);
            il.Emit(OpCodes.Ldarg, i);
            il.Emit(OpCodes.Box, argTypes[i]);
            il.Emit(OpCodes.Stelem_Ref);
        }

        // Call the method
        var targetMethod = typeof(SwaggerStubGenTests).GetMethod(nameof(HandlerImplementation));
        il.Emit(OpCodes.Call, targetMethod);
        
        // Return the result
        il.Emit(OpCodes.Ret);
        
        // Create the type
        var type = typeBuilder.CreateType();
        
        // Invoke the method
        var method = type.GetMethod("Handle");
        
        var result = method.Invoke(null, new object[] { 10, 11, 12 });
        
        // The result should be null
        Assert.IsNull(result);
    }   
}