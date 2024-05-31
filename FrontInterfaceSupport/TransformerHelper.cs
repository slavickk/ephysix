/******************************************************************
 * File: TransformerHelper.cs
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

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MXGraphHelperLibrary;
using System.Text.Json;
using Npgsql;
using static FrontInterfaceSupport.DBTable;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace FrontInterfaceSupport
{
    public class TransformerHelper
    {
        public class ParamItem
        {
            public string Name;
            public string TypeParam;
        }
        public class AppData
        {
            public string Type {get; set; }
            public string Value { get; set; }
        }

        public static async Task<string> addTransformer(string jsonMXGrapth = "", string tableDefJson = "{\"Type\": \"CSScript\",\"Value\": \"int func(out int a, int b,double c,double e) {a=1;return 456; } \"      }\r\n", bool isNew = true)
        {
            if (!isNew)
                return jsonMXGrapth;
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();

            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            

           // AppData data = new AppData() { Type = "CScript", Value = "int func(out int a, int b,double c)\r\n{\r\n    a=1;\r\n    return 456; \r\n}" };
                
            addBox(retDoc, JsonSerializer.Deserialize<AppData>(tableDefJson));
            JsonSerializerOptions options = new JsonSerializerOptions() { IgnoreNullValues = true };

            return JsonSerializer.Serialize<MXGraphHelperLibrary.MXGraphDoc>(retDoc, options);


        }
        public static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> createTransformerBox(IConfiguration conf, MXGraphDoc retDoc, string transformerDefJson, MXGraphDoc.Box oldbox)
        {
           return addBox(retDoc, JsonSerializer.Deserialize<AppData>(transformerDefJson));

        }

        const int heigthHeaderBox = 64;
        const int heigthRow = 34;

        public static async  Task<string[]> getAvailTypes()
        {
            return new string[] { "CSScript", "SQLScript", "RegularExpression" };
        }

        public static MXGraphDoc.Box addBox(MXGraphHelperLibrary.MXGraphDoc retDoc,AppData dbTableConfig)
        {
            var num=retDoc.boxes.Count + 1;
            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            retBox.id = "transformer"+num;
            retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<AppData>(dbTableConfig)).RootElement;
            retBox.category = "data transformer";
            retBox.type = "transformer";
            string[] errs;
            var pars=CSScriptTransformerHelper.AnalyzeSourceCode(dbTableConfig.Value, out errs);
            if (pars == null) 
            {
                if (errs?.Length > 0)
                    throw new Exception(errs[0]);
                else
                    throw new Exception("Unhandled err");
            }
            var inputParams = pars.parameters.Where(i1=>i1.type == CSScriptTransformerHelper.FuncDef.TypePar.input).Select(ii => new ParamItem() { Name = ii.name, TypeParam = ii.retType }).ToArray();

            var outputParams = pars.parameters.Where(i1 => i1.type != CSScriptTransformerHelper.FuncDef.TypePar.input).Select(ii => new ParamItem() { Name = ii.name, TypeParam = ii.retType }).ToArray();
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();
            if (retDoc.boxes.Count == 0)
            {
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = 100, top = 100 };
            }
            else
            {
                int delta = 15;
                int left = retDoc.boxes.Min(ii => ii.header.position.left);
                int top = retDoc.boxes.Max(ii => ii.header.position.top + ii.header.size.height) + delta;
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = left, top = top };
            }
            retBox.header.caption = "C# function";
            retBox.header.description = dbTableConfig.Value;

            retBox.header.size = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Size() { width = 300, height = heigthHeaderBox + heigthRow };
            //            retBox.id = mxGraphID;
            retBox.type = "transformer";
            retBox.body = new MXGraphHelperLibrary.MXGraphDoc.Box.Body();
//            retBox.body.header = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Header>() { new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Name" }, new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Type" } };
            retBox.body.rows = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row>();
            {
                retBox.body.rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() });
                var col = new MXGraphDoc.Box.Body.Row.Column() { header = new List<MXGraphDoc.Box.Header>() { new MXGraphDoc.Box.Header() { value = "input" } } };

                col.rows = new List<MXGraphDoc.Box.Body.Row>();
                int i1 = 0;
                foreach (var inp in inputParams)
                {
     

                    col.rows.Add(new MXGraphDoc.Box.Body.Row()
                    {
                        columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() {
                        item= new MXGraphDoc.Box.Body.Row.Column.Item() {
                            /* style="position: relative;\n    box-sizing: border-box;\n    width: calc(100% - 20px);\n    height: 30px;\n    padding: 5px 30px;\n    margin: 0px auto;\n    border-radius: 8px;\n    border: 1px dashed var(--grey-10);\n    background: var(--grey-1);"
                             ,*/ box_id=retBox.id+"_"+"input"+(++i1)
                             , caption=inp.Name
                             , colspan=2
                             ,  valid_link_type= new List<int>() { 3}
                        }
                    } }
                    }
                        );
                }

                retBox.body.rows.Last().columns.Add(col);
            }
            {
               // retBox.body.rows.Add(new MXGraphDoc.Box.Body.Row() { columns = new List<MXGraphDoc.Box.Body.Row.Column>() });
                var col = new MXGraphDoc.Box.Body.Row.Column() { header = new List<MXGraphDoc.Box.Header>() { new MXGraphDoc.Box.Header() { value = "output" } } };

                col.rows = new List<MXGraphDoc.Box.Body.Row>();
                int i1 = 0;
                foreach (var out1 in outputParams)
                {
                    col.rows.Add(new MXGraphDoc.Box.Body.Row()
                    {
                        columns = new List<MXGraphDoc.Box.Body.Row.Column>() { new MXGraphDoc.Box.Body.Row.Column() {
                        item= new MXGraphDoc.Box.Body.Row.Column.Item() {
                            /* style="position: relative;\n    box-sizing: border-box;\n    width: calc(100% - 20px);\n    height: 30px;\n    padding: 5px 30px;\n    margin: 0px auto;\n    border-radius: 8px;\n    border: 1px dashed var(--grey-10);\n    background: var(--grey-1);"
                             ,*/ box_id=retBox.id+"_"+"output"+(++i1)
                             , caption=out1.Name
                             , colspan=2
                             ,  valid_link_type= new List<int>() { 3}
                        }
                    } }
                    }
                        );
                }

                retBox.body.rows.Last().columns.Add(col);
            }
            retBox.header.size.height += heigthRow*Math.Max(outputParams.Length,inputParams.Length);

            retDoc.boxes.Add(retBox);
            return retBox;

        }


    }

    public class  CSScriptTransformerHelper
    {
        public class FuncDef
        {
            public string Name { get; set; }
            public enum TypePar { input, output, reference, returnValue };
            public class ItemPar
            {
                public TypePar type;
                public string name;
                public string retType;
            }
            public List<ItemPar> parameters = new List<ItemPar>();
        }

        public static FuncDef AnalyzeSourceCode(string functionSourceCode, out string[] errors)
        {
            FuncDef retValue = new FuncDef();
            errors = null;
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(functionSourceCode);
            var root = syntaxTree.GetCompilationUnitRoot();
            var diagnostics = syntaxTree.GetDiagnostics();

            if (diagnostics.Any())
            {
                errors = diagnostics.Select(ii => ii.ToString()).ToArray();
                return null;
                /*foreach (var diagnostic in diagnostics)
                {
                    Console.WriteLine(diagnostic.ToString());
                }
                Console.WriteLine("Function definition has errors.");*/
            }
            else
            {

                var methodDeclaration = root.DescendantNodes().OfType<MethodDeclarationSyntax>().First();
                retValue.Name = methodDeclaration.Identifier.Text;


                retValue.parameters = methodDeclaration.ParameterList.Parameters.Select(ii => new FuncDef.ItemPar()
                {
                    name = ii.Identifier.Text,
                    retType = ((Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax)ii.Type).Keyword.Value.ToString()
                ,
                    type = (ii.Modifiers.Count == 0) ? (FuncDef.TypePar.input) : (ii.Modifiers.First().Text switch
                    {
                        "out" => FuncDef.TypePar.output,
                        "ref" => FuncDef.TypePar.reference,
                        _ => FuncDef.TypePar.input
                    })
                }).ToList();
                if (methodDeclaration.ReturnType.ToString() != "void")
                    retValue.parameters.Add(new FuncDef.ItemPar() { name = "output", retType = methodDeclaration.ReturnType.ToString(), type = FuncDef.TypePar.returnValue });
                /*                Console.WriteLine("Function Name: " + methodDeclaration.Identifier.Text);

                                foreach (var parameter in methodDeclaration.ParameterList.Parameters)
                                {
                                    Console.WriteLine("Parameter Type: " + parameter.Type.ToString());
                                    Console.WriteLine("Parameter Name: " + parameter.Identifier.Text);
                                    bool isOut = parameter.Modifiers.Count(ii => ii.Text == "out") > 0;
                                    bool isRef = parameter.Modifiers.Count(ii => ii.Text == "ref") > 0;
                                    var tt=((Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax)parameter.Type).Keyword.Value;
                                }


                                Console.WriteLine("Return Type: " + methodDeclaration.ReturnType.ToString());*/
                return retValue;
            }
        }
    }
}
