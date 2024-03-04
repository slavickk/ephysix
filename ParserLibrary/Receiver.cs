using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    protected virtual async Task sendResponseInternal(string response, ContextItem context)
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

    /// <summary>
    /// Number of times to receive the mock message (default is 1).
    /// This allows to simulate processing multiple messages after a single pipeline initialization - for benchmarking.
    /// </summary>
    [YamlIgnore]
    public int MockReceiveCount = 1;
    
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

            metricUpTime?.Add(time1);
        }
        catch (Exception e)
        {
            metricUpTimeError?.Add(time1);
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
            Logger.log("Send answer to {step} : {content} ", Serilog.Events.LogEventLevel.Debug, "any", owner, response);
        if (owner?.owner.saver?.enable ?? false)
            owner.owner.saver.save(response, contextItem + owner.IDStep + "RecAns_" + contextItem.fileNameT);
       // LogExtendedStat(contextItem); 
        if (Pipeline.isSaveHistory)
            Logger.log("{data} {context} ", Serilog.Events.LogEventLevel.Information, "hist", response.MaskSensitive(), contextItem.GetPrefix(owner.IDStep + "RecAns"));


        if (!MocMode)
            await sendResponseInternal(response, contextItem);
    }

    protected void LogExtendedStat(string result,string request,ContextItem contextItem)
    {
        if (Pipeline.isExtendingStat && contextItem != null)
        {
            contextItem.stats[0].ticks = (DateTime.Now - contextItem.startTime).Ticks;
            var st = contextItem.stats.Select(ii => new KeyValuePair<string, long>(ii.Name, ii.ticks))
                .ToDictionary(x => x.Key, x => x.Value);
            Logger.log("{result} {context} {@stats} {request}", Serilog.Events.LogEventLevel.Information, "hist",result
                , contextItem.GetPrefix(owner.IDStep + "RecAns"), st,request);
        }
    }

    public async Task start()
    {
        if (MocMode)
        {
            if (string.IsNullOrEmpty(this.MocBody))
            {
                using StreamReader sr = new StreamReader(MocFile);
                this.MocBody = sr.ReadToEnd();
            }
            string hz = "hz";

            var sw = new Stopwatch();
            sw.Start();
            
            for (var i = 0; i < this.MockReceiveCount; i++)
                await signal(this.MocBody,hz);

            sw.Stop();
            
            Logger.log("Receiver signalled {count} times in {total_time} ms, average signal processing time is {avg_time} us",
                Serilog.Events.LogEventLevel.Information, "any",
                this.MockReceiveCount,
                sw.ElapsedMilliseconds,
                (double)sw.ElapsedTicks / Stopwatch.Frequency * 1000000 / this.MockReceiveCount);
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