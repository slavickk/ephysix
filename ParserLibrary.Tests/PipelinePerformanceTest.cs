using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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
            HttpClient httpClient = new HttpClient();

            string jsonContent = File.ReadAllText("TestData/AReq.xml");
            for ( int i=0; i < 100;i++)
            {
                using var httpContent = new StringContent
           (jsonContent, Encoding.UTF8, "application/xml");

                using var response = await httpClient.PutAsync("http://localhost:8080", httpContent);

                response.EnsureSuccessStatusCode();
                var st=await response.Content.ReadAsStringAsync();
                //                httpClient.PutAsync("localhost:8080",);

            }
        }

        static async Task runTest(string path)
    {
        Pipeline pip;
        try
        {
            pip = Pipeline.load(path);
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
