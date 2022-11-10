using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ParserLibrary;

public class Pipeline
{
    [YamlIgnore]
    public AbstrParser.UniEl lastExecutedEl = null;
    public static Metrics metrics = new Metrics();

    public List<MetricBuilder> metricsBuilder = new List<MetricBuilder>(); 

    public string hashWord { get; set; } = "QWE123";
    public string pipelineDescription = "Pipeline example";
    [YamlIgnore]
    public string tempMocData ="";
    public Step[] steps { get; set; } = new Step[] { };// new Step[] { new Step() };
    public bool debugMode
    {
        set
        {
            foreach (Step step in steps)
            {
                step.debugMode = value;
                if(step.receiver!= null)
                    step.receiver.debugMode = value;
                /*if (step.sender != null)
                        step.sender.debugMode = value;*/
            }
        }
    }

    //public AbstrParser.UniEl rootElement = null;
    public async Task<bool> SelfTest()
    {
        // check that sender implemented ISelfTested
        if (steps[0].sender is not ISelfTested testableSender)
            return false; // treating this as a failed self test
            
        var res1 = await testableSender.isOK();
        if(res1.Item3 == null)
            Logger.log("{Item}.Results:{Res}", Serilog.Events.LogEventLevel.Information,"any", res1.Item2.ToString(),(res1.Item1 ? "OK" : "Fail"));
        else
            Logger.log("{Item}.Results:{Res}",res1.Item3, Serilog.Events.LogEventLevel.Information, "any", res1.Item2.ToString(), (res1.Item1 ? "OK" : "Fail"));
        return res1.Item1;
    }
    static List<Type> getAllRegTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAssignableTo(typeof(ComparerV)) || t.IsAssignableTo(typeof(ConverterOutput)) || t.IsSubclassOf(typeof(Receiver)) || t.IsSubclassOf(typeof(Filter)) || t.IsSubclassOf(typeof(Sender)) || t.IsSubclassOf(typeof(OutputValue)) || t.IsAssignableTo(typeof(AliasProducer)) || t.IsAssignableTo(typeof(HashConverter))).ToList();

        //            return new List<Type> { typeof(ScriptCompaper),typeof(PacketBeatReceiver), typeof(ConditionFilter), typeof(JsonSender), typeof(ExtractFromInputValue), typeof(ConstantValue),typeof(ComparerForValue) };
    }
    public Pipeline()
    {
        Metrics.MetricAuto metric1 = new Metrics.MetricAuto("iu_cpu_milliseconds_total", "CPU usage", () => Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds);
        Metrics.MetricAuto metric2 = new Metrics.MetricAuto("iu_privileged_cpu_milliseconds_total", "CPU usage", () => Process.GetCurrentProcess().PrivilegedProcessorTime.TotalMilliseconds);
        Metrics.MetricAuto metric3 = new Metrics.MetricAuto("iu_user_cpu_milliseconds_total", "CPU usage", () => Process.GetCurrentProcess().UserProcessorTime.TotalMilliseconds);
        Metrics.MetricAuto metric4 = new Metrics.MetricAuto("iu_memory_size_total", "Memory usage", () => { var proc = Process.GetCurrentProcess(); return proc.NonpagedSystemMemorySize64 + proc.PagedMemorySize64; });
        
    }
    public static string ToStringValue(Pipeline pipes)
    {
        var ser = new SerializerBuilder();
        foreach (var type in getAllRegTypes())
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
    public async Task run()
    {
        CryptoHash.pwd = this.hashWord;
        string key = "12345reqwt12345abcde";
        this.key = Encoding.UTF8.GetBytes(key);
        //aes.IV = iv;

        foreach (var step in steps)
        {
            if (step.receiver != null)
                step.receiver.owner = step;
            if (step.sender != null)
                step.sender.owner = step;
            step.Init(this);

        }
        foreach(var mb in this.metricsBuilder)
        {
            mb.Init();
        }
//            step.owner = this;
        await steps.First(ii=>ii.IDPreviousStep=="" && ii.receiver != null).run();
    }

    static IEnumerable<string> getEnvVariables(string body)
    {
        string pattern = "{#\\b\\w+\\b#}";
        foreach (Match match in Regex.Matches(body, pattern,
                     RegexOptions.None,
                     TimeSpan.FromSeconds(1)))
            yield return match.Value.Substring(2,match.Value.Length-4);
    }
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
    public static Pipeline load(string fileName = @"C:\d\model.yml")
    {
        string Body;
        using (StreamReader sr = new StreamReader(fileName))
        {
            Body = sr.ReadToEnd();
        }
        return loadFromString(Body);
    }

    public static Pipeline loadFromString(string Body)
    {
        var ser = new DeserializerBuilder();
        foreach (var type in getAllRegTypes())
            ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
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
        var deserializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        var pip = deserializer.Deserialize<Pipeline>(Body);// (File.OpenText(fileName));
        foreach (var step in pip.steps)
            step.owner = pip;
        return pip;
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

    public void Save( string fileName = @"C:\d\aa1.yml")
    {
        ISerializer serializer = getSerializer();
        using (StreamWriter sw = new StreamWriter(fileName))
        {
            serializer.Serialize(sw, this);
        }

    }

    private static ISerializer getSerializer()
    {
        var ser = new SerializerBuilder();
        foreach (var type in getAllRegTypes())
            ser = ser.WithTagMapping(new YamlDotNet.Core.TagName("!" + type.Name), type);
        var serializer = ser.WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        return serializer;
    }

    public void Save(TextWriter stream)
    {
        ISerializer serializer = getSerializer();
        serializer.Serialize(stream, this);

    }



}