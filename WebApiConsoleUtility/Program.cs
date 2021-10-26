using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParserLibrary;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiConsoleUtility
{
    public class Program
    {

        public static int MaxConcurrentRequests = -1;
        //Request queue length limit
        public static int RequestQueueLimit = -1;
        static Pipeline pip;
        public static void Main(string[] args)
        {
            /*            var req = Environment.GetEnvironmentVariable("MAX_CONCURRENT_REQUEST");
                        if (req != null)
                        {
                            MaxConcurrentRequests = Convert.ToInt32(req);
                        }
                        req = Environment.GetEnvironmentVariable("REQUEST_QUEUE_LIMIT");
                        if (req != null)
                        {
                            RequestQueueLimit = Convert.ToInt32(req);
                        }*/

            /*            RexMain.SetAdditionalLogLevel = (action) => {
                            if (action == 0)
                                levelSwitch.MinimumLevel = LogEventLevel.Debug;
                            else
                                levelSwitch.MinimumLevel = LogEventLevel.Warning;
                        };
                        levelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);*/
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;     
            AssemblyLoadContext.Default.Unloading += Default_Unloading;
            string LogPath =Environment.GetEnvironmentVariable("LOG_PATH");
            string YamlPath = Environment.GetEnvironmentVariable("YAML_PATH");
            string LogLevel = Environment.GetEnvironmentVariable("LOG_LEVEL");
            LogEventLevel defLevel = LogEventLevel.Debug;
            object outVal;
            string levelInfo = "";
            if (LogLevel == null)
                levelInfo = "LOG_LEVEL variable not set.Set default value " + Enum.GetName<LogEventLevel>(defLevel) + ". Available values : Verbose, Debug, Information, Warning, Error, Fatal.";
            else
            if (Enum.TryParse(typeof(LogEventLevel), LogLevel, true, out outVal))
                defLevel = (LogEventLevel)outVal;
            else
                levelInfo = "LOG_LEVEL variable is not correct ("+LogLevel+").Set default value " + Enum.GetName<LogEventLevel>(defLevel) + ". Available values : Verbose, Debug, Information, Warning, Error, Fatal.";

            ParserLibrary.Logger.levelSwitch = new LoggingLevelSwitch(defLevel);

            if (LogPath == null )
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(ParserLibrary.Logger.levelSwitch)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Mashine", System.Environment.MachineName)
//        .Enrich.With<>
        .WriteTo.Console(new RenderedCompactJsonFormatter())
        .CreateLogger();
            else
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(ParserLibrary.Logger.levelSwitch)
        .Enrich.FromLogContext()
        .WriteTo.File(new CompactJsonFormatter(), LogPath).CreateLogger();

            }
            
            if (levelInfo != "")
                Log.Error(levelInfo);
//            ParserLibrary.Logger.levelSwitch.MinimumLevel = LogEventLevel.Debug;
            try
            {
                if(YamlPath == null)
                {
                    Log.Fatal("YAML_PATH environment variable not set.Set default config file placed at /app/Data/model.yml (saved in container)");
                    YamlPath = "/app/Data/model.yml";
                }
                if(!File.Exists(YamlPath))
                {
                    var cc = Directory.GetFiles("/app/Data", "*.*");
                    Log.Fatal(YamlPath +"not accessible.");
                    var dir = Directory.GetCurrentDirectory();
                    var dirs1=Directory.GetDirectories(dir);
                    return;

                }
                Log.Information("... Parsing " + YamlPath);
                pip = Pipeline.load(YamlPath);
                Log.Information("Parsing done.Making self test.");
                var suc = pip.SelfTest().GetAwaiter().GetResult();
                if (suc)
                {
                    Log.Information("Self test OK. Run pipeline.");
                    Action action = async () => { await pip.run(); };
                    new TaskFactory().StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).ContinueWith((runner) => { Log.Information("Pipeline execution stopped.Terminating application..."); System.Diagnostics.Process.GetCurrentProcess().Kill(); });
                }
                else
                {
                    Log.Fatal("Self test failed. Terminate execution.");
                    return;

                }


                Log.Information("Starting Integrity Utility web host ");
                if(LogPath == null)
                    Log.Information("Environment variable LOG_PATH undefined.Logging to standard stdout/stderr.");
                else
                    Log.Information("Logging to "+LogPath+".");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Integrity Utility Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }


        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("aa1");
            Log.Information("11 Signal detected");
           
//            throw new NotImplementedException();
        }

        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            Console.WriteLine("aa2");
            Log.Information("Signal detected");
//            throw new NotImplementedException();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
        .UseSerilog();
    }
}
