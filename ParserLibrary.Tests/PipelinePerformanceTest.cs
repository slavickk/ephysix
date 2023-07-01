using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static ParserLibrary.Metrics;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class PipelinePerformanceTest
    {
        [Test]
        public static async Task testHTTPPerformance()
        {

            Environment.SetEnvironmentVariable("SECRET_WORD", "QWE123");
            Environment.SetEnvironmentVariable("REX_URL", "http://nginx.service.consul");
            Environment.SetEnvironmentVariable("AUTH_HOST_URL", "http://10.74.28.30:30212");
            runTest(@"C:\Users\User\source\repos\Polygons\ACS_TWO_TX\IntegrationTools\ACS_TW.yml");
            await Task.Delay(1000);
            HttpClient httpClient = new HttpClient();

            string jsonContent = File.ReadAllText("TestData/AReq.xml");
            int ccount = 500;
            int err = 0;
            //            List<Task> listTask= new List<Task>();
            var time = DateTime.Now;
            var tasks=Enumerable.Range(0, ccount).Select(async (ii) =>
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
                    err++;
                    Console.WriteLine(ex);
                }
            });

            /*            for (int i = 0; i < ccount; i++)
                        {
                            Enumerable.Range(0,ccount).Select(async (ii) =>
                            {

                                using var httpContent = new StringContent
                       (jsonContent, Encoding.UTF8, "application/xml");

                                using var response = await httpClient.PutAsync("http://localhost:8080", httpContent);

                                response.EnsureSuccessStatusCode();
                                var st = await response.Content.ReadAsStringAsync();
                            });
                            //                httpClient.PutAsync("localhost:8080",);


                        }*/
            await Task.WhenAll(tasks);
            var milli = (DateTime.Now - time).TotalMilliseconds;
            int countopen=(Metrics.metric.allMetrics.First(ii => ii.Name == "HTTPOpenConnectCount") as MetricCount).getCount();
            int countsuc = (Metrics.metric.allMetrics.First(ii => ii.Name == "packagesReceivedSuccess") as MetricCount).getCount();
            var avgTime=(Metrics.metric.allMetrics.First(ii => ii.Name == "HTTPExecutedTime") as MetricCount).sum / (Metrics.metric.allMetrics.First(ii => ii.Name == "HTTPExecutedTime") as MetricCount).getCount();
            Console.WriteLine($"Test finished with errors {err} finished {countsuc}");
            Console.WriteLine("**Metrics**");
            foreach (var met in Metrics.metric.allMetrics)
            {
                Console.WriteLine(met.getBody());
            }
            Console.WriteLine($"***** timeexec ={milli} avg tps={ ccount*1000/milli}");
            Assert.AreEqual(0,countopen, $"Count of open connection not 0 ({countopen})");
            Assert.AreEqual(countsuc, ccount, $"Not all {ccount} finished with success ({countsuc})");
            Assert.AreEqual( 0,err,$"Count of errors not 0 ({err})");
        }
        static Pipeline pip;

        static async Task runTest(string path)
    {
        try
        {
            pip = Pipeline.load(path);
            pip.steps.First(ii=>ii.IDStep=="Step_ToTWO").sender.MocMode = true;
                pip.steps.First(ii => ii.IDStep == "Step_ToTWO").sender.mocker.MocTimeoutInMilliseconds=0;
               /* pip.steps.First(ii => ii.IDStep == "Step_0").sender.mocker.MocTimeoutInMilliseconds = 0;
                pip.steps.First(ii => ii.IDStep == "Step_0").sender.MocMode = true;//.mocker.MocTimeoutInMilliseconds = 0;
               */
                await pip.run();

            //                pip = pips[0];
        }
        catch (Exception e77)
        {
            Console.WriteLine("Error:" + e77.Message);
            return ;
        }
    }
    }
}
