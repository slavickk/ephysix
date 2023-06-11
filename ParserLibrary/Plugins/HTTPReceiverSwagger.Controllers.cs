using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSwag.CodeGeneration.CSharp;
using ParserLibrary;

namespace Plugins;

public partial class HTTPReceiverSwagger
{
    /// <summary>
    /// Compiles the given Swagger specification into an assembly and returns it as an ApplicationPart
    /// </summary>
    /// <returns></returns>
    private async Task<AssemblyPart> CompileControllerAssemblyPartAsync()
    {
        var swaggerSpecFullPath = Path.GetFullPath(this.swaggerSpecPath);
        Logger.log("HTTPReceiverSwagger: Compiling controller from " + swaggerSpecFullPath);
        var doc = await NSwag.OpenApiDocument.FromFileAsync(swaggerSpecFullPath);

        if (doc == null)
            throw new Exception("Failed to load swagger spec");

        var serverGen = new CSharpControllerGenerator(doc, new CSharpControllerGeneratorSettings());
        var serverCode = serverGen.GenerateFile();

        if (string.IsNullOrWhiteSpace(serverCode))
            throw new Exception("Failed to generate server code");

        if (!string.IsNullOrEmpty(serverCodePath))
        {
            var fullPath = Path.GetFullPath(serverCodePath);
            Logger.log("HTTPReceiverSwagger: Saving server code to " + fullPath);
            File.WriteAllText(fullPath, serverCode);
        }

        // Compile the code using Roslyn
        var syntaxTree = CSharpSyntaxTree.ParseText(serverCode);

        // Add the ApiController attribute to the generated controller class,
        // so that the API controller logic described at
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-7.0#apicontroller-attribute,
        // kicks in.
        // We especially need model binding and automatic input validation.
        var root = await syntaxTree.GetRootAsync();
        var controllerClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var controllerClassWithAttribute = controllerClass.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Attribute(SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.QualifiedName(
                                SyntaxFactory.IdentifierName("Microsoft"),
                                SyntaxFactory.IdentifierName("AspNetCore")),
                            SyntaxFactory.IdentifierName("Mvc")),
                        SyntaxFactory.IdentifierName("ApiController"))))));

        var rootWithAttribute = root.ReplaceNode(controllerClass, controllerClassWithAttribute);
        syntaxTree = syntaxTree.WithRootAndOptions(rootWithAttribute, syntaxTree.Options);

        // If serverCodePath is given, write the updated syntax tree to serverCodePath-patched
        if (!string.IsNullOrEmpty(serverCodePath))
        {
            // Add "modified" to the file name but keep the extension
            var fullPath = Path.GetFullPath(serverCodePath);
            var extension = Path.GetExtension(fullPath);
            fullPath = fullPath.Substring(0, fullPath.Length - extension.Length) + "-patched" + extension;
            Logger.log("HTTPReceiverSwagger: Saving patched server code to " + fullPath);
            File.WriteAllText(fullPath, syntaxTree.ToString());
        }

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
}