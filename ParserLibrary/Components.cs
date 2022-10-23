using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet;
using YamlDotNet.Serialization;
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
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using YamlDotNet.Core.Tokens;
using static ParserLibrary.HTTPReceiver;
using Microsoft.AspNetCore.Http.Metadata;
using CSScripting;
using System.Runtime.InteropServices;

namespace ParserLibrary
{
    public interface ISelfTested
    {
        Task<(bool,string,Exception)> isOK();
    }

    public abstract class Receiver
    {
        public bool  cantTryParse=false;
/*        public virtual bool cantTryParse()
        {
            return false;
        }*/

        public virtual void Init(Pipeline owner)
        {

        }

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
                owner_internal = value;
                debugMode = value.debugMode;
            }
            get
            {
                return owner_internal; 
            }
        }
        [YamlIgnore]
        Step owner_internal;

        public delegate  void StringReceived(string input);
        public delegate void BytesReceived(byte[] input);
        [YamlIgnore]
        public Func<string,object,Task> stringReceived;
        public async Task signal(string input,object context)
        {
            if(debugMode)
            {
                Logger.log("Receive step:{o} {input} {thr}", Serilog.Events.LogEventLevel.Debug, "any", owner, input,Thread.CurrentThread.ManagedThreadId);
            }
            if (saver != null)
                saver.save(input);
            if(stringReceived != null)
                await stringReceived(input,context);
        }


        public async Task sendResponse(string response, object context)
        {
            if (debugMode)
                Logger.log("Send answer to {step} : {content} ", Serilog.Events.LogEventLevel.Debug, "any",owner, response);
            if (saver != null)
                saver.save(response);

            if (!MocMode)
                await sendResponseInternal(response, context);
        }

        public virtual async Task sendResponseInternal(string response,object context)
        {
            if(debugMode)
                Logger.log("Responcer do nothing, mocMode^{MocMode}!!!", Serilog.Events.LogEventLevel.Debug,MocMode);
        }

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
        public virtual void Init(Pipeline owner)
        {

        }

        public override string ToString()
        {
            return $"Sender:{this.GetType().Name} Step:{owner.IDStep}";
        }
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
        //  public string IDResponsedReceiverStep = "";
        string MocContent = "";
        object syncro= new object();
        public async Task<string> send(AbstrParser.UniEl root)
        {
            DateTime time1 = DateTime.Now;
            string ans;
            if (!MocMode)
            {
                if (typeContent == TypeContent.internal_list)
                    ans= await sendInternal(root);
                else
                    ans = await send(root.toJSON());
            } else
            {
               // await Task.Delay(10);
                if ((MocBody ?? "") != "")
                    ans = MocBody;
                else
                {
                    if (MocContent == "")
                    {
                        lock (syncro)
                        {
                            if (MocContent == "")
                            {

                                // string ans;
                                using (StreamReader sr = new StreamReader(MocFile))
                                {
                                    MocContent = sr.ReadToEnd();
                                    //       return ans;
                                }
                            }
                        }
                    }
                    ans = MocContent;
                }
            }
            if(owner.debugMode)
                Logger.log(time1, "{Sender} Send:{Request}  ans:{Response}", "JsonSender", Serilog.Events.LogEventLevel.Information, this, root.toJSON(), ans);
            return ans;
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

    public abstract class ConverterOutput
    {
        public abstract AbstrParser.UniEl Convert(string value, AbstrParser.UniEl input_el, AbstrParser.UniEl output_el);
    }

    public abstract class AliasProducer
    {
        public abstract string getAlias(string originalValue);
    }
    public class SensitiveAttribute:Attribute
    {
        public string NameSensitive;
        public SensitiveAttribute(string name)
        {
            NameSensitive = name;   
        }

    }
    [Sensitive("PHONE")]
    public class AliasPhone : AliasProducer
    {
   
        public override string getAlias(string originalValue)
        {
            if (originalValue.Length > 3)
                return $"{originalValue.Substring(0, 2)}*{originalValue.Substring(originalValue.Length-3)}";
            return "*";
        }
    }

    [Sensitive("PAN")]

    public class AliasPan : AliasProducer
    {
 
        public override string getAlias(string originalValue)
        {
            if (originalValue.Length >= 16)
                return $"{originalValue.Substring(0, 4)}***{originalValue.Substring(originalValue.Length - 3)}";
            return "*";
        }
    }

    [Sensitive("FIO")]

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

        public  AbstrParser.UniEl ConvertToNew( AbstrParser.UniEl el)
        {
            var value = el.Value.ToString();
            AbstrParser.UniEl result= new AbstrParser.UniEl() { Name=el.Name};
            if (aliasProducer == null)
            {
                result.Value = hashConverter.hash(value);
                return result;
            }
            else
            {
                var el1 = new AbstrParser.UniEl(result) { Name = "h", Value = hashConverter.hash(value) };
                var el2 = new AbstrParser.UniEl(result) { Name = "a", Value = aliasProducer.getAlias(value) };
                return result;
            }
            //            return JsonSerializer.Serialize<OutItem>(new OutItem() { alias = aliasProducer.getAlias(value), hash = hashConverter.hash(value) });   
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

        public bool viewAsJsonString = false;
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
        public abstract IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl);

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
            if (viewAsJsonString)
                rootEl.packToJsonString = true;
            return rootEl;
        }

        public bool getNodeNameOnly = false;
        public bool returnOnlyFirstRow = true;

        public virtual bool addToOutput(AbstrParser.UniEl inputRoot, ref AbstrParser.UniEl outputRoot)
        {
            // skipped--------------------------- Пока поддерживается только линейная структура записи
            //     if (typeCopy == TypeCopy.Value)
            bool found = false;
            foreach (var el1 in getNodes(inputRoot))
            {
                found = true;
                if (!this.canReturnObject)
                {

                }

                if (el1 == null && onEmptyValueAction == OnEmptyAction.Skip && this.canReturnObject)
                    return false;
                var el = createOutPath(outputRoot);
                //                AbstrParser.UniEl el = new AbstrParser.UniEl(outputRoot);
                //el.Name = outputPath;
                //                if(el.)
                //                if(el1.)
                if (!this.canReturnObject || el1.childs.Count == 0)
                {
                    object elV;
                    if (getNodeNameOnly && el1 != null)
                        elV = el1.Name;
                    else
                    {
                        if (canReturnObject)
                            elV = el1.Value.ToString();
                        else
                            elV = getValue(inputRoot);
                    }
                    if (elV != null)
                    {
                        if (converter != null)
                            el = converter.Convert(elV.ToString(), inputRoot, el);
                        else
                            el.Value = elV;
                    }
                }
                else
                {
                    CopyNode(el1, el);
                    //                    el.childs.Add(el1.copy(el));
                }

                /*   else
                   {
                       var el = createOutPath(outputRoot);

                       el.childs.Add(inputRoot.copy(outputRoot));
                   }*/
                if (returnOnlyFirstRow)
                    return true;
            }
            return found;
        }

        protected virtual void CopyNode(AbstrParser.UniEl el1, AbstrParser.UniEl el)
        {
            el1.copy(el);
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
        public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
        {
            return new AbstrParser.UniEl[] { null };
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
        public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
        {
            return new AbstrParser.UniEl[] { null };
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

        public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
        {
            return new AbstrParser.UniEl[] { null };
        }
    }

    public class ExtractFromInputValue : OutputValue
    {

        protected override void CopyNode(AbstrParser.UniEl el1, AbstrParser.UniEl el)
        {
            if(this.copyChildsOnly)
            {
                foreach (var it in el1.childs)
                    base.CopyNode(it, el);
            }
            else
            base.CopyNode(el1, el);
        }
        public override string ToString()
        {
            return outputPath + "; from " + conditionPath;
        }
        public bool copyChildsOnly = false;


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
            while (index >= 0 && AbstrParser.isEqual(item1.Name ,nodes[index]))
            {
                item1 = item1.ancestor;
                index--;

            }

            return item1;
        }


        public virtual AbstrParser.UniEl getFinalNode(AbstrParser.UniEl node)
        {
            return node;
        }

        public override IEnumerable<AbstrParser.UniEl> getNodes(AbstrParser.UniEl rootEl)
        {
            if (conditionPathToken == null)
                conditionPathToken = conditionPath.Split("/");

            var rootEl1 = AbstrParser.getLocalRoot(rootEl, conditionPathToken);

            foreach (var item in rootEl1.getAllDescentants(conditionPathToken, rootEl1.rootIndex).Where(ii => ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
            {
                var item1 = item;
                if (valuePath != "")
                {
                    if (valuePathToken == null)
                        valuePathToken = valuePath.Split("/");
                    item1 = AbstrParser.getLocalRoot(item1, valuePathToken);
                    foreach (var item2 in item1.getAllDescentants(valuePathToken, item1.rootIndex))
                        yield return getFinalNode(item2);
                }
                else
                    yield return getFinalNode(item);

            }

        }

        public override AbstrParser.UniEl getNode(AbstrParser.UniEl rootEl)
        {
            if (ConditionFilter.isNew)
            {
                if (conditionPathToken == null)
                    conditionPathToken = conditionPath.Split("/");

                var rootEl1 = AbstrParser.getLocalRoot(rootEl, conditionPathToken);

                foreach (var item in rootEl1.getAllDescentants(conditionPathToken, rootEl1.rootIndex).Where(ii => ((conditionCalcer == null) ? true : conditionCalcer.Compare(ii))))
                {
                    var item1 = item;
                    if (valuePath != "")
                    {
                        if (valuePathToken == null)
                            valuePathToken = valuePath.Split("/");
                        item1 = AbstrParser.getLocalRoot(item1, valuePathToken);
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
                var ll = rootElement.toJSON();
            }
        }
        public async Task run()
        {

            if (receiver != null)
                receiver.owner = this;
            if (sender != null)
                sender.owner = this;
            receiver.stringReceived = Receiver_stringReceived;
            try
            {
                await receiver.start();
            }
            catch (Exception e88)
            {
                throw;
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
                if (local_rootOutput == null)
                    local_rootOutput = new AbstrParser.UniEl() { Name = "root" };

                foreach (var ff in outputFields)
                {
                    if (ff.addToOutput(rootElInput, ref local_rootOutput))
                        count++;
                }
                return count;
            }

        }
        public List<ItemFilter> converters { get; set; } = new List<ItemFilter>() { };
        /*        public List<Filter> filters = new List<Filter> { new ConditionFilter() };
                public List<OutputValue> outputFields = new List<OutputValue> { new ConstantValue() { outputPath = "stream", Value = "CheckRegistration" }, new ExtractFromInputValue() { outputPath = "IP", conditionPath = "aa/bb/cc", conditionCalcer = new ComparerForValue() { value_for_compare = "tutu" }, valuePath = "cc/dd" } };*/
        //        public RecordExtractor transformer;
        public Sender sender { get; set; } = new LongLifeRepositorySender();
        [YamlIgnore]
        public Pipeline owner { get; set; }

        public void Init(Pipeline owner)
        {
            this.owner = owner;
            this.sender?.Init(owner);
            this.receiver?.Init(owner);
            if (!string.IsNullOrEmpty(this.SaveErrorSendDirectory))
            {
                var moveDir = Path.Combine(this.SaveErrorSendDirectory, "Move");
                if (Directory.Exists(moveDir))
                {
                    foreach(var file in Directory.GetFiles(moveDir))
                    {
                        try
                        {
                            File.Move(file, Path.Combine(SaveErrorSendDirectory, Path.GetFileName(file)));
                        }
                        catch
                        {

                        }

                    }


                }
                if (Directory.GetFiles(this.SaveErrorSendDirectory).Count() > 0)
                {
                    isErrorSending = true;
                    tRestore = restoreSenderState(SaveErrorSendDirectory);
                }
            }
        }
        public Step(/*Pipeline owner1*/)
        {
            //            owner = owner1;
            /*  sucMetric = Pipeline.metrics.getMetric("packagesReceived", false, true, "All packages, sended to utility");
              errMetric = Pipeline.metrics.getMetric("packagesReceived", true, false, "All packages, sended to utility");*/
        }

        static Metrics.MetricCount sucMetric = new Metrics.MetricCount("packagesReceivedSuccess", "All packages, sended to utility");
        static Metrics.MetricCount errMetric = new Metrics.MetricCount("packagesReceivedUnsuccess", "All packages, sended to utility with error");
        //   LongLifeRepositorySender repo = new LongLifeRepositorySender();

        public class ContextItem
        {
            public List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            public object context;
        }
        public bool isBridge = false;
        private async Task Receiver_stringReceived(string input, object context)
        {

            DateTime time2 = DateTime.Now;
            ContextItem contextItem = new ContextItem() { context = context };
            //            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
            var rootElement = AbstrParser.CreateNode(null, contextItem.list, this.IDStep);
            rootElement = AbstrParser.CreateNode(rootElement, contextItem.list, "Rec");
            owner.lastExecutedEl = rootElement;
            if (!isBridge)
            {
                await FilterInfo(input, time2, contextItem, rootElement);

                await checkChilds(contextItem, rootElement);
            }
            else
            {
                try
                {
                    /*if(sender?.GetType() == typeof(HTTPSender) && receiver.GetType() == typeof(HTTPReceiver))
                    {
                        (sender as HTTPSender).headers = (context as HTTPReceiver.SyncroItem).headers;
                        
                    }*/
                    var ans = await sender?.send(input);
                    await receiver.sendResponse(ans, context);
                }
                catch (Exception e66)
                {
                    Logger.log($"On send error{e66.ToString()}", Serilog.Events.LogEventLevel.Error);
                    throw;
                }
            }

            contextItem.list.Clear();
            contextItem = null;

            rootElement = null;
        }

        private async Task checkChilds(ContextItem contextItem, AbstrParser.UniEl rootElement)
        {

            foreach (var nextStep in this.owner.steps.Where(ii => ii.IDPreviousStep == this.IDStep))
            {
                await nextStep.FilterStep(contextItem, rootElement);
                await nextStep.checkChilds(contextItem, rootElement);
            }
        }
        public override string ToString()
        {
            return $"Step:{this.IDStep}";
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
            {

                Logger.log("{this} {filter} transform to output {count} items", Serilog.Events.LogEventLevel.Debug, this, item, count);
            }
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
                                AbstrParser.UniEl rEl = null;
                                foreach (var item1 in item.filter.filter(list, ref rEl))
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
            bool cantTryParse = false;
            if (receiver != null)
                cantTryParse = receiver.cantTryParse;
            foreach (var pars in AbstrParser.availParser)
                if (pars.canRazbor(input, rootElement, context.list, cantTryParse))
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

                if (tryParse(input, context, rootElement))
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
                // throw new Exception("aaa");
            }
            catch (Exception e77)
            {
                if (errMetric != null)
                    errMetric.Add(time2);
                Logger.log(e77.ToString(), Serilog.Events.LogEventLevel.Error);
                rootElement = null;
                throw;
            }
        }

        public bool isHandleSenderError = false;

        public string SaveErrorSendDirectory = "";
        Task tRestore;
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
                    AbstrParser.UniEl rEl = null;
                    if (this.IDStep == "Step_3")
                    {
                        int y = 0;
                    }
                    foreach (var item1 in item.filter.filter(context.list, ref rEl))
                        found = await FindAndCopy(rootElement, time1, item, item1, context, local_rootOutput);
                    //                    found = true;
                }
                if (found)
                {
                    if (rootElement?.ancestor?.Name != this.IDStep)
                    {
                        var root = CheckAndFillNode(rootElement, IDStep, true);// new AbstrParser.UniEl(rootElement.ancestor) { Name = IDStep };
                        rootElement = CheckAndFillNode(root, "Rec");
                    }
                    try
                    {
                       if (!isErrorSending)
                            await SendToSender(rootElement, context, local_rootOutput);
                       else
                            SaveRestoreFile(local_rootOutput);

                        new AbstrParser.UniEl(rootElement.ancestor) { Name = "SendErrorCode", Value = 0 };
                    }
                    catch (Exception e77)
                    {
                        if (isHandleSenderError)
                        {
                            Logger.log("ErrorSender:" + e77.ToString(), Serilog.Events.LogEventLevel.Error);
                           
                            new AbstrParser.UniEl(rootElement.ancestor) { Name = "SendErrorCode", Value = 1 };
                            if (!string.IsNullOrEmpty(SaveErrorSendDirectory))
                            {
                                SaveRestoreFile(local_rootOutput); 
                                if (!isErrorSending)
                                    tRestore = restoreSenderState(SaveErrorSendDirectory);
                                isErrorSending = true;
                            }

                        }
                        else
                            throw;

                    }
                }


            }
        }

        private void SaveRestoreFile(AbstrParser.UniEl local_rootOutput)
        {
            using (var sw = new StreamWriter(Path.Combine(SaveErrorSendDirectory, Path.GetFileName(Path.GetRandomFileName()))))
                sw.Write(local_rootOutput.toJSON());
        }

        bool isErrorSending = false;
        async Task restoreSenderState(string Dir)
        {

            var moveDir = Path.Combine(Dir, "Move");
            if (!Directory.Exists(moveDir))
                Directory.CreateDirectory(moveDir);

            bool found = false;
            do
            {
                found = false;
                foreach (var file in Directory.GetFiles(Dir).OrderBy(ii => File.GetCreationTime(ii)))
                {
                    var file1 = Path.Combine(moveDir, Path.GetFileName(file));
                    File.Move(file, file1);
                    List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
                    string line = "";
                    using (StreamReader sr = new StreamReader(file1))
                    {
                        line = sr.ReadToEnd();
                    }
                    AbstrParser.UniEl rootEl = AbstrParser.CreateNode(null, list, "Item");
                    if (line != "")
                    {
                        foreach (var pars in AbstrParser.availParser)
                            if (pars.canRazbor(line, rootEl, list))
                                break;
                    }

                restart:
                    try
                    {
                        await sender.send(rootEl);
                    }
                    catch (Exception e77)
                    {
                        await Task.Delay(1000);
                        goto restart;

                    }
                    try
                    {
                        File.Delete(file1);
                    }
                    catch { }
                }
                if(!found && isErrorSending)
                {
                    isErrorSending = false;
                    await Task.Delay(100);
                    found = true;    
                }
            } while (found);
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
            if (debugMode )
                Logger.log("{this} {filter} transform to output  added {count} items, filt:{l} out:{out}", Serilog.Events.LogEventLevel.Debug,this,item, count,el.toJSON(),local_rootOutput.toJSON());
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
                if (debugMode)
                    Logger.log("Send answer initializer {step}  ", Serilog.Events.LogEventLevel.Debug, "any", this);

                var step = this.owner.steps.FirstOrDefault(ii => ii.IDStep == IDResponsedReceiverStep);
                string content;
                if(sender == null)
                    content = local_rootOutput.toJSON();
                else
                    content= await sender.send(local_rootOutput);
                await step.receiver.sendResponse(content, context.context);

            }
            else
            {
                var sendNode =CheckAndFillNode(rootElInput,"Send",true);
                var toNode = CheckAndFillNode(sendNode, "To");
                var ans = await sender.send(local_rootOutput);
                foreach (var node in local_rootOutput.childs)
                {
                    node.ancestor = toNode;
          //          toNode.childs.Add(node);
                }
                recordSendCount++;
                // bool isSave = false;
                if (ans != "")
                {
               /*     var send1 = sender as ResponseSender;
                    if (send1 != null)
                    {
                        if (sender.IDResponsedReceiverStep != null && sender.IDResponsedReceiverStep != "")
                        {
                            var step = this.owner.steps.FirstOrDefault(ii => ii.IDStep == sender.IDResponsedReceiverStep);
                            await step.receiver.sendResponse(ans, context.context);

                        }
                    }*/
                    // Swap the statements
                    if (this.owner.steps.Count(ii => ii.IDPreviousStep == this.IDStep) > 0)
                    {
                        var nextStep = this.owner.steps.First(ii => ii.IDPreviousStep == this.IDStep);
//                        tryParse(ans, context, CheckAndFillNode(sendNode, "From"));
                        var newRoot = new AbstrParser.UniEl(rootElInput.ancestor) { Name = nextStep.IDStep };
                        newRoot = new AbstrParser.UniEl(newRoot) { Name = "Rec" };



                        nextStep.tryParse(ans, context, newRoot);
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


    public  class HTTPSender:Sender,ISelfTested
    {
        [YamlIgnore]
        HttpClient client;
        public string certName = "";
        public string certPassword = "";
//        public string certThumbprint= "E77587679318FED87BB040F00D76AB461B962D95";
        public List<string> certThumbprints = new List<string> { "A77587679318FED87BB040F00D76AB461B962D95" };
        public double timeoutSendInSeconds = 5;
        public List<KestrelServer.Header> headers;
        public HTTPSender()
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


        public string ResponseType = "application/json";
        bool init = false;
        object syncro = new object();

        public override async Task<string> send(string JsonBody)
        {
            return await  internSend(JsonBody);
        }
        public  async Task<string> internSend(string body)
        {
            if(!init)
            {
                lock (syncro)
                {
                    if (!init)
                    {
                        InitClient();
                        init = true;
                    }
                }
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
            var stringContent = new StringContent(body, UnicodeEncoding.UTF8, ResponseType); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            if (0 == 1)
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Pragma", "no-cache");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "no-cache");
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Java/12.0.1");

                client.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            }
            //stringContent.Headers
            //            client.DefaultRequestHeaders
            //            "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2"
            //          client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue)= "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2"
            //        stringContent.Headers.Add("Accept", "text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2");
            //stringContent.Headers.
            /*if(headers!= null)
            {
                foreach (var head in headers.Where(ii => ii.Key != "SOAPAction" && ii.Key != "Connection"))
                {
                    try
                    {
                        stringContent.Headers.Add(head.Key, head.Value);
                    }
                    catch(Exception e77)
                    {

                    }
                }
            }*/
            //stringContent.Headers.Add()
            try
            {
                result = await client.PostAsync(urls[index], stringContent);
                if(!result.IsSuccessStatusCode)
                {
                    Logger.log("Error send http request {res}", Serilog.Events.LogEventLevel.Error,result.StatusCode.ToString());

                }

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
                IEnumerable<string> values;
                //string rules = "";
                if (result.Headers.TryGetValues("InactiveRules", out values))
                {
                    response=response.Substring(0,response.Length - 1)+((response.Length>2)?",":"")+"{\"Inactive\":["+string.Join(",",values.First().Split(";").Select(ii=>$"\"{ii}\""))+"]}]";
                    //rules = values.First();
                }
                //if (result.Headers["AAA"])
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
        static Metrics.MetricCount sendToRex = new Metrics.MetricCount("sendToRexSuc",  "Sended transactions to CCFA time exec"); 
        static Metrics.MetricCount sendToRexErr = new Metrics.MetricCount("sendToRexErr", "Sended transactions to CCFA with error");
        //static Metrics.MetricCount metricExecRex = new Metrics.MetricCount("RexTimeExecution", "Rex time execution");
        protected string getVal(AbstrParser.UniEl el)
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
            string str = formBody(root);
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

                //                Logger.log(time1, "{Sender} Send:{Request}  ans:{Response}", "JsonSender", Serilog.Events.LogEventLevel.Information,this, str, ans);
                //              createMetrics();
                if (sendToRex != null)
                    sendToRex.Add(time1);
                return ans;
            }
            catch (Exception e77)
            {
                //                createMetrics();
                if (sendToRex != null)
                    sendToRexErr.Add(time1);
                throw;
            }
        }

        protected virtual string formBody(AbstrParser.UniEl root)
        {
            if (ResponseType == "application/xml" || ResponseType == "text/xml")
                return root.childs[0].toXML();
            else
                return root.toJSON();
        }

        private void createMetrics()
        {
           /* if (sendToRex == null)
            {
                sendToRex = Pipeline.metrics.getMetric("sendToRex", false, true, "Sended transactions to CCFA");
                sendToRexErr = Pipeline.metrics.getMetric("sendToRex", true, false, "Sended transactions to CCFA");
            }*/
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
                Logger.log(time1,"{Sender}-Send:{ans}" ,"SelfTest",Serilog.Events.LogEventLevel.Information,this,ans);
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

    [Annotation("Пустой Sender")]
    public class NullSender:Sender
    {
        public override TypeContent typeContent => TypeContent.internal_list;
     //   public string IDResponsedReceiverStep = "";

    }

    public class StreamSender:HTTPSender
    {
        public static TimeSpan Interval; 
        public string streamName  = "checkRegistration5";
        public static long countOpenRexRequest = 0;

        public static Metrics.MetricCount metricStreamConcurrent = new Metrics.MetricCount("StreamConcurrCount", "Some time concurrent count");
        public static Metrics.MetricCount metricPerformanceStreams = new Metrics.MetricCount("StreamTime", "Stream peformance time");
        string getVal1(AbstrParser.UniEl el,Stream stream)
        {
            var conv = stream.fields[el.Name].SensitiveConverter;
            if (conv != null)
                return getVal(conv.ConvertToNew(el));
            else
                return getVal(el);

        }
        protected override string formBody(AbstrParser.UniEl root)
        {
            string currentStream = streamName;
            //         getStream()
            var it = root.childs.FirstOrDefault(ii => ii.Name == "stream");
            if (it != null)
                currentStream = it.Value.ToString();

            var stream = getStream(currentStream);
            string str = "{";
            str = "{" + String.Join(",", root.childs.Select(ii => $"\"{ii.Name}\":{getVal1(ii,stream)}")) + "}";
            return str;
        }

        public async override Task<string> sendInternal(AbstrParser.UniEl root)
        {
            metricStreamConcurrent.Increment();
            Interlocked.Increment(ref countOpenRexRequest);
            string currentStream = streamName;
   //         getStream()
            var it=root.childs.FirstOrDefault(ii => ii.Name == "stream");
            if (it == null)
            {
                root.childs.Add(new AbstrParser.UniEl() { Name = "stream", Value = streamName });
            } else
                currentStream = it.Value.ToString();
           
         //   foreach(var item in stream.fields.Where(ii => ii.SensitiveData.IsNotEmpty()))

            DateTime time1=DateTime.Now;
            var ret= await base.sendInternal(root);
            Interval+=(DateTime.Now-time1); 
            Interlocked.Decrement(ref countOpenRexRequest);
            metricStreamConcurrent.Decrement();
            metricPerformanceStreams.Add(time1);
            return ret;
        }

        public class Stream
        {
            public class Field
            {
                public string Name { get; set; } = "";
                public string Type { get; set; }
                public string Detail { get; set; }
                public string SensitiveData { get; set; }
                HashOutput converter = null;
                public HashOutput SensitiveConverter
                {
                    get
                    {
                        if(!String.IsNullOrEmpty(SensitiveData) && converter==null)
                        {
                            Type typeProducer= Assembly.GetAssembly(typeof(AliasProducer)).GetTypes().First(t => t.IsAssignableTo(typeof(AliasProducer)) && !t.IsAbstract && t.CustomAttributes.Count(ii => ii.AttributeType == typeof(SensitiveAttribute) && ii.ConstructorArguments[0].Value.ToString()== SensitiveData) >0);
                                converter = new HashOutput() { hashConverter = new CryptoHash(), aliasProducer = Activator.CreateInstance(typeProducer) as AliasProducer };
                        }
                        return converter;
                    }
                }
                
                //converter = new HashOutput() { hashConverter = new CryptoHash(), aliasProducer = Activator.CreateInstance(comboBoxTypeAlias.SelectedItem as Type) as AliasProducer };

                public long? linkedColumn { get; set; }
            }
            public string Name { get; set; }
            public string Description { get; set; }
            public Dictionary<string,Field> fields { get; set; } = new Dictionary<string,Field>();
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
            using (var cmd = new NpgsqlCommand(@"select n.nodeid,n.name,a.val,np.name,'String',ap.val,asd.val,rl.toid from md_node n
inner join md_node_attr_val a  
on ( a.nodeid=n.nodeid and attrid=22)
inner join md_arc l on (l.toid=n.nodeid and l.isdeleted=false)
inner join md_node np on (l.fromid=np.nodeid and np.isdeleted=false)
inner join md_node_attr_val ap on ( ap.nodeid=np.nodeid and ap.attrid=22)
left join md_node_attr_val asd on ( asd.nodeid=np.nodeid and asd.attrid=51)
left join md_arc rl on ( rl.fromid=np.nodeid and rl.typeid=16)
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
                        stream.fields.Add(reader.GetString(3), new Stream.Field() { Name = reader.GetString(3), Type = reader.GetString(4), Detail =reader.IsDBNull(5)?"": reader.GetString(5),SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6), linkedColumn = reader.IsDBNull(7) ? null : reader.GetInt64(7) });
                    }
                }
            }

            conn.Close();
            if(stream.Name =="")
            {
                stream.Name = key;
                stream.fields.Add("stream",new Stream.Field() { Name = "stream", Detail = "Name of stream", Type = "String" });
                stream.fields.Add("originalTime",new Stream.Field() { Name = "originalTime", Detail = "Time of stream", Type = "DateTime" });
            }
            stream.fields.Where(ii => !string.IsNullOrEmpty(ii.Value.SensitiveData)).Select(ii1 => ii1.Value.SensitiveConverter);

            streams.TryAdd(key, stream);
            return (stream);
        }

        public override void Init(Pipeline owner)
        {
            getStream(streamName);
            base.Init(owner);
        }
        private static string formJson(Stream stream)
        {
            return "{" + string.Join<string>(",", stream.fields.Select(ii => $"\"{ii.Value.Name}\":\"{((ii.Value.Name=="stream")?stream.Name:"")}\"")) + "}";
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
