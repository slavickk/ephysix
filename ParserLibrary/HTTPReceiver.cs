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
//using Microsoft.

namespace ParserLibrary
{
    public class HTTPReceiver : Receiver
    {
        public int port = 8080;

        KestrelServer server ;
        public string ResponseType = "application/json";
        public HTTPReceiver()
        {
            server = new KestrelServer(this);
        }
        public override async Task startInternal()
        {
            await server.Start(port,1000);
           // return base.startInternal();
        }
        //namespace Kestrel;
        public override async Task sendResponseInternal(string response, object context)
        {
            if (debugMode)
            {
                Logger.log("Send response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner, response);
            }

            var item = context as SyncroItem;
            if (item != null)
            {
                item.answer = response;
                Interlocked.Increment(ref item.srabot);
                item.semaphore.Set();//.Release();
                /*if (item.semaphore.CurrentCount == 0)
                {
                    int ii = 0;
                    Interlocked.Increment(ref item.srabot);
                    item.semaphore.Release();
                }*/

            }
            // return base.sendResponseInternal(response, context);
        }

        public async Task signal1(string body,SyncroItem semaphoreItem)
        {
            await signal(body, semaphoreItem);
           // semaphoreItem.semaphore.
            if (semaphoreItem.srabot==0)
            {
                if (debugMode)
                {
                    Logger.log("Answer without response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner, "-");

                }
                semaphoreItem.semaphore.Set();
            }
        }
        public class SyncroItem
        {
            public int srabot = 0;
            public int unwait = 0;
            public string answer="";
            public List<KestrelServer.Header> headers = new List<KestrelServer.Header>(); 
            public AsyncAutoResetEvent semaphore = new AsyncAutoResetEvent();
        }
        public class KestrelServer: Kestrel.KestrelServerImplement
        {
            HTTPReceiver owner;
            public KestrelServer(HTTPReceiver owner)
            {
                this.owner = owner;
            }


            public static long CountExecuted = 0;
            public static long CountOpened = 0;
            public static Metrics.MetricCount metricCountOpened = new Metrics.MetricCount("HTTPOpenConnectCount", "opened http connection at same time ");
            public static Metrics.MetricCount metricErrors = new Metrics.MetricCount("HTTPErrorCount", "Error http request ");
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
                if (httpContext.Request.Path.Value.Contains("/metrics"))
                {
                    SetResponseType(httpContext, "text/plain");
                    await SetResponseContent(httpContext,await GetMetrics());
                    return;
                }

                    if (httpContext.Request.Path.Value.Contains("/swagger"))
                {
                    string json_body;
                    using (StreamReader sr = new StreamReader(@"C:\Users\ygasnikov\source\repos\swagger-to-html-standalone-master\example\swagger.json"))
                    {
                        json_body = sr.ReadToEnd();
                    }

                    string content=GetSwaggerHtmlBody(json_body);
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
                    string str = await stream.ReadToEndAsync();
                    if (owner.debugMode)
                        Logger.log("Stream reading:{o} ", Serilog.Events.LogEventLevel.Debug, "any", Thread.CurrentThread.ManagedThreadId);
                  //  headers.Clear();
                    foreach(var head in httpContext.Request.Headers)
                    {
                        item.headers.Add(new Header() { Key = head.Key, Value = head.Value });   
                      /*  var st = head.Key;
                        var st1 = head.Value;*/
                    }
                    
                    try
                    {
                        owner.signal1(str, item).ContinueWith(antecedent =>
                        {
                            iError = true;
                            metricCountOpened.Decrement();
                            metricErrors.Increment();
                            httpContext.Response.StatusCode = 404;
                            item.semaphore.Set();
                           // Console.WriteLine($"Error {antecedent}!");
                            //Console.WriteLine($"And how are you this fine {antecedent.Result}?");
                        },TaskContinuationOptions.OnlyOnFaulted);
                    }
                    catch(Exception e77)
                    {
                        metricCountOpened.Decrement();
                        metricErrors.Increment();
                        httpContext.Response.StatusCode = 404;
                        return;
                    }
                }

                await item.semaphore.WaitAsync();
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
                metricCountOpened.Decrement();
                Interlocked.Decrement(ref CountOpened);
                metricTimeExecuted.Add(time1);
                //return base.ReceiveRequest(httpContext);
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
