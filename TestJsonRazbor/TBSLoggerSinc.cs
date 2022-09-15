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
