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
using UniElLib;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;
using DotLiquid;
using System.Xml.Linq;
using static ParserLibrary.Pipeline;
using System.Reflection.Emit;

namespace ParserLibrary;
/// <summary>
/// Step is a sequence of steps that includes (optionally) Sender, Receiver and a set of filters for selecting information
/// </summary>
public partial class Step : ILiquidizable
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
                AbstrParser.getApropriateParser("", input, rootElement, list);
/*                foreach (var pars in AbstrParser.availParser)
                    if (pars.canRazbor(input, rootElement, list))
                    {

                    }*/
            }
            catch
            {

            }
            var ll = rootElement.toJSON();
        }
    }
    public async Task run()
    {
        if (ireceiver != null)
            await _receiverHost.start();
        //New ***
        if (sender != null)
            sender.owner = this;
        if (receiver != null)
        {
            receiver.owner = this;
            receiver.stringReceived = Receiver_stringReceived;
            await receiver.start();
        }
        //New ***
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
            if (this.ireceiver != null)
                this.ireceiver.debugMode = value;
            if (this.receiver != null)
                this.receiver.debugMode = value;
        }
    }
    private bool _debugMode = false;

    // The actual receiver object as specified in the pipeline definition file
    public Receiver receiver { get; set; }
    public IReceiver ireceiver
    {
        get => this._receiverHost?.Receiver;
        set
        {
            if (this._receiverHost != null)
                this._receiverHost.Release();
            if (value != null)
                this._receiverHost = new ReceiverHost(this, value);
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
        public int exec(AbstrParser.UniEl rootElInput, ref AbstrParser.UniEl local_rootOutput, ContextItem context)
        {
            int count = 0;
            if (local_rootOutput == null)
                local_rootOutput = new AbstrParser.UniEl() { Name = "root" };

            foreach (var ff in outputFields)
            {
                if(ff.outputPath=="browserIP")
                {
                    int yy = 0;
                }
                if (ff.addToOutput(rootElInput, ref local_rootOutput,context))
                    count++;
            }
            return count;
        }

    }
    public List<ItemFilter> converters { get; set; } = new List<ItemFilter>() { };
    /*        public List<Filter> filters = new List<Filter> { new ConditionFilter() };
                public List<OutputValue> outputFields = new List<OutputValue> { new ConstantValue() { outputPath = "stream", Value = "CheckRegistration" }, new ExtractFromInputValue() { outputPath = "IP", conditionPath = "aa/bb/cc", conditionCalcer = new ComparerForValue() { value_for_compare = "tutu" }, valuePath = "cc/dd" } };*/
    //        public RecordExtractor transformer;
    // The actual sender object whose class is specified in the pipeline definition file
    public Sender sender { get; set; }  
    public ISender isender
    {
        get => this._senderHost?.Sender;
        set
        {
            if (this._senderHost != null)
                this._senderHost.Release();
            if (value != null)
                this._senderHost = new SenderHost(this, value);
        }
    }
    
    private SenderHost _senderHost;

    [YamlIgnore]
    public Pipeline owner { get; set; }
    [YamlIgnore]
    public int countDelayMessages = 0;
    public void Init(Pipeline owner)
    {
        this.owner = owner;
        
        _receiverHost?.Init(owner);
        if (receiver != null)
        {
            receiver.owner = this;
            receiver.Init(owner);
        }

        _senderHost?.Init(owner);
        if (this.sender != null)
        {
            this.sender.owner = this;
            this.sender.Init(owner);
        }

        if (!string.IsNullOrEmpty(this.SaveErrorSendDirectory))
        {
            // Ensure the save error directory exists
            Directory.CreateDirectory(this.SaveErrorSendDirectory);

            var moveDir = Path.Combine(this.SaveErrorSendDirectory, "Move");
            if (Directory.Exists(moveDir))
            {
                foreach (var file in Directory.GetFiles(moveDir))
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
            countDelayMessages = files.Count();
            if (countDelayMessages > 0)
            {
                SizeDirectory = files.Select(ii => new FileInfo(ii).Length).Sum();

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
        
        // Preserve the original logic of initializing the sender with a LongLifeRepositorySender,
        // only this time we use the new, ISender-based, version of the sender.
       // this.isender = new Plugins.LongLifeRepositorySender();
    }

    static Metrics.MetricCount sucMetric = new Metrics.MetricCount("packagesReceivedSuccess", "All packages, sended to utility");
    static Metrics.MetricCount errMetric = new Metrics.MetricCount("packagesReceivedUnsuccess", "All packages, sended to utility with error");
    static Metrics.MetricCount errMetricRetry = new Metrics.MetricCount("packagesSendedUnsucRetry", "All packages, resended sucessfully after error");
    static Metrics.MetricHistogram metricRetryTimeError = new Metrics.MetricHistogram("retryPackagesTime", "retry time on error", new double[] {  100, 500, 1000, 5000, 10000 ,30000,60000,600000});
    static Metrics.MetricAuto metricDelayMessages; 

    //   LongLifeRepositorySender repo = new LongLifeRepositorySender();

    /// <summary>
    /// In the bridge mode, after the Step receives an input message from the Receiver,
    /// it immediately sends it to the Sender object, receives the response from the Sender,
    /// and sends its back as the response to the initiating Receiver call.
    /// Receiver --(received request)--> Sender --(response)--> Receiver
    /// </summary>
    public bool isBridge = false;

    public static bool saveAllResponses = false;


    Task saveScenarious = null;

    public static int incrValue = 0;
    private async Task Receiver_stringReceived(string input, object context)
    {
//        owner.mainActivity = owner.GetActivity("receive package", null);
        DateTime time2 = DateTime.Now;
        ContextItem contextItem = new ContextItem() {startTime=time2, context = context ,mainActivity= owner.GetActivity("receive package", null), increment=Interlocked.Increment(ref incrValue) };
        if (Pipeline.isExtendingStat)
        {
            contextItem.stats = new System.Collections.Generic.List<ContextItem.StatItem>();
            contextItem.stats.Add(new ContextItem.StatItem() { Name = "All" });
        }
        if (contextItem?.mainActivity != null)
        {
            contextItem?.mainActivity?.SetTag("context.url", owner.SaveContext(input));
        }
        var item = context as HTTPReceiver.SyncroItem;
        if (item != null)
            item.ctnx = contextItem;
            //            List<AbstrParser.UniEl> list = new List<AbstrParser.UniEl>();
        var rootElement = AbstrParser.CreateNode(null, contextItem.list, this.IDStep);
        rootElement = AbstrParser.CreateNode(rootElement, contextItem.list, "Rec");
        if(owner.saver?.enable ?? false)
        {
            contextItem.fileNameT = Path.GetRandomFileName();
/*            contextItem.currentScenario = new Scenario() { Description = $"new Scenario on {DateTime.Now}", mocs = new List<Scenario.Item>() };
            if (owner.saver != null)*/
                owner.saver.save(input, contextItem + IDStep + "Receiver_"+contextItem.fileNameT);

        }
        if (Pipeline.isSaveHistory)
            Logger.log("{data} {context}", Serilog.Events.LogEventLevel.Information, "hist", input.MaskSensitive(), contextItem.GetPrefix( IDStep + "Receiver"));

        /*        if (saveAllResponses)
                {
                    contextItem.currentScenario = new Scenario() { Description = $"new Scenario on {DateTime.Now}", mocs = new List<Scenario.Item>() };
                    contextItem.currentScenario.mocs.Add(new Scenario.Item() { IDStep = this.IDStep, isMocReceiverEnabled = true, MocFileReceiver = input });
                }*/
        owner.lastExecutedEl = rootElement;
        if (!isBridge)
        {
            await FilterInfo(input, time2, contextItem, rootElement);

            await checkChilds(contextItem, rootElement);
        }
        else
        {
            // In the bridge mode we immediately send the input to the Sender object.
            // The response is then sent back to the receiver.
            try
            {
                /*if(sender?.GetType() == typeof(HTTPSender) && receiver.GetType() == typeof(HTTPReceiver))
                    {
                        (sender as HTTPSender).headers = (context as HTTPReceiver.SyncroItem).headers;
                        
                    }*/
                string ans;
                if(isender != null)
                    ans = await isender?.send(input,contextItem);
                else
                    ans = await sender?.send(input, contextItem);


                // TODO: consider passing the original context object coming from sender, not the ContextItem wrapper
                if (_receiverHost!= null)
                await _receiverHost.sendResponse(ans, contextItem);
                else 
                await receiver.sendResponse(ans, contextItem);

                /*if (owner.saver?.enable ?? false)
                {
                    owner.saver.save(JsonSerializer.Serialize<Scenario>(contextItem?.currentScenario), ".scn");
                }*/
   /*             if(saveAllResponses)
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

                }*/
            }
            catch (Exception e66)
            {
                contextItem?.mainActivity?.SetTag("pipelineError", "true");
                contextItem ? .mainActivity?.Stop();
                contextItem?.mainActivity?.Dispose();
                Logger.log($"On send error", e66, Serilog.Events.LogEventLevel.Error);
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
    private async Task<string> FindAndCopy1(AbstrParser.UniEl rootElInput, DateTime time1, ItemFilter item, AbstrParser.UniEl el, List<AbstrParser.UniEl> list,ContextItem context)
    {
        int count = 0;
        var local_rootOutput = new AbstrParser.UniEl() { Name = "root" };
        count = item.exec(rootElInput, ref local_rootOutput, context);
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
            return await _senderHost.send(local_rootOutput,context);
    }

    public async Task<string> FilterInfo1(string input, DateTime time2, List<AbstrParser.UniEl> list, AbstrParser.UniEl rootElement,ContextItem context)
    {
        try
        {
            //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };
            if(AbstrParser.getApropriateParser("", input, rootElement, list))
/*            foreach (var pars in AbstrParser.availParser)
                if (pars.canRazbor(input, rootElement, list))*/
                {
                    DateTime time1 = DateTime.Now;

                    //                    var fltr = filters.First().filter(list);
                    if (converters != null && converters.Count > 0)
                    {
                        bool found = false;
                        foreach (var item in converters/*.First().filter(list)*/)
                        {
                            AbstrParser.UniEl rEl = null;
                            foreach (var item1 in item.filter.filter(list, ref rEl, context))
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
                    //break;
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
    private bool tryParse(string nameContext,string input, ContextItem context, AbstrParser.UniEl rootElement)
    {
        bool cantTryParse = false;
        if (ireceiver != null)
            cantTryParse = ireceiver.cantTryParse;
        if (receiver != null)
            cantTryParse = receiver.cantTryParse;
        return AbstrParser.getApropriateParser(nameContext, input, rootElement, context.list, cantTryParse);
/*        foreach (var pars in AbstrParser.availParser)
            if (pars.canRazbor(input, rootElement, context.list, cantTryParse))
            {
                return true;
            }
        return false;*/
    }
    public async Task FilterInfo(string input, DateTime time2, ContextItem context, AbstrParser.UniEl rootElement)
    {
        // When called from string_received, the rootElement is the container for the parsed record.
        
        try
        {
            //            AbstrParser.UniEl rootElOutput = new AbstrParser.UniEl() { Name = "root" };

            if (tryParse(this.IDStep+"Rec",input, context, rootElement))
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
                if (this.IDStep == "Step_0")
                {
                    int y = 0;
                }
                foreach (var item1 in item.filter.filter(context.list, ref rEl,context))
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
                    {
                            await SendToSender(rootElement, context, local_rootOutput);
                    }
                    else
                        SaveRestoreFile(local_rootOutput);

                    new AbstrParser.UniEl(rootElement.ancestor) { Name = "SendErrorCode", Value = (isErrorSending?1: 0) };
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


    void CheckMetric()
    {
        if(metricDelayMessages == null)
        {
            metricDelayMessages = new Metrics.MetricAuto("delayingMessages", "count of delayingMessages", () => { return countDelayMessages; });
            metricDelayMessages.AddLabels(new Metrics.Label [] { new Metrics.Label("Step",this.IDStep) } );
        }
    }
    private void SaveRestoreFile(AbstrParser.UniEl local_rootOutput)
    {
        if(nonSavedError) 
            return;
        Interlocked.Increment(ref countDelayMessages);
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
                        countDelayMessages = 0;
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
    //    return;//!!!!!!

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
                    if(AbstrParser.getApropriateParser("", line, rootEl, list))
                    /*foreach (var pars in AbstrParser.availParser)
                        if (pars.canRazbor(line, rootEl, list))*/
                            break;
                }

                restart:
                try
                {
                    if (_senderHost != null)
                        await _senderHost.send(rootEl, null);
                    else
                    {
                        //Path.GetFullPath(@"/CACHE1\\j20ddkl2.p21")
                        await sender.send(rootEl, null);
                    }
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
                    Interlocked.Decrement(ref countDelayMessages);

                    FileInfo fi = new FileInfo(file1);
                    Interlocked.Add(ref SizeDirectory,-(fi.Length));
                    File.Delete(file1);
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
        count = item.exec(el/*rootElInput*/, ref local_rootOutput,context);
        /*            foreach (var ff in item.outputFields)
                        {
                            if(ff.addToOutput(rootElInput, ref local_rootOutput))
                                count++;
                        }*/
        if (debugMode )
            Logger.log("{this} {filter} transform to output  added {count} items, filt:{l} out:{out}", Serilog.Events.LogEventLevel.Debug,this,item, count,el.toJSON(true),local_rootOutput.toJSON());
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
            string responseFromSender;
            if (isender == null && sender==null)
                responseFromSender = local_rootOutput.toJSON();
            else
            {
                if(isender != null)
                    responseFromSender = await isender.send(local_rootOutput, context);
                else
                    responseFromSender = await sender.send(local_rootOutput, context);

            }
            if (step.ireceiver != null) 
                await step.ireceiver.sendResponse(responseFromSender, context);
            if (step.receiver != null)
                await step.receiver.sendResponse(responseFromSender, context);

            foreach (var node in local_rootOutput.childs)
            {
                node.ancestor = toNode;
                //          toNode.childs.Add(node);
            }
            StoreAnswer("Resp", rootElInput, context, responseFromSender, step);

        }
        else
        {
            string ans;
            if(isender != null)
                ans = await isender.send(local_rootOutput,context);
            else
                ans = await sender.send(local_rootOutput, context);

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
                    if(nextStep.IDStep =="Step_ToTWO")
                    {
                        int yy = 0;
                    }
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



        nextStep.tryParse(nextStep.IDStep+ans,ans, context, newRoot);
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

    public object ToLiquid()
    {
        int ref1 = 1;
        List<OutputValue> outputs = new List<OutputValue>();
        foreach (var conv in this.converters)
            outputs.AddRange(conv.outputFields.Where(ii=>ii.getLiquidDict().Count>0));
        //Dictionary<string,object> outputs1= new Dictionary<string, object>[outp]
        var ret = new Dictionary<string, object> { { "Name", this.IDStep }, { "Description", this.description }, { "filters", outputs }, { "sender", new LiquidPoint(owner.steps, -1, ((this.isender == null)?sender:isender),ref ref1
            ) } };
        if(IDStep== "Step_ToTWO")
        {
            int yy = 0;
        }
        return ret;
    }
}