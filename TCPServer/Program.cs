using System;
using System.Threading;
using System.Threading.Tasks;
using ParserLibrary;
using PluginBase;
using UniElLib;
using Serilog;
using Serilog.Formatting.Compact;

namespace TCPServer
{
    class Program
    {
        private const string twfaHost = "192.168.75.166";
        private const int twfaPort = 21003;
        private const int frame = 6;


        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            var sender = new ParserLibrary.TICSender() { ticFrame = frame, twfaHost = twfaHost, twfaPort = twfaPort };
            var reciever = new TICReceiver()
            {
                port = 5000,
                ticFrame = frame,
                stringReceived = (s, o) => sender.send(s, new ContextItem { context = o })
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
        }
    }
}