/******************************************************************
 * File: Pipeline.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Concurrent;
using PluginBase;
using Plugins;
using UniElLib;
using DotLiquid;
using System.Xml.Linq;
using Parlot.Fluent;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace ParserLibrary;

public class Pipeline:ILiquidizable
{
    public ReplaySaver saver = null;

    private static readonly ActivitySource Activity = new("Pipeline");
    [YamlIgnore]
    public AbstrParser.UniEl lastExecutedEl = null;
    public static Metrics metrics = new Metrics();


    [YamlIgnore]
    Int64 sizeOfSavedContextInMB = 0;


    public Int64 LimitOfSavedContextInMB = 10;
    async Task saveTraceContext(string fileName,string body)
    {
        if(!string.IsNullOrEmpty(traceSaveDirectory))
        {
            if(!Directory.Exists(traceSaveDirectory))
                Directory.CreateDirectory(traceSaveDirectory);
            using(StreamWriter sw = new StreamWriter(Path.Combine(traceSaveDirectory, fileName))) 
            {
                await sw.WriteAsync(body);
            }       
        }
    }


    public const string EnvironmentVar = "NOMAD_ADDR_http_ctrl";
    public static string ServiceAddr = "localhost:44352";

    public static bool saveContext = false;
    public string SaveContext(string body,string extension="txt")
    {
        if (saveContext)
        {
            Guid myuuid = Guid.NewGuid();
            string fileName = $"{myuuid.ToString().Replace("-", "")}.{extension}";
            Task.Run(async () => await saveTraceContext(fileName, body)).ContinueWith((t1) =>
            {
                // Console.WriteLine(t1.Result);
            });

            return $"http://{ServiceAddr}/api/Context/GetContext?fileName={fileName.Replace(".", "_")}";
        }
        else
            return $"http://{ServiceAddr}/api/Context/GetContext";
    }

    /// <summary>
    /// Directory which save log context files
    /// </summary>
    public static string traceSaveDirectoryStat = "C:\\D\\Store";
    public string traceSaveDirectory = "C:\\D\\Store";

    public static bool isSaveHistory = false;
    public static bool isExtendingStat = false;
    public string hashWord { get; set; } = "QWE123";
    public string pipelineDescription = "Pipeline example";
    [YamlIgnore]
    public string tempMocData ="";
    public Step[] steps { get; set; } = new Step[] { };// new Step[] { new Step() };
    [YamlIgnore]
    public ConcurrentQueue<Scenario> scenarios { get; set; }= new ConcurrentQueue<Scenario>();
    public bool debugMode
    {
        set
        {
            foreach (Step step in steps)
            {
                step.debugMode = value;
                /*if (step.sender != null)
                        step.sender.debugMode = value;*/
            }
        }
    }

    /// <summary>
    /// If true, then the self test should be skipped.
    /// skipSelfTest does not affect the behavior of the pipeline,
    /// it is supposed to be used by upstream code that controls the pipeline
    /// and decides whether to call SelfTest() or not.
    /// </summary>
    public bool skipSelfTest = false;

    //public AbstrParser.UniEl rootElement = null;
    /// <summary>
    /// Performs a self-test on the pipeline by calling the SelfTest() method of each sender that implements ISelfTested.
    /// </summary>
    /// <returns>A boolean value indicating whether the self-test passed or failed.</returns>
    public async Task<(bool Result,string Description)> SelfTest()
    {
        // check that sender implemented ISelfTested
/*        if (steps[0].sender is not ISelfTested testableSender)
            return false; // treating this as a failed self test*/
        bool retValue = true;
        string description = "";
        foreach (var step in steps)
        {
            if (step.isender != null)
            {
                var testableSender = step.isender as ISelfTested;
                if (testableSender != null)
                {
                    Logger.log("SelfTest {Step} ", Serilog.Events.LogEventLevel.Information, "any", step.IDStep);

                    var res1 = await testableSender.isOK();
                    if (res1.Item3 == null)
                    {
                        Logger.log("{Item}.Results:{Res}", Serilog.Events.LogEventLevel.Information, "any", res1.Item2.ToString(), (res1.Item1 ? "OK" : "Fail"));
                        description += $"{res1.Item2} returns {(res1.Item1 ? "OK" : "Fail")}\r\n";
                    }
                    else
                    {
                        Logger.log("{Item}.Results:{Res}", res1.Item3, Serilog.Events.LogEventLevel.Information, "any", res1.Item2.ToString(), (res1.Item1 ? "OK" : "Fail"));
                        description += $"{res1.Item2} returns {(res1.Item1 ? "OK" : "Fail")}\r\n";
                    }
                    if (!res1.Item1)
                        retValue = false;
                }
            }
            if (step.sender != null && string.IsNullOrEmpty(step.IDResponsedReceiverStep))
            {
                var testableSender = step.sender as ISelfTested;
                if (testableSender != null && !step.sender.MocMode)
                {
                    Logger.log("SelfTest {Step} ", Serilog.Events.LogEventLevel.Information, "any", step.IDStep);

                    var res1 = await testableSender.isOK();
                    if (res1.Item3 == null)
                    {
                        Logger.log("{Item}.Results:{Res}", Serilog.Events.LogEventLevel.Information, "any", res1.Item2.ToString(), (res1.Item1 ? "OK" : "Fail"));
                        description += $"{res1.Item2} returns {(res1.Item1 ? "OK" : "Fail")}\r\n";

                    }
                    else
                    {
                        Logger.log("{Item}.Results:{Res}", res1.Item3, Serilog.Events.LogEventLevel.Information, "any", res1.Item2.ToString(), (res1.Item1 ? "OK" : "Fail"));
                        description += $"{res1.Item2} returns {(res1.Item1 ? "OK" : "Fail")}\r\n";
                    }
                    if (!res1.Item1)
                        retValue = false;
                }
            }
        }
        return (retValue,description);
    }
    

    public static List<Assembly> pluginAssemblies = new List<Assembly>();

    static bool predicate(Type t)
    {
        return t.IsAssignableTo(typeof(ComparerV)) || t.IsAssignableTo(typeof(ConverterOutput)) ||
            t.IsSubclassOf(typeof(Filter)) ||
            t.IsSubclassOf(typeof(OutputValue)) || t.IsAssignableTo(typeof(AliasProducer)) ||
            t.IsAssignableTo(typeof(HashConverter))
            ||
            t.IsAssignableTo(typeof(Receiver))
             ||
            t.IsAssignableTo(typeof(Sender))
                       ||
            t.IsAssignableTo(typeof(IReceiver))
             ||
            t.IsAssignableTo(typeof(ISender))

             ||
            t.IsAssignableTo(typeof(HTTPReceiver.PathItem))
                   /*       ||
                           typeof(IReceiver).IsAssignableFrom(t)
                           ||
                           typeof(ISender).IsAssignableFrom(t)*/;
    }
    public static List<Type> getAllRegTypes(params Assembly[] addAssemblies)
    {
        if(addAssemblies[0] !=null)

        return addAssemblies.SelectMany(a => a.GetTypes().Where(predicate))
            .Concat(Assembly.GetAssembly(typeof(OutputValue)).GetTypes().Where(predicate))
            .Concat(Assembly.GetAssembly(typeof(Receiver)).GetTypes().Where(predicate))
            .ToList();
        else
            return Assembly.GetAssembly(typeof(OutputValue)).GetTypes().Where(predicate)
                 .Concat(Assembly.GetAssembly(typeof(Receiver)).GetTypes().Where(predicate))
                 .ToList();

    }
    static bool predicateParser(Type t)
    {
        return t.IsAssignableTo(typeof(AbstrParser));
    }
    static List<Type> getAllRegTypesParser(Assembly addAssembly)
    {
        return 
            addAssembly.GetTypes().Where(t => predicateParser(t)).ToList();

        //            return new List<Type> { typeof(ScriptCompaper),typeof(PacketBeatReceiver), typeof(ConditionFilter), typeof(JsonSender), typeof(ExtractFromInputValue), typeof(ConstantValue),typeof(ComparerForValue) };
    }
    DateTime timeStart =DateTime.Now;
    //public Assembly addAssembly;
    public Pipeline()
    {
      //  this.addAssembly = addAssembly;
        Metrics.MetricAuto metric1 = new Metrics.MetricAuto("iu_cpu_milliseconds_total", "CPU usage", () => Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds);
        Metrics.MetricAuto metric2 = new Metrics.MetricAuto("iu_privileged_cpu_milliseconds_total", "CPU usage", () => Process.GetCurrentProcess().PrivilegedProcessorTime.TotalMilliseconds);
        Metrics.MetricAuto metric3 = new Metrics.MetricAuto("iu_user_cpu_milliseconds_total", "CPU usage", () => Process.GetCurrentProcess().UserProcessorTime.TotalMilliseconds);
        Metrics.MetricAuto metric4 = new Metrics.MetricAuto("iu_memory_size_total", "Memory usage", () => { var proc = Process.GetCurrentProcess(); return proc.NonpagedSystemMemorySize64 + proc.PagedMemorySize64; });
        Metrics.MetricAuto metric5 = new Metrics.MetricAuto("iu_uptime", "Uptime in seconds", () => {  return (DateTime.Now-timeStart).TotalSeconds; });

    }
    public static string ToStringValue(Pipeline pipes,Assembly assembly)
    {
        var ser = new SerializerBuilder();
        foreach (var type in getAllRegTypes(assembly))
            ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
        var serializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        using (StringWriter sw = new StringWriter())
        {
            serializer.Serialize(sw, pipes);
            return sw.ToString();
        }

    }
    [YamlIgnore]
    public byte[] key;
    [YamlIgnore]
    public byte[] IV;

    public const int keyLength = 256;
    private static string initialisationVector = "26744a68b53dd87b";
    public ConcurrentDictionary<string, string> xmlNameSpaces= new ConcurrentDictionary<string, string>();
    bool alreadyInit = false;
    public async Task run()
    {
        if(xmlNameSpaces.Count>0)
        {
            XmlParser.namespaces = xmlNameSpaces;
            XmlParser.isCorrectedNamespace = true;
        }
        if (!alreadyInit)
        {
            EmbeddedFunctions.Init();
            if (EmbeddedFunctions.cacheProvider == null)
            {
                var services = new ServiceCollection();
                services.AddDistributedMemoryCache();
                var provider = services.BuildServiceProvider();
                EmbeddedFunctions.cacheProvider = provider.GetRequiredService<IDistributedCache>();// (typeof(IDistributedCache)) as IDistributedCache;

            }
            alreadyInit = true;
        }
        traceSaveDirectoryStat = traceSaveDirectory;
        var activity = Activity.StartActivity("Process Message", ActivityKind.Consumer);

        Logger.log("Starting the pipeline", Serilog.Events.LogEventLevel.Warning);
        Metrics.Label lab;
        if(!Metrics.common_labels.TryGetValue("Pipeline",out lab))
            Metrics.common_labels.Add( "Pipeline", new Metrics.Label("Pipeline", Path.GetFileNameWithoutExtension(this.fileName)));
        CryptoHash.pwd = this.hashWord;
        string keyString = CryptoHash.pwd;
        if(keyString.Length> keyLength / 8)
            keyString = CryptoHash.pwd.Substring(0, keyLength / 8);
        for (int i = CryptoHash.pwd.Length; i < keyLength / 8; i++)
            keyString += "Y";
       // string key = "12345reqwt12345abcde";
        this.key = Encoding.UTF8.GetBytes(keyString);
/*        using (AesManaged aes = new AesManaged())
        {
            aes.GenerateIV();
            IV = aes.IV;
        }*/
            IV= Encoding.UTF8.GetBytes(initialisationVector);
            //aes.IV = iv;

        foreach (var step in steps)
            step.Init(this);
        foreach(var mb in this.metricsBuilder)
            mb.Init();
//            step.owner = this;
        var entryStep = GetEntryStep();

        Logger.log($"Starting the entry step {entryStep.IDStep}", Serilog.Events.LogEventLevel.Debug);
        await entryStep.run();
        
        Logger.log($"Entry step {entryStep.IDStep} finished", Serilog.Events.LogEventLevel.Debug);
    }

    /// <summary>
    /// Stops the pipeline by calling stop() on the entry step.
    /// </summary>
    public async Task stop()
    {
        var entryStep = GetEntryStep();

        Logger.log($"Stopping the pipeline by calling stop() on step {entryStep.IDStep}", Serilog.Events.LogEventLevel.Debug);
        await entryStep.stop();
        
        Logger.log($"Execution of stop() on step {entryStep.IDStep} finished", Serilog.Events.LogEventLevel.Debug);
    }
    
    // helper method to get the entry step
    private Step GetEntryStep()
    {
        return steps.First(ii => ii.IDPreviousStep == "" && (ii.ireceiver != null || ii.receiver != null));
    }

    static IEnumerable<string> getEnvVariables(string body)
    {
        string pattern = "{#\\b\\w+\\b#}";
        foreach (Match match in Regex.Matches(body, pattern,
                     RegexOptions.None,
                     TimeSpan.FromSeconds(1)))
            yield return match.Value.Substring(2,match.Value.Length-4);
    }

    public Activity GetActivity(string Name,Activity mainActivity)
    {
        if (tracerBuilder == null)
            return null;
        if(mainActivity == null)
            return Activity.StartActivity(Name, ActivityKind.Internal);
        return Activity.StartActivity(Name, ActivityKind.Internal, mainActivity.Context);
    }
