using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotLiquid;
using PluginBase;
using UniElLib;
using YamlDotNet.Serialization;

namespace ParserLibrary;


public abstract class Receiver/*:IReceiver*/
{
    public int port = -1;

    public enum ProtocolType { unknown,tcp,http};
    [YamlIgnore]
    virtual public ProtocolType protocolType
    {
        get
        {
            return ProtocolType.unknown;
        }
    }

    public int MaxConcurrentConnections = 1000;
    public int ConnectionTimeoutInMilliseconds = 5000;


    /// <summary>
    /// Abstract and virtual methods
    /// </summary>
    public virtual void Init(Pipeline owner)
    {
        metricUpTime = new Metrics.MetricHistogram("iu_inbound_request_duration_msec", "handle performance receiver");
        metricUpTimeError = new Metrics.MetricHistogram("iu_inbound_errors_total", "handle performance receiver", new double[] { 30, 100, 500, 1000, 5000, 10000 });
        metricUpTime.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
    }
    protected virtual async Task sendResponseInternal(string response, object context)
    {
        if (debugMode)
            Logger.log("Responcer do nothing, mocMode^{MocMode}!!!", Serilog.Events.LogEventLevel.Debug, MocMode);
    }

    public delegate void StringReceived(string input);
    public delegate void BytesReceived(byte[] input);

    [YamlIgnore]
    public Func<string, object, Task> stringReceived;





    public object saver;

    [YamlIgnore]

    public Metrics.MetricHistogram metricUpTime;
    [YamlIgnore]

    public Metrics.MetricHistogram metricUpTimeError;
    public bool  cantTryParse=false;
    /*        public virtual bool cantTryParse()
            {
                return false;
            }*/

    [YamlIgnore]
    public Mocker mocker= new Mocker();

    [YamlIgnore]
    public bool MocMode = false;
    public string MocFile
    {
        get
        { return mocker.MocFile; }
        set
        {
            mocker.MocFile = value;
        }
    }
    public string MocBody
    {
        get { return mocker.MocBody; }
        set { mocker.MocBody = value; }
    }

    [YamlIgnore]
    public bool debugMode = false;

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
    /*
    public IReceiverHost host { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    bool IReceiver.cantTryParse => this.cantTryParse;//throw new NotImplementedException();

    bool IReceiver.debugMode { get => this.debugMode; set => this.debugMode = value;}
    */
    [YamlIgnore]
    Step owner_internal;

    public async Task signal(string input,object context)
    {
        DateTime time1= DateTime.Now;
        try
        {
            if (debugMode)
            {
                Logger.log("Receive step:{o} {input} {thr}", Serilog.Events.LogEventLevel.Debug, "any", owner, input.MaskSensitive(), Thread.CurrentThread.ManagedThreadId);
            }
            if (stringReceived != null)
                await stringReceived(input, context);

            metricUpTime.Add(time1);
        }
        catch (Exception e)
        {
            metricUpTimeError.Add(time1);
            throw;
        }
    }


    /// <summary>
    /// Send response back to the initiating client behind the receiver.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="contextItem"></param>
    public async Task sendResponse(string response,ContextItem  contextItem)
    {
        if (debugMode)
            Logger.log("Send answer to {step} : {content} ", Serilog.Events.LogEventLevel.Debug, "any",owner, response);
        if (owner?.owner.saver?.enable??false)
            owner.owner.saver.save(response,contextItem+owner.IDStep+"RecAns_"+contextItem.fileNameT);
        if (Pipeline.isSaveHistory)
            Logger.log("{data} {context}", Serilog.Events.LogEventLevel.Information, "hist", response.MaskSensitive(), contextItem.GetPrefix(owner.IDStep + "RecAns"));

        if (!MocMode)
            await sendResponseInternal(response, contextItem.context);
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


    protected async virtual Task startInternal()
    { 
        
    }

    public Task sendResponse(string response, object context)
    {
        throw new NotImplementedException();
    }
}