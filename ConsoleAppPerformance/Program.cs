// See https://aka.ms/new-console-template for more information
using System.Net.Http;
using System.Reflection;
using System.Text;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ParserLibrary;
using Plugins.DummyProtocol1;

Console.WriteLine("Hello, World!");
BenchmarkRunner.Run<PipelinePerformanceTest>();
[MemoryDiagnoser]
[ShortRunJob]
public class PipelinePerformanceTest
{
    HttpClient httpClient;
    string jsonContent;

    [Benchmark]
    public async Task Test()
        {
            try
            {
                using var httpContent = new StringContent
       (jsonContent, Encoding.UTF8, "application/xml");

                using var response = await httpClient.PutAsync("http://localhost:8080", httpContent);

response.EnsureSuccessStatusCode();
var st = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
{
    Console.WriteLine(ex);
}
        }
    public PipelinePerformanceTest()
    {

        Environment.SetEnvironmentVariable("SECRET_WORD", "QWE123");
        Environment.SetEnvironmentVariable("REX_URL", "http://nginx.service.consul");
        Environment.SetEnvironmentVariable("AUTH_HOST_URL", "http://10.74.28.30:30212");
        Task.Run(async() => { runTest(@"C:\Users\User\source\repos\Polygons\ACS_TWO_TX\IntegrationTools\ACS_TW.yml"); });
        Thread.Sleep(1000);
        httpClient = new HttpClient();

        jsonContent = File.ReadAllText("TestData/AReq.xml");
    }
    static Pipeline pip;

    static async Task runTest(string path)
    {
        try
        {
            pip = Pipeline.load(path, Assembly.GetAssembly(typeof(ParserLibrary.DummyProtocol1Receiver)));
            pip.steps.First(ii => ii.IDStep == "Step_ToTWO").sender.MocMode = true;
            pip.steps.First(ii => ii.IDStep == "Step_ToTWO").sender.mocker.MocTimeoutInMilliseconds = 0;
            /* pip.steps.First(ii => ii.IDStep == "Step_0").sender.mocker.MocTimeoutInMilliseconds = 0;
             pip.steps.First(ii => ii.IDStep == "Step_0").sender.MocMode = true;//.mocker.MocTimeoutInMilliseconds = 0;
            */
            await pip.run();

            //                pip = pips[0];
        }
        catch (Exception e77)
        {
            Console.WriteLine("Error:" + e77.Message);
            return;
        }
    }
}
[MemoryDiagnoser]
[ShortRunJob]
public class StringReplace
{
    readonly Dictionary<string, string> map = new()
    {
        { "×", "*" },
        { "÷", "/" },
        { "SIN", "Sin" },
        { "COS", "Cos" },
        { "TAN", "Tan" },
        { "ASIN", "Asin" },
        { "ACOS", "Acos" },
        { "ATAN", "Atan" },
        { "LOG", "Log" },
        { "EXP", "Exp" },
        { "LOG10", "Log10" },
        { "POW", "Pow" },
        { "SQRT", "Sqrt" },
        { "ABS", "Abs" },
    };

    private const string target = "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |" +
                                  "1 x 1 ÷ 1 * SIN(COS(TAN(LOG(ASIN(ATAN(EXP(POW(SQRT(ABS(1))))))))))  |";

    [Benchmark]
    public string String()
    {
        var result = target;
        foreach (var key in map.Keys)
        {
            result = result.Replace(key, map[key]);
        }

        return result;
    }

    [Benchmark]
    public string StringBuilder()
    {
        var result = new StringBuilder(target);
        foreach (var key in map.Keys)
        {
            result.Replace(key, map[key]);
        }

        return result.ToString();
    }
}