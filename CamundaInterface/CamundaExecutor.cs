using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;
using ParserLibrary;
using Serilog;

namespace CamundaInterface
{
    public class CamundaExecutor
    {
//        const string camundaPath = @"http://localhost:8080/engine-rest/";
        const string camundaPath = @"http://192.168.75.217:18080/engine-rest/";
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

        public class ItemFailure
        {
            public string workerId { get; set; }
            public string errorMessage { get; set; }
            public string errorDetails { get; set; }
            
            public int retries { get; set; }
            public int retryTimeout { get; set; }
        }

        public static async Task fetch(string[] topics)
        {
            ExternalTaskAnswer it1 = null;
            if (client == null)
                client = new HttpClient();
            Log.Information("Start fetching");

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
                            var dictOutput = new Dictionary<string, CamundaCompleteItem.Variable>();
                            /*                            if (item.topicName == "rest_executor1")
                                                        {
                                                            var ans1 = await TestRest.SendRest(item.variables["url"].value.ToString(), item.variables["request_body"].value.ToString());
                                                            var dict = new Dictionary<string, CamundaCompleteItem.Variable>();
                                                            dict.Add("response_body", new CamundaCompleteItem.Variable() { value = ans1 });
                                                            var ans3 = await client.PostAsJsonAsync($"{camundaPath}external-task/{item.id}/complete", new CamundaCompleteItem() { workerId = workerId, variables = dict });
                                                        }
                                                        else*/
                            if (item.topicName == "to_dict_sender")
                            {
                                Log.Information("Send to dict start");
                                /*   try
                                   {*/
                                var itog = await SendToRefDataLoader.putRequestToRefDataLoader(client, item.processDefinitionId + ":" + item.topicName
                                     , item.variables["ConnSelect"].value.ToString(), item.variables["ConnAdm"].value.ToString(), item.variables["DictName"].value.ToString(), "TEST", item.variables["SQLText"].value.ToString()
                                     , Convert.ToInt32(item.variables["MaxRecords"].value.ToString()), item.variables["DictAddr"].value.ToString(), item.variables["SensitiveData"].value.ToString() ,(int)item.variables["CountInKey"]?.value);
                                Log.Information("Send to dict end");
                                /*                                } 
                                                                catch(Exception e77)
                                                                { 
                                                                    Log.Error(e77.ToString()); 
                                                                }*/
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
                    var ans3 = await client.PostAsJsonAsync($"{camundaPath}external-task/{it1.id}/failure", new ItemFailure()
                    {
                        workerId = workerId,
                        errorMessage = "error hapenned!",
                        errorDetails= ex5.ToString(),
                        retries = 0,
                        retryTimeout = 600

                    });
                }
            }
            //            topics.Select(x => new ItemFetchAndLock.(x)).ToList();
        }
    }

}