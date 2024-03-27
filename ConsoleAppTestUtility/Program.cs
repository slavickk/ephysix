using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using ParserLibrary;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Sensitive;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using Logger = ParserLibrary.Logger;
using UniElLib;

namespace ConsoleAppTestUtility
{
    [MemoryDiagnoser]
    public class ParserTest
    {
        [Benchmark(Description = "SimpleTest")]
        public  bool SimpleTest()
        {
            ContextItem context = new ContextItem();
            context.list = new List<AbstrParser.UniEl>();
            var rootElement = AbstrParser.CreateNode(null, context.list, "Root");
            var str = @"{""Fields"":{""AccountBalanceData"":{""AvailableBalance"":0,""BalanceCurrency"":false,""LedgerBalance"":0},""AcquiringInstitutionIdentification"":""00000000001"",""AdditionalPOSData"":{""Clerk"":"""",""CVV2"":"""",""DraftCapture"":1,""InvoiceNumber"":"""",""PosBatchAndShiftData"":"""",""TransactionCategory"":0},""CardIssuerData"":{""AuthFIName"":""DEMO"",""AuthPSName"":""CPLS""},""AcquirerFeeAmount"":{""IsWithdraw"":true,""_isWithdraw"":""D"",""Amount"":0},""MBR"":0,""MiscellaneousTransactionAttributes"":{""Type"":3,""Value"":{""CC"":{""Value"":""0"",""Type"":0},""IB"":{""Value"":""11000"",""Type"":0},""IC"":{""Value"":""4"",""Type"":0},""SPA"":{""Value"":""0"",""Type"":0},""TCE"":{""Value"":""0"",""Type"":0}}},""MiscellaneousTransactionAttributes2"":{""Type"":3,""Value"":{""AED"":{""Value"":[{""Type"":1,""Value"":[{""Value"":""51"",""Type"":0},{""Type"":3,""Value"":{""BrowserInfo"":{""Value"":""eyJ2ZXJzaW9uIjoiMS4wIiwiYWNjZXB0TGFuZ3VhZ2UiOiIiLCJhY2NlcHRIZWFkZXJzIjoidGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC43IiwidXNlckFnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgSGVhZGxlc3NDaHJvbWUvMTIwLjAuNjA5OS4yMDAgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJJcCI6IjEwLjguMTkuMjQxIiwidGltZVpvbmUiOiItMzAwIiwiY29sb3JEZXB0aCI6IjI0Iiwic2NyZWVuSGVpZ2h0IjoiMjQiLCJzY3JlZW5XaWR0aCI6IjgwMCIsImphdmFzY3JpcHRFbmFibGVkIjoidHJ1ZSIsImphdmFFbmFibGVkIjoiZmFsc2UifQ=="",""Type"":0},""DsReferenceNumber"":{""Value"":""1000"",""Type"":0},""MessageCategory"":{""Value"":""01"",""Type"":0},""TdsReqAuthIndicator"":{""Value"":""01"",""Type"":0},""TdsServerTranId"":{""Value"":""e015ebfd-8c93-47b1-8aff-53831fc82a6a"",""Type"":0},""Term/acquirerBIN"":{""Value"":""2201380101"",""Type"":0},""Term/acquirerMerchantID"":{""Value"":""POS_1"",""Type"":0},""Term/deviceChannel"":{""Value"":""02"",""Type"":0}}}]},{""Type"":1,""Value"":[{""Value"":""52"",""Type"":0},{""Type"":3,""Value"":{""Extensions"":{""Value"":[{""Type"":1,""Value"":[{""Value"":""extensionField1"",""Type"":0},{""Value"":""ID1"",""Type"":0},{""Value"":""0"",""Type"":0},{""Value"":""{\u0022valueOne\u0022:\u0022value\u0022}"",""Type"":0}]},{""Type"":1,""Value"":[{""Value"":""CpDummySystem1DeviceGuid"",""Type"":0},{""Value"":""CpDummySystem1DeviceGuid"",""Type"":0},{""Value"":""0"",""Type"":0},{""Value"":""{ \u0022guid\u0022: \u0022541b19ee-3884-4afa-8406-f8dbbe06de8e\u0022 }"",""Type"":0}]},{""Type"":1,""Value"":[{""Value"":""DummySystem1_3DS"",""Type"":0},{""Value"":""374890234"",""Type"":0},{""Value"":""0"",""Type"":0},{""Value"":""{\u00223ds\u0022:\u0022challenge\u0022,\u0022declinereason\u0022:\u0022No rules signalled.Default behaviour\u0022}"",""Type"":0}]}],""Type"":2}}}]}],""Type"":2},""EC"":{""Type"":3,""Value"":{""3DS/3DS2ProtovolVer"":{""Value"":""2.2.0"",""Type"":0},""3DS/CallbackUrl"":{""Value"":"""",""Type"":0},""3DS/DSTransId"":{""Value"":""21089df6-d761-48e6-9090-f0c0415d6ec3"",""Type"":0},""3DS/ExpTimeInterval"":{""Value"":""30"",""Type"":0},""3DS/MessCategory"":{""Value"":""01"",""Type"":0},""3DS/PrevACSTranId"":{""Value"":"""",""Type"":0},""3DS/ReqAuthIndicator"":{""Value"":""01"",""Type"":0},""3DS/TdsServerTranId"":{""Value"":""e015ebfd-8c93-47b1-8aff-53831fc82a6a"",""Type"":0},""3DS/acsTranId"":{""Value"":""29cb25da-8a50-4c5f-9f46-effb2053dcd3"",""Type"":0},""Ext/Network"":{""Value"":""44"",""Type"":0},""Ext/NetworkRid"":{""Value"":""ACS"",""Type"":0},""Merchant/BIN"":{""Value"":""2201380101"",""Type"":0},""Merchant/Name"":{""Value"":""compass"",""Type"":0},""Merchant/Rid"":{""Value"":""2201380101"",""Type"":0},""Merchant/URL"":{""Value"":""http://compassplus.ru"",""Type"":0},""3DS/PrepareAuthTranId"":{""Value"":""9909707"",""Type"":0}}},""GT"":{""Value"":""20240109224115"",""Type"":0}}},""NumericMessage"":0,""PAN"":""22200*****038"",""POSConditionCode"":0,""POSEntryMode"":{""EntryMethod"":0,""PinMethod"":1},""ProcessingCode"":{""FromAccountType"":1,""ToAccountType"":0,""TransactionCode"":623},""SecureData3D"":""0000000021089df6d76148e69090f0c0415d6ec3"",""SIC"":3000,""SytemTraceAuditNumber"":278065,""Track2"":""22200*****038=2512"",""TransactionAmount"":2537,""TransactionCurrencyCode"":840,""TransactionRetrievalReferenceNumber"":""000009909707"",""TransmissionGreenwichTime"":""0109174121"",""CardAcceptorTerminal"":{""ID"":""EC2"",""Info"":{""Address"":"""",""Branch"":"""",""City"":"""",""Class"":4,""CountryCode"":643,""CountyCode"":0,""Date"":""00000000"",""FiName"":"""",""Owner"":""POS_1"",""PSName"":""CPLS"",""Region"":"""",""RetailerName"":""compass_mrc_1"",""StateCode"":0,""TimeOffset"":0,""ZipCode"":0}},""LocalTransactionTime"":""22:41:15"",""LocalTransactionDate"":""2024-01-09T00:00:00""},""Header"":{""ProtocolVersion"":19,""RejectStatus"":0},""MessageType"":{""IsReject"":false,""TypeIdentifier"":103}}";
            AbstrParser.ParseStringTest(str, context.list, rootElement, true);

            var find = "Root/Fields/MiscellaneousTransactionAttributes2/Value/AED/Value/Value/Value/BrowserInfo/Value/timeZone";
            var tokens = find.Split("/");

            var el = rootElement.getAllDescentants(tokens, 0, context).First();
            // el.Value = "500";
            var el1 = rootElement.getAllDescentants(tokens.Take(tokens.Length - 1).ToArray(), 0, context).First();
            string val = el1.Value.ToString();
            var el3 = el1.copy(AbstrParser.CreateNode(null, new List<AbstrParser.UniEl>(), "Root1"));


            var newJson=rootElement.toJSON();
            bool comp = newJson == str;
          
            return el3.Value.ToString() == val;
        }

    }
    
    [MemoryDiagnoser]
    public class ReceiverTest
    {
        private Pipeline _pipeline;
        
        public ReceiverTest(int mockReceiveCount = 1)
        {
            // Load the pipeline from YAML
            Console.WriteLine("Loading pipeline...");
            Console.WriteLine($"Working directory is {Directory.GetCurrentDirectory()}");
            
            // print files in current dir
            // var files = Directory.GetFiles(Directory.GetCurrentDirectory()).OrderBy(path => path);
            // foreach (var file in files) Console.WriteLine(file);
            
            ParserLibrary.Logger.levelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);
            Log.Logger = CreateSerilog("IU", ParserLibrary.Logger.levelSwitch, true, false);
            Log.Information($"Service url on {Pipeline.ServiceAddr}");
            
            Pipeline.AgentPort = -1; // Disable Jaeger (it's off during normal operation anyway)
            
            _pipeline = Pipeline.load("Data/ACS_TW_mocked.yml", Assembly.GetAssembly(typeof(Plugins.DummyProtocol1.DummyProtocol1Receiver)));
            
            if (_pipeline is null)
                throw new InvalidOperationException("Pipeline is null");
            
            if (_pipeline.steps.Length == 0)
                throw new InvalidOperationException("Pipeline has no steps");
            
            // Switch all receivers and senders to mock mode
            foreach (var step in _pipeline.steps)
            {
                if (step.receiver is not null)
                {
                    step.receiver.MocMode = true;
                    step.receiver.MockReceiveCount = mockReceiveCount;
                }
                if (step.sender is not null)
                    step.sender.MocMode = true;
            }
            
            _pipeline.steps[0].Init(_pipeline);
        }
        
        [Benchmark]
        public async Task ReceiverBenchmark()
        {
            await _pipeline.run();
        }
        
        // Copied from WebApiConsoleUtility/Program.cs
        private static Serilog.ILogger CreateSerilog(string ServiceName, LoggingLevelSwitch levelSwitch, bool isAsync,
            bool LogHttpRequest, bool maskedSensitive = false)
        {
            LoggerConfiguration log = null;
            log = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                //            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithExceptionDetails()
                // .Enrich.WithProperty("TraceID", ((System.Diagnostics.Activity.Current != null) ? System.Diagnostics.Activity.Current.TraceId.ToString() : "-"))
                // .Enrich.WithProperty("ThreadID", Thread.CurrentThread.ManagedThreadId)
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithProperty("Service", ServiceName)
                .Filter.ByExcluding(c =>
                    !LogHttpRequest && c.Properties.ContainsKey("SourceContext") && c.Exception == null &&
                    c.Level != LogEventLevel.Error && c.Level != LogEventLevel.Fatal &&
                    c.Level != LogEventLevel.Warning)
                /*                 .Filter.ByExcluding(c =>
                                 c.Properties.ContainsKey("Method") && c.Properties["Method"].
                                 c.Properties.ContainsKey("SourceContext")
                                                  !LogHealthAndMonitoring &&
                                                  (c.Properties.Any(p => p.Value.ToString().Contains("ConsulHealthCheck")) || c.Properties.Any(p => p.Value.ToString().Contains("getMetrics")))
                                 )*/
                .Enrich.WithSpan(new SpanOptions()
                {
                    LogEventPropertiesNames = new SpanLogEventPropertiesNames
                        { TraceId = "TraceId", SpanId = "SpanId", ParentId = "ParentId" },
                })
                .Enrich.FromLogContext();
            if (isAsync)
            {
#if DEBUG

                log = log.WriteTo.Async(writeTo => writeTo.File("errors.log", LogEventLevel.Information,
                        "[{Timestamp:dd/MM/yy HH:mm:ss.ffff} {Level:u3} TraceId: {TraceId}] [{Properties}] {Message:lj}  {NewLine} {Exception}",
                        retainedFileCountLimit: 3))
                    .WriteTo.Async(writeTo =>
                        writeTo.Console(theme: AnsiConsoleTheme.Code, applyThemeToRedirectedOutput: true,
                            outputTemplate:
                            "[{Timestamp:dd/MM/yy HH:mm:ss.ffff} {Level:u3} TraceId: {TraceId}] [{Properties}] {Message:lj}  {NewLine} {Exception}"));
#else
                log = log.WriteTo.Async(writeTo => writeTo.Console(new RenderedCompactJsonFormatter()));
#endif
            }

            else
                log = log.WriteTo.Console(new RenderedCompactJsonFormatter());

            if (maskedSensitive)
                log = log.Enrich.WithSensitiveDataMasking(
                    options =>
                    {
                        options.MaskingOperators = new List<IMaskingOperator>
                        {
                            new EmailAddressMaskingOperator(),
                            new IbanMaskingOperator(),
                            new CreditCardMaskingOperator()
                            // etc etc
                        };
                    });
            return log.CreateLogger();
            // <<#<<#<<
        }

    }
    
    class Program
    {
        static async Task Main(string[] args)
        {
            /*  int cycle = 1;
              for (int i = 0; i < cycle; i++)
              {
                  string Body;
                  PutObjectS("POST", @"http://localhost:5000/WeatherForecast", "{\"Count\":" + i + "}", out Body);
              }
            */
            
            // Get the benchmark class from the first CLI argument
            var benchmarkClass = args[0];
            var simpleRun = args.Length > 1 && args[1].ToUpper() == "SIMPLE";
            switch (benchmarkClass)
            {
                case "ParserTest":
                    if (simpleRun)
                    {
                        Logger.log("Performing a simple run of ParserTest(), not using BenchmarkRunner");
                        new ParserTest().SimpleTest();
                    }
                    else
                    {
                        Logger.log("Performing full ParserTest() using BenchmarkRunner");
                        BenchmarkRunner.Run<ParserTest>();
                    }
                    break;
                case "ReceiverTest":
                    if (simpleRun)
                    {
                        const int mockReceiveCount = 20000;
                        
                        Logger.log("Performing a simple run of ReceiverTest() with mockReceiveCount={mockReceiveCount}", Serilog.Events.LogEventLevel.Information, "any", mockReceiveCount);
                        
                        var t0 = DateTime.Now;
                        await new ReceiverTest(mockReceiveCount: mockReceiveCount).ReceiverBenchmark();
                        var t1 = DateTime.Now;
                        Logger.log("ReceiverBenchmark() completed in {ms} ms", Serilog.Events.LogEventLevel.Information, "any", (t1 - t0).TotalMilliseconds);
                    }
                    else
                    {
                        Logger.log("Performing full ReceiverBenchmark() using BenchmarkRunner");
                        BenchmarkRunner.Run<ReceiverTest>();
                    }
                    Logger.log("ReceiverBenchmark() completed");
                    break;
                default:
                    Logger.log("Unknown benchmark class", LogEventLevel.Error);
                    break;
            }
        }
        public static byte[] PutObjectS(string Method, string postUrl, string payLoad, out string Body)
        {
            Body = "";
            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = Method;
            request.ContentType = "application/json";
            if (payLoad != null)
            {
                //                request.Headers.Add("X-Consul-Namespace", "team-1");
                var bytes = System.Text.Encoding.ASCII.GetBytes(payLoad);

                request.ContentLength = (bytes.Length);
                Stream dataStream = request.GetRequestStream();
                //                var bytes = System.Text.Encoding.ASCII.GetBytes(payload);
                dataStream.Write(bytes, 0, bytes.Length);
                //                Serialize(dataStream, payload);
                dataStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
            if (returnString == "OK")
            {
                List<byte> outBuff = new List<byte>();
                Stream stream = response.GetResponseStream();
                byte[] buffer = new byte[5000/*response.ContentLength*/];
                int bytesRead;

                {
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);

                        while (bytesRead > 0)
                        {
                            outBuff.AddRange(buffer[..bytesRead]);

                            bytesRead = stream.Read(buffer, 0, 256);
                        }

                        ASCIIEncoding coding = new ASCIIEncoding();
                        char[] chars = coding.GetChars(outBuff.ToArray());
                        Body = new string(chars);
                        return outBuff.ToArray();
                        //           binWriter.Write(buffer1);
                    }
                }

            }
            return new byte[] { };
        }

    }
}
