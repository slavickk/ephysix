using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Core;

using System.IO.Pipelines;
using System.Runtime.Loader;
using CamundaInterface;
using WebApiCamundaExecutors;
using System.Security.Cryptography;
using System.Text.Json;
using ParserLibrary;
//CSScripterExecutor.testScript().GetAwaiter().GetResult();
//Test().GetAwaiter().GetResult();
Prepare();
/*var builder = WebApplication.CreateBuilder(args);

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
app.Run();*/
CreateHostBuilder(args).Build().Run();


// Prepare();
static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureLogging(loggingBuilder =>
    {
        /*                builder.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId |
                                                                                      ActivityTrackingOptions.ParentId |
                                                                                      ActivityTrackingOptions.TraceId);

                        */
        loggingBuilder.Configure(options =>
        {
            options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId;
        });
    })
               .UseSerilog() // <-- Add this line
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    
static async Task Test()
{
    string json = @"{
  ""Name"": ""j4iwkkbWO5rAAQz8YMWNto_gGpFJWgCq4wv"",
  ""Description"": """",
  ""ReadOnce"": false,
  ""Fields"": [
    {
      ""Type"": ""String"",
      ""Detail"": """",
      ""Name"": ""xDyBOhB8UH0bMWYeIcIDggB9NEEpxvvNPjYjqWzYXLqKnp7aZZqY7XIK""
    }
  ],
  ""Key"": ""DTjUJbgJQ3yKqTiDobyhMdOsdpctGWjJthHVvP8g2QJ22yQmjqCQcvcDHdoeMK0wS_0xU_BbYvKyPDSXYhy0K"",
  ""Type"": ""DICTIONARY""
}";
    json = @"{""Name"":""currencyrates"",""Description"":""Dictionary exported from ETL"",""Fields"":[{""Name"":""numericcode"",""Type"":""String"",""Detail"":""Exported from ETL package""},{""Name"":""rate"",""Type"":""Double"",""Detail"":""Exported from ETL package""}],""Key"":""numericcode"",""Type"":""DICTIONARY""}
";
    var FID = "Test";
    var baseAddr = @"https://referenceDataLoader.service.dc1.consul:16666";
    HttpClient client = new HttpClient();
    var url1 = $"/api/v0/schema/dict/{FID}";
    //                                "http://192.168.75.213:16666/api/v0/schema/dict/TEST"
    Uri uri1 = new Uri(new Uri(baseAddr), url1);
    var dict = JsonSerializer.Deserialize<SendToRefDataLoader.Dictionary>(json);

    string dict1 = JsonSerializer.Serialize<SendToRefDataLoader.Dictionary>(dict);
    /*                    HttpContent content = new StringContent(dict1);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");  //  "application/json";
                        var res = await client.PostAsync(uri1, content);
    */

    var options = new JsonSerializerOptions();
    options.PropertyNameCaseInsensitive = false;
    Console.WriteLine($"Send request on addr {uri1.ToString()} sended {dict1}");
    var res = await client.PostAsJsonAsync<SendToRefDataLoader.Dictionary>(uri1, dict, options);

    var ans=await res.Content.ReadAsStringAsync();
}
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

        .Filter.ByExcluding(c => c.MessageTemplate.Text.Contains("GetHealthCheck"))
         .Filter.ByExcluding(c =>
         c.Properties.Any(p => p.Value.ToString().Contains("ConsulHealthCheck"))
         )

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
    var env_var = Environment.GetEnvironmentVariable("CRON_STRING");
    if (env_var != null)
    {
        CSScripterExecutor.Start("Data/DictCurrencyRates.cs", env_var);
    }
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

                    await CamundaExecutor.fetch(new string[] { "integrity_utility", "to_dict_sender", "url_crowler", "to_exec_proc", "FimiConnector" });
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
//        Log.CloseAndFlush();
    }
}