//    public Activity mainActivity;
    public async static Task<string> GetContentFromGit(string git_url)
    {

        var httpClient = new HttpClient();
        var username = "JugV60";
        var password = "Avensisa15869";
        string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
            .GetBytes(username + ":" + password));
//            httpWebRequest.Headers.Add("Authorization", "Basic " + encoded);
        httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
        var r=await httpClient.GetStringAsync(git_url);
        //var client = new WebClient { Credentials = new NetworkCredential("user_name", "password") };

        using (WebClient wc = new WebClient() { Credentials = new NetworkCredential("JugV60", "Avensisa15869") })
        {
            wc.Headers.Add("a", "a");
            try
            {
                wc.DownloadFile(git_url, @"C:/D/Out/test.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        return "";
    }
    [YamlIgnore]
    public string fileName;
    public static string basepath;
    public static Pipeline load(string fileName,Assembly assembly)
    {
        string Body;
        basepath= Path.GetDirectoryName(fileName);
        using (StreamReader sr = new StreamReader(fileName))
        {
            Body = sr.ReadToEnd();
        }
        var pip= loadFromString(Body, assembly);
        pip.fileName= fileName;
        return pip;
    }
    public class YamlIncludeNodeDeserializer : INodeDeserializer
    {
        private readonly IDeserializer deserializer;

        public YamlIncludeNodeDeserializer(IDeserializer deserializer)
        {
            this.deserializer = deserializer;
        }

        public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value, ObjectDeserializer rootDeserializer)
        {
            var scalar = parser.Peek<Scalar>();
            if (scalar != null && scalar.Tag == "!include")
            {
                var ext = Path.GetExtension(scalar.Value);
                if (ext == ".yml" || ext == ".yaml")
                {
                    using (var sr = new StreamReader(Path.Combine(Pipeline.basepath, scalar.Value)))
                    {

                        //      var body = sr.ReadToEnd();

                        //Console.WriteLine(expectedType.FullName);
                        //             value = deserializer.Deserialize(new Parser(sr), expectedType);
                        value = deserializer.Deserialize<Step>(new Parser(sr));
                        parser.MoveNext();
                        return true;
                    }

                }
                else
                {
                    using (var sr = new StreamReader(Path.Combine(Pipeline.basepath, scalar.Value)))
                    {

                        value = sr.ReadToEnd();

                        //Console.WriteLine(expectedType.FullName);
                        //value = deserializer.Deserialize(new Parser(includedFile), expectedType);
                        parser.MoveNext();
                        return true;
                    }
                }
                // scalar.Value
                // Replace this line with code that opens the file from scalar.Value
                /*              using (var includedFile = new System.IO.StringReader("two"))
                              {
                                  Console.WriteLine(expectedType.FullName);
                                  value = deserializer.Deserialize(new Parser(includedFile), expectedType);
                                  parser.MoveNext();
                                  return true;
                              }*/
            }

            value = null;
            return false;
        }
/*
        bool INodeDeserializer.Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
        {
            var scalar = parser.Peek<Scalar>();
            if (scalar != null && scalar.Tag == "!include")
            {
                var ext=Path.GetExtension(scalar.Value);
                if (ext == ".yml" || ext == ".yaml")
                {
                    using (var sr = new StreamReader(Path.Combine(Pipeline.basepath, scalar.Value)))
                    {

                        value = deserializer.Deserialize<Step>(new Parser(sr));
                        parser.MoveNext();
                        return true;
                    }

                }
                else
                {
                    using (var sr = new StreamReader(Path.Combine(Pipeline.basepath, scalar.Value)))
                    {

                        value = sr.ReadToEnd();

                        //Console.WriteLine(expectedType.FullName);
                        //value = deserializer.Deserialize(new Parser(includedFile), expectedType);
                        parser.MoveNext();
                        return true;
                    }
                }
                // scalar.Value
                // Replace this line with code that opens the file from scalar.Value
            }

            value = null;
            return false;
        }
*/    
    }
   static TracerProvider tracerBuilder = null;

    public static string AgentHost = "localhost";
    public static int AgentPort = -1;// 6831;
    static void InitJaeger(string Name)
    {
        // Jaeger is needed only for tracing.
        // It should be off during normal pipeline operation.
        
        if (tracerBuilder == null && AgentPort>0)
        {
            tracerBuilder= Sdk.CreateTracerProviderBuilder()
          .AddHttpClientInstrumentation()
          .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Name))
          .AddSource(nameof(Pipeline))
          .AddJaegerExporter(opts =>
          {
              opts.AgentHost = AgentHost;// _configuration["Jaeger:AgentHost"];
              opts.AgentPort = AgentPort;// Convert.ToInt32(_configuration["Jaeger:AgentPort"]);
              opts.ExportProcessorType = ExportProcessorType.Simple;
          })
          .Build();
            //tracerBuilder.Shutdown(100);
        }
    }

    /// <summary>
    /// A node deserializer for assemblies. It will load the assembly from the given path.
    /// </summary>
    // public class AssemblyNodeDeserializer : INodeDeserializer
    // {
    //     bool INodeDeserializer.Deserialize(IParser parser, Type expectedType,
    //         Func<IParser, Type, object> nestedObjectDeserializer, out object value)
    //     {
    //         // if expected type is Assembly
    //         if (expectedType == typeof(Assembly))
    //         {
    //             // get the scalar value
    //             var scalar = parser.Consume<Scalar>();
    //             // load the assembly
    //             value = Assembly.LoadFrom(scalar.Value);
    //             return true;
    //         }
    //         value = null;
    //         return false;
    //     }
    // }

    public class occurencyItem
    {
        public string variable;
        public string pattern;
        public string value;
        public string comment = "";
    }
    static List<occurencyItem> occurencyItems= new List<occurencyItem>()   ;


    public void SetMocModeForSenders(string[] stepNames)
    {
        foreach(var name in stepNames)
        {
            var step=steps.FirstOrDefault(ii => ii.IDStep == name);
            if(step != null)
            {
                if (step.sender != null)
                    step.sender.MocMode = true;

            }
        }
    }

    static List<(string oldValue, string newValue)> replacement = new List<(string oldValue, string newValue)> { { ("converters:", "filterCollection:") }, { ("filter:", "condition:") } };
    public static Pipeline loadFromString(string Body,Assembly assembly)
    {
        foreach(var it in replacement)
            Body=Body.Replace(it.oldValue,it.newValue);
        occurencyItems.Clear();
        var ser = new DeserializerBuilder();
        if (assembly != null)
        {
            AddCustomParsers(assembly);
        }
            foreach (var type in getAllRegTypes(assembly))
            {
                // ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.FullName), type);

                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            }
       
        var deserializer = ser
        .WithTagMapping("!include", typeof(object)) // This tag needs to be registered so that validation passes
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .WithNodeDeserializer(new YamlIncludeNodeDeserializer(ser.Build()), s => s.OnTop())
        .Build();
        foreach (var var in getEnvVariables(Body))
        {
            string val = Environment.GetEnvironmentVariable(var);
            if (val == null)
            {
                throw new Exception($"Unknown environment variable {var}");
            }
            //    MessageBox.Show($"{var}:{val}");
            int index = -1;
            while ((index = Body.IndexOf("{#" + var + "#}", index + 1)) != -1)
            {
                int indexBeg = Body.LastIndexOf("\n", index);
                occurencyItems.Add(new occurencyItem() { pattern = ((indexBeg >= 0) ? (Body.Substring(indexBeg + 1, index - indexBeg - 1)) : ""), variable = var, value = val });
            }
            // Body.
        }
        foreach (var var in getEnvVariables(Body))
        {
            string val = Environment.GetEnvironmentVariable(var);
            if (val == null)
            {
                throw new Exception($"Unknown environment variable {var}");
            }
            //    MessageBox.Show($"{var}:{val}");
            Body = Body.Replace("{#" + var + "#}", val);
        }
        //        var deserializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        var pip = deserializer.Deserialize<Pipeline>(Body);// (File.OpenText(fileName));
        if (pip.steps != null)
        {
            foreach (var step in pip.steps)
                step.owner = pip;
        }
        InitJaeger(pip.pipelineDescription);
        return pip;
    }

    public static void AddCustomParsers(Assembly assembly)
    {
        foreach (var type in getAllRegTypesParser(assembly))
        {
            AbstrParser.availParser.Add(Activator.CreateInstance(type) as AbstrParser);
        }
    }

    /*  public static Pipeline loadFromString(string content)
        {
            var ser = new DeserializerBuilder();
            foreach (var type in getAllRegTypes())
                ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
            var deserializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var pip = deserializer.Deserialize<Pipeline>(content);
            foreach (var step in pip.steps)
                step.owner = pip;
            return pip;
        }*/


    void formMD(string fileName)
    {
        string md_fileName = fileName.Replace(".yml", ".md");
        if(File.Exists(md_fileName))
        {
            using(StreamReader sr= new StreamReader(md_fileName))
            {
                var content=sr.ReadToEnd();
                foreach(var item in occurencyItems)
                {
                    string patt = $" * {item.variable} ";
                    int index=content.IndexOf(patt);
                    if(index>=0)
                    {
                        int index1=content.IndexOf("\n",index+1);
                        if (index1 == -1)
                            index1 = content.Length;
                        item.comment= content.Substring(index+patt.Length,index1-(index + patt.Length)-1);
                    }

                }
                


            }
        }
        using (StreamWriter sw = new StreamWriter(md_fileName))
        {
            sw.WriteLine($"# {this.pipelineDescription}");
            if (occurencyItems.Count > 0)
                sw.WriteLine($"## ENVIRONMENT VARIABLES");

            List<string> usedVars = new List<string>();
            foreach (var item in occurencyItems)
            {
                if (!usedVars.Contains(item.variable))
                {
                    usedVars.Add(item.variable);
                    sw.WriteLine($" * {item.variable} {item.comment}");
                }
            }
            sw.WriteLine($"## PORTS(input)");
            sw.WriteLine(@"|PORT|PROTOCOL|
| ------ | ------ |");
            foreach (var step in steps.Where(ii => ii.receiver?.port > 0))
            {
                sw.WriteLine($"| {step.receiver.port} | {step.receiver.protocolType} |");
                if(step.receiver.protocolType == Receiver.ProtocolType.http)
                {
                    sw.WriteLine("\r\nTo check the correctness of the pipeline settings, run ```curl http://<entry_point_url>:<entry_point_port>/SelfTest``` \r\n\r\nOR\r\n\r\n");
                }

            }
            sw.WriteLine(" run ```curl -X 'GET' 'https://<entry_point_url>:<monitoring_port>/api/Monitoring/SelfTest'  -H 'accept: */*'```");
            sw.WriteLine();
            sw.WriteLine("\r\n\r\nOther ports( output ) see in ENVIRONMENT VARIABLES section");

        }
    }
    public void Save( string fileName,Assembly assembly)
    {
        ISerializer serializer = getSerializer(assembly);

      
        using (StringWriter sw1 = new StringWriter())
        {
            serializer.Serialize(sw1, this);
            var content=sw1.ToString();
            foreach(var item in occurencyItems)
            {
                content=content.Replace(item.pattern + item.value, item.pattern + "{#" + item.variable + "#}");
            }
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(content);
            }
            formMD(fileName);
        }

    }

    private static ISerializer getSerializer(Assembly assembly)
    {
        var ser = new SerializerBuilder();
        foreach (var type in getAllRegTypes(assembly))
            ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
        var serializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        return serializer;
    }

    public void Save(TextWriter stream,Assembly assembly)
    {
        ISerializer serializer = getSerializer(assembly);
        serializer.Serialize(stream, this);

    }

    public class LiquidPoint:ILiquidizable
    {
        public static List<LiquidPoint> All = new List<LiquidPoint>();
        public string Name { get; set; }
        public int nOrder;
        public bool isFirst=false;
        public string Protocol="Http";
        public List<LiquidPoint> child = new List<LiquidPoint>();
        Step currentStep;
        public LiquidPoint(Step[] steps,int index, object sender,ref int nOrder)
        {
            Name = sender.GetType().Name;
            if (index >= 0)
            {
                All.Add(this);
                nOrder = AddChilds(steps, index, nOrder);
                currentStep = steps[index];
            }
        }

        private int AddChilds(Step[] steps, int index, int nOrder)
        {
            if (nOrder >= 0)
            {
                this.nOrder = nOrder;
                if (!string.IsNullOrEmpty(steps[index].IDResponsedReceiverStep))
                    child.Add(All.First());
                if (index < steps.Length - 1)
                {
                    nOrder++;
                    child.Add(new LiquidPoint(steps, index + 1, ((steps[index + 1].isender== null)? steps[index + 1].sender : steps[index + 1].isender), ref nOrder));
                }

            }

            return nOrder;
        }

        public LiquidPoint(Step[] steps, int index, object rec,ref int nOrder,int dummy)
        {
            if(index>=0)
                All.Add(this);
            Name = rec.GetType().Name;
            this.nOrder=nOrder;
            nOrder = AddChilds(steps, index, nOrder);
            isFirst= true;
            currentStep = steps[index];

        }
        public object ToLiquid()
        {
            return new Dictionary<string, object> { { "Name", this.Name }, { "nOrder", this.nOrder }, { "Childs", this.child},{ "isFirst",this.isFirst }, { "Protocol", this.Protocol },{ "Step", currentStep } };
        }
    }

    public object ToLiquid()
    {
        LiquidPoint.All.Clear();
        List<LiquidPoint> senders = new List<LiquidPoint>();
        int nOrder = 1;
        senders.Add(new LiquidPoint(this.steps, 0, ((steps[0].ireceiver==null)? steps[0].receiver: steps[0].ireceiver), ref nOrder,1));

        return new Dictionary<string, object> { { "Name", this.fileName }, { "Description", this.pipelineDescription }, { "Steps", this.steps },{ "Childs",LiquidPoint.All} };
    }

    public List<MetricBuilder> metricsBuilder = new List<MetricBuilder>();

}
