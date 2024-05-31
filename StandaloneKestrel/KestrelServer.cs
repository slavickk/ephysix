/******************************************************************
 * File: KestrelServer.cs
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

using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Extensions.Logging;

namespace Kestrel
{

    class KestrelContext
    {
        public IFeatureCollection features;

        public KestrelContext(IFeatureCollection features)
        {
            this.features = features;
           
        }
    }

    class KestrelApplication : IHttpApplication<KestrelContext>
    {
        private readonly WebSocketMiddleware wsMiddleware;
//        KestrelServerImplement owner;
        public KestrelApplication(ILoggerFactory loggerFactory, KestrelServerImplement owner)
        {
  //          this.owner = owner;
            var wsOptions = new WebSocketOptions();
          //  wsOptions.
            wsMiddleware = new WebSocketMiddleware(owner.ContinueRequest, new OptionsWrapper<WebSocketOptions>(wsOptions),
                loggerFactory);
        }

        public KestrelContext CreateContext(IFeatureCollection contextFeatures)
        {
            return new KestrelContext(contextFeatures);
        }

        public void DisposeContext(KestrelContext context, Exception? exception)
        {
        }

        public async Task ProcessRequestAsync(KestrelContext context)
        {
            HttpContext httpContext = new DefaultHttpContext(context.features);
            await wsMiddleware.Invoke(httpContext);
        }
        /*
        private async Task ContinueRequest(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                var message = Encoding.ASCII.GetBytes("hello world");
                await socket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true,
                    CancellationToken.None);

                await socket.ReceiveAsync(new byte[4096], CancellationToken.None);
            }
            else
            {
                httpContext.Response.Headers.Add("Content-Type", new StringValues("text/plain"));
                await httpContext.Response.Body.WriteAsync(Encoding.ASCII.GetBytes("hello world\n"));
            }
        }*/
    }

    public class KestrelServerImplement
    {
        public async Task ContinueRequest(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                var message = Encoding.ASCII.GetBytes("hello world");
                await socket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true,
                    CancellationToken.None);

                await socket.ReceiveAsync(new byte[4096], CancellationToken.None);
            }
            else
            {
                await ReceiveRequest(httpContext);
            }
        }

        public virtual async Task ReceiveRequest(HttpContext httpContext)
        {
            SetResponseType(httpContext, "text/plain");
            await SetResponseContent(httpContext);
        }

        public static async Task SetResponseContent(HttpContext httpContext,string content= "hello world\n")
        {
            await httpContext.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(content));
        }
        public static async Task SetResponseStatusCode(HttpContext httpContext, int StatusCode)
        {
            httpContext.Response.StatusCode=StatusCode;
        }

        public static void SetResponseType(HttpContext httpContext,string type= "text/plain")
        {
            httpContext.Response.Headers.Add("Content-Type", new StringValues(type));
        }

        public async Task Start(int port,int maxConnectionLimits=100)
        {
            var serverOptions = new KestrelServerOptions();
            serverOptions.ListenAnyIP(port);
            serverOptions.Limits.MaxConcurrentConnections=maxConnectionLimits;
            
          //  serverOptions.C

            var transportOptions = new SocketTransportOptions();
//            var loggerFactory = new NullLoggerFactory();
            var loggerFactory = new LoggerFactory()
    .AddSerilog(Log.Logger);
            HttpClient client = new HttpClient();

            Microsoft.Extensions.Logging.ILogger logger = loggerFactory.CreateLogger("KestrelLogger");

            var transportFactory = new SocketTransportFactory(
                new OptionsWrapper<SocketTransportOptions>(transportOptions), loggerFactory);
            restart:
            using (var server = new KestrelServer(
                new OptionsWrapper<KestrelServerOptions>(serverOptions), transportFactory, loggerFactory))
            { 
                // server.Options.
                await server.StartAsync(new KestrelApplication(loggerFactory, this), CancellationToken.None);
            string url = $"http://localhost:{port}/healthcheck";
                while (0 == 0)
                {
                    await Task.Delay(1000);
                    try
                    {
                        var resp = await client.GetAsync(url);
                        if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            Log.Error("{\"Mess\":\"@StatusCode\"}", resp.StatusCode);

                            await server.StopAsync(CancellationToken.None);
                            await server.StartAsync(new KestrelApplication(loggerFactory, this), CancellationToken.None);

                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error on healthcheck");
                        server.Dispose();
                        goto restart;

                    }
                    //                resp.EnsureSuccessStatusCode();
                }

            }
            await Task.Delay(Timeout.Infinite);
        }
    }
}
