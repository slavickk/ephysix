/******************************************************************
 * File: Sender.cs
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
using PluginBase;
using YamlDotNet.Serialization;
using static ParserLibrary.Step;
using UniElLib;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace ParserLibrary;

public abstract class Sender: DiagramExecutorItem/*:ISender*/
{
    public string IDStepForTransactionRollback = "";
    public string IDStepForTransactionRollbackCancel = "";

    [YamlIgnore]

    protected Metrics.MetricHistogram metricUpTime;
    [YamlIgnore]

    private Metrics.MetricHistogram metricUpTimeError;

    public string description = "";
    [YamlIgnore]
    DistributedCacheEntryOptions cache_options ;
    public double cacheTimeInMilliseconds /*{ get; set; } = 0*/;
    public bool ignoreErrors = false;

    public string mandatoryFields;

    //Dictionary<string,>

    public virtual void Init(Pipeline owner)
    {
        if(cacheTimeInMilliseconds!= 0)
        {
            cache_options = new()
            {
                AbsoluteExpirationRelativeToNow =
        TimeSpan.FromMilliseconds(cacheTimeInMilliseconds)
            };
        }
        metricUpTimeError = new Metrics.MetricHistogram("iu_outbound_errors_total", "handle performance receiver", new double[] { 30, 100, 500, 1000, 5000, 10000 });
        metricUpTimeError.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });

        metricUpTime = new Metrics.MetricHistogram("iu_outbound_request_duration_msec", "handle performance receiver");
        metricUpTime.AddLabels(new Metrics.Label[] { new Metrics.Label("Name", this.GetType().Name) });
    }


    public override string ToString()
    {
        return $"Sender:{this.GetType().Name} Step:{owner?.IDStep}";
    }
    public virtual  string getExample()
    {
        return "";
    }

    public virtual string getTemplate(string key)
    {
        return "";
    }

    public virtual void setTemplate(string key,string body)
    {
    }



    [YamlIgnore]
    public Mocker mocker = new Mocker();

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
    public Step owner;
    
    [YamlIgnore]
    public abstract TypeContent typeContent
    {
        get;
    }
   /* ISenderHost _host;
    public ISenderHost host { get => _host; set => _host=value; }

    global::TypeContent ISender.typeContent => this.typeContent;
   */
    //  public string IDResponsedReceiverStep = "";
    string MocContent = "";
    object syncro= new object();
    protected Activity sendActivity;

    public string getAnswer(AbstrParser.UniEl root)
    {
        return formBody(root);
        
    }
    protected virtual string formBody(AbstrParser.UniEl root)
    {
        return "";
    }
    public virtual  async Task<string> send(AbstrParser.UniEl root, ContextItem context)
    {
        if (Pipeline.configuration["SAVED_DUMP_SENDERS"] != null)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(owner.owner.fileName), this.owner.IDStep + "_Send.tmp")))
            {
                sw.Write(root.toJSON());
            }
        }
        if(!string.IsNullOrEmpty(mandatoryFields))
        {
            List<string> mandatory = mandatoryFields.Split(';').ToList();
            foreach(var el in root.getAllDescentants())
            {
                if(mandatory.Contains(el.path.Substring(5)))
                {
                    mandatory.Remove(el.path.Substring(5));
                    if (mandatory.Count == 0)
                        break;
                }
            }
            if(mandatory.Count >0)
            {
                throw new HTTPStatusException() { StatusCode = 400, StatusReasonJson = $"Fields not present on {description} : {string.Join(',',mandatory)} ",sourceSender=this.description };

            }

        }
        DateTime time1 = DateTime.Now;
        string ans="";
        sendActivity = owner.owner.GetActivity($"Send{this.GetType().Name}", context?.mainActivity);
        sendActivity?.AddTag("typeSender", this.GetType().Name);
        string cacheKey = "";
        try
        {
            if (cacheTimeInMilliseconds != 0)
            {
                foreach(var item in root.childs)
                {
                    if(item.Name == "cacheKey")
                    {
                        cacheKey=item.Value.ToString();
                        root.childs.Remove(item);
                        break;
                    }

                }
                //cacheProvider.GetStringAsync()
                ans = await EmbeddedFunctions.cacheProvider.GetStringAsync(EmbeddedFunctions.cacheProviderPrefix + cacheKey);

            }
            if (string.IsNullOrEmpty(ans))
            {
                if (!MocMode)
                {
                    ans = await formAnswer(root, context, ans);
                    if (context.stats != null)
                        context.stats.Add(new ContextItem.StatItem() { Name = description ?? GetType().Name, ticks = (DateTime.Now - time1).Ticks });
                }

                else
                {
                    if ((this as HTTPSender)?.ResponseType?.Contains("/xml") == true)
                    {
                        var ans1 = root.childs[0].toXML();
                    }
                   // ans = await formAnswer(root, context, ans);

                    ans = await mocker.getMock();

                }

                if (Pipeline.configuration["SAVED_DUMP_SENDERS"] != null)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(owner.owner.fileName), this.owner.IDStep + "_Ans.tmp")))
                    {
                        sw.Write(ans);
                    }
                }

                if (cacheTimeInMilliseconds != 0)
                {
                    await EmbeddedFunctions.cacheProvider.SetStringAsync(EmbeddedFunctions.cacheProviderPrefix + cacheKey, ans, cache_options);
                }

            }


            if (!this.ignoreErrors && (context.context as HTTPReceiver.SyncroItem).isError)
            {
                throw new HTTPStatusException() { StatusCode = (context.context as HTTPReceiver.SyncroItem).HTTPStatusCode, StatusReasonJson = (context.context as HTTPReceiver.SyncroItem).HTTPErrorJsonText, sourceSender = this.description };

            }


            if (this.ignoreErrors && (context.context as HTTPReceiver.SyncroItem).isError)
                ans = (context.context as HTTPReceiver.SyncroItem).errorContent;
            if(!(context.context as HTTPReceiver.SyncroItem).isError)
            {
                if (!string.IsNullOrEmpty(IDStepForTransactionRollback))
                    context.needRollback.Add(IDStepForTransactionRollback);
                if (!string.IsNullOrEmpty(IDStepForTransactionRollbackCancel))
                    context.needRollback.Remove(IDStepForTransactionRollbackCancel);

            }
            if (owner.debugMode)
                Logger.log(time1, "{Sender} Send:{Request}  ans:{Response}", "JsonSender", Serilog.Events.LogEventLevel.Debug, this, root.toJSON(true), ans);
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

    private async Task<string> formAnswer(AbstrParser.UniEl root, ContextItem context, string ans)
    {
        if (typeContent == TypeContent.internal_list)
            ans = await sendInternal(root, context);
        else
            ans = await send(root.toJSON(), context);
        return ans;
    }

    public async virtual Task<string> sendInternal(AbstrParser.UniEl root,ContextItem context)
    {
        /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
        return "";

    }
    public async virtual Task<string> send(string JsonBody,ContextItem context)
    {
        /*            if (owner.debugMode)
                            Console.WriteLine("send result");*/
        return "";

    }

    
}