using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Core;

using System.IO.Pipelines;
using System.Runtime.Loader;
using CamundaInterface;
Prepare();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//Prepare();
app.Run();


// Prepare();

static void Prepare()
{
    /*AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
    AssemblyLoadContext.Default.Unloading += Default_Unloading;*/
    ConsulKV.CONSUL_ADDR = (Environment.GetEnvironmentVariable("CONSUL_ADDR") == null) ? "http://127.0.0.1:8500" : Environment.GetEnvironmentVariable("CONSUL_ADDR");

//    ConsulKV.CONSUL_ADDR=
    string LogPath = Environment.GetEnvironmentVariable("LOG_PATH");
    //string YamlPath = Environment.GetEnvironmentVariable("YAML_PATH");
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
        levelInfo = "LOG_LEVEL variable is not correct (" + LogLevel + ").Set default value " + Enum.GetName<LogEventLevel>(defLevel) + ". Available values : Verbose, Debug, Information, Warning, Error, Fatal.";

    //  ParserLibrary.Logger.levelSwitch = new LoggingLevelSwitch(defLevel);

    if (LogPath == null)
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.ControlledBy(new Serilog.Core.LoggingLevelSwitch())
.Enrich.FromLogContext()
.Enrich.WithProperty("Machine", System.Environment.MachineName)
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
        Log.Information(" Run executors.");
        Task.Run( async () =>
        {
            while (0 == 0)
            {
                try
                {
                    Log.Information(" Fetching...");

                    await CamundaExecutor.fetch(new string[] { "integrity_utility", "to_dict_sender", "url_crowler" });
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    Log.Information("Restart fetch");
                }
            }
        });
        
     //   new TaskFactory().StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).ContinueWith((runner) => { Log.Information("Camunda  execurs stopped.Terminating application..."); System.Diagnostics.Process.GetCurrentProcess().Kill(); });

        Log.Information("Starting camunda executors web host ");
        if (LogPath == null)
            Log.Information("Environment variable LOG_PATH undefined.Logging to standard stdout/stderr.");
        else
            Log.Information("Logging to " + LogPath + ".");
        //  app.Run();

    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "camunda executors Host terminated unexpectedly");
        return;
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

