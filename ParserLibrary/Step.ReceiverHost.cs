/******************************************************************
 * File: Step.ReceiverHost.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PluginBase;
using Plugins;
using UniElLib;
using YamlDotNet.Serialization;
using static ParserLibrary.HTTPReceiver;

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
public /*static*/   bool choosePath(HTTPReceiver.SyncroItem item, List<PathItem> paths, string path)
        {
            if (paths?.Count > 0)
            {
                var nextStepName = paths.FirstOrDefault(ii => path.Contains(ii.Path))?.Step;
                if (nextStepName == null)
                {
                    return false;
                    /* await SetResponseStatusCode(httpContext, 404);
                     // await SetResponseContent(httpContext, content);
                     return;
                    */
                }
                var nextStep = owner.owner.steps.FirstOrDefault(ii => ii.IDStep == nextStepName);
                if (nextStep == null)
                {
                    Logger.log("not found path");
                    return false;
                    /*  await SetResponseStatusCode(httpContext, 404);
                      // await SetResponseContent(httpContext, content);
                      return;
                    */
                }
                Logger.log("path found on step {step}", Serilog.Events.LogEventLevel.Debug, "any", nextStep.IDStep);
                item.initialStep = nextStep;
            }
            else
                Logger.log("paths is empty", Serilog.Events.LogEventLevel.Debug);
            return true;
        }


        /// <summary>
        /// Handle an input message from client
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        public async Task signal(string input,object context)
        {
            DateTime time1= DateTime.Now;
            HTTPReceiverSwagger.SyncroItem context1 = context as HTTPReceiverSwagger.SyncroItem;
            bool isError = false;
            try
            {
                if (debugMode)
                {
                    Logger.log("Receive step:{o} {input} {thr}", Serilog.Events.LogEventLevel.Debug, "any", owner, input.MaskSensitive(), Thread.CurrentThread.ManagedThreadId);
                }


                if (saver != null)
                    saver.save(input);
 
                await ((context1?.initialStep != null) ? context1.initialStep : owner).Receiver_stringReceived(input, context,this.IDStep);
                //await owner.Receiver_stringReceived(input, context);
                        
                metricUpTime.Add(time1);
            }
            catch (Exception e)
            {

                if (context1.ctnx.needRollback.Count != 0)
                {
                    bool isFatal = false;
                    foreach (var nameStep in context1.ctnx.needRollback)
                    {
                        try
                        {
                            var step = this.owner.owner.steps.First(ii => ii.IDStep == nameStep);
                            await Step.execStep(step, context1.ctnx, context1.ctnx.list[0].childs[0]);
                        }
                        catch (Exception e66)
                        {
                            isFatal = true;
                        }
                    }
                }
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
                await signal(input, null);
            }
            else
                await _receiver.start();
        }
        
        public async Task stop()
        {
            await _receiver.stop();
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