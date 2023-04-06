using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using PluginBase;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace ParserLibrary;
/// <summary>
/// Step is a sequence of steps that includes (optionally) Sender, Receiver and a set of filters for selecting information
/// </summary>
public partial class Step
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

        if (sender != null)
            sender.owner = this;
        if (receiver != null)
        {
            await _receiverHost.start();
        }
    }
    public string IDStep { get; set; } = "Example";


    public string IDPreviousStep { get; set; } = "";

    public string IDResponsedReceiverStep { get; set; } = "";


    public string description { get; set; } = "Some comments in this place";
    
    [YamlIgnore]
    public bool debugMode
    {
        get => this._debugMode;
        set
        {
            this._debugMode = value;
            if (this.receiver != null)
                this.receiver.debugMode = value;
        }
    }
    private bool _debugMode = true;

    // The actual receiver object as specified in the pipeline definition file
    public IReceiver receiver
    {
        get => this._receiverHost?.Receiver;
        set
        {
            if (this._receiverHost != null)
                this._receiverHost.Release();
            this._receiverHost = new ReceiverHost(this, value);
            this._receiverHost.Init(owner);
        }
    }
    private ReceiverHost _receiverHost;

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

        if (this.receiver != null)
        {
            _receiverHost = new ReceiverHost(this, receiver);
            _receiverHost.Init(owner);
        }
        
        if (!string.IsNullOrEmpty(this.SaveErrorSendDirectory))
        {
            // Ensure the save error directory exists
            Directory.CreateDirectory(this.SaveErrorSendDirectory);
                
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
            var files = Directory.GetFiles(this.SaveErrorSendDirectory);
            if (files.Count() > 0)
            {
                SizeDirectory=files.Select(ii => new FileInfo(ii).Length).Sum();

               // Directory.GetF
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
    static Metrics.MetricCount errMetricRetry = new Metrics.MetricCount("packagesSendedUnsucRetry", "All packages, resended sucessfully after error");
    static Metrics.MetricHistogram metricRetryTimeError = new Metrics.MetricHistogram("retryPackagesTime", "retry time on error", new double[] {  100, 500, 1000, 5000, 10000 ,30000,60000,600000});

    //   LongLifeRepositorySender repo = new LongLifeRepositorySender();

    public class ContextItem
    {
        public List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
        public object context;
        public Activity mainActivity;
        public Scenario currentScenario = null;
    }
    public bool isBridge = false;

    public static bool saveAllResponses = false;


    Task saveScenarious = null;
    private async Task Receiver_stringReceived(string input, object context)
    {
//        owner.mainActivity = owner.GetActivity("receive package", null);
        DateTime time2 = DateTime.Now;
        ContextItem contextItem = new ContextItem() { context = context ,mainActivity= owner.GetActivity("receive package", null) };
        if (contextItem?.mainActivity != null)
        {
            contextItem?.mainActivity?.SetTag("context.url", owner.SaveContext(input));
        }
        //            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
        var rootElement = AbstrParser.CreateNode(null, contextItem.list, this.IDStep);
        rootElement = AbstrParser.CreateNode(rootElement, contextItem.list, "Rec");
        if(saveAllResponses)
        {
            contextItem.currentScenario = new Scenario() { Description = $"new Scenario on {DateTime.Now}", mocs = new List<Scenario.Item>() };
            contextItem.currentScenario.mocs.Add(new Scenario.Item() { IDStep = this.IDStep, isMocReceiverEnabled = true, MocFileReceiver = input });
        }
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
                var ans = await sender?.send(input,contextItem);

                await _receiverHost.sendResponse(ans, context);
                if(saveAllResponses)
                {
                    Scenario.Item item= contextItem.currentScenario.mocs.FirstOrDefault(ii=>ii.IDStep== this.IDStep);
                    if (item == null)
                    {
                        item = new Scenario.Item() { IDStep = this.IDStep, isMocSenderEnabled = true, MocFileSender = ans };
                        contextItem.currentScenario.mocs.Add(item);
                    }
                    else
                    {
                        item.isMocSenderEnabled = true;
                        item.MocFileSender = ans;   
                    }
                    this.owner.scenarios.Enqueue(contextItem.currentScenario);
                    if (saveScenarious == null)
                        saveScenarious = Task.Run( () => 
                        {
                            if(!Directory.Exists(SaveScenariousDirectory))
                                Directory.CreateDirectory(SaveScenariousDirectory);
                            Scenario scenario;
                            while (0 == 0)
                            {
                                while (owner.scenarios.TryDequeue(out scenario))
                                {
                                    string fileName = Path.Combine(SaveScenariousDirectory, Path.GetFileName(Path.GetRandomFileName()));
                                    using (StreamWriter sw = new StreamWriter(fileName))
                                    {
                                        sw.Write(JsonSerializer.Serialize<Scenario>(scenario));
                                    }


                                }
                                Thread.Sleep(100);
                            }

                    //        this.SizeDirectory
                        }
                        );

                }
            }
            catch (Exception e66)
            {
                contextItem?.mainActivity?.SetTag("pipelineError", "true");
                contextItem ? .mainActivity?.Stop();
                contextItem?.mainActivity?.Dispose();
                Logger.log($"On send error{e66.ToString()}", Serilog.Events.LogEventLevel.Error);
                throw;
            }
        }
        foreach(var mb in owner.metricsBuilder)
            await mb.Fill(rootElement); 
        contextItem.list.Clear();
      //  owner.mainActivity?.SetTag("pipelineError", "true");
        contextItem?.mainActivity?.Stop();
        contextItem?.mainActivity?.Dispose();
        //contextItem?.mainActivity = null;
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
    private async Task<string> FindAndCopy1(AbstrParser.UniEl rootElInput, DateTime time1, ItemFilter item, AbstrParser.UniEl el, List<AbstrParser.UniEl> list,Step.ContextItem context)
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
            return await sender.send(local_rootOutput,context);
    }

    public async Task<string> FilterInfo1(string input, DateTime time2, List<AbstrParser.UniEl> list, AbstrParser.UniEl rootElement,Step.ContextItem context)
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
                                var st = await FindAndCopy1(rootElement, time1, item, item1, list,context);
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

    /// <summary>
    /// Tries available parsers one by one to parse input string
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <param name="rootElement"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Maximum amount of stored data, what sender unreacheble in mB. 
    /// </summary>
    /// <remarks>
    /// If SaveErrorSendDirectory is shared by multiple program instances, the total amount available must be multiplied by the number of instances.
    /// </remarks>
    public Int64 maxSavedLimitInMB = 0;
    public string SaveErrorSendDirectory = "";
    public string SaveScenariousDirectory = "C:\\d\\Scenarious\\";
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

    string musor = "qwertybbvgghhnbbbjkkll988765433222345556gggbgggghhhbbbbn";
    [YamlIgnore]
    Int64 SizeDirectory = 0;
    [YamlIgnore]
    bool nonSavedError = false;
    private void SaveRestoreFile(AbstrParser.UniEl local_rootOutput)
    {
        if(nonSavedError) 
            return;

        string fileName = Path.Combine(SaveErrorSendDirectory, Path.GetFileName(Path.GetRandomFileName()));
        using (AesManaged aes = new AesManaged())
        {
            // Create encryptor    
            ICryptoTransform encryptor = aes.CreateEncryptor(owner.key,owner.IV);
            // Create MemoryStream    
            using (FileStream ms = new FileStream(fileName,FileMode.CreateNew))
            {
                // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                // to encrypt    
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    // Create StreamWriter and write data to a stream    
                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(local_rootOutput.toJSON());
                  //  encrypted = ms.ToArray();
                }
            }
            FileInfo fi = new FileInfo(fileName);
            Interlocked.Add(ref SizeDirectory, (fi.Length));
            if(SizeDirectory/(1024*1024)>= maxSavedLimitInMB) 
            {
                nonSavedError = true;
                Logger.log($"Size of directory {SaveErrorSendDirectory} exceed {maxSavedLimitInMB} MB , restored impossible .All saved dataErased.");
                foreach (var file in Directory.GetFiles(this.SaveErrorSendDirectory))
                {
                    try
                    {

                        File.Delete(file);
                    }
                    catch
                    { }
                }
            }

//            SizeDirectoryInMB += (fi.Length * 1024 * 1024);
        }
        /*    using (var sw = new StreamWriter(Path.Combine(SaveErrorSendDirectory, Path.GetFileName(Path.GetRandomFileName()))))
                sw.Write(local_rootOutput.toJSON());*/
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
                using (AesManaged aes = new AesManaged())
                {
                    // Create a decryptor    
                    ICryptoTransform decryptor = aes.CreateDecryptor(owner.key, owner.IV);
                    // Create the streams used for decryption.    
                    using (FileStream sr = new FileStream(file1,FileMode.Open))
                    {
                        // Create crypto stream    
                        using (CryptoStream cs = new CryptoStream(sr, decryptor, CryptoStreamMode.Read))
                        {
                            // Read crypto stream    
                            using (StreamReader reader = new StreamReader(cs))
                                line = reader.ReadToEnd();
                        }
                    }
                }

              /*  using (StreamReader sr = new StreamReader(file1))
                {
                    line = sr.ReadToEnd();
                }*/
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
                    await sender.send(rootEl,null);
                    errMetricRetry.Increment();
                    
                }
                catch (Exception e77)
                {
                    await Task.Delay(1000);
                    if (nonSavedError)
                    {
                        try
                        {
                            var time=new FileInfo(file1).CreationTime;
                            File.Delete(file1);
                            metricRetryTimeError.Add(time);
                        }
                        catch { }

                        return;
                    }
                    goto restart;

                }
                try
                {
                    FileInfo fi = new FileInfo(file1);
                    File.Delete(file1);
                    Interlocked.Add(ref SizeDirectory,-(fi.Length));
                    //SizeDirectoryInMB -= (fi.Length * 1024 * 1024);
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
    [YamlIgnore]
    public int recordSendCount = 0;
    private async Task SendToSender(AbstrParser.UniEl rootElInput, ContextItem context, AbstrParser.UniEl local_rootOutput)
    {
        // Save sender context
        //rootElInput.
        var sendNode = CheckAndFillNode(rootElInput, "Send", true);
        var toNode = CheckAndFillNode(sendNode, "To");
        if (IDResponsedReceiverStep != "")
        {
            if (debugMode)
                Logger.log("Send answer initializer {step}  ", Serilog.Events.LogEventLevel.Debug, "any", this);

            var step = this.owner.steps.FirstOrDefault(ii => ii.IDStep == IDResponsedReceiverStep);
            string content;
            if(sender == null)
                content = local_rootOutput.toJSON();
            else
                content= await sender.send(local_rootOutput,context);
            await step.receiver.sendResponse(content, context.context);
            foreach (var node in local_rootOutput.childs)
            {
                node.ancestor = toNode;
                //          toNode.childs.Add(node);
            }
            StoreAnswer("Resp", rootElInput, context, content, step);

        }
        else
        {
            var ans = await sender.send(local_rootOutput,context);
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
                    StoreAnswer("Rec",rootElInput, context, ans, nextStep);
                }


                /*                    if (step != null)
                                        {
                                            var newRoot = new AbstrParser.UniEl(rootElInput) { Name = "SendResponse" };
                                            await step.FilterInfo(ans, time1, list, newRoot);
                                        }*/
            }
        }
    }

    private static void StoreAnswer(string name_ans,AbstrParser.UniEl rootElInput, ContextItem context, string ans, Step nextStep)
    {

        if(nextStep.IDStep.Contains("ToTWO"))
        {
            int yy = 0;
        }
        var newRoot = new AbstrParser.UniEl(rootElInput.ancestor) { Name = nextStep.IDStep };
        newRoot = new AbstrParser.UniEl(newRoot) { Name = name_ans };



        nextStep.tryParse(ans, context, newRoot);
    }

    private static AbstrParser.UniEl CheckAndFillNode(AbstrParser.UniEl rootElInput,string Name,bool getAncestor =false)
    {
        if(Name=="Step_ToTWO")
        {
            int yy = 0;
        }
        AbstrParser.UniEl contextNode = (getAncestor?(rootElInput.ancestor): rootElInput).childs.FirstOrDefault(ii => ii.Name == Name);
        if (contextNode == null)
            contextNode = new AbstrParser.UniEl((getAncestor ? (rootElInput.ancestor) : rootElInput)) { Name = Name};
        return contextNode;
    }
}