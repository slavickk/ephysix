/******************************************************************
 * File: PipelinePerformanceTest.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using static ParserLibrary.Metrics;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class PipelinePerformanceTest
    {
        static MockHTTPServer httpServer;
        static bool isLoad=false;
        [Test]
        public static async Task testHTTPPerformance()
        {
            if(!isLoad)
            {
                httpServer = new MockHTTPServer(8070,700) { mockItems = new List<MockHTTPServer.MockItem>()
                {
                    new MockHTTPServer.MockItem("healthcheck")
                    { reqContentPath=(path)=>path.Contains("/healthcheck"), respStatusCode=(a)=>200, respContentBody=(a)=>"[]", respContentType=(a)=>"application/json"   }
                    ,new MockHTTPServer.MockItem("loadstream")
                    { reqContentPath=(path)=>path.Contains("/LoadStream"), respStatusCode=(a)=>200, respContentBody=(a)=>"[]", respContentType=(a)=>"application/json"   }
                    ,new MockHTTPServer.MockItem("unknown")
                    { respStatusCode=(a)=>200, respContentBody=(a)=>@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"">
<SOAP-ENV:Body>
<Tran>
<Response Result=""Approved"" Id=""5802242"">
<Specific>
<Tds IssuerInstRid=""DEMO"" Enrolled=""true"" CanBeEnrolled=""true"" AuthenticationType=""Static"" >
</Tds>
</Specific>
</Response>
</Tran>
</SOAP-ENV:Body>
</SOAP-ENV:Envelope>", respContentType=(a)=>"text/xml"   }

                }
                };



                isLoad = true;
            }

            Environment.SetEnvironmentVariable("SECRET_WORD", "QWE123");
            Environment.SetEnvironmentVariable("FID", "Bank1");

            Environment.SetEnvironmentVariable("REX_URL", "http://localhost:8070");
            //            Environment.SetEnvironmentVariable("REX_URL", "http://nginx.service.consul");
            Environment.SetEnvironmentVariable("AUTH_HOST_URL", "http://localhost:8070");
//            Environment.SetEnvironmentVariable("AUTH_HOST_URL", "http://10.74.28.30:30212");

//            runTest(@"C:\Users\User\source\repos\Polygons\ACS_TWO_TX\IntegrationTools\ACS_TW.yml");
            runTest(@"Data\ACS_TW.yml");
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
            pip = Pipeline.load(path, Assembly.GetAssembly(typeof(TICReceiver)));
            
/*            pip.steps.First(ii=>ii.IDStep=="Step_ToTWO").sender.MocMode = true;
                pip.steps.First(ii => ii.IDStep == "Step_ToTWO").sender.mocker.MocTimeoutInMilliseconds=0;*/
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
