using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Logging;
using Newtonsoft.Json;
using Npgsql;
using ParserLibrary;
using Serilog;
using static CamundaInterface.APIExecutor;

namespace CamundaInterface
{
    public class CamundaExecutor
    {
        //        const string camundaPath = @"http://localhost:8080/engine-rest/";
        static string camundaPath = "";// @"http://192.168.75.217:18080/engine-rest/";
        public class ItemFetchAndLock
        {
            public class Topic
            {
                public string topicName { get; set; }
                public int lockDuration { get; set; }
                public List<string> variables { get; set; }
            }
            public int asyncResponseTimeout { get; set; } = 100000;
            public string workerId { get; set; }
            public int maxTasks { get; set; }
            public bool usePriority { get; set; }
            public List<Topic> topics { get; set; }
        }




        public class ExternalTaskAnswer
        {
            public class ValueInfo
            {
            }
            public class Variables
            {
                public string type { get; set; }
                public object value { get; set; }
                public ValueInfo valueInfo { get; set; }
            }

            public string activityId { get; set; }
            public string activityInstanceId { get; set; }
            public string errorMessage { get; set; }
            public string errorDetails { get; set; }
            public string executionId { get; set; }
            public string id { get; set; }
            public string lockExpirationTime { get; set; }
            public string processDefinitionId { get; set; }
            public string processDefinitionKey { get; set; }
            public string processInstanceId { get; set; }
            public object tenantId { get; set; }
            public int? retries { get; set; }
            public string workerId { get; set; }
            public int priority { get; set; }
            public string topicName { get; set; }
            public Dictionary<string, Variables> variables { get; set; } = null;
        }

        public class CamundaCompleteItem
        {
            public class Variable
            {
                public object value { get; set; }
            }
            public string workerId { get; set; }
            public Dictionary<string, Variable> variables { get; set; }
            public Dictionary<string, Variable> localVariables { get; set; } = null;
        }

        static HttpClient client = null;
        public static string workerId = "SimpleExecutor";



        
        public class ItemBpmnError
        {
            public class AnotherVariable
            {
                public bool value { get; set; }
                public string type { get; set; }
            }
            public class AVariable
            {
                public string value { get; set; }
                public string type { get; set; }
            }

            public class Variables
            {
                public AVariable aVariable { get; set; }
                public AnotherVariable anotherVariable { get; set; }
            }
            public string workerId { get; set; }
            public string errorCode { get; set; }
            public string errorMessage { get; set; }
            public Variables variables { get; set; }
        }

     


        public class ItemFailure
        {
            public string workerId { get; set; }
            public string errorMessage { get; set; }
            public string errorDetails { get; set; }
            
            public int retries { get; set; }
            public int retryTimeout { get; set; }
        }
        public static async Task runCycle()
        {
            while (0 == 0)
            {
                try
                {
                    await CamundaExecutor.fetch(new string[] { "integrity_utility", "to_dict_sender", "url_crowler", "to_exec_proc", "FimiConnector" });
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                    Log.Information("Restart fetch");
                }
            }
        }

