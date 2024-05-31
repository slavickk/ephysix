/******************************************************************
 * File: PacketDescriber.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using CamundaInterfaces;
using DotLiquid;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_DB_Interface
{
    public static class PacketDescriber
    {
        // static string DocPath = @"C:\CamundaTopics\camundatopics\BPMN\ETL";
        public static void SavePythonDefinition(this GenerateStatement.ETL_Package package)
        {

            if (File.Exists(@"Shablons/ExternalTaskShablon.py"))
            {
                using (StreamReader sr = new StreamReader(@"Shablons/ExternalTaskShablon.py"))
                {
                    var TemplateBody = sr.ReadToEnd();
                    Template template = Template.Parse(TemplateBody); // Parses and compiles the template
                                                                      //        Template template = Template.Parse("hi {{name}}"); // Parses and compiles the template
                    var res = template.Render((DotLiquid.Hash.FromDictionary(new Dictionary<string, object>() { { "package", package } }))); // => "hi tobi"
                    using (StreamWriter sw = new StreamWriter($"{GenerateStatement.pathToSaveETL}{package.NamePacket}.py"))
                    {
                        sw.Write(res);
                    }

                }
            }
            if (File.Exists(@"Shablons/ExternalTaskShablon.c1"))
            {
                using (StreamReader sr = new StreamReader(@"Shablons/ExternalTaskShablon.c1"))
                {
                    var TemplateBody = sr.ReadToEnd();
                    Template template = Template.Parse(TemplateBody); // Parses and compiles the template
                                                                      //        Template template = Template.Parse("hi {{name}}"); // Parses and compiles the template
                    var res = template.Render((DotLiquid.Hash.FromDictionary(new Dictionary<string, object>() { { "package", package } }))); // => "hi tobi"
                    using (StreamWriter sw = new StreamWriter($"{GenerateStatement.pathToSaveETL}{package.NamePacket}.cs"))
                    {
                        sw.Write(res);
                    }

                }
            }
            return;
        }
        public static void SaveMDDefinition(this GenerateStatement.ETL_Package package)
        {
           
            if (File.Exists(@"Shablons/Shablon.txt"))
            {
                using (StreamReader sr = new StreamReader(@"Shablons/Shablon.txt"))
                {
                    var TemplateBody=sr.ReadToEnd();
                    Template template = Template.Parse(TemplateBody); // Parses and compiles the template
                                                                       //        Template template = Template.Parse("hi {{name}}"); // Parses and compiles the template
                    var res = template.Render((DotLiquid.Hash.FromDictionary(new Dictionary<string, object>() { { "package", package } }))); // => "hi tobi"
                    using (StreamWriter sw = new StreamWriter($"{GenerateStatement.pathToSaveETL}{package.NamePacket}.md"))
                    {
                        sw.Write(res);
                    }

                }
                return;
            }
            using (StreamWriter sw = new StreamWriter($"{GenerateStatement.pathToSaveETL}{package.NamePacket}.md"))
            {
                sw.Write(package.DescribePackage());
            }

            foreach(var task in package.usedExternalTasks.Where(ii=>!ii.noDescribe))
            {
                using (StreamWriter sw = new StreamWriter($"{GenerateStatement.pathToSaveExternalTask}{task.topic}.md"))
                {
                    sw.Write(task.DescribeExternalTask());
                }
            }


        }


        public static string DescribeExternalTask(this CamundaProcess.ExternalTask task)
        {
            string retValue = @"# Topic: " + task.topic + @"

## Author :" + task.author + @"

## Service location: " + task.service_location + @"

## Description of functionalities 

" + task.description + @"

";
            retValue += @"


## Input parameters description



| Name | Description | Example |  
| ------ |------ |------ |
";
            foreach (var varItem in task.parameters)
                retValue += $"|{varItem.Name}|{varItem.description}|{varItem.Value.Replace("|", @"\|").Replace("\r","").Replace("\n", "")}|\r\n";

            return retValue;
        }
    

    public static string DescribePackage(this GenerateStatement.ETL_Package package)
        {
            string retValue = @"# " + package.description + @"

## File " + package.NamePacket + @"

## Functionality description 

Make  data export form next sources:

";
            string lastSrc = "";
            bool lastPCI = false;
            foreach (var tableItem in package.allTables.OrderBy(ii => ii.src_name))
            {
                if (lastSrc != tableItem.src_name)
                {
                    lastPCI = tableItem.pci_dss_zone;
                    lastSrc = tableItem.src_name;
                    retValue += "\r\n\r\n"+ @"**" + tableItem.src_name + @"**

 _Objects:_
";
                }
                retValue += "\r\n - "+ tableItem.Name;
            }
            retValue += @"

to zone 

 **" + package.dest_name + @"**

_Objects:_
 - " + string.Join("\r\n - ", package.outputTable.Split(',')) + @"

For execute ETL process call SQL statement :
``` select * from fp.etlpackage_" + package.NamePacket.ToLower()+@"("+string.Join(',',package.variables.Select(ii=> "@" + ii.Name.ToLower()))+ ",[idexternalobj])```\r\n" + @"


## Input parameters description



| Name | Type | Description | Example |  
| ------ | ------ |------ |------ |
";
            foreach (var varItem in package.variables)
                retValue += $"|{varItem.Name}|{varItem.Type}|{varItem.Description}|{varItem.DefaultValue.Replace("|", @"\|")}|\r\n";
retValue+=@"

## Used external tasks

 ";
            foreach (var et in package.usedExternalTasks)
                retValue += $"[{et.topic}](../../ExternalTasks/{et.topic}.md)\r\n\r\n";
            return retValue;   
        }
    }
}
