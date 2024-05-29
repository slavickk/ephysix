using CamundaInterface;
using DotLiquid;
using Microsoft.Extensions.Configuration;
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
using static FrontInterfaceSupport.ServiceHelper;


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
                var line = sr.ReadToEnd();
                return await getItemFromContent(boxIdPrefix, line,null);

            }
            return null;
        }

        public class ReceiverSettings
        {
            public JsonElement channelSettings { get; set; }
            public string Type { get; set; }
            public JsonElement firstJson { get; set; }
            public JsonElement secondJson { get; set; }
        }

        public static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> createReceiverBox(IConfiguration conf, MXGraphDoc retDoc, string streamDefJson, MXGraphDoc.Box oldbox)
        {
            return await createReceiver(streamDefJson, retDoc);
        }
        public async static Task<string> getReceiverBox(string jsonMXGrapth, string serviceDefJson)
        {

            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();
            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            await createReceiver(serviceDefJson, retDoc);
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };

            var st = JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);
            return st;
            //            return (type as _ApiExecutor).getDefine().Select(ii=>ii.Name).ToList();
            //            return null;
        }

        private static async Task<MXGraphDoc.Box> createReceiver(string serviceDefJson, MXGraphDoc retDoc)
        {
            var receiverConfig = JsonSerializer.Deserialize<ReceiverSettings>(serviceDefJson);
            var sourceName = receiverConfig.Type;

            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            //   retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<DBTableConfig>(dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();

            int mx = retDoc.boxes.Count(ii => ii.type == "service") + 1;
            retBox.id = sourceName + "_" + mx;
            retBox.type = "service";
            retBox.header.caption = sourceName;
            retBox.header.description = "Receiver";
            retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<ReceiverSettings>(receiverConfig)).RootElement;

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
            retBox.type = "receiver";
            retBox.category = "source";

            retDoc.boxes.Add(retBox);
            MXGraphDoc.Box.Body body = await getItemFromContent(retBox.id, JsonSerializer.Serialize(receiverConfig.firstJson), (string?)JsonSerializer.Serialize(receiverConfig.secondJson));
            retBox.body = body;
            return retBox;
        }

        public static async Task<MXGraphDoc.Box.Body> getItemFromContent(string boxIdPrefix, string fileContentFirst, string? fileContentSecond)
        {
            return getBody(boxIdPrefix,true,fileContentFirst,fileContentSecond);
            List<AbstrParser.UniEl> listFirst = new List<AbstrParser.UniEl>();
            AbstrParser.UniEl rootFirst = new AbstrParser.UniEl();
            listFirst.Add(rootFirst);
            rootFirst.Name = "Root";
            if (!AbstrParser.getApropriateParser("aa", fileContentFirst, rootFirst, listFirst, false))
                return null;
            List<AbstrParser.UniEl> listSecond = null;
            if(!string.IsNullOrEmpty(fileContentSecond))
            {
                listSecond = new List<AbstrParser.UniEl>();
                AbstrParser.UniEl rootSecond = new AbstrParser.UniEl();
                listSecond.Add(rootFirst);
                rootSecond.Name = "Root";
                if (!AbstrParser.getApropriateParser("aa", fileContentSecond, rootSecond, listSecond, false))
                    return null;
            }
            return getBody(boxIdPrefix, true, listFirst, listFirst,true);
        }
        private static MXGraphDoc.Box.Body getBody(string boxIdPrefix, bool isOut, string FileContentFirst, string? FileContentSecond = null)
        {
            var body = new MXGraphDoc.Box.Body();
            body.rows = new List<MXGraphDoc.Box.Body.Row>();
            AddRow(boxIdPrefix, isOut,FileContentFirst, body);
            if (FileContentSecond != null)
                AddRow(boxIdPrefix, isOut, FileContentSecond, body);
            return body;
        }
        private static MXGraphDoc.Box.Body getBody(string boxIdPrefix,bool isOut,List<AbstrParser.UniEl> listFirst, List<AbstrParser.UniEl> listSecond=null,bool noAddBoxList=false)
        {
            var body = new MXGraphDoc.Box.Body();
            body.rows = new List<MXGraphDoc.Box.Body.Row>();
            AddRow(boxIdPrefix, isOut, listFirst, body,noAddBoxList);
            if(listSecond!= null)
                AddRow(boxIdPrefix, isOut, listSecond, body, noAddBoxList);
            return body;
        }
        private static void AddRow(string boxIdPrefix, bool isOut, string json, MXGraphDoc.Box.Body body)
        {
            body.rows.Add(new MXGraphDoc.Box.Body.Row());
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };
            body.rows.Last().columns = new List<MXGraphDoc.Box.Body.Row.Column>();
            var cols = body.rows.Last().columns;
            {
                //  dictOutput = links[0];
                cols.Add(new MXGraphDoc.Box.Body.Row.Column()
                {
                    json = JsonDocument.Parse(json).RootElement
                }
                    );
                var addCol = new MXGraphDoc.Box.Body.Row.Column() { style = "width:0; border: none;" };
                if (isOut)
                    cols.Add(addCol);
                else
                    cols.Insert(0, addCol);

            }
        }

        private static void AddRow(string boxIdPrefix, bool isOut, List<AbstrParser.UniEl> list, MXGraphDoc.Box.Body body,bool noAddBox_ID)
        {
            body.rows.Add(new MXGraphDoc.Box.Body.Row());
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };
            body.rows.Last().columns = new List<MXGraphDoc.Box.Body.Row.Column>();
            var cols = body.rows.Last().columns;
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
                    if(it.Name=="box_id")
                    {
                        int yy = 0;
                    }
                    if (it.childs?.Count > 0 || noAddBox_ID)
                    {
                        if (it.ancestor != null)
                            links[i] = new Dictionary<string, object>();
                        obj1 = links[i];
                    }
                    else
                        obj1 = new ServiceHelper.JsonItem() { box_id = boxIdPrefix + "_Output_" + it.path };
                    if (obj1 != null && parentDict != null && !parentDict.ContainsKey(it.Name))
                        parentDict.Add(it.Name, obj1);
                }
                //  dictOutput = links[0];
                cols.Add(new MXGraphDoc.Box.Body.Row.Column()
                {
                    json = JsonDocument.Parse(JsonSerializer.Serialize<Dictionary<string, object>>(dictOutput, options)).RootElement
                }
                    );
                var addCol = new MXGraphDoc.Box.Body.Row.Column() { style = "width:0; border: none;" };
                if (isOut)
                    cols.Add(addCol);
                else
                    cols.Insert(0, addCol);

            }
        }
    }
}
