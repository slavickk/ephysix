// See https://aka.ms/new-console-template for more information
using ParserLibrary;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ConsolePerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
            //Configuration[]
            Pipeline pip;
            try
            {
                pip = Pipeline.load(args[0]);
                pip.steps[0].receiver.MocMode= true;
                pip.debugMode = false;
                // var suc = pip.SelfTest().GetAwaiter().GetResult();

                //                pip = pips[0];
            }
            catch (Exception e77)
            {
                Console.WriteLine("Error:" + e77.Message);
                return;
            }
            int cycle = 1000;
            DateTime time = DateTime.Now;
            for (int i = 0; i < cycle; i++)
            {
                DateTime time1 = DateTime.Now;
                pip.run().GetAwaiter().GetResult();
                if(i==0)        
                    Console.WriteLine($"{i}:{(DateTime.Now-time).TotalMilliseconds}");
            }
            Console.WriteLine($"{cycle}:{(DateTime.Now-time).TotalMilliseconds} to send:{StreamSender.Interval.TotalMilliseconds}");

        }
    }

}

