﻿/******************************************************************
 * File: Program.cs
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
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace TCPServer
{
    class Program
    {
        private const string dummySystem3Host = "10.74.28.30";
        private const int dummySystem3Port = 5595;
        private const int frame = 6;
        
        private static Serilog.ILogger CreateSerilog(string ServiceName)
        {
            LoggerConfiguration log = null;
            log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithExceptionDetails()
                // .Enrich.WithProperty("TraceID", ((System.Diagnostics.Activity.Current != null) ? System.Diagnostics.Activity.Current.TraceId.ToString() : "-"))
                // .Enrich.WithProperty("ThreadID", Thread.CurrentThread.ManagedThreadId)
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithProperty("Service", ServiceName)
                .Enrich.WithSpan(new SpanOptions()
                {
                    LogEventPropertiesNames = new SpanLogEventPropertiesNames
                        { TraceId = "TraceId", SpanId = "SpanId", ParentId = "ParentId" },
                })
                .Enrich.FromLogContext();
                // log = log.WriteTo.Async(writeTo => writeTo.Console(new RenderedCompactJsonFormatter()));
                log = log.WriteTo.Console(theme: AnsiConsoleTheme.Code, applyThemeToRedirectedOutput: true,
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level:u3} TraceId: {TraceId}] [{Properties}] {Message:lj}  {NewLine} {Exception}");
          
            return log.CreateLogger();
            // <<#<<#<<
        }

        private static async Task Main(string[] args)
        {
            Log.Logger = CreateSerilog("TCPServer");

/*            var sender = new ParserLibrary.DummyProtocol1Sender() { dummyProtocol1Frame = frame, dummySystem3Host = dummySystem3Host, dummySystem3Port = dummySystem3Port };
            var reciever = new DummyProtocol1Receiver()
            {
                port = 15001,
                dummyProtocol1Frame = frame
            };
            reciever.stringReceived = async (s, o) =>
            {
                var context = new ContextItem { context = o };
                var resp = await sender.send(s, context);
                await reciever.sendResponse(resp, context);
            };
            var cancellationTokenSource = new CancellationTokenSource();
            Task serving;
            try
            {
                serving = reciever.start();
                Console.ReadKey();
                cancellationTokenSource.Cancel();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }
            finally
            {
                Log.Information("Close Programm");
            }
*/
        }
    }
}