using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UniElLib;
using System.Net.Http;
using Confluent.Kafka;
using System.Reflection;

namespace ParserLibrary.Tests
{
    internal static class EmbeddedFunctionTest
    {
        [Test]
        public static async Task  TestAnyWay()
        {
            var handler=new HttpClientHandler { AllowAutoRedirect = true };
            HttpClient client;
            client = new HttpClient(handler);
            client.Timeout = new TimeSpan(0, 0, 25);
            Dictionary<string, string> headers =new Dictionary<string, string> { { "SOAPAction", "http://10.200.200.112:10001/csp/awl/AWGW.WS.PUPAY" } };
            string body;
            using(StreamReader sr = new StreamReader(@"C:\Users\jurag\source\repos\ms-payment-service\AnyWay.Sample\ReqPuPay_DBO_51_Check.xml"))
            {
                body=sr.ReadToEnd();
            }
            var stringContent = new StringContent(body, UnicodeEncoding.UTF8, "text/xml"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            if (headers != null)
            {
                foreach (var item in headers)
                    stringContent.Headers.Add(item.Key, item.Value);
            }
            try
            {
                var result = await client.PostAsync(@"http://10.200.200.112:10001/csp/awl/AWGW.WS.cls", stringContent);
                if (result.IsSuccessStatusCode)
                {
                    string resultStr = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex) 
            {
            }
        }

        [Test]
        public static void test5()
        {
            string[] arr = new string[] { "Рога и копыта", "200.0", "RUR", "987390" };
            var arr1 = arr.Select(ii => Transliteration.Front(ii)).ToArray();
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\jurag\Downloads\Telegram Desktop\RespProviders.xml");
            using (StreamWriter sw = new StreamWriter(@"c:\***\examples5.txt"))
            {
                foreach (XmlAttribute attr in doc.DocumentElement.SelectNodes("//@Name_RU"))
                {
                    arr[0] = attr.Value;
                    var arr2 = arr.Select(ii => Transliteration.Front(ii)).ToArray();
                    sw.WriteLine(string.Format("Oplata uslugi {0} v summe {1} {2} SMS-kod: {3}", arr2));
                }
            }
/*            using (StreamReader sr = new StreamReader(@"C:\Users\jurag\Downloads\Telegram Desktop\RespProviders.xml"))
            {
                var jsonObject = JObject.Parse(sr.ReadToEnd());

                // Select all Inputs where Visible is true
                var visibleInputs = jsonObject["Path"];

            }*/
            var tr1 = string.Format("Oplata uslugi {0} v summe {1} {2} SMS-kod: {3}", arr1);
            var tr = new CSScriptTransformer("Oplata uslugi {@Input1} v summe {@Input2} {@Input3} SMS-kod: {@Input4}");
            var out1 = tr.transform(arr);

            var res = Transliteration.Front("У попа была собака", TransliterationType.ISO);
        }
        [Test]
        public static void TestEval()
        {
           var ff= EmbeddedFunctions.Parse("this.format(\"IDAnyWay:{0},Type: {1},Vendor: {2},Summa: {3},Comission: {4}\",this.path(\"awl:STEPResult/@ID/#text\",node),this.path(\"awl:STEPResult/@Type/#text\",node),this.path(\"awl:STEPResult/awl:VendorInfo/@Name/#text\",node),this.path(\"awl:STEPResult/awl:Sums/-SumSTrs/#text\",node),this.path(\"awl:STEPResult/awl:Sums/-AWGWFee/#text\",node))");
            var ss = ff(new AbstrParser.UniEl());
         //   var interpreter = new Interpreter();
          /*  CSScript.RoslynEvaluator
               .CreateDelegate(body)*/
        }
        [Test]
        public static void Test()
        {
            var aa = "asdd;(asd;asd(ddd;sss));aa(bb)".SplitSpec();
           // EmbeddedFunctions.SplitSpec
            var services = new ServiceCollection();
            services.AddDistributedMemoryCache();
            var provider = services.BuildServiceProvider();
            EmbeddedFunctions.cacheProvider = provider.GetRequiredService<IDistributedCache>();// (typeof(IDistributedCache)) as IDistributedCache;

            EmbeddedFunctions.Init();

            var ss=EmbeddedFunctions.exec("$format(Oplata uslugi {0} v summe {1} {2} SMS-kod: {3};$translit($path(Usluga/b/@d;$1$)$)$;$path(Summa/b/@d;$1$)$;RUR;$path(SMS/b/@d;$1$)$)$", new List<AbstrParser.UniEl> { new AbstrParser.UniEl() });
        }
    }
}
