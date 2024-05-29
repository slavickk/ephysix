using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ParserLibrary;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Sensitive;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Enrichers.Span;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Enrichers.OpenTracing;
using Serilog.Sinks.SystemConsole.Themes;

namespace WebApiConsoleUtility
{
    public class Program
    {
        public static int MaxConcurrentRequests = -1;

        //Request queue length limit
        public static int RequestQueueLimit = -1;
        public static Pipeline pip;
        static bool IgnoreAll = false;

        private static Serilog.ILogger CreateSerilog(string ServiceName, LoggingLevelSwitch levelSwitch, bool isAsync,
            bool LogHttpRequest, bool maskedSensitive = false)
        {
            LoggerConfiguration log = null;
            log = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                //            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithExceptionDetails()
                // .Enrich.WithProperty("TraceID", ((System.Diagnostics.Activity.Current != null) ? System.Diagnostics.Activity.Current.TraceId.ToString() : "-"))
                // .Enrich.WithProperty("ThreadID", Thread.CurrentThread.ManagedThreadId)
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithProperty("Service", ServiceName)
                .Filter.ByExcluding(c =>
                    !LogHttpRequest && c.Properties.ContainsKey("SourceContext") && c.Exception == null &&
                    c.Level != LogEventLevel.Error && c.Level != LogEventLevel.Fatal &&
                    c.Level != LogEventLevel.Warning)
                /*                 .Filter.ByExcluding(c =>
                                 c.Properties.ContainsKey("Method") && c.Properties["Method"].
                                 c.Properties.ContainsKey("SourceContext")
                                                  !LogHealthAndMonitoring &&
                                                  (c.Properties.Any(p => p.Value.ToString().Contains("ConsulHealthCheck")) || c.Properties.Any(p => p.Value.ToString().Contains("getMetrics")))
                                 )*/
                .Enrich.WithSpan(new SpanOptions()
                {
                    LogEventPropertiesNames = new SpanLogEventPropertiesNames
                        { TraceId = "TraceId", SpanId = "SpanId", ParentId = "ParentId" },
                })
                .Enrich.FromLogContext();
            if (isAsync)
            {
#if DEBUG

                log = log.WriteTo.Async(writeTo => writeTo.File("errors.log", LogEventLevel.Information,
                        "[{Timestamp:dd/MM/yy HH:mm:ss.ffff} {Level:u3} TraceId: {TraceId}] [{Properties}] {Message:lj}  {NewLine} {Exception}",
                        retainedFileCountLimit: 3))
                    .WriteTo.Async(writeTo =>
                        writeTo.Console(theme: AnsiConsoleTheme.Code, applyThemeToRedirectedOutput: true,
                            outputTemplate:
                            "[{Timestamp:dd/MM/yy HH:mm:ss.ffff} {Level:u3} TraceId: {TraceId}] [{Properties}] {Message:lj}  {NewLine} {Exception}"));
#else
                log = log.WriteTo.Async(writeTo => writeTo.Console(new RenderedCompactJsonFormatter()));
#endif
            }

            else
                log = log.WriteTo.Console(new RenderedCompactJsonFormatter());

            if (maskedSensitive)
                log = log.Enrich.WithSensitiveDataMasking(
                    options =>
                    {
                        options.MaskingOperators = new List<IMaskingOperator>
                        {
                            new EmailAddressMaskingOperator(),
                            new IbanMaskingOperator(),
                            new CreditCardMaskingOperator()
                            // etc etc
                        };
                    });
            return log.CreateLogger();
            // <<#<<#<<
        }


        public static async Task Main(string[] args)
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
            string LogPath = Environment.GetEnvironmentVariable("LOG_PATH");
            string YamlPath = Environment.GetEnvironmentVariable("YAML_PATH");
            string LogLevel = Environment.GetEnvironmentVariable("LOG_LEVEL");
            string DEBUG_MODE = Environment.GetEnvironmentVariable("DEBUG_MODE");
            string LOG_HISTORY_MODE = Environment.GetEnvironmentVariable("LOG_HISTORY_MODE");
            string LOG_EXT_STAT = Environment.GetEnvironmentVariable("LOG_EXT_STAT");
            Pipeline.AgentHost = Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST");
            string sport = Environment.GetEnvironmentVariable("JAEGER_AGENT_PORT");
            string SAVE_CONTEXT = Environment.GetEnvironmentVariable("JAEGER_SAVE_CONTEXT");
            if (string.IsNullOrEmpty(sport))
                Pipeline.AgentPort = -1;
            else
                Pipeline.AgentPort = Convert.ToInt32(sport);
            if (!string.IsNullOrEmpty(SAVE_CONTEXT))
                Pipeline.saveContext = true;
            else
                Pipeline.saveContext = false;
            Pipeline.ServiceAddr = Environment.GetEnvironmentVariable(Pipeline.EnvironmentVar);
            if (Pipeline.ServiceAddr == null)
                Pipeline.ServiceAddr = "localhost:44352";
            LogEventLevel defLevel = LogEventLevel.Information;
            bool LogHealthAndMonitoring = (Environment.GetEnvironmentVariable("LOG_HELTH_CHECK") != null);

            // TODO: add Enrich with error details in the Serilog.Exception initialization

