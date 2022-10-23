using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet;
using YamlDotNet.Serialization;
using CSScriptLib;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.Json;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using YamlDotNet.Core.Tokens;
using Microsoft.AspNetCore.Http.Metadata;
using CSScripting;
using System.Runtime.InteropServices;

namespace ParserLibrary
{
    public interface ISelfTested
    {
        Task<(bool,string,Exception)> isOK();
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

    public class SensitiveAttribute:Attribute
    {
        public string NameSensitive;
        public SensitiveAttribute(string name)
        {
            NameSensitive = name;   
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
}
