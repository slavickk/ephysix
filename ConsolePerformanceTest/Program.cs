// See https://aka.ms/new-console-template for more information
using System.Net.Http;
using System;
using static ConsolePerformanceTest.Sender;
using System.Threading.Tasks;
using ConsolePerformanceTest;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using UniElLib;
//await new TransformerBenchmark().test();
BenchmarkRunner.Run<TransformerBenchmark>();
[MemoryDiagnoser]
public class TransformerBenchmark
{
    RegexpTransformer tr1;
    string[] arr;
    string[] arr1;
    CSScriptTransformer tr;
    CSScriptTransformer tr2;
    public TransformerBenchmark() 
    {
        tr1 = new RegexpTransformer();
        arr = tr1.transform(new string[] { "22200*****037=2512" });
        //            var tr = new CSScriptTransformer("(@Input1+@Input0)/10", true);
        tr = new CSScriptTransformer("$\"20{@Input1}-{@Input2}-01T00:00:00\"");
        tr2 = new CSScriptTransformer("(@Input1+@Input0)/10", true);
        arr1 = new string[] { "2000", "3000" };

    }
    [Benchmark(Description = "Transform")]
    public async Task test()
    {
//        var arr = tr1.transform(new string[] { "22200*****037=2512" });
 //       var out1 = tr.transform(arr);
        var out2 = tr2.transform(arr1);
    }
}


/*async Task Dummy()
{
    Console.WriteLine("Hello, World!");
    int allCount = 100000;
    int packetSize = 1000;
    int tick = 10000;
    var tasksSend = new List<System.Threading.Tasks.Task<ResponseFromRex>>();
    int execCount = 0;
    DateTime time1 = DateTime.Now;
    HttpClient httpClient = new HttpClient();
    Uri url = new Uri("http://localhost:8080");
    string request;
    using (StreamReader sr = new StreamReader(@"C:\D\a.json"))
    {
        request = sr.ReadToEnd();
    }
    DateTime timeLast = DateTime.Now;
    while (allCount > execCount)
    {
        tasksSend.Clear();
        for (int i = 0; i < packetSize; i++)
            tasksSend.Add(Sender.SendToRexx(httpClient, url, request));
        var ans = await System.Threading.Tasks.Task.WhenAll(tasksSend);
        execCount += packetSize;
        if (execCount % tick == 0)
        {
            DateTime time2 = DateTime.Now;
            Console.WriteLine($"{(int)(tick / (time2 - timeLast).TotalSeconds)} tot:{execCount}");
            timeLast = DateTime.Now;
        }
    }
    var totalMS = (DateTime.Now - time1).TotalMilliseconds;
    Console.WriteLine($"ended{totalMS} ");// count_unsucces{ans.Count(ii=>ii.resp!= null && ii.resp.StatusCode != System.Net.HttpStatusCode.OK)} skipped {ans.Count(ii => ii.resp == null)}");
                                          //Console.WriteLine($"ended{totalMS} count_unsucces{ans.Count(ii => ii.resp != null && ii.resp.StatusCode != System.Net.HttpStatusCode.OK)} skipped {ans.Count(ii => ii.resp == null)}");

    //Task.Run(() => { });
}
*/