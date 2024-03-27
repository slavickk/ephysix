using CamundaInterface;
using DotLiquid;
using MXGraphHelperLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UniElLib;
using static FrontInterfaceSupport.DBTable;


namespace FrontInterfaceSupport
{
    public class ReceiverHelper
    {
        public static async Task<string[]> getExamples(string arch_dir, string[] savedIID=null,string filterName="", string filterValue ="")
        {
            if(savedIID == null )
                savedIID= Directory.GetFiles(arch_dir, "*.*");
            if(!string.IsNullOrEmpty(filterName) )
                return await filterExamples(savedIID,filterName,filterValue);
            return savedIID;
        }

       async static Task<string[]> filterExamples(string[] savedIID, string filterName, string filterValue = "")
        {
            var res= await Task.WhenAll(
                                savedIID.Select(async(IID) =>
                                {
                                    using (StreamReader sr = new StreamReader(IID))
                                    {
                                        var line = sr.ReadToEnd();
                                        List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                                        AbstrParser.UniEl root = new AbstrParser.UniEl();
                                        list.Add(root);
                                        root.Name = "Root";
                                        if (AbstrParser.getApropriateParser("aa", line, root, list, false))
                                        {
                                            if (list.Count(ii => ii.Name == filterName && (string.IsNullOrEmpty(filterValue) || filterValue==ii.Value.ToString() ))>0)
                                                return IID;
//                                            return list.Where(ii=>ii.Name == filterName && (string.IsNullOrEmpty(filterValue) || filterValue.Equals(ii.Value)));
                                        }

                                    }
                                    return string.Empty;
                                }));
            return res.Where(b => !string.IsNullOrEmpty(b)).ToArray();
        }

        public static async Task<MXGraphDoc.Box.Body> getItem(string boxIdPrefix="aa",string IID= "C:\\D\\OUT_DummyProtocol1\\UniElLib.ContextItemStep_0Receiver_po2vgdbb.2dk")
        {
            using(StreamReader sr = new StreamReader(IID))
            {
                var line =sr.ReadToEnd();
                List<AbstrParser.UniEl> list= new List<AbstrParser.UniEl>();
                AbstrParser.UniEl root= new AbstrParser.UniEl();
                list.Add(root);
                root.Name = "Root";
                if(AbstrParser.getApropriateParser("aa", line, root, list, false))
                {
                    var body= getBody(boxIdPrefix, list[0], list);
/*                    var options = new JsonSerializerOptions() { IgnoreNullValues = true };
                    var ss =JsonSerializer.Serialize<MXGraphDoc.Box.Body>(body,options);*/
                    return body;
                }

            }
            return null;
        }

        private static MXGraphDoc.Box.Body getBody(string boxIdPrefix, AbstrParser.UniEl root,List<AbstrParser.UniEl> list)
        {
            var body = new MXGraphDoc.Box.Body();
            body.rows = new List<MXGraphDoc.Box.Body.Row>();
            body.rows.Add(new MXGraphDoc.Box.Body.Row());
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };
            body.rows[0].columns = new List<MXGraphDoc.Box.Body.Row.Column>();
            var cols = body.rows[0].columns;
            {
                var rootOutput = new Dictionary<string, object>();

                Dictionary<string, object> dictOutput = new Dictionary<string, object>();
                var localOutput = new Dictionary<string, object>();
/*                dictOutput.Add(new Dictionary<string, object>("Root", localOutput));*/
                Dictionary<string, object>[] links = new Dictionary<string, object>[list.Count];
                //links[0] = dictOutput;

                for (int i = 0; i < list.Count; i++)
                {
                    var it = list[i];
                    var parentDict = localOutput;
                    if (it.ancestor != null)
                        parentDict = links[list.IndexOf(it.ancestor)];
                    else
                    {
                        localOutput = new Dictionary<string, object>();
                        links[i] = localOutput;
                        dictOutput.Add(it.Name, localOutput);
                        parentDict = null;
                    }
                    object obj1 = null;

                    if (it.childs?.Count > 0)
                    {
                        if(it.ancestor != null)
                            links[i] = new Dictionary<string, object>();
                        obj1 = links[i];
                    }
                    else
                        obj1 = new ServiceHelper.JsonItem() { box_id = boxIdPrefix + "_Output_" + it.path };
                    if(parentDict != null && !parentDict.ContainsKey(it.Name))
                    parentDict.Add(it.Name, obj1);
                }
              //  dictOutput = links[0];
                cols.Add(new MXGraphDoc.Box.Body.Row.Column()
                {
                    json = JsonDocument.Parse(JsonSerializer.Serialize<Dictionary<string, object>>(dictOutput, options)).RootElement
                }
                    );
            }
            return body;
        }

    }
}
