using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PluginBase;
using YamlDotNet.Serialization;

namespace ParserLibrary;


public abstract class Receiver
{
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
    public bool MocMode = false;
    public string MocFile;
    public string MocBody;

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
    [YamlIgnore]
    Step owner_internal;

    public async Task signal(string input,object context)
    {
        DateTime time1= DateTime.Now;
        try
        {
            if (debugMode)
            {
                Logger.log("Receive step:{o} {input} {thr}", Serilog.Events.LogEventLevel.Debug, "any", owner, input, Thread.CurrentThread.ManagedThreadId);
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


    public async Task sendResponse(string response,ContextItem  contextItem)
    {
        if (debugMode)
            Logger.log("Send answer to {step} : {content} ", Serilog.Events.LogEventLevel.Debug, "any",owner, response);
        if (owner?.owner.saver != null)
            contextItem.currentScenario.getStepItem(this.owner.IDStep).MocFileResponce=owner.owner.saver.save(response);

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
}