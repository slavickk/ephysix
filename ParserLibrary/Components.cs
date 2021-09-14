using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using CSScriptLib;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading;
using System.Net.Http;

namespace ParserLibrary
{
    public abstract class Receiver
    {
        [YamlIgnore]
        public bool debugMode = false;

        public ReplaySaver saver = null;
        [YamlIgnore]

        public Step owner
        {
            set
            {
                debugMode = value.debugMode;
            }
        }
        public delegate void StringReceived(string input);
        public delegate void BytesReceived(byte[] input);
        public event StringReceived stringReceived;
        public void signal(string input)
        {
            if (saver != null)
                saver.save(input);
            stringReceived?.Invoke(input);
        }
        public abstract void start();
    }
    public abstract class Sender
    {
        [YamlIgnore]
        public Step owner;
        public virtual void send(AbstrParser.UniEl root)
        {
            if (owner.debugMode)
                Console.WriteLine("send result");

        }
    }
    public abstract class Filter
    {
        public abstract IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list);
    }

    public class ScriptCompaper : ComparerV
    {
        public ScriptCompaper()
        {

        }
        MethodDelegate checker = null;
        string body = @"using System;
using ParserLibrary;
bool Filter(AbstrParser.UniEl el)
{                                                
    return true;
}";
        public string ScriptBody
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
                checker = CSScript.RoslynEvaluator
                  .CreateDelegate(body);

            }
        }

        /* = @"using System;
using ParserLibrary;
bool Filter(AbstrParser.UniEl el)
{                                                
return true;
}";*/
        public bool Compare(AbstrParser.UniEl el)
        {
            DateTime time1 = DateTime.Now;
            var ret = (bool)checker(el);
            AbstrParser.regEvent("CS", time1);
            return ret;
        }
    }

    public class ConditionFilter : Filter
    {
        public string conditionPath { get; set; }
        //                                  Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
        //Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
        //       [JsonInclude]
        public ComparerV conditionCalcer { get; set; } = new ScriptCompaper();

        public override IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list)
        {
            return list.Where(ii => ii.path == conditionPath && conditionCalcer.Compare(ii));
        }
    }

    public abstract class OutputValue
    {
        public string outputPath;
        public enum TypeCopy { Value, Structure };
        public TypeCopy typeCopy = TypeCopy.Value;
        public abstract string getValue(AbstrParser.UniEl rootEl);
        public virtual void addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot)
        {
            // Пока поддерживается только линейная структура записи
            if (typeCopy == TypeCopy.Value)
            {
                AbstrParser.UniEl el = new AbstrParser.UniEl(outputRoot);
                el.Name = outputPath;
                el.Value = getValue(inputRoot);
            }
            else
                outputRoot.childs.Add(inputRoot.copy(outputRoot));
        }
    }

    public class ConstantValue : OutputValue
    {
        public override string ToString()
        {
            return outputPath + ";" + Value;
        }
        public string Value { get; set; }
        public override string getValue(AbstrParser.UniEl rootEl)
        {
            return Value;
        }
    }

    public class ExtractFromInputValue : OutputValue
    {
        public override string ToString()
        {
            return outputPath + "; from " + conditionPath;
        }
        public string conditionPath { get; set; }
        public ComparerV conditionCalcer { get; set; }
        public string valuePath { get; set; } = "";
        public override string getValue(AbstrParser.UniEl rootEl)
        {

            var pathOwn = rootEl.path;
            var patts1 = AbstrParser.PathBuilder(new string[] { pathOwn, conditionPath });


            var patts = AbstrParser.PathBuilder(new string[] { conditionPath, valuePath });

            var rootEl1 = getLocalRoot(patts1, 0, rootEl);


            foreach (var item in rootEl1.getAllDescentants().Where(ii => ii.path == conditionPath && ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
            {
                var item1 = item;
                if (valuePath != "")
                {
                    item1 = getLocalRoot(patts, 0, item1);
                    foreach (var item2 in item1.getAllDescentants().Where(ii => ii.path == valuePath))
                        return item2.Value?.ToString();
                }
                else
                    return item.Value.ToString();

            }
            return "";
        }
        private AbstrParser.UniEl getLocalRoot(string[] patts, int indexF, AbstrParser.UniEl item1)
        {
            var nodes = patts[indexF].Split("/");
            var index = nodes.Length - 1;
            while (index >= 0 && item1.Name == nodes[index])
            {
                item1 = item1.ancestor;
                index--;

            }

            return item1;
        }
    }
    public class FilterComparer
    {
        /*        public virtual bool filter(List<Filter> filters, List<AbstrParser.UniEl> list)
                {
                    if (filters.Count == 1)
                        return filters[0].filter(list);
                }*/
    }



    public class ArrFilter : List<Filter>
    {

    }
    public class ReplaySaver
    {
        public string path;
        ConcurrentQueue<string> queue = null;
        Thread t;
        void writeToReplay()
        {
            //            using (StreamWriter sw = new StreamWriter(@"Log.info"))
            {
                for (; ; )
                {
                    string el;
                    while (queue.TryDequeue(out el))
                    {
                        var fileName=Path.GetRandomFileName();
                        using(StreamWriter sw = new StreamWriter(Path.Combine(path,fileName)))
                        {
                            sw.Write(el);

                        }
                        el = null;
                    }

                    Thread.Sleep(100);
                }
            }
        }
        public virtual void save(string input)
        {
            if (queue == null)
            {
                queue = new ConcurrentQueue<string>();
                t = new Thread(writeToReplay);
                t.Start();
            }
            queue.Enqueue(input);
        }

    }

    public class Pipeline
    {
        public string pipelineDescription = "Pipeline example";
        public Step[] steps = new Step[] { new Step() };
        static List<Type> getAllRegTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAssignableTo(typeof(ComparerV)) || t.IsSubclassOf(typeof(Receiver)) || t.IsSubclassOf(typeof(Filter)) || t.IsSubclassOf(typeof(Sender)) || t.IsSubclassOf(typeof(OutputValue))).ToList();

            //            return new List<Type> { typeof(ScriptCompaper),typeof(PacketBeatReceiver), typeof(ConditionFilter), typeof(JsonSender), typeof(ExtractFromInputValue), typeof(ConstantValue),typeof(ComparerForValue) };
        }

        public static string ToStringValue(Pipeline pipes)
        {
            var ser = new SerializerBuilder();
            foreach (var type in getAllRegTypes())
                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            var serializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, pipes);
                return sw.ToString();
            }

        }
        public void run()
        {
            steps[0].run();
        }

        public static Pipeline load(string fileName = @"C:\d\model.yml")
        {
            var ser = new DeserializerBuilder();
            foreach (var type in getAllRegTypes())
                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            var deserializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            return deserializer.Deserialize<Pipeline>(File.OpenText(fileName));

        }

        public void Save( string fileName = @"C:\d\aa1.yml")
        {
            var ser = new SerializerBuilder();
            foreach (var type in getAllRegTypes())
                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            var serializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                serializer.Serialize(sw, this);
            }

        }


    }
    public class Step
    {
        public void run()
        {
            receiver.owner = this;
            sender.owner = this;
            receiver.stringReceived += Receiver_stringReceived;
            receiver.start();

        }
        public string IDStep="Example";
        public string description = "Some comments in this place";
        public bool debugMode = true;
        public Receiver receiver = new PacketBeatReceiver();
        public List<Filter> filters = new List<Filter> { new ConditionFilter() };
        public List<OutputValue> outputFields = new List<OutputValue> { new ConstantValue() { outputPath = "stream", Value = "CheckRegistration" }, new ExtractFromInputValue() { outputPath = "IP", conditionPath = "aa/bb/cc", conditionCalcer = new ComparerForValue() { value_for_compare = "tutu" }, valuePath = "cc/dd" } };
//        public RecordExtractor transformer;
        public Sender sender=new LongLifeRepositorySender();
        public Step()
        {
        }

     //   LongLifeRepositorySender repo = new LongLifeRepositorySender();
        private void Receiver_stringReceived(string input)
        {
            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            AbstrParser.UniEl rootElInput = AbstrParser.CreateNode(null, list, "Item");
//            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
            foreach (var pars in AbstrParser.availParser)
                if (pars.canRazbor(input, rootElInput, list))
                {
                    DateTime time1 = DateTime.Now;

//                    var fltr = filters.First().filter(list);
                    if (filters != null && filters.Count > 0)
                    {
                        bool found = false;
                        foreach (var item in filters.First().filter(list))
                        {
                            FindAndCopy(rootElInput, time1);
                            found = true;
                        }
                        if (!found && debugMode)
                            Console.WriteLine("no filtered records");

                    } else
                    {
                        FindAndCopy(rootElInput, time1);
                    }
                    //                    repo.Add(list);
                    break;
                }

        }

        private void FindAndCopy(AbstrParser.UniEl rootElInput, DateTime time1)
        {
            int count = 0;
            var local_rootOutput = new AbstrParser.UniEl() { Name = "root" };
            foreach (var ff in this.outputFields)
            {
                ff.addToOutput(rootElInput, ref local_rootOutput);
                count++;
            }
            if (debugMode)
                Console.WriteLine(" transform to output " + count + " items");
            var msec = (DateTime.Now - time1).TotalMilliseconds;
            AbstrParser.regEvent("FP", time1);
            sender.send(local_rootOutput);
        }


    }
    public  class JsonSender:Sender
    {
        HttpClient client;
        public JsonSender()
        {
            var handler = new HttpClientHandler();
            //handler.MaxConnectionsPerServer = 2;
            client = new HttpClient(handler);
        }

        public  async Task<string> internSend(string body)
        {
//            var myContent = JsonConvert.SerializeObject(data);
           // Затем вам нужно будет создать объект контента для отправки этих данных, я буду использовать объект ByteArrayContent , но вы можете использовать или создать другой тип, если хотите.

/*            var buffer = System.Text.Encoding.UTF8.GetBytes(body);
            var byteContent = new ByteArrayContent(buffer);
            HttpContent content;
            content.
*/
            var stringContent = new StringContent(body, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

            var result=await client.PostAsync(url, stringContent);


            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                return response;
            }
            return "";
        }

        public string url = @"https://195.170.67:51200/Rec";
        public override void send(AbstrParser.UniEl root)
        {
            base.send(root);
            string str = "{";
            bool first = true;
            foreach (var el in root.childs)
            {
                str += ((first?"":",")+"\"" + el.Name + "\":\"" + el.Value + "\"");
                first = false;
            }
            str += "}";
            if (owner.debugMode)
                Console.WriteLine("Send:" + str);

        }
    }
    public class FileReceiver : Receiver
    {
        string delim = "---------------------------RRRRR----------------------------------";

        public string file_name = @"C:\Data\scratch_1.txt";

        public override void start()
        {
            int ind = 0;
            using (StreamReader sr = new StreamReader(file_name))
            {
                while (!sr.EndOfStream && ind < 50)
                {
                    ind++;

                    var line = sr.ReadLine();
                    int pos = line.IndexOf(delim);
                    if (pos >= 0)
                        line = line.Substring(0, pos);
                    if (line != "")
                    {
                        signal(line);
                    }
                }

            }
        }
    }

    public class PacketBeatReceiver1:Receiver
    {
        public int port = 15001;


        string norm(string line)
        {
            return line;
            //return line.Replace("\\n", "").Replace("\\", "");
        }

        public override void start()
        {
            string delim = "---------------------------RRRRR----------------------------------";

            var file_name = @"C:\Data\scratch_1.txt";
            int ind = 0;
            using (StreamReader sr = new StreamReader(file_name))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    ind++;
                    int pos = line.IndexOf(delim);
                    if (pos >= 0)
                        line = line.Substring(0, pos);
                    if (line != "")
                    {
                        DateTime time1 = DateTime.Now;
                        signal(line);
                        AbstrParser.regEvent("AL", time1);
                        /*int iBeg = 0;
                        int iSco = 0;
                        for (int i = 0; i < line.Length; i++)
                        {
                            if (line[i] == '{')
                                iSco++;
                            if (line[i] == '}')
                                iSco--;
                            if(iSco==0 )
                            {
                                var line1 = line.Substring(iBeg, (i - iBeg) + 1);
                                if(line1.Trim() !="")
                                    signal(norm(line1));
                                iBeg = i + 1;
                            }

                        }
                        {
                            var line1 = line.Substring(iBeg);
                            if (line1.Trim() != "")
                                signal(norm(line1));
                        }*/

                    }
                }

            }

        }
    }
}
