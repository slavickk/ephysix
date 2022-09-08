﻿using System;
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
using System.Data.HashFunction.xxHash;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text.Json;
using System.Security.Cryptography;
using Npgsql;

namespace ParserLibrary
{
    public interface ISelfTested
    {
        Task<(bool,string,Exception)> isOK();
    }

    public abstract class Receiver
    {
        [YamlIgnore]
        public bool MocMode = false;
        public string MocFile;
        public string MocBody;

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
        public delegate  void StringReceived(string input);
        public delegate void BytesReceived(byte[] input);
        [YamlIgnore]
        public Func<string,object,Task> stringReceived;
        public async Task signal(string input,object context)
        {
            if (saver != null)
                saver.save(input);
            if(stringReceived != null)
                await stringReceived(input,context);
        }


        public async Task sendResponse(string response, object context)
        {
            if (!MocMode)
                await sendResponseInternal(response, context);
        }

        public virtual async Task sendResponseInternal(string response,object context)
        { }

        public async Task start()
        {
            if (MocMode)
            {
                string input;
                using (StreamReader sr = new StreamReader(MocFile))
                {
                    input = sr.ReadToEnd();
                }
                string hz = "hz";
                await signal(input,hz);
            }
            else
                await startInternal();
        }


        public async virtual Task startInternal()
        { 
        
        }
    }

    public interface SenderDataExchanger
    {
        string getContent();
        void setContent(string content);
    }

    public class AnnotationAttribute:Attribute
    {
        public string Description
        {
            get
            {
                return description;
            }
        }
        string description;
        public AnnotationAttribute(string description)
        {
            this.description = description;
        }
    }


    public abstract class Sender
    {


        public virtual  string getExample()
        {
            return "";
        }

        public virtual string getTemplate(string key)
        {
            return "";
        }

        public virtual void setTemplate(string key,string body)
        {
        }



        [YamlIgnore]
        public bool MocMode = false;
        public string MocFile;
        public string MocBody="";

