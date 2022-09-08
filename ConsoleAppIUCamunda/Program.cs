using System;
//using System.Reflection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using CamundaInterface;
using Microsoft.Extensions.Configuration.Json;

namespace ConsoleAppIUCamunda
{
    class Program
    {
        async static Task<int> Main(string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
   .AddEnvironmentVariables()
   .AddCommandLine(args)
   .Build();

            /*            JsonSender sender = new JsonSender() { url = "http://192.168.75.160:25080/api/Streams/LoadStream1" };
                        var answer=sender.internSend("{\"stream\":\"loginEnter\",\"originalTime\":\"2020-12-19T21:06:35.2387735+05:00\",\"login\":\"+79222310645\"}").Result;*/
            /*            var bytes=Convert.FromBase64String("M0RTLzNSSUluZGljYXRvcj0QM0RTL0V4cFRpbWVJbnRlcnZhbD02MBBFeHQvTmV0d29yaz0xMQ==");

                        string value = System.Text.Encoding.UTF8.GetString(bytes);*/
            /*            var pip2 = new Pipeline();
                        pip2.Save( @"aa3.yml");*/
            /*     var tt=typeof(LongLifeRepositorySender).IsSubclassOf(typeof(Sender));
                 var tt1 = typeof(LongLifeRepositorySender).IsAssignableTo(typeof(Sender));
                 // typeof(ComparerForValue).GenericTypeParameters
                 //           typeof(ComparerForValue).IsAssignableTo(typeof(ComparerV));*/
            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);

            Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
            Log.Information("Start log");
            while (0 == 0)
            {
                try
                {
                    await CamundaExecutor.fetch(new string[] { "integrity_utility", "to_dict_sender" });
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    Log.Information("Restart fetch");
                }
            }
            Console.ReadKey();
            Log.Information("Close program");
            //Configuration[]
            return 0;
        }
    }
}
