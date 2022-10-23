using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ParserLibrary;

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