            object outVal;
            string levelInfo = "";
            if (!IgnoreAll)
            {
                if (LogLevel == null)
                    levelInfo = "LOG_LEVEL variable not set.Set default value " +
                                Enum.GetName<LogEventLevel>(defLevel) +
                                ". Available values : Verbose, Debug, Information, Warning, Error, Fatal.";
                else if (Enum.TryParse(typeof(LogEventLevel), LogLevel, true, out outVal))
                    defLevel = (LogEventLevel)outVal;
                else
                    levelInfo = "LOG_LEVEL variable is not correct (" + LogLevel + ").Set default value " +
                                Enum.GetName<LogEventLevel>(defLevel) +
                                ". Available values : Verbose, Debug, Information, Warning, Error, Fatal.";

                ParserLibrary.Logger.levelSwitch = new LoggingLevelSwitch(defLevel);
                Log.Logger = CreateSerilog("IU", ParserLibrary.Logger.levelSwitch,
                    Environment.GetEnvironmentVariable("SYNC_LOG") == null,
                    Environment.GetEnvironmentVariable("LOG_HTTP_REQUESTS") != null);
                    Log.Information($"Service url on {Pipeline.ServiceAddr}");
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUFF_DIR")))
                    Log.Information("save error messages is off");
                else
                    Log.Information("save error messages is on");

                if (!IgnoreAll)
                {
                    if (levelInfo != "")
                        Log.Error(levelInfo);
                    //            ParserLibrary.Logger.levelSwitch.MinimumLevel = LogEventLevel.Debug;
                    try
                    {
                        if (YamlPath == null)
                        {
                            YamlPath = "/app/Data/ACS_TW.yml";
                            Log.Fatal(
                                $"YAML_PATH environment variable not set.Set default config file placed at {YamlPath} (saved in container)");
                        }

                        if (!File.Exists(YamlPath))
                        {
                            var cc = Directory.GetFiles("/app/Data", "*.*");
                            Log.Fatal(YamlPath + "not accessible.");
                            var dir = Directory.GetCurrentDirectory();
                            var dirs1 = Directory.GetDirectories(dir);
                            return;
                        }

                        if (Pipeline.AgentPort > 0)
                            Log.Information($"set jaeger host {Pipeline.AgentHost} on port {Pipeline.AgentPort}");
                        else
                            Log.Information($"jaeger host not set");

                        var yamlFullPath = Path.GetFullPath(YamlPath);
                        Log.Information("... Parsing " + yamlFullPath);
                        try
                        {
                            pip = Pipeline.load(yamlFullPath, Assembly.GetAssembly(typeof(HTTPReceiver)));
                        }
                        catch (Exception e66)
                        {
                            Log.Fatal($"Error parsing {e66}");
                            return;
                        }

                        if (DEBUG_MODE != null)
                        {
                            pip.debugMode = true;
                            Log.Information("Set debugMode ");
                        }

                        if (LOG_HISTORY_MODE != null)
                        {
                            Pipeline.isSaveHistory = true;
                            Log.Information("Set logHistoryMode ");
                        }
                        if (LOG_EXT_STAT != null)
                        {
                            Pipeline.isExtendingStat = true;
                            Log.Information("Set extendedStat ");
                        }

                        Log.Information("Parsing done");
                        if (!pip.skipSelfTest)
                        {
                            Log.Information("Making self test.");
                            var res = await pip.SelfTest();
                            var suc = res.Result;
                            if (suc)
                            {
                                Log.Information("Self test OK. Run pipeline.");
                                pip.run().ContinueWith((runner) =>
                                {
                                    Log.Information(
                                        $"Pipeline execution stopped: IsFaulted={runner.IsFaulted}, exception: {runner.Exception?.ToString() ?? "None"}. Terminating application...");
                                    Console.WriteLine(runner.Exception?.ToString());
                                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                                    return;
                                });
                            }
                            else
                            {
                                Log.Error(
                                    "Self test failed. Pipeline won't be started. Running just the Integration Utility web host.");
                                //                        return;
                            }
                        }
                        else
                        {
                            // TODO: deduplicate the code that runs the pipeline
                            Log.Information("Running the pipeline without self-test because SkipSelfTest is true.");
                            pip.run().ContinueWith((runner) =>
                            {
                                Log.Information(
                                    "Pipeline execution stopped with result {a} exception {exc}.Terminating application...",
                                    runner.IsFaulted, runner.Exception?.ToString());
                                Console.WriteLine(runner.Exception?.ToString());
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                            });
                        }

                        Log.Information("Starting Integrity Utility web host ");
                        if (LogPath == null)
                            Log.Information(
                                "Environment variable LOG_PATH undefined.Logging to standard stdout/stderr.");
                        else
                            Log.Information("Logging to " + LogPath + ".");
                        await CreateHostBuilder(args).Build().RunAsync();
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
                else
                {
                    Log.Information("Starting Integrity Utility web host ");
                    await CreateHostBuilder(args).Build().RunAsync();
                }
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            //Console.WriteLine("aa1");
            Log.Information("11 Signal detected");

            //            throw new NotImplementedException();
        }

        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            //Console.WriteLine("aa2");
            Log.Information("Signal detected");
            //            throw new NotImplementedException();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog();
    }
}