using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ParserLibrary;

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