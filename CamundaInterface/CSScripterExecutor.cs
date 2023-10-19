using CSScriptLib;
using NCrontab;
using OpenTelemetry.Metrics;
using ParserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInterface
{
    public class CSScripterExecutor
    {
        public static async Task Start(string filePath = @"C:\Users\User\source\repos\projectx\WebApiCamundaExecutors\bin\Debug\net6.0\Data\DictCurrencyRates.cs ", string cronString = "15 9 * * *")
        {
            var countSuc=(Metrics.MetricCount)Metrics.metric.getMetricCount("suc_autoload_count", "count of success loading curses retries");
            var countFail = (Metrics.MetricCount)Metrics.metric.getMetricCount("fail_autoload_count", "count of failed loading curses retries");
            Logger.log($"current directory {Directory.GetCurrentDirectory()} path {Path.GetFullPath(filePath)}");
            string body = string.Empty;
            if (File.Exists(filePath))
            {
                //DictCurrencyRates.cs
                using (StreamReader sr = new StreamReader(filePath))
                {
                    body = sr.ReadToEnd();
                }
            }
            if (string.IsNullOrEmpty(body))
            {
                Logger.log($"start handler load curses");
                var checker = CSScript.RoslynEvaluator.CreateDelegate<Task<int>>(body);
                var ConnSelect = $"User ID={Environment.GetEnvironmentVariable("DB_USER_FPDB")};Password={Environment.GetEnvironmentVariable("DB_PASSWORD_FPDB")};Host={Environment.GetEnvironmentVariable("DB_URL_FPDB")};Port=5432;Database=fpdb;";
                var ConnAdm = $"User ID={Environment.GetEnvironmentVariable("DB_USER_FPDB")};Password={Environment.GetEnvironmentVariable("DB_PASSWORD_FPDB")};Host={Environment.GetEnvironmentVariable("DB_URL_FPDB")};Port=5432;Database=fpdb;SearchPath=md;";
                var schedule = CrontabSchedule.Parse("1 2 * * *");
                var errors = await checker(ConnSelect, ConnAdm);
                Task.Run(async () =>
                {
                    while (true)
                    {
                        var nextTime = schedule.GetNextOccurrence(DateTime.Now);
                        await Task.Delay((int)(nextTime - DateTime.Now).TotalMilliseconds);
                        try
                        {
                            errors = await checker(ConnSelect, ConnAdm);
                            if (errors == 0)
                                countSuc.Increment();
                            else
                                countFail.Increment();
                            //                        Logger.log("Error of loading curses ", Serilog.Events.LogEventLevel.Error, ex);
                        }
                        catch (Exception ex)
                        {
                            countFail.Increment();
                            Logger.log("Error of loading curses ", Serilog.Events.LogEventLevel.Error, ex);
                        }

                    }
                });
            }
        }

                public static async Task testScript()
        {
            string body = @"using System;
using System.Threading.Tasks;
using System.Net.Http;
using CamundaInterface;
using System.Linq;
async Task<bool>  ConvObject()
{                           

                var client = new HttpClient();
                var itog = await url_crowler.execGet(client
                                     , ""User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.consul;Port=5432;Database=fpdb;"", ""User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.consul;Port=5432;Database=fpdb;SearchPath=md;""
                                     , ""dictcurrency"", ""https://pkgstore.datahub.io/core/currency-codes/codes-all_json/data/029be9faf6547aba93d64384f7444774/codes-all_json.json""
                                     , @""select DISTINCT *
from json_to_recordset(@body::json)
as x(""""AlphabeticCode"""" text,""""Currency"""" text, """"NumericCode"""" float,""""MinorUnit"""" text)
WHERE x.""""AlphabeticCode"""" is not null and x.""""NumericCode"""" is not null""
                                     , 200);

return true;
}
";
            var schedule = CrontabSchedule.Parse("15 9 * * *");
            var nextTime= schedule.GetNextOccurrence(DateTime.Now);

            var filePath = @"C:\Users\User\source\repos\projectx\WebApiCamundaExecutors\bin\Debug\net6.0\Data\DictCurrencyRates.cs ";
            if (File.Exists(filePath))
            {
                //DictCurrencyRates.cs
                using (StreamReader sr = new StreamReader(filePath))
                {
                    body = sr.ReadToEnd();
                }
            }


                    var checker = CSScript.RoslynEvaluator.CreateDelegate<Task<int>>(body);
            //            var checker = CSScript.RoslynEvaluator.CreateDelegate(body);
            var ConnSelect = @"User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;";
            var ConnAdm = @"User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;SearchPath=md;";

            var ret =await checker(ConnSelect,ConnAdm);

        }

    }
}
