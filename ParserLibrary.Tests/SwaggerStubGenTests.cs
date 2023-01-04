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
using Microsoft.CSharp;
using NSwag.CodeGeneration.CSharp;

namespace ParserLibrary.Tests;

[TestFixture]
public class SwaggerStubGenTests
{
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
        var assembly = Assembly.Load(stream.ToArray());
        Assert.NotNull(assembly);
    }
}