        public static string certPath = "";
        public static async Task fetch(string[] topics)
        {
            if (string.IsNullOrEmpty(camundaPath))
            {
                var addr = Resolver.ResolveConsulAddr("Camunda");
//                addr = "localhost:8080";
                camundaPath = $"http://{addr}/engine-rest/";
            }
            ExternalTaskAnswer it1 = null;
            if (client == null)
            {
                // using System.Net.Http;
                // using System.Security.Authentication;
                // using System.Security.Cryptography.X509Certificates;
                if (!string.IsNullOrEmpty(certPath))
                {
                    var handler = new HttpClientHandler();
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.SslProtocols = SslProtocols.Tls12;
                    handler.ClientCertificates.Add(new X509Certificate2(certPath/*"cert.crt"*/));
                    client = new HttpClient(handler);
                } else
                    client = new HttpClient();
            }
            Log.Information("Start fetching on addr {@camundaPath}", camundaPath);

            while (0 == 0)
            {
                try
                {
                    Log.Information("Wait fetching");
                    var ans = await client.PostAsJsonAsync($"{camundaPath}external-task/fetchAndLock", new ItemFetchAndLock() { maxTasks = 1, usePriority = true, workerId = workerId, topics = topics.Select(ii => new ItemFetchAndLock.Topic() { lockDuration = 100000, topicName = ii }).ToList() });
                    if (ans.IsSuccessStatusCode)
                    {
                        Log.Information("input request");
                        var body = await ans.Content.ReadAsStringAsync();
                        var ret = JsonConvert.DeserializeObject<ExternalTaskAnswer[]>(body);// JsonSerializer.Deserialize<ExternalTaskAnswer[]>(body);

                        foreach (var item in ret)
                        {
                            it1 = item;
                            Log.Information("topic {@topicName} {@vars}", item.topicName, item.variables);
                            var dictOutput = new Dictionary<string, CamundaCompleteItem.Variable>();
                            /*                            if (item.topicName == "rest_executor1")
                                                        {
                                                            var ans1 = await TestRest.SendRest(item.variables["url"].value.ToString(), item.variables["request_body"].value.ToString());
                                                            var dict = new Dictionary<string, CamundaCompleteItem.Variable>();
                                                            dict.Add("response_body", new CamundaCompleteItem.Variable() { value = ans1 });
                                                            var ans3 = await client.PostAsJsonAsync($"{camundaPath}external-task/{item.id}/complete", new CamundaCompleteItem() { workerId = workerId, variables = dict });
                                                        }
                                                        else*/
                            if (item.topicName == "url_crowler")
                            {
                                Log.Information("get from url start");
                                /*   try
                                   {*/
                                var itog = await url_crowler.execGet(client
                                     , item.variables["ConnSelect"].value.ToString(), item.variables["ConnAdm"].value.ToString(), item.variables["Table"].value.ToString(), item.variables["URL"].value.ToString()
                                     , item.variables["SQL"].value.ToString()
                                     , Convert.ToInt32(item.variables["UpdateTimeout"].value.ToString()));
                                Log.Information("get from url  end");
                                /*                                } 
                                                                catch(Exception e77)
                                                                { 
                                                                    Log.Error(e77.ToString()); 
                                                                }*/
                                dictOutput.Add("All", new CamundaCompleteItem.Variable() { value = itog.all });
                                dictOutput.Add("Errors", new CamundaCompleteItem.Variable() { value = itog.errors });
                            }
                            if (item.topicName == "FimiConnector")
                            {
                                Log.Information("Start fimi connector ");
                                /*   try
                                   {*/
                                var trans = new FimiXmlTransport();

                                var ans1 = await new APIExecutor().ExecuteApiRequest(trans, System.Text.Json.JsonSerializer.Deserialize<ExecContextItem[]>(item.variables["FIMICommands"].value.ToString()), System.Text.Json.JsonSerializer.Deserialize<TableDefine[]>(item.variables["Tables"].value.ToString()), item.variables["SQLText"].value.ToString(), "User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;", item.variables,item.processInstanceId);
                                dictOutput.Add("All", new CamundaCompleteItem.Variable() { value = ans1.All });
                                dictOutput.Add("Errors", new CamundaCompleteItem.Variable() { value = ans1.Errors });
                                dictOutput.Add("OperUUID", new CamundaCompleteItem.Variable() { value = ans1.OperUUID });

                                /* if(!ans1)
                                 {
                                     var err = trans.getError().Reason.Text;
                                 }*/
                                /*                                var itog = await url_crowler.execGet(client
                                                                     , item.variables["FIMICommands"].value.ToString(), item.variables["ConnAdm"].value.ToString(), item.variables["Table"].value.ToString(), item.variables["URL"].value.ToString()
                                                                     , item.variables["SQL"].value.ToString()
                                                                     , Convert.ToInt32(item.variables["UpdateTimeout"].value.ToString()));
                                                                Log.Information("get from url  end");*/
                                /*                                } 
                                                                catch(Exception e77)
                                                                { 
                                                                    Log.Error(e77.ToString()); 
                                                                }*/
                                /*                                dictOutput.Add("All", new CamundaCompleteItem.Variable() { value = itog.all });
                                                                dictOutput.Add("Errors", new CamundaCompleteItem.Variable() { value = itog.errors });*/
                            }
                            if (item.topicName == "to_dict_sender")
                            {
                                Log.Information("Send to dict start");
                                /*   try
                                   {*/
                                var itog = await SendToRefDataLoader.putRequestToRefDataLoader(client, item.processDefinitionId + ":" + item.topicName
                                     , item.variables["ConnSelect"].value.ToString(), item.variables["ConnAdm"].value.ToString(), item.variables["DictName"].value.ToString(), "TEST", item.variables["SQLText"].value.ToString()
                                     , Convert.ToInt32(item.variables["MaxRecords"].value.ToString()), item.variables["DictAddr"].value.ToString(), item.variables["SensitiveData"].value.ToString() ,Convert.ToInt32(item.variables["CountInKey"]?.value), item.variables["Fields"].value.ToString());
                                Log.Information("Send to dict end");
                                /*                                } 
                                                                catch(Exception e77)
                                                                { 
                                                                    Log.Error(e77.ToString()); 
                                                                }*/
                                dictOutput.Add("All", new CamundaCompleteItem.Variable() { value = itog.all });
                                dictOutput.Add("Errors", new CamundaCompleteItem.Variable() { value = itog.errors });
                            }
                            if (item.topicName == "to_exec_proc")
                            {
                                Log.Information("Send to exec proc");


                                var params1=item.variables["ProcJson"].value;
                                var connString= item.variables["connectionString"].value;
                                SendToRefDataLoader.ExportItem itog = new SendToRefDataLoader.ExportItem();
                                itog=await ExecProcExecutor.execRequestProcedure(client);
                                /*   try
                                   {*/
                                /*var itog = await SendToRefDataLoader.putRequestToRefDataLoader(client, item.processDefinitionId + ":" + item.topicName
                                     , item.variables["ConnSelect"].value.ToString(), item.variables["ConnAdm"].value.ToString(), item.variables["DictName"].value.ToString(), "TEST", item.variables["SQLText"].value.ToString()
                                     , Convert.ToInt32(item.variables["MaxRecords"].value.ToString()), item.variables["DictAddr"].value.ToString(), item.variables["SensitiveData"].value.ToString(), Convert.ToInt32(item.variables["CountInKey"]?.value));
                                */
                                Log.Information("Send to dict end");
                                /*                                } 
                                                                catch(Exception e77)
                                                                { 
                                                                    Log.Error(e77.ToString()); 
                                                                }*/
                                itog.all = 1;
                                itog.errors = 0;
                                dictOutput.Add("All", new CamundaCompleteItem.Variable() { value = itog.all });
                                dictOutput.Add("Errors", new CamundaCompleteItem.Variable() { value = itog.errors });
                            }
                            if (item.topicName == "integrity_utility")
                            {
                                var ConnString = item.variables["DbConnectionString"].value.ToString();
                                var task_yaml = item.variables["task_yml"].value.ToString();
                                if (Regex.Match(task_yaml, @"^@\d+@$").Success)
                                {
                                    var tt = task_yaml.Trim();
                                    int id = Convert.ToInt32(tt.Substring(1, tt.Length - 2));
                                    NpgsqlConnection conn = new NpgsqlConnection(ConnString);
                                    conn.Open();
                                    var hash = "";
                                    await using (var cmd1 = new NpgsqlCommand("select val from MD_Camunda_CLOB where id=@id", conn))
                                    {
                                        cmd1.Parameters.AddWithValue("@id", id);
                                        await using (var reader = await cmd1.ExecuteReaderAsync())
                                        {
                                            while (await reader.ReadAsync())
                                            {
                                                task_yaml = reader.GetString(0);
                                            }
                                        }
                                    }
                                    conn.Close();
                                }
                                Pipeline pip = Pipeline.loadFromString(task_yaml);
                                await pip.run();
                                int sendCount = pip.steps.Sum(ii => ii.recordSendCount);

                                //                                var dict = new Dictionary<string, CamundaCompleteItem.Variable>();
                                dictOutput.Add("All", new CamundaCompleteItem.Variable() { value = sendCount });
                                dictOutput.Add("Errors", new CamundaCompleteItem.Variable() { value = 0 });
                            }
                            //$"/external-task/{id}/complete"
                                                        var ans3 = await client.PostAsJsonAsync($"{camundaPath}external-task/{item.id}/complete", new CamundaCompleteItem() { workerId = workerId, variables = dictOutput });
                            /*                            var ans3 = await client.PostAsJsonAsync($"{camundaPath}external-task/{item.id}/failure", new ItemFailure()
                                                        {
                                                            workerId = workerId,
                                                            errorMessage = "Error Message",
                                                            retries = 1,
                                                            retryTimeout = 6000
                                                        });*/
                            Log.Information("End topic execute {@a}", dictOutput);

                        }


                    }
                    else
                    {
                        Log.Information("unable get fetch ");

                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException ex)
                {
                    Log.Information("task cancelled:TimeoutExpired"/*+ex.ToString()*/);
                    continue;
                }
                catch (Exception ex5)
                {
                    Log.Error("Error:{@topic} {@err}",it1.topicName, ex5);
                    var ans3 = await client.PostAsJsonAsync($"{camundaPath}external-task/{it1.id}/bpmnError", new ItemBpmnError()
                    {
                        workerId = workerId,
                        errorCode="ErrorHappening",// $"error hapenned on {it1.topicName}",
                        errorMessage = $"topic:{it1.topicName} Detail {ex5.Message}!",
                        /*errorDetails = ex5.ToString(),
                        retries = 0,
                        retryTimeout = 600
                        */
                    });
/*                    var ans3 = await client.PostAsJsonAsync($"{camundaPath}external-task/{it1.id}/failure", new ItemFailure()
                    {
                        workerId = workerId,
                        errorMessage = $"error hapenned on {it1.topicName}!",
                        errorDetails= ex5.ToString(),
                        retries = 0,
                        retryTimeout = 600

                    });*/
                }
            }
            //            topics.Select(x => new ItemFetchAndLock.(x)).ToList();
        }
    }

}