        [YamlIgnore]
        public Step owner;
        public enum TypeContent { internal_list,json};
        [YamlIgnore]
        public abstract TypeContent typeContent
        {
            get;
        }
        public string IDResponsedReceiverStep = "";
        public async Task<string> send(AbstrParser.UniEl root)
        {
            if(!MocMode)
            {
                if (typeContent == TypeContent.internal_list)
                    return await sendInternal(root);
                else
                    return await send(root.toJSON());
            } else
            {
                if ((MocBody??"") != "")
                    return MocBody;
                string ans;
                using (StreamReader sr = new StreamReader(MocFile))
                {
                    ans = sr.ReadToEnd();
                    return ans;
                }
            }
        }
        public async virtual Task<string> sendInternal(AbstrParser.UniEl root)
        {
            /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
            return "";

        }
        public async virtual Task<string> send(string JsonBody)
        {
            /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
            return "";

        }
    }
    public abstract class Filter
    {
        public abstract IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list);
    }

    public class AndOrFilter : Filter
    {
        public enum Action { OR, AND, DEL };
        public Action action { get; set; } = Action.AND;
        public Filter[] filters = new Filter[] { new ConditionFilter() };
        public override IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list)
        {
            //            List<AbstrParser.UniEl> answers = new List<AbstrParser.UniEl>();
            IEnumerable<AbstrParser.UniEl> answer = Enumerable.Empty<AbstrParser.UniEl>();
            foreach (var filt in filters)
            {
                var ans = filt.filter(list);
                if (ans.Count() != 0)
                {                   
                    answer = ans;
                } else
                {
                    if (action == Action.AND)
                        return Enumerable.Empty<AbstrParser.UniEl>();
                }
            }
            return answer;
        }
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
        [YamlIgnore]
        public string[] tokens=null;

        /*public ConditionFilter()
        {

        }*/
        //                                  Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
        //Item/everything!/http/request/body/content/Envelope/Body/Invoke/ActionRq/Action/-Name
        //       [JsonInclude]
        public ComparerV conditionCalcer { get; set; } = new ScriptCompaper();
        public static bool isNew = true;
        public override IEnumerable<AbstrParser.UniEl> filter(List<AbstrParser.UniEl> list)
        {
            if (isNew)
            {
                if (tokens == null)
                    tokens = conditionPath.Split("/");
                return list[0].getAllDescentants(tokens,0).Where(ii =>  conditionCalcer.Compare(ii));
            }
            else
            return list.Where(ii => ii.path == conditionPath && conditionCalcer.Compare(ii));
        }
    }

    public abstract class ConverterOutput
    {
        public abstract AbstrParser.UniEl Convert(string value, AbstrParser.UniEl input_el, AbstrParser.UniEl output_el);
    }

    public abstract class AliasProducer
    {

        public abstract string getAlias(string originalValue);
    }

    public class AliasPhone : AliasProducer
    {
        public override string getAlias(string originalValue)
        {
            if (originalValue.Length > 3)
                return $"{originalValue.Substring(0, 2)}*{originalValue.Substring(originalValue.Length-3)}";
            return "*";
        }
    }
    public class AliasPan : AliasProducer
    {
        public override string getAlias(string originalValue)
        {
            if (originalValue.Length >= 16)
                return $"{originalValue.Substring(0, 4)}***{originalValue.Substring(originalValue.Length - 3)}";
            return "*";
        }
    }
    public class AliasFIO : AliasProducer
    {
        public override string getAlias(string originalValue)
        {
            var tt =originalValue.Split(' ');
            return tt.Select(query => query.Substring(0,1)).Aggregate((a, b) => a + " " + b+"."); ;
        }
    }

    public class HashOutput:ConverterOutput
    {
        public AliasProducer aliasProducer { get; set; }
        public HashConverter hashConverter { get; set; }
        public class OutItem
        {
            public string hash { get; set; }
            public string alias { get; set; }
        }
        public override AbstrParser.UniEl Convert(string value, AbstrParser.UniEl input_el, AbstrParser.UniEl output_el)
        {
            if (aliasProducer == null)
            {
                output_el.Value= hashConverter.hash(value);
                return output_el;
            }
            else
            {
                var el1=new AbstrParser.UniEl(output_el) {  Name="h",Value= hashConverter.hash(value) };
                var el2 = new AbstrParser.UniEl(output_el) { Name = "a", Value = aliasProducer.getAlias(value) };
                return output_el;
            }
//            return JsonSerializer.Serialize<OutItem>(new OutItem() { alias = aliasProducer.getAlias(value), hash = hashConverter.hash(value) });   
        }

    }

    public class Hash:HashOutput
    {
        public int SizeInBits = 64;
        public Hash()
        {
            hashConverter = new Hasher();
        }
    }

    public abstract class HashConverter
    {
        public abstract string hash(string value);

    }
    public class CryptoHash : HashConverter
    {
        static public string  pwd = "QWE123";
        public override string hash(string value)
        {
            var data = Encoding.UTF8.GetBytes(pwd + value);
//            string sHash;
            using (SHA256 shaM = new SHA256Managed())
            {
                byte[] hash = shaM.ComputeHash(data);
                return Convert.ToBase64String(hash);
//                Console.WriteLine(sHash);
            }
  //          throw new NotImplementedException();
        }
    }
    public class Hasher : HashConverter
    {
        public int SizeInBits = 64;
        bool init = false;
        public Hasher()
        {

        }
         IxxHash instance ;
        void Init()
        {
            if(!init)
            {
                init = true;
                instance = xxHashFactory.Instance.Create(new xxHashConfig() { HashSizeInBits = SizeInBits });
            }

        }
        public override string hash(string value)
        {
            Init();
            return instance.ComputeHash(Encoding.ASCII.GetBytes(value)).AsHexString();
        }
    }


    public abstract class OutputValue
    {
        public string outputPath;
        public bool isUniqOutputPath = true;
        public enum TypeCopy { Value, Structure };
        public TypeCopy typeCopy = TypeCopy.Value;
        public enum OnEmptyAction { Skip, FillEmpty };
        public OnEmptyAction onEmptyValueAction = OnEmptyAction.Skip;

        public ConverterOutput converter = null;
        [YamlIgnore]
        public virtual bool canReturnObject => true;
        
        public abstract object getValue(AbstrParser.UniEl rootEl);
        public abstract AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl);

        string[] outs = null;
        protected virtual AbstrParser.UniEl createOutPath(AbstrParser.UniEl outputRoot)
        {
            if(outs==null && outputPath != "")
            {
                outs = outputPath.Split("/");
            }
            if (outs == null)
                return outputRoot;
            var rootEl = outputRoot;
            for(int i=0;i < outs.Length;i++)
            {
                var el=rootEl.childs.LastOrDefault(ii => ii.Name == outs[i]);
                if (el == null || ( !isUniqOutputPath &&   i== outs.Length-1))
                    el = new AbstrParser.UniEl(rootEl) { Name = outs[i] };
                rootEl = el;
            }
            return rootEl;
        }
        public virtual bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot)
        {
            // skipped--------------------------- Пока поддерживается только линейная структура записи
       //     if (typeCopy == TypeCopy.Value)
            {
                var el1 = getNode(inputRoot);
                if(!this.canReturnObject)
                {

                }

                if (el1 == null  && onEmptyValueAction == OnEmptyAction.Skip && this.canReturnObject)
                    return false;
                var el = createOutPath(outputRoot);
                //                AbstrParser.UniEl el = new AbstrParser.UniEl(outputRoot);
                //el.Name = outputPath;
                //                if(el.)
                //                if(el1.)
                if (!this.canReturnObject || el1.childs.Count == 0)
                {
                    object elV;
                    if (canReturnObject)
                        elV = el1.Value.ToString();
                    else
                        elV=getValue(inputRoot);
                    if (elV != null)
                    {
                        if (converter != null)
                            el = converter.Convert(elV.ToString(), inputRoot, el);
                        else
                            el.Value = elV;
                    }
                } else
                {
                    el1.copy(el);
//                    el.childs.Add(el1.copy(el));
                }
            }
         /*   else
            {
                var el = createOutPath(outputRoot);

                el.childs.Add(inputRoot.copy(outputRoot));
            }*/
            return true;
        }
    }

    public class GUIAttribute:Attribute
    {
        Type settingsType;
        public GUIAttribute(Type setType)
        {
            settingsType = setType;
        }
    }
    public class TemplateSenderOutputValue : OutputValue
    {
        string templ;
        AbstrParser.UniEl rootElement;
        public string templateBody
        {
            get
            {
                return ownerSender.getTemplate(key);
            }
            set
            {
                ownerSender.setTemplate(key,value);
                SetTemplate(value);

            }
        }

        private void SetTemplate(string value)
        {
            templ = value;
           rootElement= AbstrParser.ParseString(templ);
        }

      

        Sender ownerSender;
        string key;
        public TemplateSenderOutputValue(Sender sender,string key1)
        {
            key = key1;
            ownerSender = sender;
        }

        public override bool canReturnObject => false;
        public override bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot)
        {
            foreach (var el in rootElement.childs)
                el.copy(outputRoot);
            return true;
            //            return base.addToOutput(inputRoot, ref outputRoot);
        }
        public override object getValue(AbstrParser.UniEl rootEl)
        {
            return null;
        }

        public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
        {
            return null;
        }

    }
    public class TemplateOutputValue:OutputValue
    {
        string templ;
        AbstrParser.UniEl rootElement ;
        public string templateBody
        {
            get
            {
                return templ;
            }
            set
            {
                templ=value;
                List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                rootElement = AbstrParser.CreateNode(null, list, "TT");
                try
                {
                    //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
                    foreach (var pars in AbstrParser.availParser)
                        if (pars.canRazbor(templ, rootElement, list))
                        {

                        }
                }
                catch
                {

                }

            }
        }
        public override bool canReturnObject => false;
        public override bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot)
        {
            foreach( var el in rootElement.childs)
              el.copy(outputRoot);
            return true;
//            return base.addToOutput(inputRoot, ref outputRoot);
        }
        public override object getValue(AbstrParser.UniEl rootEl)
        {
            return null;
        }

        public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
        {
            return null;
        }

    }

    public class ConstantValue : OutputValue
    {

        public static object ConvertFromType(string value,TypeObject tObject)
        {
            switch (tObject)
            {
                case  TypeObject.String:
                    return value;
                    break;

                case  TypeObject.Number:
                    return Convert.ToDouble(value);
                    break;
                case TypeObject.Boolean:
                    return Convert.ToBoolean(value);
                    break;

            }
            return value;
        }
        public enum TypeObject { String,Number,Boolean};
        public TypeObject typeConvert = TypeObject.String; 
        public override string ToString()
        {
            return outputPath + ";" + Value;
        }
        public object Value { get; set; }

        public override bool canReturnObject => false;

        public override object getValue(AbstrParser.UniEl rootEl)
        {
            if(Value.GetType() == typeof(string) &&typeConvert != TypeObject.String)
                return ConvertFromType( Value.ToString(), typeConvert);
            return Value;
        }

        public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
        {
            return null;
        }
    }

    public class ExtractFromInputValue : OutputValue
    {
        public override string ToString()
        {
            return outputPath + "; from " + conditionPath;
        }
        public string conditionPath { get; set; }
        [YamlIgnore]
        public string[] conditionPathToken = null;
        public ComparerV conditionCalcer { get; set; }
        public string valuePath { get; set; } = "";
        [YamlIgnore]
        public string[] valuePathToken = null;
        public override object getValue(AbstrParser.UniEl rootEl)
        {
            return getNode(rootEl).Value ;
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

        private AbstrParser.UniEl getLocalRoot( AbstrParser.UniEl item1, string[] patts)
        {
            var item = item1;
            AbstrParser.UniEl itemRet = null;
            for(int i=Math.Min(item1.rootIndex,patts.Length-1);i>=0;i--)
            {
                while (item.rootIndex > i)
                    item = item.ancestor;
                if (item.Name != patts[i])
                    itemRet = item.ancestor;
//                        return item;
            }
            if (itemRet == null)
                return item;
            return itemRet;
        }

        public virtual AbstrParser.UniEl getFinalNode(AbstrParser.UniEl node)
        {
            return node;
        }
        public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
        {
            if (ConditionFilter.isNew)
            {
                if (conditionPathToken == null)
                    conditionPathToken = conditionPath.Split("/");

                var rootEl1 = getLocalRoot(rootEl, conditionPathToken);

                foreach (var item in rootEl1.getAllDescentants(conditionPathToken, rootEl1.rootIndex).Where(ii => ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
                {
                    var item1 = item;
                    if (valuePath != "")
                    {
                        if (valuePathToken == null)
                            valuePathToken = valuePath.Split("/");
                        item1 = getLocalRoot(item1, valuePathToken);
                        foreach (var item2 in item1.getAllDescentants(valuePathToken, item1.rootIndex))
                            return getFinalNode( item2);
                    }
                    else
                        return getFinalNode(item);

                }

            }
            else
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
                            return getFinalNode(item2);
                    }
                    else
                        return getFinalNode(item);

                }
            }
            return null;
        }
    }

    public class ExtractFromInputValueWithScript: ExtractFromInputValue
    {
        MethodDelegate checker = null;
        string body = @"using System;
using System.Linq;
using ParserLibrary;
AbstrParser.UniEl  ConvObject(AbstrParser.UniEl el)
{                                                           
            var sb = new StringBuilder();
            el.ancestor.childs.ForEach(s => sb.Append(s.Value));
            return new AbstrParser.UniEl() { Value = sb};
}
";
        
/*        AbstrParser.UniEl ConvObject(AbstrParser.UniEl el)
        {
            var sb = new StringBuilder();
            el.ancestor.childs.ForEach(s => sb.Append(s.Value+";"));
            return new AbstrParser.UniEl() { Value = sb };
        }*/
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

        public override AbstrParser.UniEl getFinalNode(AbstrParser.UniEl el)
        {
            return checker(el) as AbstrParser.UniEl;
//            return base.getNode(rootEl);
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
        public AbstrParser.UniEl lastExecutedEl = null;
        public static Metrics metrics = new Metrics();

        public string hashWord { get; set; } = "QWE123";
        public string pipelineDescription = "Pipeline example";
        public Step[] steps { get; set; } = new Step[] { };// new Step[] { new Step() };
        //public AbstrParser.UniEl rootElement = null;
        public async Task<bool> SelfTest()
        {
            var res1=await (this.steps[0].sender as ISelfTested).isOK();
            if(res1.Item3 == null)
                Logger.log(  "{Item}.Results:{Res}",  Serilog.Events.LogEventLevel.Information,"any" , res1.Item2.ToString(),(res1.Item1 ? "OK" : "Fail"));
            else
                Logger.log("{Item}.Results:{Res}",res1.Item3, Serilog.Events.LogEventLevel.Information, "any", res1.Item2.ToString(), (res1.Item1 ? "OK" : "Fail"));

            return res1.Item1;
        }
        static List<Type> getAllRegTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAssignableTo(typeof(ComparerV)) || t.IsAssignableTo(typeof(ConverterOutput)) || t.IsSubclassOf(typeof(Receiver)) || t.IsSubclassOf(typeof(Filter)) || t.IsSubclassOf(typeof(Sender)) || t.IsSubclassOf(typeof(OutputValue)) || t.IsAssignableTo(typeof(AliasProducer)) || t.IsAssignableTo(typeof(HashConverter))).ToList();

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
        public async Task run()
        {
            CryptoHash.pwd = this.hashWord;
            await steps.First(ii=>ii.IDPreviousStep=="" && ii.receiver != null).run();
        }

        public static Pipeline load(string fileName = @"C:\d\model.yml")
        {
            var ser = new DeserializerBuilder();
            foreach (var type in getAllRegTypes())
                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            var deserializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var pip= deserializer.Deserialize<Pipeline>(File.OpenText(fileName));
            foreach (var step in pip.steps)
                step.owner = pip;
            return pip;
        }
        public static Pipeline loadFromString(string content)
        {
            var ser = new DeserializerBuilder();
            foreach (var type in getAllRegTypes())
                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            var deserializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var pip = deserializer.Deserialize<Pipeline>(content);
            foreach (var step in pip.steps)
                step.owner = pip;
            return pip;
        }

        public void Save( string fileName = @"C:\d\aa1.yml")
        {
            ISerializer serializer = getSerializer();
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                serializer.Serialize(sw, this);
            }

        }

        private static ISerializer getSerializer()
        {
            var ser = new SerializerBuilder();
            foreach (var type in getAllRegTypes())
                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            var serializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            return serializer;
        }

        public void Save(TextWriter stream)
        {
            ISerializer serializer = getSerializer();
            serializer.Serialize(stream, this);

        }



    }
    public class Step
    {

        public static void Test()
        {

            using (StreamReader sr = new StreamReader(@"C:\Users\Juriy\Downloads\Telegram Desktop\test200.json"))
            {
                var input = sr.ReadToEnd();
                List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                var rootElement = AbstrParser.CreateNode(null, list, "TT");
                try
                {
                    //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
                    foreach (var pars in AbstrParser.availParser)
                        if (pars.canRazbor(input, rootElement, list))
                        {

                        }
                }
                catch
                {

                }
                var ll= rootElement.toJSON();
            }
        }
                public async Task run()
        {

            if(receiver != null)
                receiver.owner = this;
            if(sender != null)
                sender.owner = this;
            receiver.stringReceived = Receiver_stringReceived;
            try
            {
                await receiver.start();
            } 
            catch(Exception e88)
            {
 //               MessageBox.Show(e88.ToString());
            }
        }
        public string IDStep { get; set; } = "Example";


        public string IDPreviousStep { get; set; } = "";

        public string IDResponsedReceiverStep { get; set; } = "";


        public string description { get; set; } = "Some comments in this place";
        [YamlIgnore]
        public bool debugMode { get; set; } = true;
        public Receiver receiver { get; set; } = null;// new PacketBeatReceiver();
        public class ItemFilter
        {
            public string Name { get; set; } = "example";
            public override string ToString()
            {
                return $"filter:{Name}";
            }
            public Filter filter { get; set; } = new ConditionFilter();
            public List<OutputValue> outputFields { get; set; } = new List<OutputValue> { new ConstantValue() { outputPath = "stream", Value = "CheckRegistration" }, new ExtractFromInputValue() { outputPath = "IP", conditionPath = "aa/bb/cc", conditionCalcer = new ComparerForValue() { value_for_compare = "tutu" }, valuePath = "cc/dd" } };
            public int exec(AbstrParser.UniEl rootElInput, ref AbstrParser.UniEl local_rootOutput)
            {
                int count = 0;
                if(local_rootOutput == null)
                    local_rootOutput = new AbstrParser.UniEl() { Name = "root" };

                foreach (var ff in outputFields)
                {
                    if (ff.addToOutput(rootElInput, ref local_rootOutput))
                        count++;
                }
                return count;
            }

    }
    public List<ItemFilter> converters { get; set; } = new List<ItemFilter>() {  };
/*        public List<Filter> filters = new List<Filter> { new ConditionFilter() };
        public List<OutputValue> outputFields = new List<OutputValue> { new ConstantValue() { outputPath = "stream", Value = "CheckRegistration" }, new ExtractFromInputValue() { outputPath = "IP", conditionPath = "aa/bb/cc", conditionCalcer = new ComparerForValue() { value_for_compare = "tutu" }, valuePath = "cc/dd" } };*/
//        public RecordExtractor transformer;
        public Sender sender { get; set; } = new LongLifeRepositorySender();
        [YamlIgnore]
        public Pipeline owner { get; set; }
        public Step(/*Pipeline owner1*/)
        {
//            owner = owner1;
            sucMetric = Pipeline.metrics.getMetric("packagesReceived", false, true, "All packages, sended to utility");
            errMetric = Pipeline.metrics.getMetric("packagesReceived", true, false, "All packages, sended to utility");
        }

        Metrics.Metric sucMetric = null;
        Metrics.Metric errMetric = null;
        //   LongLifeRepositorySender repo = new LongLifeRepositorySender();

        public class ContextItem
        {
            public List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            public object context;
        }
        
        private async Task Receiver_stringReceived(string input,object context)
        {

            DateTime time2 = DateTime.Now;
            ContextItem contextItem = new ContextItem() { context = context };
            //            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            var rootElement = AbstrParser.CreateNode(null, contextItem.list, this.IDStep);
            rootElement = AbstrParser.CreateNode(rootElement, contextItem.list, "Rec");
            owner.lastExecutedEl = rootElement;
            await FilterInfo(input, time2, contextItem, rootElement);

            await checkChilds(contextItem, rootElement);

            contextItem.list.Clear();
            contextItem = null;

            rootElement = null;
        }

        private async Task checkChilds(ContextItem contextItem, AbstrParser.UniEl rootElement)
        {

            foreach(var nextStep in this.owner.steps.Where(ii => ii.IDPreviousStep == this.IDStep))
            {
                await nextStep.FilterStep(contextItem, rootElement);
                await nextStep.checkChilds(contextItem, rootElement);
            }
        }

        private async Task<string> FindAndCopy1(AbstrParser.UniEl rootElInput, DateTime time1, ItemFilter item, AbstrParser.UniEl el, List<AbstrParser.UniEl> list)
        {
            int count = 0;
            AbstrParser.UniEl local_rootOutput = new AbstrParser.UniEl() { Name = "root" };
            count = item.exec(rootElInput, ref local_rootOutput);
            /*            foreach (var ff in item.outputFields)
                        {
                            if(ff.addToOutput(rootElInput, ref local_rootOutput))
                                count++;
                        }*/
            if (debugMode)
                Logger.log(" transform to output {count} items", Serilog.Events.LogEventLevel.Debug, count);
            var msec = (DateTime.Now - time1).TotalMilliseconds;
            AbstrParser.regEvent("FP", time1);
            if (IDResponsedReceiverStep != "")
                return "";
            else
                return await sender.send(local_rootOutput);
        }

                public async Task<string> FilterInfo1(string input, DateTime time2, List<AbstrParser.UniEl> list, AbstrParser.UniEl rootElement)
        {
            try
            {
                //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
                foreach (var pars in AbstrParser.availParser)
                    if (pars.canRazbor(input, rootElement, list))
                    {
                        DateTime time1 = DateTime.Now;

                        //                    var fltr = filters.First().filter(list);
                        if (converters != null && converters.Count > 0)
                        {
                            bool found = false;
                            foreach (var item in converters/*.First().filter(list)*/)
                            {
                                foreach (var item1 in item.filter.filter(list))
                                {
                                    var st = await FindAndCopy1(rootElement, time1, item, item1, list);
                                    if (st != "")
                                        return st;
                                }
                                found = true;
                            }
                            /*                        if (!found && debugMode)
                                                        Console.WriteLine("no filtered records");*/

                        } /*else
                    {
                        FindAndCopy(rootElInput, time1);
                    }*/
                        //                    repo.Add(list);
                        break;
                    }
                return "";
            }
            catch (Exception e77)
            {
                throw;
            }
        }

        private bool tryParse(string input, ContextItem context, AbstrParser.UniEl rootElement)
        {
            foreach (var pars in AbstrParser.availParser)
                if (pars.canRazbor(input, rootElement, context.list))
                {
                    return true;
                }
            return false;

        }
        public async Task FilterInfo(string input, DateTime time2, ContextItem context, AbstrParser.UniEl rootElement)
        {
            try
            {
                //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };

                if(tryParse(input, context, rootElement))
                    await FilterStep(context, rootElement);
/*                foreach (var pars in AbstrParser.availParser)
                if (pars.canRazbor(input, rootElement, context.list))
                    {
                        await FilterStep(context, rootElement);
                        break;
                    }
*/

                if (sucMetric != null)
                    sucMetric.Add(time2);
            }
            catch (Exception e77)
            {
                if(errMetric != null)
                    errMetric.Add(time2);
                rootElement = null;
                throw;
            }
        }

        private async Task FilterStep(ContextItem context, AbstrParser.UniEl rootElement)
        {
            DateTime time1 = DateTime.Now;

            //                    var fltr = filters.First().filter(list);
            if (converters != null && converters.Count > 0)
            {
                AbstrParser.UniEl local_rootOutput = new AbstrParser.UniEl() { Name = "root" };
                bool found = false;
                foreach (var item in converters/*.First().filter(list)*/)
                {
                    foreach (var item1 in item.filter.filter(context.list))
                        found=await FindAndCopy(rootElement, time1, item, item1, context,local_rootOutput);
//                    found = true;
                }
                if (found)
                {
                    if(rootElement?.ancestor?.Name != this.IDStep )
                    {
                        var root = CheckAndFillNode(rootElement, IDStep, true);// new AbstrParser.UniEl(rootElement.ancestor) { Name = IDStep };
                        rootElement = CheckAndFillNode(root,"Rec");
                    }
                    await SendToSender(rootElement, context, local_rootOutput);
                }


            }
        }

        private async Task<bool> FindAndCopy(AbstrParser.UniEl rootElInput, DateTime time1,ItemFilter item,AbstrParser.UniEl el,ContextItem context, AbstrParser.UniEl local_rootOutput)
        {
            int count = 0;
            count = item.exec(el/*rootElInput*/, ref local_rootOutput);
            /*            foreach (var ff in item.outputFields)
                        {
                            if(ff.addToOutput(rootElInput, ref local_rootOutput))
                                count++;
                        }*/
            if (debugMode)
                Logger.log(" transform to output {count} items", Serilog.Events.LogEventLevel.Debug, count);
            var msec = (DateTime.Now - time1).TotalMilliseconds;
            AbstrParser.regEvent("FP", time1);
            return true;

            //            if(this.)
        }
        public int recordSendCount = 0;
        private async Task SendToSender(AbstrParser.UniEl rootElInput, ContextItem context, AbstrParser.UniEl local_rootOutput)
        {
            // Save sender context
            //rootElInput.
            if (IDResponsedReceiverStep != "")
            {
                var step = this.owner.steps.FirstOrDefault(ii => ii.IDStep == IDResponsedReceiverStep);
                await step.receiver.sendResponseInternal(local_rootOutput.toJSON(), context.context);

            }
            else
            {
                var sendNode =CheckAndFillNode(rootElInput,"Send",true);
                var toNode = CheckAndFillNode(sendNode, "To");
                toNode.childs.AddRange(local_rootOutput.childs);
                var ans = await sender.send(local_rootOutput);
                recordSendCount++;
                // bool isSave = false;
                if (ans != "")
                {
                    if (sender.IDResponsedReceiverStep != null && sender.IDResponsedReceiverStep != "")
                    {
                        var step = this.owner.steps.FirstOrDefault(ii => ii.IDStep == sender.IDResponsedReceiverStep);
                        await step.receiver.sendResponse(ans, context.context);

                    }
                    // Swap the statements
                    if (this.owner.steps.Count(ii => ii.IDPreviousStep == this.IDStep) > 0)
                    {
//                        tryParse(ans, context, CheckAndFillNode(sendNode, "From"));
                        var newRoot = new AbstrParser.UniEl(rootElInput.ancestor) { Name = this.owner.steps.First(ii => ii.IDPreviousStep == this.IDStep).IDStep };
                        newRoot = new AbstrParser.UniEl(newRoot) { Name = "Rec" };



                        this.tryParse(ans, context, newRoot);
                    }


                    /*                    if (step != null)
                                        {
                                            var newRoot = new AbstrParser.UniEl(rootElInput) { Name = "SendResponse" };
                                            await step.FilterInfo(ans, time1, list, newRoot);
                                        }*/
                }
            }
        }

        private static AbstrParser.UniEl CheckAndFillNode(AbstrParser.UniEl rootElInput,string Name,bool getAncestor =false)
        {
            AbstrParser.UniEl contextNode = (getAncestor?(rootElInput.ancestor): rootElInput).childs.FirstOrDefault(ii => ii.Name == Name);
            if (contextNode == null)
                contextNode = new AbstrParser.UniEl((getAncestor ? (rootElInput.ancestor) : rootElInput)) { Name = Name};
            return contextNode;
        }
    }


    public  class JsonSender:Sender,ISelfTested
    {
        [YamlIgnore]
        HttpClient client;
        public string certName = "";
        public string certPassword = "";
//        public string certThumbprint= "E77587679318FED87BB040F00D76AB461B962D95";
        public List<string> certThumbprints = new List<string> { "A77587679318FED87BB040F00D76AB461B962D95" };
        public double timeoutSendInSeconds = 5;
        public JsonSender()
        {
            createMetrics();
       //     InitClient();
        }

        private void InitClient()
        {
            var handler = new HttpClientHandler();
            handler.MaxConnectionsPerServer = 256;
            handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation;

            if (certName != "")
            {
                X509Certificate2 certificate = new X509Certificate2(certName, certPassword);
                /*        X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet |
                        X509KeyStorageFlags.Exportable);*/
                handler.ClientCertificates.Add(certificate);
            }
            client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(timeoutSendInSeconds);
        }
        private bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors)
        {
            if(certThumbprints.Count >0 && !certThumbprints.Contains(certificate.Thumbprint))
            {
                Logger.log("Invalid certificate {Incorrect} , valid thumbprint {valid}.", Serilog.Events.LogEventLevel.Error, "any", certificate.Thumbprint, certThumbprints);
                return false;
            }
            return true;
            return sslErrors == SslPolicyErrors.None;

            // It is possible inpect the certificate provided by server
            Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
            Console.WriteLine($"Effective date: {certificate.GetEffectiveDateString()}");
            Console.WriteLine($"Exp date: {certificate.GetExpirationDateString()}");
            Console.WriteLine($"Issuer: {certificate.Issuer}");
            Console.WriteLine($"Subject: {certificate.Subject}");

            // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
            Console.WriteLine($"Errors: {sslErrors}");
            return sslErrors == SslPolicyErrors.None;
        }
        [YamlIgnore]
        public int index = 0;

        bool init = false;
        public  async Task<string> internSend(string body)
        {
            if(!init)
            {
                init = true;
                InitClient();
            }
            int kolRetry = 0;
            int indexDelay = 0;
        //            var myContent = JsonConvert.SerializeObject(data);
        // Затем вам нужно будет создать объект контента для отправки этих данных, я буду использовать объект ByteArrayContent , но вы можете использовать или создать другой тип, если хотите.

        /*            var buffer = System.Text.Encoding.UTF8.GetBytes(body);
                    var byteContent = new ByteArrayContent(buffer);
                    HttpContent content;
                    content.
        */
        restart:
            kolRetry++;
            HttpResponseMessage result = null;
            var stringContent = new StringContent(body, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            try
            {
                result = await client.PostAsync(urls[index], stringContent);

            }
            catch(Exception e63)
            {
                if (kolRetry / urls.Length >= timeoutsBetweenRetryInMilli.Length)
                    throw;
                Logger.log("Error send", e63, Serilog.Events.LogEventLevel.Error);
                indexDelay = nextStepCalc(kolRetry, indexDelay);
                goto restart;

            }
            if (result!= null && result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                return response;
            }
            if (kolRetry / urls.Length >= timeoutsBetweenRetryInMilli.Length)
                return "";
            indexDelay = nextStepCalc(kolRetry, indexDelay);
            goto restart;

        }

        private int nextStepCalc(int kolRetry, int indexDelay)
        {
            if (++index >= urls.Length)
                index = 0;
            if (kolRetry % urls.Length == 0 && indexDelay < timeoutsBetweenRetryInMilli.Length)
            {
                Thread.Sleep(timeoutsBetweenRetryInMilli[indexDelay]);
                indexDelay++;
            }

            return indexDelay;
        }

        public string[] urls = { @"https://195.170.67:51200/Rec" };
        public string url
        {
            set
            {
                urls = new string[] { value };
                timeoutsBetweenRetryInMilli = new int[] { 100 };
            }
            get
            {
                return (urls== null || urls.Length ==0)?"":urls[0];
            }
        }

        public override TypeContent typeContent => TypeContent.internal_list;//throw new NotImplementedException();

        public int[] timeoutsBetweenRetryInMilli = { 100};
        Metrics.Metric sendToRex = null; 
        Metrics.Metric sendToRexErr = null;
        string getVal(AbstrParser.UniEl el)
        {
            if(el.childs.Count ==0)
            return $"\"{el.Value}\"";
            else
            {
                return "{"+String.Join(",",el.childs.Select(ii=>$"\"{ii.Name}\":\"{ii.Value}\""))+"}";
            }
        }
        public async override Task<string> sendInternal(AbstrParser.UniEl root)
        {
            await base.sendInternal(root);
            string str = "{";
            bool first = true;
            str = "{" + String.Join(",", root.childs.Select(ii => $"\"{ii.Name}\":{getVal(ii)}")) + "}";
/*            foreach (var el in root.childs)
            {
                str += ((first?"":",")+"\"" + el.Name + "\":" + getVal(el );
                first = false;
            }
            str += "}";*/

/*            if (owner.debugMode)
                Console.WriteLine("Send:" + str);*/
            DateTime time1 = DateTime.Now;
            try
            {
                var ans = await internSend(str);
                Logger.log(time1, " Send:{Request}  ans:{Response}", "JsonSender", Serilog.Events.LogEventLevel.Information, str, ans);
  //              createMetrics();
              if(sendToRex != null)
                sendToRex.Add(time1);
              return ans;
            }
            catch (Exception e77)
            {
//                createMetrics();
                if(sendToRex!= null)
                    sendToRexErr.Add(time1);
                throw;
            }
        }

        private void createMetrics()
        {
            if (sendToRex == null)
            {
                sendToRex = Pipeline.metrics.getMetric("sendToRex", false, true, "Sended transactions to CCFA");
                sendToRexErr = Pipeline.metrics.getMetric("sendToRex", true, false, "Sended transactions to CCFA");
            }
        }

        public async Task<(bool,string,Exception)> isOK()
        {
            string details;
            bool isSuccess = true;
            Exception exc = null;
            details = "Make http request to " + this.urls[0];
            try
            {
                DateTime time1 = DateTime.Now;
                var ans = await internSend("{\"stream\":\"checkTest\",\"originalTime\":\"2021-12-18T02:05:04.500Z\"}"); 
                Logger.log(time1,"-Send:{ans}" ,"SelfTest",Serilog.Events.LogEventLevel.Information,ans);
                if (ans == "")
                    isSuccess = false;
            }
            catch(Exception e77)
            {
                isSuccess = false;
                exc = e77;
            }
            //            if(ans)
            return (isSuccess,details,exc);
        }
    }

    public class StreamSender:JsonSender
    {
        public string streamName  = "checkRegistration5";
        public async override Task<string> sendInternal(AbstrParser.UniEl root)
        {
            if(root.childs.Count(ii=>ii.Name =="stream")==0)
                root.childs.Add(new AbstrParser.UniEl() {  Name="stream", Value=streamName});
            return await base.sendInternal(root);
        }

        public class Stream
        {
            public class Field
            {
                public string Name { get; set; } = "";
                public string Type { get; set; }
                public string Detail { get; set; }
                public bool Hash { get; set; }
                public long? linkedColumn { get; set; }
            }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Field> fields { get; set; } = new List<Field>();
        }
        Dictionary<string, Stream> streams = new Dictionary<string, Stream>();  


        public string db_connection_string = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
        public override string getTemplate(string key)
        {
            return formJson(getStream(streamName));
        }

        public Stream getStream(string key)
        {
            Stream stream;
            if (streams.TryGetValue(key, out stream))
                return (stream);
            //                return JsonSerializer.Serialize<IEnumerable<string>>(stream.fields.Select(ii=>"\"ii.Name)); 
            stream = new Stream();
            stream.Name = "";
            var conn = new NpgsqlConnection(db_connection_string);
            conn.Open();
            using (var cmd = new NpgsqlCommand(@"select n.nodeid,n.name,a.val,np.name,'String',ap.val from md_node n
inner join md_node_attr_val a  
on ( a.nodeid=n.nodeid and attrid=22)
inner join md_arc l on (l.fromid=n.nodeid and l.isdeleted=false)
inner join md_node np on (l.toid=np.nodeid and np.isdeleted=false)
inner join md_node_attr_val ap on ( ap.nodeid=np.nodeid and ap.attrid=22)
where n.typeid=md_get_type('Stream') and n.name =@name and n.isdeleted=false
", conn))
            {
                cmd.Parameters.AddWithValue("@name", key);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (stream.Name == "")
                        {
                            stream.Name = reader.GetString(1);
                            stream.Description = reader.GetString(2);
                        }
                        stream.fields.Add(new Stream.Field() { Name = reader.GetString(3), Type = reader.GetString(4), Detail = reader.GetString(5) });
                    }
                }
            }

            conn.Close();
            if(stream.Name =="")
            {
                stream.Name = key;
                stream.fields.Add(new Stream.Field() { Name = "stream", Detail = "Name of stream", Type = "String" });
                stream.fields.Add(new Stream.Field() { Name = "originalTime", Detail = "Time of stream", Type = "DateTime" });
            }
            streams.TryAdd(key, stream);
            return (stream);
        }

        private static string formJson(Stream stream)
        {
            return "{" + string.Join<string>(",", stream.fields.Select(ii => $"\"{ii.Name}\":\"{((ii.Name=="stream")?stream.Name:"")}\"")) + "}";
        }

        public override void setTemplate(string key, string body)
        {
//            setStream(body);

            base.setTemplate(key, body);
        }

        public void setStream(string body)
        {
            var conn = new NpgsqlConnection(db_connection_string);
            conn.Open();
            using (var cmd = new NpgsqlCommand(@"select * from md.md_add_stream_description(@json)",conn))
            {
                cmd.Parameters.AddWithValue("@json", NpgsqlTypes.NpgsqlDbType.Json, body);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class FileReceiver : Receiver
    {
        string delim = "---------------------------RRRRR----------------------------------";

        public string file_name = @"C:\Data\scratch_1.txt";

        public async override Task startInternal()
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
                        await signal(line,null);
                    }
                }

            }
        }
    }

    public class TestReceiver:Receiver
    {
        public string path;
        public string pattern="";



        public async override Task startInternal()
        {

            foreach (var file_name in ((pattern == "") ? new string[] { path } : Directory.GetFiles(path, pattern)))
            {
                using (StreamReader sr = new StreamReader(file_name))
                {
                    var body = sr.ReadToEnd();
                    await signal(body,null);

                }
            }

        }
    }
}
