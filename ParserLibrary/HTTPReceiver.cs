﻿/******************************************************************
 * File: HTTPReceiver.cs
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

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Net.WebSockets;
using System.Text;
using Kestrel;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.VisualStudio.Threading;
using System.Text.Json;
using static ParserLibrary.HTTPSender;
using UniElLib;
using Serilog;
using static ParserLibrary.SwaggerDef.GET.Responses.CodeRet;
using Namotion.Reflection;
using System.Text.Encodings.Web;
//using Microsoft.

namespace ParserLibrary
{
    public class HTTPReceiver : Receiver
    {
        public class PathItem
        {
            public string Path { get; set; }
            public string Step { get; set; }
        }

        public List<PathItem> paths { get; set; }= new List<PathItem>() { { new PathItem() { Path = "/aa", Step = "Step_0" } } };
        public override ProtocolType protocolType => ProtocolType.http;
        //        public int port = 8080;

        public string swaggerSpecPath = null;        

        KestrelServer server ;
        public string ResponseType = "application/json";
        public HTTPReceiver()
        {
            if (port == -1)
                port = 8080;
            server = new KestrelServer(this);
        }
        protected override async Task startInternal()
        {
            await server.Start(port,MaxConcurrentConnections);
           // return base.startInternal();
        }
        //namespace Kestrel;
        protected override async Task sendResponseInternal(string response, ContextItem context)
        {
            if (debugMode)
            {
                Logger.log(" Send response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any",  owner, response.MaskSensitive());
            }

            var item = context.context as SyncroItem;
            if (item != null)
            {
                item.answer = response;
//                item.ctnx = context.GetPrefix("SendResp");
            //    Interlocked.Increment(ref item.srabot);
           //     item.semaphore.Set();//.Release();
                /*if (item.semaphore.CurrentCount == 0)
                {
                    int ii = 0;
                    Interlocked.Increment(ref item.srabot);
                    item.semaphore.Release();
                }*/

            }
            // return base.sendResponseInternal(response, context);
        }
