/******************************************************************
 * File: TBSLoggerSinc.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestJsonRazbor
{
    public class TbsLoggerSink : ILogEventSink
    {
        public static TbsLoggerSink LoggerSink = new TbsLoggerSink();

        public static readonly Serilog.Core.Logger Log = new LoggerConfiguration()
                    .WriteTo.Sink(LoggerSink)
                    .CreateLogger();

        public event EventHandler NewLogHandler;

        public TbsLoggerSink() { }

        public void Emit(LogEvent logEvent)
        {
#if DEBUG
            Console.WriteLine($"{logEvent.Timestamp}] {logEvent.MessageTemplate}");
#endif
            NewLogHandler?.Invoke(typeof(TbsLoggerSink), new LogEventArgs() { Log = logEvent });
        }
    }

    public class LogEventArgs : EventArgs
    {
        public LogEvent Log { get; set; }
    }
}
