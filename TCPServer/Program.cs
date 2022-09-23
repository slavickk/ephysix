using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Compact;

namespace TCPServer
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            var reciever = new DummyProtocol1Reciever(IPAddress.Any, 5000, 12);
            var cancellationTokenSource = new CancellationTokenSource();
            var serving = reciever.StartServing(cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            await serving;
            Log.Information("Close Programm");
        }
    }
}