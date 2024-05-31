/******************************************************************
 * File: DiagramExecutor.cs
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
using DotLiquid;
using MXGraphHelperLibrary;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using ParserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static FrontInterfaceSupport.ImplementApiExecutor;

namespace FrontInterfaceSupport
{
    public class DiagramExecutor
    {
        static Dictionary<string,Type> whatTypesHandle= new Dictionary<string, Type>() { 
            { "service", typeof(ImplementApiExecutor) }
            ,{ "constant", typeof(ImplementConstantExecutor)}
        };

        public static void createActors(MXGraphDoc doc)
        {
            OfflineActor.ancestors = createAncestors(doc);
            foreach (var box in doc.boxes)
            {
                Type actorType;
                if (whatTypesHandle.TryGetValue(box.type, out actorType))
                {
                    var actor=(OfflineActor)Activator.CreateInstance(actorType,new object[] { box});
                }
            }
            ActorContext cont= new ActorContext();

        }

        private static Dictionary<string, List<string>> createAncestors(MXGraphDoc doc)
        {
            Dictionary<string, List<string>> ancestors = new Dictionary<string, List<string>>();
            foreach (var box in doc.boxes)
            {
                var links = box.enumLinks();
                foreach (var link in links)
                {
                    foreach (var link2 in link.boxLinks)
                    {
                        var pos = link2.link.box_id.IndexOf(":");
                        var prevId = link2.link.box_id.Substring(0, pos);
                        List<string> links1;
                        if (!ancestors.TryGetValue(prevId, out links1))
                        {
                            links1 = new List<string>();
                            ancestors.Add(prevId, links1);
                        }

                        if (!links1.Contains(box.id))
                        {
                            links1.Add(box.id);
                            ancestors[prevId] = links1;
                        }


                    }
                }

            }

            return ancestors;
        }

 
    }

    public interface ActorCreator
    {
        OfflineActor createActor(MXGraphDoc doc);
    }

    public class GetterFunc : OfflineActor.LinkGetter
    {
        Func<OfflineActor.ContextItem, string[]> func;
        public GetterFunc(Func< OfflineActor.ContextItem, string[]> func) 
        {
            this.func = func;
        }

        public IEnumerable<string> getValue(OfflineActor.ContextItem context)
        {
            return func(context);
        }
    }


    public class ImplementConstantExecutor : OfflineActor
    {

        public class AppData
        {
            public string Value { get; set; }
            public string Type { get; set; }
        }

        /*        public class Root
                {
                    public AppData AppData { get; set; }
                }*/

        string[] Value;

        public ImplementConstantExecutor(MXGraphDoc.Box box) : base(box.id)
        {
            isPresetted = true;
            var appData=JsonSerializer.Deserialize<AppData>((JsonElement)box.AppData);
            Value = new string[] { appData.Value };
            var links = box.enumLinks();
            foreach (var outItem in links)
            {
                outboundLink.Add(outItem.box_id, (outItem.boxLinks.Select(ii => new OutLink(ii.link.box_id)).ToArray(), new GetterFunc((a) => { return Value; })));
            }

        }
        public override ContextItem createContext(ActorContext owner)
        {
            return new ContextItem( this,owner);
//            throw new NotImplementedException();
        }

        public override async Task<bool> execute(ContextItem context)
        {
            return true;
        }
    }

    public class ImplementApiExecutor: OfflineActor
    {
        public class OutputApi : OfflineActor.LinkGetter
        {
                _ApiFilter filter;
                string path;
            public OutputApi(string path,_ApiFilter filter)
            {
                    this.path=path;
                    this.filter=filter; 
            }
            public IEnumerable<string> getValue()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<string> getValue(ContextItem context)
            {
                throw new NotImplementedException();
            }
        }
        public class ParamApi : OfflineActor.LinkSetter
        {
            ExecContextItem.ItemParam par;
            public ParamApi(ExecContextItem.ItemParam par)
            {
                this.par = par;
            }
          
            public void setValue(IEnumerable<string> values, ContextItem context)
            {
                par.Value = values.First();

//                throw new NotImplementedException();
            }
        }
        static bool predicate(Type t)
        {
            return t.IsAssignableTo(typeof(_ApiExecutor)) && !t.IsAbstract;
        }

        static List<Type> getAllRegTypes(params Assembly[] addAssemblies)
        {
            return addAssemblies.SelectMany(a => a.GetTypes().Where(predicate))
                .Concat(Assembly.GetExecutingAssembly().GetTypes().Where(predicate))
                .ToList();
        }
        static Assembly[] addAsseblies = new Assembly[] { Assembly.GetAssembly(typeof(_ApiExecutor)), Assembly.GetAssembly(typeof(DummySystem2Helper)) };

 
        _ApiExecutor.ItemCommand commandItem;
        string GetNameOutput(string name)
        {
            return name.Substring(7);
        }

        public class ContextItemApi : ContextItem
        {
            public ExecContextItem[] execContextItems;

            public ContextItemApi(OfflineActor owner, ActorContext context, ExecContextItem[] execContextItems) : base(owner, context)
            {
                this.execContextItems = execContextItems;   
            }
        }
        public override ContextItem createContext(ActorContext owner)
        {
            return new ContextItemApi(this, owner,execContextItems.Select(ii=>new ExecContextItem() {  Command=ii.Command, Params=ii.Params.Select(i1=>new ExecContextItem.ItemParam(i1.Key,i1.Value)).ToList() }).ToArray());
            //            throw new NotImplementedException();
        }
        _ApiFilter result;

        ExecContextItem[] execContextItems;
        public override async Task<bool> execute(ContextItem context)
        {
            try
            {
                await executor.beginSessionAsync();
                var filter = await executor.ExecAsync(execContextItems);
                await executor.endSessionAsync();
                if (filter == null)
                {
                    /*   var lastError = executor.getError();
                       await ReportError(ConnString, guid, processId, lastError.content, lastError.error, variables);
                       retValue.Errors++;*/

                }
                else
                {
                }
            }
            catch(Exception ex) 
            { 
            }
            return true;

        }

        const string prefix = "DummySystem2XmlTransport__";
        _ApiExecutor executor;


///        List<ExecContextItem.ItemParam> execContextItemParams = new List<ExecContextItem.ItemParam>();
        public ImplementApiExecutor(MXGraphDoc.Box box):base(box.id)
        {
            ServiceHelper.ServiceConfig conf = JsonSerializer.Deserialize<ServiceHelper.ServiceConfig>((JsonElement)box.AppData);
            var type = getAllRegTypes(addAsseblies).First(ii => ii.Name == conf.Type);
            executor =(_ApiExecutor) Activator.CreateInstance(type);
            
            execContextItems = new ExecContextItem[] { executor.getDefine().First(ii => ii.Name == conf.Function).toExecItem() };

            var doc = box.body.rows[0].columns[1].json.Value;

            var links = box.enumLinks();
            foreach (var par in execContextItems[0].Params)
            {
                inboundLink.Add(prefix+"Input_"+par.Key, new ParamApi(execContextItems[0].Params.First(ii=>ii.Key==par.Key)));

            }
            foreach(var outItem in links)
            {
                    outboundLink.Add(outItem.box_id, (outItem.boxLinks.Select(ii => new OutLink(ii.link.box_id)).ToArray(), new OutputApi(outItem.box_id.Substring(prefix.Length+7), this.result)));
            }

        }


    }
}
