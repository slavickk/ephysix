/******************************************************************
 * File: Step.SenderHost.cs
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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ParserLibrary;
using PluginBase;
using YamlDotNet.Serialization;

namespace ParserLibrary;

public partial class Step
{
    public class SenderHost : ISenderHost
    {
        [YamlIgnore] protected Metrics.MetricHistogram metricUpTime;
        [YamlIgnore] private Metrics.MetricHistogram metricUpTimeError;

        public virtual void Init(Pipeline owner)
        {
            metricUpTimeError = new Metrics.MetricHistogram("iu_outbound_errors_total", "handle performance receiver",
                new double[] { 30, 100, 500, 1000, 5000, 10000 });
            metricUpTimeError.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });

            metricUpTime =
                new Metrics.MetricHistogram("iu_outbound_request_duration_msec", "handle performance receiver");
            metricUpTime.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
            
            this._sender?.Init();
        }


        public override string ToString()
        {
            return $"Sender:{this.GetType().Name} Step:{owner?.IDStep}";
        }

        public virtual string getExample()
        {
            return "";
        }

        public virtual string getTemplate(string key)
        {
            return "";
        }

        public virtual void setTemplate(string key, string body)
        {
        }


        [YamlIgnore] private ISender _sender;


        [YamlIgnore] public bool MocMode = false;
        public string MocFile;
        public string MocBody = "";

        [YamlIgnore] public Step owner;

        public enum TypeContent
        {
            internal_list,
            json
        };

        [YamlIgnore] public TypeContent typeContent { get; }
        public ISender Sender => _sender;

        //  public string IDResponsedReceiverStep = "";
        string MocContent = "";
        object syncro = new object();
        public Activity sendActivity { get; private set; }

        public SenderHost(Step owner, ISender sender)
        {
            _sender = sender;
            this.owner = owner;
        }

        public async Task<string> send(UniElLib.AbstrParser.UniEl root, UniElLib.ContextItem context)
        {
            DateTime time1 = DateTime.Now;
            string ans;
            sendActivity = owner.owner.GetActivity($"Send{this.GetType().Name}", context?.mainActivity);
            sendActivity?.AddTag("typeSender", this.GetType().Name);
            try
            {
                if (!MocMode)
                {
                    if (typeContent == TypeContent.internal_list)
                        // ans = await sendInternal(root, context);
                        ans = await _sender.send(root, context);
                    else
                        // ans = await send(root.toJSON(), context);
                        ans = await _sender.send(root.toJSON(), context);
                }
                else
                {
                    // await Task.Delay(10);
                    if ((MocBody ?? "") != "")
                        ans = MocBody;
                    else
                    {
                        if (MocContent == "")
                        {
                            lock (syncro)
                            {
                                if (MocContent == "")
                                {

                                    // string ans;
                                    using (StreamReader sr = new StreamReader(MocFile))
                                    {
                                        MocContent = sr.ReadToEnd();
                                        //       return ans;
                                    }
                                }
                            }
                        }

                        ans = MocContent;
                    }
                }

                if (owner.debugMode)
                    Logger.log(time1, "{Sender} Send:{Request}  ans:{Response}", "JsonSender",
                        Serilog.Events.LogEventLevel.Debug, this, root.toJSON(true), ans);
                metricUpTime.Add(time1);
            }
            catch (Exception ex)
            {
                sendActivity?.AddTag("errorSend", "true");
                sendActivity?.Dispose();
                metricUpTimeError.Add(time1);
                throw;
            }

            sendActivity?.Dispose();
            return ans;
        }

        /// <summary>
        /// Release resources if any
        /// </summary>
        public void Release()
        {
            if (_sender is IDisposable disposable)
                disposable.Dispose();
        }

        public string IDStep => this.owner.IDStep;

        public string SavePipelineContext(string body, string extension = "txt")
        {
            return owner.owner.SaveContext(body, extension);
        }
    }
}
