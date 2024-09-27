/******************************************************************
 * File: HTTPReceiverSwagger.Controllers.cs
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using ParserLibrary;
using Serilog.Events;
using UniElLib;

namespace Plugins;


public class ExactPropertyNameGenerator : IPropertyNameGenerator
{
    public string Generate(JsonSchemaProperty property) => property.Name;
}

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
        
        Logger.log("HTTPReceiverSwagger: calling serverGen.GenerateFile() to generate controller code", LogEventLevel.Debug);
        var serverGen = new CSharpControllerGenerator(doc, new CSharpControllerGeneratorSettings());
        serverGen.Settings.CSharpGeneratorSettings.PropertyNameGenerator = new ExactPropertyNameGenerator();
        var serverCode = serverGen.GenerateFile();

        if (string.IsNullOrWhiteSpace(serverCode))
            throw new Exception("Failed to generate server code");

        if (!string.IsNullOrEmpty(serverCodePath))
        {
            var fullPath = Path.GetFullPath(serverCodePath);
            Logger.log("HTTPReceiverSwagger: Saving server code to " + fullPath);
            File.WriteAllText(fullPath, serverCode);
        }

        Logger.log("HTTPReceiverSwagger: Compile the controller code using Roslyn", LogEventLevel.Debug);
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

        Logger.log("HTTPReceiverSwagger: Adding assembly references", LogEventLevel.Debug);
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

        /*foreach (var rr in references) 
        {
            Logger.log("Path:"+rr.Location+" Name:"+rr.GetName().Name);
            if(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{rr.GetName().Name}.dll")))
 Logger.log(" Name:" + rr.GetName().Name+ "exists");
        }*/
        //references.Select(r => MetadataReference.CreateFromFile(r.Location))

        var compilation = CSharpCompilation.Create("ParserLibrary")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(references.Select(r => MetadataReference.CreateFromFile(r.Location)))
//            .AddReferences(references.Select(r => MetadataReference.CreateFromAssembly(r)))
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

        var asmPart = new AssemblyPart(assembly);
        Logger.log("HTTPReceiverSwagger: successfully generated an AssemblyPart for controller code", LogEventLevel.Debug);
        return asmPart;
    }
}