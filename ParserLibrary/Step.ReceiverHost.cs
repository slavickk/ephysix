using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PluginBase;
using UniElLib;
using YamlDotNet.Serialization;

namespace ParserLibrary;

public partial class Step
{
    public sealed class ReceiverHost : IReceiverHost
    {
        public ReceiverHost(Step owner, IReceiver receiver)
        {
            this.owner = owner;
            this._receiver = receiver;
            if(receiver != null )
            receiver.host = this;
        }
        
        public void Init(Pipeline owner)
        {
            metricUpTime = new Metrics.MetricHistogram("iu_inbound_request_duration_msec", "handle performance receiver");
            metricUpTimeError = new Metrics.MetricHistogram("iu_inbound_errors_total", "handle performance receiver", new double[] { 30, 100, 500, 1000, 5000, 10000 });
            metricUpTime.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
            
            if (saver != null)
                saver.Init();
        }

        public delegate void StringReceived(string input);
        public delegate void BytesReceived(byte[] input);
        
        private IReceiver _receiver;

        public IReceiver Receiver => _receiver;

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

        public ReplaySaver saver = null;
        [YamlIgnore]

        public Step owner
        {
            set
            {
                owner_internal = value;
                debugMode = value.debugMode;
            }
            get => owner_internal;
        }
        [YamlIgnore]
        Step owner_internal;

        /// <summary>
        /// Handle an input message from client
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        public async Task signal(string input,object context)
        {
            DateTime time1= DateTime.Now;
            try
            {
                if (debugMode)
                {
                    Logger.log("Receive step:{o} {input} {thr}", Serilog.Events.LogEventLevel.Debug, "any", owner, input.MaskSensitive(), Thread.CurrentThread.ManagedThreadId);
                }


                if (saver != null)
                    saver.save(input);
                await owner.Receiver_stringReceived(input, context);
                        
                metricUpTime.Add(time1);
            }
            catch (Exception e)
            {
                metricUpTimeError.Add(time1);
                throw;
            }
        }

        public string IDStep => this.owner.IDStep;

        public int MaxConcurrentConnections => throw new NotImplementedException();

        public int ConnectionTimeoutInMilliseconds => throw new NotImplementedException();


        /// <summary>
        /// Send response back to the initiating client behind the receiver.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="context"></param>
        public async Task sendResponse(string response, object context)
        {
            if (debugMode)
                Logger.log("Send answer to {step} : {content} ", Serilog.Events.LogEventLevel.Debug, "any",owner, response);
            if (saver != null)
                saver.save(response);

            if (!MocMode)
                await this._receiver.sendResponse(response, context);
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
                await signal(input, hz);
            }
            else
                await _receiver.start();
        }
 
        /// <summary>
        /// Release resources if any
        /// </summary>
        public void Release()
        {
            if (_receiver is IDisposable disposable)
                disposable.Dispose();
        }
    }
}