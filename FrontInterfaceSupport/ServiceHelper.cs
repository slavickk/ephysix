/******************************************************************
 * File: ServiceHelper.cs
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

using CamundaInterface;
using MXGraphHelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FrontInterfaceSupport.DBTable;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;

namespace FrontInterfaceSupport
{
    public class ServiceHelper
    {
        public class JsonItem
        {
            public Dictionary<string, string>? tooltip_info { get; set; }
            public MXGraphDoc.Box.Body.Row.Column.Item.BoxLink link { get; set; }
            public string? box_id { get; set; }
        }
        static bool predicate(Type t)
        {
            return t.IsAssignableTo(typeof(_ApiExecutor)) && !t.IsAbstract ;
        }
        static List<Type> getAllRegTypes(params Assembly[] addAssemblies)
        {
            return addAssemblies.SelectMany(a => a.GetTypes().Where(predicate))
                .Concat(Assembly.GetExecutingAssembly().GetTypes().Where(predicate))
                .ToList();
        }
        public static string apyType = "AnyHelper";
        public static Assembly[] addAsseblies = new Assembly[] { Assembly.GetAssembly(typeof(_ApiExecutor)) ,GetAssemblyNameContainingType( apyType) };
        public static Assembly GetAssemblyNameContainingType(String typeName)
        {
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = currentassembly.GetType(typeName, false, true);
                if (t != null) { return currentassembly; }
            }

            return null;
        }
        public static async  Task<List<string>> getMethodsForSource(string source)
        {
            var type = getAllRegTypes(addAsseblies).First(ii=>ii.Name== source);

            var obj=Activator.CreateInstance(type);
            return (obj as _ApiExecutor).getDefine().Select(ii => ii.Name).ToList();
            //            return (type as _ApiExecutor).getDefine().Select(ii=>ii.Name).ToList();
            //            return null;
        }

   
    public class ServiceConfig
    {
        public string? Type { get; set; }
        public string? Function { get; set; }

    }
    public async static Task<string> getServiceBox(string jsonMXGrapth , string serviceDefJson)
        {

            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();
            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            createServiceBox(serviceDefJson, retDoc); 
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };

            var st = JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);
            return st;
            //            return (type as _ApiExecutor).getDefine().Select(ii=>ii.Name).ToList();
            //            return null;
        }
        public static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> createServiceBox(IConfiguration conf, MXGraphDoc retDoc, string streamDefJson, MXGraphDoc.Box oldbox)
        {
            return createServiceBox(streamDefJson, retDoc);
        }

        private static MXGraphDoc.Box createServiceBox(string serviceDefJson, MXGraphDoc retDoc)
        {
            ServiceConfig dbTableConfig = JsonSerializer.Deserialize<ServiceConfig>(serviceDefJson);
            var sourceName = dbTableConfig.Type;
            var methodName = dbTableConfig.Function;
            var type = getAllRegTypes(addAsseblies).First(ii => ii.Name == sourceName);

            string boxIdPrefix = sourceName + "_";

            var obj = Activator.CreateInstance(type);
            var item = (obj as _ApiExecutor).getDefine().First(ii => ii.Name == methodName);
            //retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();

            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            //   retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<DBTableConfig>(dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();

            int mx = retDoc.boxes.Count(ii => ii.type == "service") + 1;
            retBox.id = sourceName + "_" + methodName + "_" + mx;

            retBox.header.caption = sourceName + " " + methodName;
            retBox.header.description = "API call of method " + methodName;
            retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<ServiceConfig>(dbTableConfig)).RootElement;

            if (retDoc.boxes.Count == 0)
            {
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = 100, top = 100 };
            }
            else
            {
                int delta = 15;
                int left = retDoc.boxes.Max(ii => ii.header.position.left + ii.header.size.width) + delta;
                int top = retDoc.boxes.Min(ii => ii.header.position.top);
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = left, top = top };
            }

            retBox.header.size = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Size() { width = 300, height = 300 };
            //            retBox.id = mxGraphID;
            retBox.type = "service";
            retBox.category = "data transformer";

            retDoc.boxes.Add(retBox);
            MXGraphDoc.Box.Body body = getBody(boxIdPrefix, item);
            retBox.body = body;
            return retBox;
        }

        public static async  Task<string> getMethodsDetail(string sourceName ,string methodName)
        {
            var type = getAllRegTypes(addAsseblies).First(ii => ii.Name == sourceName);
            string boxIdPrefix = sourceName + "_";

            var obj = Activator.CreateInstance(type);
            var item = (obj as _ApiExecutor).getDefine().First(ii => ii.Name == methodName);
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();
            retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            

            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            retBox.category = "receiving";
            retBox.type ="service";

            //   retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<DBTableConfig>(dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();
            retBox.id = sourceName + "_" + methodName;
            retBox.header.caption= sourceName + " " + methodName;
            retBox.header.description= "API call of method " + methodName;
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

            retBox.header.size = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Size() { width = 300, height = 300 };
            //            retBox.id = mxGraphID;
            retBox.type = "service";

            retDoc.boxes.Add(retBox);
            MXGraphDoc.Box.Body body=getBody(boxIdPrefix, item);
            retBox.body = body;
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };

            var st = JsonSerializer.Serialize<MXGraphDoc.Box.Body>(body, options);
            return st;
            //            return (type as _ApiExecutor).getDefine().Select(ii=>ii.Name).ToList();
            //            return null;
        }

        private static MXGraphDoc.Box.Body getBody(string boxIdPrefix, _ApiExecutor.ItemCommand item)
        {
            var body = new MXGraphDoc.Box.Body();
            body.rows = new List<MXGraphDoc.Box.Body.Row>();
            body.rows.Add(new MXGraphDoc.Box.Body.Row());
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };
            body.rows[0].columns = new List<MXGraphDoc.Box.Body.Row.Column>();
            var cols = body.rows[0].columns;
            {
                var rootInput = new Dictionary<string, object>();

                Dictionary<string, object> dictInput = new Dictionary<string, object>();
                var localInput = new Dictionary<string, object>();
                dictInput.Add("Input parameters", localInput);
                foreach (var it in item.parameters)
                {
                    localInput.Add(it.name, new JsonItem() { box_id = "Input_" + it.name });
                }
                cols.Add(new MXGraphDoc.Box.Body.Row.Column()
                {
                    json = JsonDocument.Parse(JsonSerializer.Serialize<Dictionary<string, object>>(dictInput, options)).RootElement
                }
                    );
            }

            {
                var rootOutput = new Dictionary<string, object>();

                Dictionary<string, object> dictOutput = new Dictionary<string, object>();
                var localOutput = new Dictionary<string, object>();
                dictOutput.Add("Output parameters", localOutput);
                Dictionary<string, object>[] links = new Dictionary<string, object>[item.outputItems.Count];

                for (int i = 0; i < item.outputItems.Count; i++)
                {
                    var it = item.outputItems[i];
                    var parentDict = localOutput;
                    if (it.parent != null)
                        parentDict = links[item.outputItems.IndexOf(it.parent)];
                    object obj1 = null;

                    if (it.children?.Count > 0)
                    {
                        links[i] = new Dictionary<string, object>();
                        obj1 = links[i];
                    }
                    else
                        obj1 = new JsonItem() { box_id = "Output_" + it.path };
                    parentDict.Add(it.Name, obj1);
                }
                cols.Add(new MXGraphDoc.Box.Body.Row.Column()
                {
                    json = JsonDocument.Parse(JsonSerializer.Serialize<Dictionary<string, object>>(dictOutput, options)).RootElement
                }
                    );
            }
            return body;
        }

        public static async  Task<List<string>> getSources()
        {
            var types = getAllRegTypes(addAsseblies);
            return types.Select(ii => ii.Name).ToList();
//            return null;
        }

    }
}
