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
//using Microsoft.

namespace ParserLibrary
{
    public class HTTPReceiver : Receiver
    {
        public int port = 8080;

        KestrelServer server ;
        public HTTPReceiver()
        {
            server = new KestrelServer(this);
        }
        public override async Task startInternal()
        {
            await server.Start(port);
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
                var semaphoreCount = item.semaphore.Release();
                if (item.semaphore.CurrentCount == 0)
                {
                    int ii = 0;
                    Interlocked.Increment(ref item.srabot);
                    item.semaphore.Release();
                }

            }
            // return base.sendResponseInternal(response, context);
        }

        public async Task signal1(string body,SyncroItem semaphoreItem)
        {
            await signal(body, semaphoreItem);
            if (semaphoreItem.semaphore.CurrentCount == 0)
            {
                if (debugMode)
                {
                    Logger.log("Answer without response step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner, "-");

                }
                semaphoreItem.semaphore.Release();
            }
        }
        public class SyncroItem
        {
            public int srabot = 0;
            public int unwait = 0;
            public string answer="";
            public SemaphoreSlim semaphore= new SemaphoreSlim(0);
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
            public static Metrics.MetricCount metricCountExecuted = new Metrics.MetricCount("HTTPExecutedConnections", "All http executed connection's ");
            public static Metrics.MetricCount metricTimeExecuted = new Metrics.MetricCount("HTTPExecutedTime", "All http executed connection's time");
            public async Task<string> GetMetrics()
            {
                return Metrics.metric.getPrometeusMetric();
                //            return 1;
            }

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
                HTTPReceiver.SyncroItem item = new SyncroItem();
                using (var stream = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                {
                    string str = await stream.ReadToEndAsync();
                    owner.signal1(str,item);
                }
                await item.semaphore.WaitAsync();
                Interlocked.Increment(ref item.unwait);

                if (owner.debugMode)
                {
                    Logger.log("Answer to client step:{o} {input}", Serilog.Events.LogEventLevel.Debug, "any", owner.owner, item.answer);

                }
                metricCountExecuted.Increment();
                Interlocked.Increment(ref CountExecuted);
                // await httpContext.Request.Body.
                SetResponseType(httpContext, "application/json");
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