//        public long TimeoutInMilliseconds = 15000;

        public async Task signal1(string body,SyncroItem semaphoreItem)
        {
            await signal(body, semaphoreItem).WaitAsync(TimeSpan.FromMilliseconds(ConnectionTimeoutInMilliseconds));
           // semaphoreItem.semaphore.
            if (semaphoreItem.srabot==0)
            {
                if (debugMode)
                {
                    Logger.log("Answer without response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner, "-");

                }
         //       semaphoreItem.semaphore.Set();
            }
        }
        public class SyncroItem
        {
            public int HTTPStatusCode = 200;
            public string HTTPErrorJsonText = "";
            public string errorContent = "";
            public bool isError = false;
            public bool isRollbackPhase = false;
            public class ErrorMessage
            {
                public string[] Reasons { get; set; }    
            }
            public void SetErrorMessage(string message)
            {
                this.HTTPErrorJsonText = message;
            }
            public async Task<object> formAnswer(HttpContext context)
            {
                string answer = "";
                context.Response.StatusCode = HTTPStatusCode;
                if(isError )
                {
                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
,
                        IgnoreNullValues = true
                    };

                    context.Response.ContentType = "application/json";
                   //       context.Response.
                   /*            JsonSerializerOptions options = new JsonSerializerOptions()
                               {
                                   WriteIndented = true,
                                   Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                               ,
                                   IgnoreNullValues = true
                               };
                   */
                   return JsonSerializer.Deserialize<JsonElement>(HTTPErrorJsonText,options);// JsonSerializer.Serialize(HTTPErrorJsonText, HTTPErrorJsonText.GetType(),options);
                    //await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(answer));
                }
                return answer;
            }

            public Step initialStep= null;
            public int srabot = 0;
            public int unwait = 0;
            public string answer="";
            public ContextItem ctnx ;
            public List<KestrelServer.Header> headers = new List<KestrelServer.Header>();
            public string UrlPath;
          // public AsyncAutoResetEvent semaphore = new AsyncAutoResetEvent();
        }
        public class KestrelServer: Kestrel.KestrelServerImplement
        {
            HTTPReceiver owner;
            public KestrelServer(HTTPReceiver owner)
            {
                this.owner = owner;
                //this..GetMetrics();
            }


            public static long CountExecuted = 0;
            public static long CountOpened = 0;
            public static Metrics.MetricCount metricCountOpened = new Metrics.MetricCount("HTTPOpenConnectCount", "opened http connection at same time ");
            public static Metrics.MetricCount metricErrors = new Metrics.MetricCount("HTTPErrorCount", "Error http request ");
            public static Metrics.MetricCount metricTimeouts = new Metrics.MetricCount("HTTPTimeoutCount", "Timeout http request ");
            public static Metrics.MetricCount metricCountExecuted = new Metrics.MetricCount("HTTPExecutedConnections", "All http executed connection's ");
            public static Metrics.MetricCount metricTimeExecuted = new Metrics.MetricCount("HTTPExecutedTime", "All http executed connection's time");
            public async Task<string> GetMetrics()
            {
                return Metrics.metric.getPrometeusMetric();
                //            return 1;
            }

            public class Header
            {
                public string Key;
                public string? Value;
            }
            // List<Header> headers = new List<Header>();
            public override async Task ReceiveRequest(HttpContext httpContext)
            {
                if (httpContext.Request.Path.Value.ToLower().Contains("/selftest"))
                {
                    SetResponseType(httpContext, "text/plain");
                    var res =await owner.owner.owner.SelfTest();
                    if (res.Result)
                        await SetResponseStatusCode(httpContext, 200);
                    else
                        await SetResponseStatusCode(httpContext, 502);
                    await SetResponseContent(httpContext, res.Description);

                   
                    return;
                }
                if (httpContext.Request.Path.Value.Contains("/healthcheck"))
                {
                    SetResponseType(httpContext, "text/plain");
                    await SetResponseContent(httpContext, "OK");
//                    Thread.Sleep(10000);
                    return;
                }
                if (httpContext.Request.Path.Value.Contains("/metrics"))
                {
                    SetResponseType(httpContext, "text/plain");
                    await SetResponseContent(httpContext,await GetMetrics());
                    return;
                }

                    if (httpContext.Request.Path.Value.Contains("/swagger"))
                {
                    string json_body,content;
                    Logger.log("Get swagger request");
                    try
                    {
                        using (StreamReader sr = new StreamReader(this.owner.swaggerSpecPath))
                        {
                            json_body = sr.ReadToEnd();
                        }

                        content = GetSwaggerHtmlBody(json_body);
                    }
                    catch (Exception ex)
                    {
                        content= ex.Message;
                    }

                    SetResponseType(httpContext, "text/html");
                    await SetResponseContent(httpContext, content);
                    return;

                }
                Interlocked.Increment(ref CountOpened);
                metricCountOpened.Increment();
                DateTime time1=DateTime.Now;
                bool iError = false;
                HTTPReceiver.SyncroItem item = new SyncroItem();
                using (var stream = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                {
                    Logger.log("Receive request from {path}",Serilog.Events.LogEventLevel.Information, httpContext.Request.Path.Value);
                    string str = await stream.ReadToEndAsync();
                    if (owner.debugMode)
                        Logger.log("Stream reading:{o} ", Serilog.Events.LogEventLevel.Debug, "any", Thread.CurrentThread.ManagedThreadId);
                  //  headers.Clear();
                   item.UrlPath = httpContext.Request.Path.Value;
                    foreach(var head in httpContext.Request.Headers)
                    {
                        item.headers.Add(new Header() { Key = head.Key, Value = head.Value });   
                      /*  var st = head.Key;
                        var st1 = head.Value;*/
                    }
                    
                    try
                    {
                        if (!choosePath(item, httpContext.Request.Path.Value))
                        {
                            await SetResponseStatusCode(httpContext, 404);
                            return;
                        }

                        await owner.signal1(str, item);
                        /*    owner.signal1(str, item).ContinueWith(antecedent =>
                            {
                                iError = true;
                                metricCountOpened.Decrement();
                                metricErrors.Increment();
                                httpContext.Response.StatusCode = 404;
                                item.semaphore.Set();
                               // Console.WriteLine($"Error {antecedent}!");
                                //Console.WriteLine($"And how are you this fine {antecedent.Result}?");
                            },TaskContinuationOptions.OnlyOnFaulted);*/
                    }

                    catch (TaskCanceledException e77)
                    {
                        iError = true;
                        metricCountOpened.Decrement();
                        metricErrors.Increment();
                        metricTimeouts.Increment();
                        httpContext.Response.StatusCode = 429;
                        Logger.log("Final:ConnectionBusy error :{o} {input} {context}", e77, Serilog.Events.LogEventLevel.Error, "any", owner.owner, str, item.ctnx.GetPrefix("FinishError"));
                        owner.LogExtendedStat("Fail",str,item.ctnx);
                        return;

                    }
                    catch (TimeoutException e77)
                    {
                        iError = true;
                        metricCountOpened.Decrement();
                        metricErrors.Increment();
                        metricTimeouts.Increment();
                        httpContext.Response.StatusCode = 408;
                        Logger.log("Final:Timeout reached :{o} {input} {context}", e77, Serilog.Events.LogEventLevel.Error, "any", owner.owner, str, item.ctnx.GetPrefix("FinishError"));
                        owner.LogExtendedStat("Fail",str, item.ctnx);
                        return;

                    }
                    catch (ConnectionBusyException e77)
                    {
                        iError = true;
                        metricCountOpened.Decrement();
                        metricErrors.Increment();
                        metricTimeouts.Increment();
                        httpContext.Response.StatusCode = 429;
                        Logger.log("Final:ConnectionBusy error :{o} {input} {context}", e77, Serilog.Events.LogEventLevel.Error, "any", owner.owner, str,  item.ctnx.GetPrefix("FinishError"));
                        owner.LogExtendedStat("Fail",str, item.ctnx);
                        return;

                    }
                    catch (Exception e77)
                    {
                        
                        metricCountOpened.Decrement();
                        metricErrors.Increment();
                        Logger.log("Final:Error on input request  {input} {context}", e77, Serilog.Events.LogEventLevel.Error, str, item.ctnx.GetPrefix("FinishError"));
                        owner.LogExtendedStat("Fail",str, item.ctnx);
                        httpContext.Response.StatusCode = 503;
                        return;
                    }
                }
              /*  try
                {
                    await item.semaphore.WaitAsync().WaitAsync(TimeSpan.FromMilliseconds(TimeoutInMilliseconds));
                }
                catch(TimeoutException e77)
                {
                    iError = true;
                    metricCountOpened.Decrement();
                    metricErrors.Increment();
                    metricTimeouts.Increment();
                    httpContext.Response.StatusCode = 408;
                    Logger.log("Timeout reached :{o} {input}", Serilog.Events.LogEventLevel.Error, "any", owner.owner, item.answer);


                }*/
                if (iError)
                    return;
                Interlocked.Increment(ref item.unwait);

                if (owner.debugMode)
                {
                    Logger.log("Answer to client step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner.owner, item.answer);

                }
                metricCountExecuted.Increment();
                Interlocked.Increment(ref CountExecuted);
                // await httpContext.Request.Body.
                SetResponseType(httpContext, owner.ResponseType);
                await SetResponseContent(httpContext, item.answer);
                owner.LogExtendedStat("Success",item.answer,item.ctnx);
//                Logger.log("Send ans {ans} {ctn}", Serilog.Events.LogEventLevel.Information, item.answer, item.ctnx);
                metricCountOpened.Decrement();
                Interlocked.Decrement(ref CountOpened);
                metricTimeExecuted.Add(time1);
                //return base.ReceiveRequest(httpContext);
            }

            protected /*static*/   bool choosePath(SyncroItem item,string path)
            {
                if (owner.paths?.Count > 0)
                {
                    var nextStepName = owner.paths.FirstOrDefault(ii => path.Contains(ii.Path))?.Step;
                    if (nextStepName == null)
                    {
                        return false;
                        /* await SetResponseStatusCode(httpContext, 404);
                         // await SetResponseContent(httpContext, content);
                         return;
                        */
                    }
                    var nextStep = owner.owner.owner.steps.FirstOrDefault(ii => ii.IDStep == nextStepName);
                    if (nextStep == null)
                    {
                        Logger.log("not found path");
                        return false;
                        /*  await SetResponseStatusCode(httpContext, 404);
                          // await SetResponseContent(httpContext, content);
                          return;
                        */
                    }
                    Logger.log("path found on step {step}", Serilog.Events.LogEventLevel.Debug,"any", nextStep.IDStep);
                    item.initialStep = nextStep;
                }
                else
                    Logger.log("paths is empty");
                return true;
            }
        }
        static string template = "";


        public static string GetSwaggerHtmlBody(string json_body)
        {
            if(template =="")
            {
                using(StreamReader sr = new StreamReader("SwaggerTemplate.txt"))
                {
                    template = sr.ReadToEnd();
                }
            }
            return template.Replace("{0}", json_body);
        }
    }

}
