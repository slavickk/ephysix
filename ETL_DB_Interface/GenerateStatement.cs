using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CamundaInterface;
using CamundaInterfaces;
using MaxMind.Db;
using Npgsql;
using YamlDotNet.Core.Tokens;
using static System.Net.Mime.MediaTypeNames;
using static CamundaInterface.CamundaExecutor.ExternalTaskAnswer;


namespace ETL_DB_Interface
{
    public class GenerateStatement
    {
       static HttpClient client;
        public static string ConnectionStringAdm = "User ID=fp;Password=rav1234;Host=192.168.75.219;Port=5432;Database=fpdb;SearchPath=md;";
        public class CamundaAnswer1
        {
            public class Link
            {
                public string method { get; set; }
                public string href { get; set; }
                public string rel { get; set; }
            }

            public List<Link> links { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string source { get; set; }
//            public DateTime deploymentTime { get; set; }
            public string tenantId { get; set; }
            public object deployedCaseDefinitions { get; set; }
            public object deployedDecisionDefinitions { get; set; }
            public object deployedDecisionRequirementsDefinitions { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class StartProcOptions
        { 
            public ItemVariable variables { get; set; }= new ItemVariable();
            public string businessKey { get; set; }
        }
        public class ItemVariable
        {
            public class Item
            {
                public object value { get; set; }
                public string type { get; set; }
                public class ValueInfo
                {
                    public string objectTypeName { get; set; }
                    public string serializationDataFormat { get; set; }
                }
                public ValueInfo? valueInfo { get; set; }

            }
            [JsonExtensionData]
            public IDictionary<string, object> variables { get; set; }= new Dictionary<string, object>();
            public class AnotherVariable
            {
                public bool value { get; set; } = false;
                public string type { get; set; } = "Boolean";
            }

            public class AVariable
            {
                public string value { get; set; } = "String";
                public string type { get; set; } = "String";
            }
/*            public Variables variables { get; set; }
            public class Variables
            {
                public AVariable aVariable { get; set; } = new AVariable();
                public AnotherVariable anotherVariable { get; set; } = new AnotherVariable();
            }*/
        }


        public class ItemInfo
        {
            public class Link
            {
                public string method { get; set; }
                public string href { get; set; }
                public string rel { get; set; }
            }

            public List<Link> links { get; set; }
            public string id { get; set; }
            public string definitionId { get; set; }
            public object businessKey { get; set; }
            public object caseInstanceId { get; set; }
            public bool ended { get; set; }
            public bool suspended { get; set; }
            public object tenantId { get; set; }
        }


        public class HistoricalItem
        {
            public string id { get; set; }
            public object businessKey { get; set; }
            public string processDefinitionId { get; set; }
            public string processDefinitionKey { get; set; }
            public string processDefinitionName { get; set; }
            public int processDefinitionVersion { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
            public object removalTime { get; set; }
            public int durationInMillis { get; set; }
            public object startUserId { get; set; }
            public string startActivityId { get; set; }
            public object deleteReason { get; set; }
            public string rootProcessInstanceId { get; set; }
            public object superProcessInstanceId { get; set; }
            public object superCaseInstanceId { get; set; }
            public object caseInstanceId { get; set; }
            public object tenantId { get; set; }
            public string state { get; set; }
        }

        public class IncidentItem
        {
            public string id { get; set; }
            public string processDefinitionId { get; set; }
            public string processInstanceId { get; set; }
            public string executionId { get; set; }
//            public DateTime incidentTimestamp { get; set; }
            public string incidentType { get; set; }
            public string activityId { get; set; }
            public object failedActivityId { get; set; }
            public string causeIncidentId { get; set; }
            public string rootCauseIncidentId { get; set; }
            public string configuration { get; set; }
            public string incidentMessage { get; set; }
            public object tenantId { get; set; }
            public object jobDefinitionId { get; set; }
            public object annotation { get; set; }

            //http://localhost:8080/engine-rest/incident?processDefinitionId=ETL_Process532730%3A1%3Ae34e6e77-1484-11ed-a400-9eb6d0d49a5b
        }



//        public static string camundaAddr = "192.168.75.217:23536";
        public static string camundaAddr = "localhost:8080";

        public async static Task SendTest()
        {
            /*            examplefunction(
                timeExample date, IDEvent integer, comment1 text)*/

            CamundaInterface.ExecProcExecutor.ProcCalls proc = new ExecProcExecutor.ProcCalls()
            {
                procName = "examplefunction",
                param_proc = new ExecProcExecutor.ItemParams[]
                { new ExecProcExecutor.ItemParams() { Name="timeExample", Value="2023-03-11T00:00:00Z" },
                 new ExecProcExecutor.ItemParams() { Name="IDEvent", Value=5 },
                 new ExecProcExecutor.ItemParams() { Name="comment1", Value="all right" }
                }
            };

            var ss=JsonSerializer.Serialize<CamundaInterface.ExecProcExecutor.ProcCalls>(proc);
            var pars1 = JsonSerializer.Deserialize<CamundaInterface.ExecProcExecutor.ProcCalls>(ss);
            return;
            //            string processId = "main:1:4c134c56-c0da-11ed-bb90-0242ac110002";
            string processId = "main";
            string camundaPath = @"http://" + camundaAddr + @"/engine-rest/";
            if (client == null)
                client = new HttpClient();
            /*using (FileStream sw = File.OpenRead(@"C:\Camunda\main.bpmn"))
            {
                //                sw.Flush();
                //                sw.Write(buffer, 0, kol);
                //   File.OpenRead(path).CopyTo(sw);
                using (var multiPartFormContent = new MultipartFormDataContent())
                {
                    var sc = new StreamContent(sw);
                    //                    var sc = new StreamContent(File.OpenRead(path));
                    sc.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                    multiPartFormContent.Add(sc, "file", Path.GetFileName(@"C:\Camunda\main.bpmn") );

                    var response = await client.PostAsync($"{camundaPath}deployment/create", multiPartFormContent);
                    response.EnsureSuccessStatusCode();

                    string ans = await response.Content.ReadAsStringAsync();
                    var item = JsonSerializer.Deserialize<CamundaAnswer1>(ans);
                    Console.WriteLine($"OK: ");
                }
            }*/
                        StartProcOptions opt = new StartProcOptions();
            //extProcId
            opt.variables.variables.Add("procParams", new ItemVariable.Item() { valueInfo = new ItemVariable.Item.ValueInfo() { objectTypeName = "java.util.ArrayList", serializationDataFormat = "application/json" }, type = "Object", value = "[\"1\",\"2\"]" });
           // opt.variables.variables.Add("extProcId", new ItemVariable.Item() { type = "String", value = "extProc" });
            //  opt.variables.variables.Add(vv.Name+"1", new ItemVariable.Item() { type = vv.Type, value = vv.DefaultValue });
            //opt.businessKey = "a1";
            var json = JsonSerializer.Serialize<StartProcOptions>(opt);
            //                            CustomVariableCoverter.ConvertFromNodes(variables);
            //                        var var = new ItemVariable();
            
            var response1 = await client.PostAsJsonAsync<StartProcOptions>($"{camundaPath}process-definition/key/{processId}/start", opt);
            try
            {
                
                response1.EnsureSuccessStatusCode();
                string ans1 = await response1.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async static Task SendToCamunda(string path,string processId,List<ItemVar> variables)
        {
//                    const string camundaPath = @"http://localhost:8080/engine-rest/";
            string camundaPath = @"http://"+camundaAddr+@"/engine-rest/";
            if (client == null)
                client= new HttpClient();
//            var ans7 =await  GetIncidents(camundaPath);
            using (FileStream sw = File.OpenRead(path))
            {
                //                sw.Flush();
//                sw.Write(buffer, 0, kol);
                //   File.OpenRead(path).CopyTo(sw);
                using (var multiPartFormContent = new MultipartFormDataContent())
                {
                    var sc = new StreamContent(sw);
                    //                    var sc = new StreamContent(File.OpenRead(path));
                    sc.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                    multiPartFormContent.Add(sc, "file", Path.GetFileName(path) /*"people.csv"*/);

                    var response = await client.PostAsync($"{camundaPath}deployment/create", multiPartFormContent);
                    try
                    {
                        response.EnsureSuccessStatusCode();

                        string ans = await response.Content.ReadAsStringAsync();
                        var item=JsonSerializer.Deserialize<CamundaAnswer1>(ans);
                        Console.WriteLine($"OK: ");

                        StartProcOptions opt = new StartProcOptions();
                        if (variables.Count > 0)
                        {
                            foreach( var vv in variables)
                            {

                                opt.variables.variables.Add(vv.Name, new ItemVariable.Item() { type = vv.Type, value = vv.ToObject });
                              //  opt.variables.variables.Add(vv.Name+"1", new ItemVariable.Item() { type = vv.Type, value = vv.DefaultValue });

                            }
                            var json = JsonSerializer.Serialize<StartProcOptions>(opt);
                        }
//                            CustomVariableCoverter.ConvertFromNodes(variables);
//                        var var = new ItemVariable();
                        var response1 = await client.PostAsJsonAsync<StartProcOptions>($"{camundaPath}process-definition/key/{processId}/start", opt);
                        try
                        {
                            response1.EnsureSuccessStatusCode();
                            string ans1 = await response1.Content.ReadAsStringAsync();
                            var item1 = JsonSerializer.Deserialize<ItemInfo>(ans1);
                            if (!item1.ended)
                            {
                                var res=await WaitEndExec(camundaPath, item1);
                                Console.WriteLine($"Task  {processId} {((res.isSucess) ? "finished successfully" : "failed")} with message: {res.Description}");
                                //MessageBox.Show($"Task  {processId} {((res.isSucess)?"finished successfully":"failed")} with message: {res.Description}");

                            }

                        }
                        catch (HttpRequestException)
                        {
                            Console.WriteLine($"{response.StatusCode}:{await response1.Content.ReadAsStringAsync()}");
                            //                        Console.WriteLine($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
                        }

                    }
                    catch (HttpRequestException)
                    {
                        Console.WriteLine($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
                        //                        Console.WriteLine($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
//                        Console.WriteLine(e);
                        throw;
                    }
                    // if (response.IsSuccessStatusCode)
                    //    MessageBox.Show("Success");
                }
            }
        }

        static string argFromType(string var,string type)
        {
            switch (type.ToUpper())
            {
                case "DATE":
                    return $"\"'||to_char({var}, 'YYYY-MM-DD\"T\"HH24:MI:SS\"Z\"')||'\"";
                case "INTEGER":
                case "BOOLEAN":
                    return $"\"'||to_char({var})||'\"";
                default:
                    return $"\"'||{var}||'\"";
            }
        }

        static string ToPostgreType( string type)
        {
            switch (type.ToUpper())
            {
                case "DATE":
                case "INTEGER":
                case "BOOLEAN":
                    return type;
                default:
                    return "text";
            }
        }
        static string ToCamundaType(string type)
        {
            switch (type.ToUpper())
            {
//                case "DATE":
                case "INTEGER":
                case "BOOLEAN":
                    return type;
                default:
                    return "String";
            }
        }

        async static Task<string> formFunctionText(NpgsqlConnection conn,long id,string CamundaID)
        {
            string packageName = "";
            string packageDescription="";
            List<string> arguments = new List<string>();    
            List<string> jsons = new List<string>();
            string command = @"select n.name,a2.val description,a4.val,a.name nameVar,at1.val typeVar,at2.val defaultValue 
from md_Node n
left join md_node_attr_val a2  on( a2.attrid=43 and a2.nodeid=n.nodeid)
left join md_node_attr_val a4  on( a4.attrid=57 and a4.nodeid=n.nodeid)
left join md_arc a1 on (a1.fromid=n.nodeid  and a1.isdeleted=false)
left join md_node a on (a.nodeid = a1.toid and a.typeid=md_get_type('ETLVariable') and a.isdeleted=false)
left join md_node_attr_val at1 on (at1.nodeid=a.nodeid and at1.attrid=49)
left join md_node_attr_val at2 on (at2.nodeid=a.nodeid and at2.attrid=50)
where n.nodeid=@id and n.isdeleted=false and a.name is not null";

            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        packageName = reader.GetString(0);//.GetInt32(0);
                        packageDescription = reader.GetString(1);
                        var var1 = reader.GetString(3);
                        var typ=reader.GetString(4);
                        jsons.Add($"\"{var1}\": {{ \"type\": \"{ToCamundaType(typ)}\", \"value\": {argFromType(var1,typ)}}}");
                        arguments.Add($"{var1} {ToPostgreType( typ)}");
                    }
                }
            }


                string body = @"CREATE OR REPLACE FUNCTION fp.etlpackage_"+packageName+@"(
	"+string.Join(',',arguments)+((arguments.Count>0)?",":"")+ @"
	command text)
    RETURNS bigint
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE
    params jsonb;
BEGIN
    select ('{"+string.Join(',',jsons)+ $",\"extProcId\":{{\"value\":\"{CamundaID}\""+ @",""type"":""String""},'||command||'}')::jsonb into params;
    /*
      Автоматически создаваемая задача исполнения ETL "+$"{packageName} {packageDescription}"+ @".
      Входные параметры:
        Параметры ETL,
        command text - Описание параметров функции : вызываемой по окончании обработки ETL
        Пример:
'""procJob"":{""value"":""{\""procName\"":\""examplefunction\"",\""param_proc\"":[{\""Name\"":\""timeExample\"",\""Type\"":\""Date\"", \""Value\"":\""2023-03-11T00:00:00Z\""},{\""Name\"":\""IDEvent\"",\""Value\"":5,\""Type\"":\""Integer\""},{\""Name\"":\""comment1\"",\""Type\"":\""Text\"",\""Value\"":\""all right\""}]}"",""type"":""String""}'
      Выходные параметры:
        id_ - идентификатор запущенного задания
    */
    return app_insert_actions(pjparam := params, pcamunda_proc_id := 'main_etl_job');
END
$BODY$;
";
            return body;
        }


        public class ItemResult
        {
            public bool isSucess;
            public string Description;
        }


        private static async Task<ItemResult> WaitEndExec(string camundaPath, ItemInfo? item1)
        {
            const int TimeoutWaiting = 60;
            DateTime time =DateTime.Now;

            while ((DateTime.Now - time).TotalSeconds < TimeoutWaiting)
            {
                var ity = await GetExecutionInfo(camundaPath, item1.id);
                if (ity != null)
                {

                    var Incidents = await GetIncidents(camundaPath, item1.definitionId);
                    if (Incidents.Length > 0)
                    {
                        return new ItemResult() { isSucess = false, Description = Incidents[0].incidentMessage };
                    }
                }
                else
                {
                    await Task.Delay(200);
                    var it = await GetExecutionHistoryInfo(camundaPath, item1.id);
                    if (it != null)
                    {
                        return new ItemResult() { isSucess = true, Description = $"Task finished at {it.durationInMillis} ms" };
                    }
                }
                await Task.Delay(300);
            }
            return new ItemResult() { isSucess = false, Description = "Timeout reached" };
        }

        private static async Task<HistoricalItem> GetExecutionHistoryInfo(string camundaPath, string executionId = "ETL_Process532730:1:e34e6e77-1484-11ed-a400-9eb6d0d49a5b" /*"ETL_Process532746:1:634c8470-1488-11ed-a400-9eb6d0d49a5b"*/)
        {
            //            incident?processDefinitionId=
            var response3 = await client.GetAsync($"{camundaPath}history/process-instance/{executionId}");
            try
            {
                response3.EnsureSuccessStatusCode();
                var ans = await response3.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<HistoricalItem>(ans);

            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"{response3.StatusCode}:{await response3.Content.ReadAsStringAsync()}");
                //                        Console.WriteLine($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
            }

            return null;
        }

        private static async Task<ItemInfo> GetExecutionInfo(string camundaPath, string executionId = "ETL_Process532730:1:e34e6e77-1484-11ed-a400-9eb6d0d49a5b" /*"ETL_Process532746:1:634c8470-1488-11ed-a400-9eb6d0d49a5b"*/)
        {
            //            incident?processDefinitionId=
            var response3 = await client.GetAsync($"{camundaPath}execution/{executionId}");
            try
            {
                if(response3.StatusCode== System.Net.HttpStatusCode.NotFound)
                {
                    return null;
//                http://localhost:8080/engine-rest/history/process-instance/e16c585d-14df-11ed-a400-9eb6d0d49a5b
                }
                response3.EnsureSuccessStatusCode();
                var ans = await response3.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ItemInfo>(ans);

            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"{response3.StatusCode}:{await response3.Content.ReadAsStringAsync()}");
                //                        Console.WriteLine($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
            }

            return null;
        }

        private static async Task<IncidentItem[]> GetIncidents(string camundaPath, string definitionId= "ETL_Process532730:1:e34e6e77-1484-11ed-a400-9eb6d0d49a5b" /*"ETL_Process532746:1:634c8470-1488-11ed-a400-9eb6d0d49a5b"*/)
        {
//            incident?processDefinitionId=
            var response3 = await client.GetAsync($"{camundaPath}incident?processDefinitionId={definitionId}");
            try
            {
                response3.EnsureSuccessStatusCode();
                var ans = await response3.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IncidentItem[]>(ans);

            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"{response3.StatusCode}:{await response3.Content.ReadAsStringAsync()}");
                //                        Console.WriteLine($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
            }

            return null;
        }


        public class ItemRel
        {
            public bool isExternal = false;
            public int srcId;
            public long IdEtlRelation;
            public long IdFkRelation;
            public bool is1Skip=false;
            public bool is2Skip = false;
            public string Name1Table = "";
            public string Alias1Table="";
            public string Name2Table="";
            public string Alias2Table="";
            public string NameColumns1="";
            public string NameColumns2="";
            public int seq_id=-1;
            public void ToTableList(List<ItemTable> list)
            {
                if (list.Count(ii => ii.Name == this.Name1Table && ii.Alias == this.Alias1Table)==0)
                    list.Add(new ItemTable() { Alias = Alias1Table, Name = Name1Table });
                if (list.Count(ii => ii.Name == this.Name2Table && ii.Alias == this.Alias2Table) == 0)
                    list.Add(new ItemTable() { Alias = Alias2Table, Name = Name2Table });
            }

            public static string getKvalPrefix(string TableName,string Alias)
            {
                return ((Alias == "") ? TableName : Alias) + ".";
            }
            public string getOnClause()
            {
                var retValue = "";
                var arr1 = NameColumns1.Split(",");
                var arr2 = NameColumns2.Split(",");
                for(int i = 0; i < arr1.Length; i++)
                {
                    retValue+=((retValue != "") ? " AND " : "") + getKvalPrefix(Name1Table, Alias1Table) + arr1[i] + "=" + getKvalPrefix(Name2Table, Alias2Table) + arr2[i];
                }
                return retValue;
            }

        }

        public class ItemTable
        {
            public int src_id;
            public string src_name;
            public bool pci_dss_zone;
            public int seq_id;
            public string Name = "";
            public string Alias="";
            public string Condition="";
            public string url = "";
            public string sqlurl = "";
            public int IntervalUpdateInSec = 0;
            public List<RelItem> optionalRelItems = new List<RelItem>();
            public class SelectListItem
            {
                public string expression = "";
                public string alias = "";
                public bool fromOriginalSelect = false;
                public string outputTable;
                public SelectListItem(string Expression,string OutputTable)
                {
                    outputTable=OutputTable;
                    Expression = Expression.Trim();
                    if(Expression =="")
                    {
                        int yy = 0;
                    }
                    var items = Expression.Split(' ');
                    if(items.Length == 1)
                    {
                        expression =Expression;
                        alias = Expression;

                    }  else
                    {
                        // Tmp
                        expression = Expression.Substring(0,Expression.Length-items.Last().Length-1);
                        alias = items.Last();
                    }

                }
                public List<ItemTable.ColumnItem> getAllColumnsFromExpression(ItemTable table)
                {
                    List<ItemTable.ColumnItem> retValue = new List<ColumnItem>();
                    foreach (var col in table.columns)
                    {
                        if (expression.Split(' ').Count(ii=>ii==col.Name)>0  && !retValue.Contains(col))
                        {
                            if (col.Name == "externalid")
                            {
                                int yy = 0;
                            }
                            retValue.Add(col);
                        }
                    }
                    return retValue;

                }


                public string ToSelectListString(ItemTable table)
                {
                    string val = expression;
                    var kvalPrefix = ItemRel.getKvalPrefix(table.Name, table.Alias);

                    foreach (var col in getColumns(table))
                    {

                        val = Regex.Replace(val, @"(?<!\.)\b" + col.Name + @"\b", kvalPrefix + col.Name);
                        //                val = Regex.Replace(val, @"\b^(.){0}" + col + @"\b", kvalPrefix + col);
                        //                val = val.Replace(col, kvalPrefix + col);
                    }
                    return val+" "+alias;
                }

                public List<ItemTable.ColumnItem> getColumns(ItemTable table)
                {
                    List<ItemTable.ColumnItem> retValue = new List<ColumnItem> ();
                    foreach (var col in table.columns)
                    {
                        if (Regex.IsMatch(expression, @"\b" + col.Name + @"\b"))
                        {
                            retValue.Add (col);
                        }
                        //                val = Regex.Replace(val, @"\b^(.){0}" + col + @"\b", kvalPrefix + col);
                        //                val = val.Replace(col, kvalPrefix + col);
                    }
                    return retValue;
                }

            }

            public List<SelectListItem> SelectList = new List<SelectListItem>();

            public List<List<ColumnItem>> indexes = new List<List<ColumnItem>> ();
            public List<ColumnItem> needed_indexes = new List<ColumnItem>();
            public string getSelectList()
            {
                string retValue = "";
                foreach(var col in SelectList)
                {
                    var val = col.ToSelectListString(this).Trim();
                    if(val.Length > 0)
                        retValue += ((retValue == "") ? "" : ",") + val;
                }
                return retValue;
            }
            public long TableId;
            public class ColumnItem
            {
                public string Name;
                public string Type;
                public int Lengtn;
                public string OutputTable;
                public string SensitiveData = "";
                public int? synonym;
                public override string ToString()
                {
                    return $"{Name}:{OutputTable}";
                }
            }
            public List<ColumnItem> columns= new List<ColumnItem>();

        }
        public class RelItem
        {
            public ItemTable srcTable;
            public ItemTable dstTable;
            public ItemTable.ColumnItem colSrc;
            public ItemTable.ColumnItem colDst;

        }
        static IEnumerable<(ItemTable,ItemRel)> enumerateTables(List<(ItemTable, ItemRel)> outTables,ItemTable table, List<ItemTable> tables, List<ItemRel> rels, List<ItemRel> usedRels ,ItemRel currentRel)
        {
//            currentRel = null;
            outTables.Add( (table,currentRel));

            //                var table = tables.First();
            foreach (var rel in rels.Where(ii => !usedRels.Contains(ii) && ((ii.Name1Table == table.Name && ii.Alias1Table == table.Alias) || (ii.Name2Table == table.Name && ii.Alias2Table == table.Alias))))
            {
                currentRel = rel;
                usedRels.Add(rel);
                var table1 = ((rel.Name1Table == table.Name && rel.Alias1Table == table.Alias) ? tables.First(ii => ii.Name == rel.Name2Table && ii.Alias == rel.Alias2Table) : tables.First(ii => ii.Name == rel.Name1Table && ii.Alias == rel.Alias1Table));
//                yield return table1;
                enumerateTables(outTables, table1, tables, rels, usedRels,currentRel);
            }
            return outTables;
//            yield return 
        }
        public static  string formSQLStatement(IEnumerable<ItemRel> items,IEnumerable<ItemTable> itemTables,out List<ItemTable.ColumnItem> columns,List<ItemVar> variables ,int dest_id,out List<RelItem> relColumns)
        {
            relColumns= new List<RelItem>();    
            bool onlyOneTable = (items.Count() == 0 && itemTables.Count() == 1);
//                test();
            //    columnList = new List<ItemTable.ColumnItem>();
            var columns1 = new List<ItemTable.ColumnItem>();
            string selectList = "";
            string whereCondition = "";
            List<ItemRel> usedRels= new List<ItemRel>();
            List<ItemTable> usedTables = new List<ItemTable>();
            List<(ItemTable, ItemRel)> outTables = new List<(ItemTable, ItemRel)>();
            ItemRel outRel =null;
            var out_tables1=enumerateTables(outTables,itemTables.First(), itemTables.ToList(), items.ToList(), usedRels, outRel);
            string returnValue1 = "";
            foreach (var item in out_tables1)// itemTables)
            {
                usedTables.Add(item.Item1);
//                if (onlyOneTable || items.Count(ii => (!ii.is1Skip && ii.Name1Table == item.Name && ii.Alias1Table == item.Alias) || (!ii.is2Skip && ii.Name2Table == item.Name && ii.Alias2Table == item.Alias)) > 0)
                {
                    if (item.Item1.SelectList.Count >0 )
                    {
                        foreach (var srcCol in item.Item1.SelectList.Where(ii => columns1.Count(ii1 => ii1.Name == ii.expression && ii1.OutputTable== ii.outputTable) == 0 && ii.alias != "" && ii.expression != "").Select(ii => new ItemTable.ColumnItem() 
                        {
                            Name = ii.alias, OutputTable=ii.outputTable, Type = ii.getAllColumnsFromExpression(item.Item1).First().Type, Lengtn = ii.getAllColumnsFromExpression(item.Item1).First().Lengtn
                        }))
                        {
                            columns1.Add(srcCol);
                            relColumns.Add(new RelItem() { srcTable = item.Item1, colSrc = srcCol, colDst = srcCol });                    
                        }
                        var str= item.Item1.getSelectList().Trim();
                        if(str.Length>0)
                        selectList +=((selectList=="")?"":",")+ str;//  prepareSQLString(item.SelectList, item);

                    }
                    if (item.Item1.Condition != "" && !usedTables.Contains(item.Item1))
                    {
                        if (whereCondition != "")
                            whereCondition += " AND ";
                        whereCondition += "(" + prepareSQLString(item.Item1.Condition, item.Item1) + ")";

                    }
                }
                if(item.Item2 == null)
                    returnValue1 += $"{item.Item1.Name}  {item.Item1.Alias} \r\n";
                else
                {
                    if (!(item.Item2.Name2Table==item.Item1.Name && item.Item2.Alias2Table== item.Item1.Alias))
                        returnValue1 += $" inner join  {item.Item2.Name1Table} {item.Item2.Alias1Table} on ({item.Item2.getOnClause()})  \r\n";
                    else
                        returnValue1 += $" inner join   {item.Item2.Name2Table} {item.Item2.Alias2Table} on ({item.Item2.getOnClause()})  \r\n";

                }


            }
            columns = columns1;
            string returnValue = $"select {selectList} from {returnValue1}";





/*            List<ItemTable> list=new List<ItemTable>();
            if (onlyOneTable)
            {
                var item = itemTables.First();
                returnValue += $"{item.Name}  \r\n";
                list.Add(item);

            }
            else
            {
                foreach (var item in items)
                {
                    bool second = true;
                    if (list.Count == 0)
                    {
                        if (!item.is1Skip)
                        {
                            returnValue += $"{item.Name1Table}  {item.Alias1Table} \r\n";
                            if (item.is2Skip)
                                second = false;
                        }
                        else
                        {
                            if (!item.is2Skip)
                            {
                                second = false;

                                returnValue += $"{item.Name2Table}  {item.Alias2Table} \r\n";
                            }
                        }

                    }
                    else
                    {
                        if (list.Count(ii => ii.Name == item.NameColumns2 && ii.Alias == item.Alias2Table) > 0)
                            second = false;
                    }
                    if (!item.isExternal  || itemTables.First(ii=>ii.Name== item.Name1Table).src_id == itemTables.First(ii => ii.Name == item.Name2Table).src_id)
                    {
                        if (!second)
                            returnValue += $" inner join  {item.Name1Table} {item.Alias1Table} on ({item.getOnClause()})  \r\n";
                        else
                            returnValue += $" inner join   {item.Name2Table} {item.Alias2Table} on ({item.getOnClause()})  \r\n";
                    }
                    item.ToTableList(list);
                }
            }*/
            if (whereCondition != "")
                returnValue = returnValue + "\r\n WHERE " + preoWhere(whereCondition,variables,dest_id);
            return returnValue;
        }
        static string preoWhere( string where,List<ItemVar> vars, int dest_id)
        {
            if(dest_id !=11)
            {
                foreach (var item in vars)
                    where = where.Replace("@" + item.Name, ":" + item.Name);
            }
            return where;
        }

        public class ItemVar
        {
            public string Name;
            public string Type;
            public string DefaultValue;
            public string Description;
            public object ToObject
            {
                get
                {
                    switch (Type)
                    {
                        case "String":
                            return DefaultValue;
                        case "Boolean":
                            return Convert.ToBoolean(DefaultValue);
                        case "Integer":
                            return Convert.ToInt64(DefaultValue);
                        case "Float":
                            return Convert.ToDouble(DefaultValue);
                        default:
                            return null;


                    }
                }
            }

        }
        public class ETL_Package
        {
            public long packet_id;
            public List<ItemVar> variables = new List<ItemVar>();
            public List<ItemRel> list = new List<ItemRel>();
//            public List<string> usedExternalTasks=  new List<string>();
            public string NamePacket = "";
            public string outputTable = "output_Table";
            public string description = "";
            public int dest_id = 2;
            public string dest_name;
            public int keyCount = 1;
            public List<ItemTable> allTables = new List<ItemTable>();
            public List<CamundaProcess.ExternalTask> usedExternalTasks= new List<CamundaProcess.ExternalTask>();
            ItemTable getTable(int i,int countAll)
            {
                bool isFinishTask = i >= countAll;
                var task = new ItemTask();
                if (isFinishTask)
                {
                    task.outputPath = this.outputTable.ToLower();

                }
                else
          
                    task.outputPath = $"tmp_table{i}_{this.packet_id}";
                task.indexes.AddRange(allTables.Where(ii => ii.seq_id == i && ii.needed_indexes.Count > 0).Select(ii => ii.needed_indexes));
                task.source_id = allTables.First().src_id;
                if (isFinishTask && i > 1)
                    task.source_id = 5; //Temp decision
                else
                    task.source_id = this.dest_id;
                List<RelItem> outCols;
                List<ItemTable.ColumnItem> columns = getColumnsForStep(list, allTables, i, variables, task,out outCols);
                task.outputTable.src_id = task.source_id;
                task.outputTable.optionalRelItems= outCols; 
                return task.outputTable;
            }
           public IEnumerable<ItemTable> tables
            {
                get
                {
                    int count=fillInitSeqID(this);
                    return allTables.Union(Enumerable.Range(0, count).Select(ii => getTable(((count==1)?1:ii),count)));
//                    List<ItemTable.ColumnItem> columns = getColumnsForStep(list, allTables, i, variables, new ItemTask());




                }
            }

        }
        public static async Task Generate(NpgsqlConnection conn, long id)
        {
            CamundaProcess process = new CamundaProcess();
            string CamundaID= $"ETL_Process{id}";
            ETL_Package package = await getPackage(conn, id);
            await SaveProcedure(conn, id,package.NamePacket,CamundaID);


            //if()

            /*            foreach (var item in allTables)
                        {
                            item.columns = await getColumns(item.TableId, conn);
                        }
            */
            List<ItemTask> tasks = new List<ItemTask>();
            process.ProcessID = CamundaID;
            process.ProcessName = $"{package.NamePacket}{id}";
            process.documentation = $"{package.description}\r\n  Not contain input variables!";
            //            process.save($"c:\\Camunda\\{NamePacket}.bpmn");
            process.tasks.Clear();
            if (package.list.Count == 0)
            {

                if (package.allTables.Count == 0)
                {
                    throw new Exception($"The package {id} is empty");
                    //                    MessageBox.Show($"The package {id} is empty");
                    return;
                }
            }

            int countTask = fillInitSeqID(package);
            //            string outputPath = "outputTable";
            bool isExternalDest = countTask == 1 /*&& list[0].srcId != 2*/ && package.dest_id != 5;
            for (int i = 1; i <= countTask; i++)
            {
                await AddTask(package,conn, process, package.list, package.allTables, tasks, (countTask == 1 && !isExternalDest), package.outputTable, i, package.dest_id, id, package.variables, package.keyCount);
            }
            if (countTask == 0)
            {
                await AddTask(package,conn, process, package.list, package.allTables, tasks, false, package.outputTable, 1, package.dest_id, id, package.variables, package.keyCount);
                countTask = 1;
                isExternalDest = countTask == 1 && package.dest_id != 5;
            }
            /* if (isExternalDest)
                 countTask++;
            */

            if (countTask > 1)
            {
                int seq_id = countTask;

                for (int i = 0; i < countTask; i++)
                {
                    var task = tasks[i];
                    ItemRel rel = FillTableRel(seq_id + 1, task.seq_id, package.list, package.allTables, tasks);
                    if (rel != null)
                    {
                        seq_id++;
                        rel.seq_id = seq_id;
                        //                        rel.isExternal = true; //No!!!!
                        package.list.Add(rel);

                        await AddTask(package,conn, process, package.list, package.allTables, tasks, true, package.outputTable, rel.seq_id, package.dest_id, id, package.variables, package.keyCount);

                    }

                }
            }
            if (isExternalDest)
            {
                tasks[0].seq_id = 1;
                tasks[0].outputTable.seq_id = 2;
                await AddTask(package, conn, process, new List<ItemRel>() { }, new List<ItemTable>() { tasks[0].outputTable }, tasks, true, package.outputTable, 2, package.dest_id, id, package.variables, package.keyCount);

            }


            var path1 = $"{pathToSaveETL}{package.NamePacket}.bpmn";
            process.save(path1);
            package.SaveMDDefinition();

            //            await SendToCamunda(@"C:\Camunda\Temp6.bpmn", "ETL_Process532730");

            await SendToCamunda(path1, process.ProcessID, package.variables);

        }
        public static string pathToSaveETL = @"C:\CamundaTopics\camundatopics\BPMN\ETL\";
        public static string pathToSaveExternalTask = @"C:\CamundaTopics\camundatopics\ExternalTasks\";
        private static async Task SaveProcedure(NpgsqlConnection conn, long id,string NamePacket,string CamundaID)
        {
         //   return;
            var body = await formFunctionText(conn, id,CamundaID);
            NpgsqlConnection connAdm = new NpgsqlConnection(GenerateStatement.ConnectionStringAdm);
            await connAdm.OpenAsync();
           /* var command = @"INSERT INTO fp.app_action(
	title_lid, title, ajson, camunda_proc_id, status,sign)
	VALUES ('ETL create action', 'ETL create action', '{}', @id_packet,   0,'ETL')
";
            await using (var cmd = new NpgsqlCommand(command, connAdm))
            {
                cmd.Parameters.AddWithValue("@id_packet", NamePacket);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                    }
                }
            }*/
            await using (var cmd = new NpgsqlCommand(body, connAdm))
            {
                //cmd.Parameters.AddWithValue("@id_packet", NamePacket);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                    }
                }
            }
        
        await connAdm.CloseAsync();
        }

        public static int fillInitSeqID(ETL_Package package)
        {
            int countTask = 0;
            if (package.list.Count > 0)
                countTask = package.list.Max(ii => ii.seq_id);
            if (countTask == 0)
            {
                var s_id = 1;
                package.allTables.First().seq_id = s_id;//1;??? 1;
            }

            return countTask;
        }
        public static async Task<ETL_Package> fromPackage(NpgsqlConnection conn, BlazorAppCreateETL.Shared.ETL_Package ext_package)
        {
            ETL_Package package = new ETL_Package();
            /*
            ETL_Package package = new ETL_Package() { packet_id = ext_package.idPackage };
            package.NamePacket = ext_package.ETLName; ;
            package.outputTable = ext_package.TableOutputName;
            package.description = ext_package.ETLDescription;
            package.dest_id = ext_package.ETL_dest_id;
            package.keyCount = 1;//???
            foreach (var var1 in ext_package.variables)
            {
                package.variables.Add(new ItemVar() { Name = var1.Name, Type = var1.VariableType, DefaultValue = var1.VariableDefaultValue });

            }

            foreach (var var2 in ext_package.relations)
            {
                ItemRel rel = new ItemRel();
                rel.IdFkRelation = var2.relationID;
                rel.isExternal = true;
                package.list.Add(rel);

            }

            //            List<ItemRel> listExternal = new List<ItemRel>();
            //          int keyCount = 1;
            //Variables

            {
            }


            {
                var command = @"select a.srcid,n.nodeid,a.nodeid from md_node a 
inner join md_node n on (n.typeid=md_get_type('ETLRelation') and n.isdeleted=false)
inner join md_arc a1 on (a.nodeid=a1.toid and a1.fromid=n.nodeid and a1.isdeleted=false)
inner join md_arc a2 on (a2.toid=n.nodeid and a2.fromid=@id and a2.isdeleted=false)
where a.typeid=md_get_type(@key) and a.isdeleted=false
";


               
                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@key", "VForeignKey");
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ItemRel rel = new ItemRel();
                            rel.srcId = reader.GetInt32(0);
                            rel.IdEtlRelation = reader.GetInt64(1);
                            rel.IdFkRelation = reader.GetInt64(2);
                            rel.isExternal = true;
                            package.list.Add(rel);
                        }
                    }
                }
            }

            {
                var command = @"select a.srcid,n.nodeid,a.nodeid from md_node a 
inner join md_node n on (n.typeid=md_get_type('ETLRelation') and n.isdeleted=false)
inner join md_arc a1 on (a.nodeid=a1.toid and a1.fromid=n.nodeid and a1.isdeleted=false)
inner join md_arc a2 on (a2.toid=n.nodeid and a2.fromid=@id and a2.isdeleted=false)
where a.typeid=md_get_type(@key) and a.isdeleted=false
";
                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@key", "ForeignKey");
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ItemRel rel = new ItemRel();
                            rel.srcId = reader.GetInt32(0);
                            rel.IdEtlRelation = reader.GetInt64(1);
                            rel.IdFkRelation = reader.GetInt64(2);
                            package.list.Add(rel);
                        }
                    }
                }
            }
            if (package.list.Count == 0)
            {
                await FillTableInfo(conn, package.allTables, null, id);

            }
            foreach (var item in package.list)
            {
                //string a1 = @"";
                await FillTableInfo(conn, package.allTables, item, item.IdEtlRelation);

                {
                    string command = @"select t.name1table,t.columns1table,t.name2table,t.columns2table from md_fk_info_as_table(@idFK) t";
                    await using (var cmd = new NpgsqlCommand(command, conn))
                    {
                        cmd.Parameters.AddWithValue("@idFK", item.IdFkRelation);
                        await using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                string table1 = reader.GetString(0);
                                string columns1 = reader.GetString(1);
                                string table2 = reader.GetString(2);
                                string columns2 = reader.GetString(3);
                                // if (item.isExternal == false)
                                {
                                    if (table1 == item.Name1Table)
                                    {
                                        item.NameColumns1 = columns1;
                                        item.NameColumns2 = columns2;
                                    }
                                    else
                                    {
                                        item.NameColumns1 = columns2;
                                        item.NameColumns2 = columns1;

                                    }
                                }
                                if (item.isExternal)
                                {
                                    var t1 = package.allTables.First(ii => ii.Name == table1);
                                    var t2 = package.allTables.First(ii => ii.Name == table2);
                                    if (t1.src_id != t2.src_id)
                                    {
                                        t1.needed_indexes = columns1.Split(',').Select(ii => t1.columns.First(i1 => i1.Name == ii.Trim())).ToList();//
                                        t1.SelectList.AddRange(columns1.Split(',').Select(ii => new ItemTable.SelectListItem(ii)));
                                        t2.needed_indexes = columns2.Split(',').Select(ii => t2.columns.First(i1 => i1.Name == ii.Trim())).ToList();//
                                        t2.SelectList.AddRange(columns2.Split(',').Select(ii => new ItemTable.SelectListItem(ii)));
                                    }
                                }
                            }
                        }
                    }

                }


            }
            SearchPaths(package.list);

            foreach (var item in package.list)
            {
                bool skipFirst = false;
                bool skipSecond = false;
                if (item.isExternal)
                {
                    if (package.list.Count(ii => ii.seq_id != item.seq_id && ((ii.Name1Table == item.Name1Table && ii.Alias1Table == item.Alias1Table) || ((ii.Name2Table == item.Name1Table && ii.Alias2Table == item.Alias1Table)))) == 0 && package.allTables.First(ii => ii.Name == item.Name1Table).src_id != package.allTables.First(ii => ii.Name == item.Name2Table).src_id)
                    {
                        skipSecond = true;
                        //                        item.Name2Table = "";
                    }
                    if (package.list.Count(ii => ii.seq_id != item.seq_id && ((ii.Name1Table == item.Name2Table && ii.Alias1Table == item.Alias2Table) || ((ii.Name2Table == item.Name2Table && ii.Alias2Table == item.Alias2Table)))) == 0 && package.allTables.First(ii => ii.Name == item.Name1Table).src_id != package.allTables.First(ii => ii.Name == item.Name2Table).src_id)
                    {
                        skipFirst = true;
                    }
                    if (skipFirst)
                        item.is1Skip = true;
                    if (skipSecond)
                        item.is2Skip = true;
                }

                if (item.Name1Table != "")
                    package.allTables.First(ii => ii.Name == item.Name1Table && ii.Alias == item.Alias1Table).seq_id = item.seq_id;
                if (item.Name2Table != "")
                    package.allTables.First(ii => ii.Name == item.Name2Table && ii.Alias == item.Alias2Table).seq_id = item.seq_id;

            }
            */
            return package;
        }

        public static async Task<ETL_Package> getPackage(NpgsqlConnection conn, long id)
        {
            ETL_Package package = new ETL_Package() { packet_id = id };
            //            List<ItemRel> listExternal = new List<ItemRel>();
            //          int keyCount = 1;
            {
                var command = @"select n.name,a1.val out_table,a2.val description,a3.val type_src,a4.val  from md_Node n
left join md_node_attr_val a1  on( a1.attrid=42 and a1.nodeid=n.nodeid)
left join md_node_attr_val a2  on( a2.attrid=43 and a2.nodeid=n.nodeid)
left join md_node_attr_val a3  on( a3.attrid=44 and a3.nodeid=n.nodeid)
left join md_node_attr_val a4  on( a4.attrid=57 and a4.nodeid=n.nodeid)
  where n.nodeid=@id and n.isdeleted=false";
                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            package.NamePacket = reader.GetString(0);
                            if (!reader.IsDBNull(1))
                                package.outputTable = reader.GetString(1);
                            if (!reader.IsDBNull(2))
                                package.description = reader.GetString(2);
                            if (!reader.IsDBNull(3))
                                package.dest_id = Convert.ToInt32(reader.GetString(3));
                            if (!reader.IsDBNull(4))
                            {
                                var val1 = reader.GetString(4);
                                if (!string.IsNullOrEmpty(val1))
                                    package.keyCount = Convert.ToInt32(reader.GetString(4));
                            }
                        }
                    }
                }


            }
            package.dest_name = (await getSrcInfo(package.dest_id, conn)).description;
            //Variables

            {
                var command = @"select a.name,at1.val,at2.val,at3.val description from md_node a 
inner join md_arc a1 on (a.nodeid=a1.toid and a1.fromid=@id  and a1.isdeleted=false)
left join md_node_attr_val at3 on (at3.nodeid=a.nodeid and at3.attrid=46)
left join md_node_attr_val at1 on (at1.nodeid=a.nodeid and at1.attrid=49)
left join md_node_attr_val at2 on (at2.nodeid=a.nodeid and at2.attrid=50)
where a.typeid=md_get_type('ETLVariable') and a.isdeleted=false
";



                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            package.variables.Add(new ItemVar() { Name = reader.GetString(0), Type = reader.GetString(1), DefaultValue = reader.GetString(2), Description = reader.GetString(3) });

                        }
                    }
                }
            }


            {
                var command = @"select a.srcid,n.nodeid,a.nodeid from md_node a 
inner join md_node n on (n.typeid=md_get_type('ETLRelation') and n.isdeleted=false)
inner join md_arc a1 on (a.nodeid=a1.toid and a1.fromid=n.nodeid and a1.isdeleted=false)
inner join md_arc a2 on (a2.toid=n.nodeid and a2.fromid=@id and a2.isdeleted=false)
where a.typeid=md_get_type(@key) and a.isdeleted=false
";


                /*                var command = @"select a.srcid,n.nodeid,a.nodeid from md_node a 
                inner join md_node n on (n.typeid=md_get_type('ETLRelation'))
                inner join md_arc a1 on (a.nodeid=a1.toid and a1.fromid=n.nodeid)
                inner join md_arc a2 on (a2.toid=n.nodeid and a2.fromid=@id)
                where a.typeid=md_get_type(@key)
                ";*/

                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@key", "VForeignKey");
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ItemRel rel = new ItemRel();
                            rel.srcId = reader.GetInt32(0);
                            rel.IdEtlRelation = reader.GetInt64(1);
                            rel.IdFkRelation = reader.GetInt64(2);
                            rel.isExternal = true;
                            package.list.Add(rel);
                        }
                    }
                }
            }

            {
                var command = @"select a.srcid,n.nodeid,a.nodeid from md_node a 
inner join md_node n on (n.typeid=md_get_type('ETLRelation') and n.isdeleted=false)
inner join md_arc a1 on (a.nodeid=a1.toid and a1.fromid=n.nodeid and a1.isdeleted=false)
inner join md_arc a2 on (a2.toid=n.nodeid and a2.fromid=@id and a2.isdeleted=false)
where a.typeid=md_get_type(@key) and a.isdeleted=false
";
                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@key", "ForeignKey");
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ItemRel rel = new ItemRel();
                            rel.srcId = reader.GetInt32(0);
                            rel.IdEtlRelation = reader.GetInt64(1);
                            rel.IdFkRelation = reader.GetInt64(2);
                            package.list.Add(rel);
                        }
                    }
                }
            }
            if (package.list.Count == 0)
            {
                await FillTableInfo(conn, package.allTables, null, id);

            }
            foreach (var item in package.list)
            {
                //string a1 = @"";
                await FillTableInfo(conn, package.allTables, item, item.IdEtlRelation);

                {
                    string command = @"select t.name1table,t.columns1table,t.name2table,t.columns2table from md_fk_info_as_table(@idFK) t";
                    await using (var cmd = new NpgsqlCommand(command, conn))
                    {
                        cmd.Parameters.AddWithValue("@idFK", item.IdFkRelation);
                        await using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                string table1 = reader.GetString(0);
                                string columns1 = reader.GetString(1);
                                string table2 = reader.GetString(2);
                                string columns2 = reader.GetString(3);
                                // if (item.isExternal == false)
                                {
                                    if (table1 == item.Name1Table)
                                    {
                                        item.NameColumns1 = columns1;
                                        item.NameColumns2 = columns2;
                                    }
                                    else
                                    {
                                        item.NameColumns1 = columns2;
                                        item.NameColumns2 = columns1;

                                    }
                                }
                                if (item.isExternal)
                                {
                                    var t1 = package.allTables.First(ii => ii.Name == table1);
                                    var t2 = package.allTables.First(ii => ii.Name == table2);
                                    if (t1.src_id != t2.src_id)
                                    {
                                        t1.needed_indexes = columns1.Split(',').Select(ii => t1.columns.First(i1 => i1.Name == ii.Trim())).ToList();//
                                        t1.SelectList.AddRange(columns1.Split(',').Select(ii => new ItemTable.SelectListItem(ii,"")));
                                        t2.needed_indexes = columns2.Split(',').Select(ii => t2.columns.First(i1 => i1.Name == ii.Trim())).ToList();//
                                        t2.SelectList.AddRange(columns2.Split(',').Select(ii => new ItemTable.SelectListItem(ii,"")));
                                    }
                                }
                            }
                        }
                    }

                }


            }
            SearchPaths(package.list);

            foreach (var item in package.list)
            {
                bool skipFirst = false;
                bool skipSecond = false;
                if (item.isExternal)
                {
                    if (package.list.Count(ii => ii.seq_id != item.seq_id && ((ii.Name1Table == item.Name1Table && ii.Alias1Table == item.Alias1Table) || ((ii.Name2Table == item.Name1Table && ii.Alias2Table == item.Alias1Table)))) == 0 && package.allTables.First(ii => ii.Name == item.Name1Table).src_id != package.allTables.First(ii => ii.Name == item.Name2Table).src_id)
                    {
                        skipSecond = true;
                        //                        item.Name2Table = "";
                    }
                    if (package.list.Count(ii => ii.seq_id != item.seq_id && ((ii.Name1Table == item.Name2Table && ii.Alias1Table == item.Alias2Table) || ((ii.Name2Table == item.Name2Table && ii.Alias2Table == item.Alias2Table)))) == 0 && package.allTables.First(ii => ii.Name == item.Name1Table).src_id != package.allTables.First(ii => ii.Name == item.Name2Table).src_id)
                    {
                        skipFirst = true;
                    }
                    if (skipFirst)
                        item.is1Skip = true;
                    if (skipSecond)
                        item.is2Skip = true;
                }

                if (item.Name1Table != "")
                    package.allTables.First(ii => ii.Name == item.Name1Table && ii.Alias == item.Alias1Table).seq_id = item.seq_id;
                if (item.Name2Table != "")
                    package.allTables.First(ii => ii.Name == item.Name2Table && ii.Alias == item.Alias2Table).seq_id = item.seq_id;

            }

            return package;
        }

        private static async Task FillTableInfo(NpgsqlConnection conn, List<ItemTable> allTables, ItemRel item,long id)
        {
            {
                string command = @"select at.val,n.name,st.val,st1.val,a1.toid,torig.srcid
,au.val url,asq.val isql,asi.val intrval,src.descr,true,st9.val
from md_arc a
inner join md_node n on(n.nodeid = a.toid and n.typeid = md_get_type('ETLTable') and n.isdeleted=false)
inner join md_arc a1 on (n.nodeid=a1.fromid  and a1.isdeleted=false)
inner join md_node torig on(torig.nodeid=a1.toid)

left join md_node_attr_val au on (a1.toid=au.nodeid and au.attrid=107 and au.isdeleted=false)
left join md_node_attr_val asq on (a1.toid=asq.nodeid and asq.attrid=108 and asq.isdeleted=false)
left join md_node_attr_val asi on (a1.toid=asi.nodeid and asi.attrid=109 and asi.isdeleted=false)

left join md_node_attr_val at on(at.nodeid = n.nodeid and at.attrid = 39/*md_get_attr('Alias')*/)
left join md_node_attr_val st on(st.nodeid = n.nodeid and st.attrid = 41/*md_get_attr('SelectList')*/)
left join md_node_attr_val st9 on(st9.nodeid = n.nodeid and st9.attrid = 115/*md_get_attr('SelectList')*/)
left join md_node_attr_val st1 on(st1.nodeid = n.nodeid and st1.attrid =40/* md_get_attr('Condition')*/)
left join md_src src on( src.srcid=torig.srcid)
where a.fromid = @id  and a.isdeleted=false";
                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string table = reader.GetString(1);
                            string alias = "";
                            if (!reader.IsDBNull(0))
                                alias = reader.GetString(0);
                            string selectList = "";
                            if (!reader.IsDBNull(2))
                                selectList = reader.GetString(2);
                            string condition = "";
                            if (!reader.IsDBNull(3))
                                condition = reader.GetString(3);
                            long table_id = reader.GetInt64(4);
                            int src_id=reader.GetInt32(5);

                            string url = "";
                            if (!reader.IsDBNull(6))
                                url = reader.GetString(6);

                            string sqlurl = "";
                            if (!reader.IsDBNull(7))
                                sqlurl = reader.GetString(7);

                            int interval=0;
                            if (!reader.IsDBNull(8))
                                interval = Convert.ToInt32(reader.GetString(8));
                            string src_name = "";
                            if (!reader.IsDBNull(9))
                                src_name = Convert.ToString(reader.GetString(9));
                            bool pci_dss_zone = false;
                            if (!reader.IsDBNull(10))
                                pci_dss_zone = reader.GetBoolean(10);
                            string outputTables = "";
                            if (!reader.IsDBNull(11))
                                outputTables = reader.GetString(11);

                            if (allTables.Count(ii => ii.Name == table && ii.Alias == alias) == 0)
                            {
                                var sel1= selectList.Split(',').Where(ii2 => ii2.Trim().Length > 0).ToArray();
                                var out1=outputTables.Split(',');
                                allTables.Add(new ItemTable() { src_name=src_name, pci_dss_zone=pci_dss_zone, url=url, sqlurl=sqlurl, IntervalUpdateInSec=interval, src_id=src_id, Name = table, Alias = alias, Condition = condition, SelectList =Enumerable.Range(0,sel1.Length).Select(ii => new ItemTable.SelectListItem(sel1[ii], ((out1.Length > ii) ? out1[ii]:"")) { fromOriginalSelect = true }).ToList(), TableId = table_id });
                            }
                            if (item != null)
                            {
                                if (item.Name1Table == "")
                                {
                                    item.Name1Table = table;
                                    item.Alias1Table = alias;
                                }
                                else
                                {
                                    item.Name2Table = table;
                                    item.Alias2Table = alias;

                                }
                            }
                        }
                    }
                }
            }

            foreach (var item1 in allTables)
            {
                item1.columns = await getColumns(item1.TableId, conn);
            }
        }

        private static async Task AddTask(ETL_Package package,NpgsqlConnection conn, CamundaProcess process, List<ItemRel> list, List<ItemTable> allTables, List<ItemTask> tasks, bool isFinishTask, string outputPath, int i,int dest_id,long package_id,List<ItemVar> variables,int keyCount)
        {
            ItemTask task = new ItemTask() { keyCount = keyCount };
            task.seq_id = i;
            if (isFinishTask)
                task.outputPath = outputPath.ToLower();
            else
                task.outputPath = $"tmp_table{i}_{package_id}";
            task.indexes.AddRange(allTables.Where(ii => ii.seq_id == i && ii.needed_indexes.Count > 0).Select(ii => ii.needed_indexes));
            task.source_id = allTables.First().src_id;
            if (isFinishTask && i > 1)
                task.source_id = 5; //Temp decision
            if (isFinishTask && i > 1)
                task.dest_id = dest_id; //Temp decision
                                        //            List<ItemTable.ColumnItem> selectList;
            List<RelItem> outCols;
            List<ItemTable.ColumnItem> columns = getColumnsForStep(list, allTables, i, variables, task,out outCols);
            //     ItemTable outputTable = new ItemTable() { Name= outputTable.columns.Add(new ItemTable.ColumnItem() { Name = item.alias, Lengtn = it.Lengtn, Type = it.Type });

            /*            var table = allTables.FirstOrDefault(ii => ii.Name == outputPath);
                        if( table == null)
                        {
                            table = new ItemTable() { Name = task.outputPath };
                            FillOutputTableStructure(table, table2, Columns);

                        }*/
           
            process.tasks.AddRange(await task.toExternalTask(package,conn, i, isFinishTask ? "Merge" : "Extract", columns, variables, allTables.Where(ii => ii.seq_id == i)));
            tasks.Add(task);
        }

        public  static List<ItemTable.ColumnItem> getColumnsForStep(List<ItemRel> list, List<ItemTable> allTables, int i, List<ItemVar> variables, ItemTask task, out List<RelItem> outCols)
        {
            List<ItemTable.ColumnItem> columns;
//            List<RelItem> outCols; 
            task.sqlExec = formSQLStatement(list.Where(ii => ii.seq_id == i), allTables.Where(ii => ii.seq_id == i), out columns, variables, task.dest_id,out outCols);
            task.outputTable = new ItemTable() { Name = task.outputPath, columns = columns, seq_id = i, SelectList = columns.Select(ii => new ItemTable.SelectListItem(ii.Name,"")).ToList() };
            return columns;
        }

        static void loadInfoFromTableToExternalTable(ItemTable outputTable,string Table,string Alias, List<ItemTable> allTables,string RelationColumns="")
        {
            var table2 = allTables.First(ii => ii.Name == Table && ii.Alias == Alias);
            string Columns = RelationColumns;
            FillOutputTableStructure(outputTable, table2, Columns);
            /*            var cols=Columns.Split(",");
                        foreach(var col in table2.SelectList)
                        {
                            if(outputTable.columns.Count(ii=>ii.Name==col.)==0)
                                outputTable.columns.Add(table2.columns.First(ii=>ii.Name== col));
                        }*/

        }

        private static void FillOutputTableStructure(ItemTable outputTable, ItemTable table2, string Columns)
        {
            foreach (var item in Columns.Split(","))
            {
                if (table2.columns.Count(ii => ii.Name == item) != 0)
                    outputTable.columns.Add(table2.columns.First(ii => ii.Name == item));
            }

            if (table2.SelectList.Count > 0)
            {
                foreach (var item in table2.SelectList.Where(i1 => i1.fromOriginalSelect))
                {
                    var it = table2.columns.FirstOrDefault(ii => ii.Name == item.alias);
                    if (it != null)
                    {
                        outputTable.columns.Add(new ItemTable.ColumnItem() { Name = item.alias, Lengtn = it.Lengtn, Type = it.Type });
                    }
                }

                outputTable.SelectList.AddRange(table2.SelectList.Where(i1 => i1.fromOriginalSelect).Select(ii => new ItemTable.SelectListItem(ii.alias,"")));// += ((outputTable.SelectList == "") ? "" : ",") + table2.SelectList;
                                                                                                                                                           //                Columns += ((Columns == "") ? "" : ",") + table2.SelectList.Se;
            }
        }

        private static ItemRel FillTableRel(int new_seq_id,int seq_id,List<ItemRel> list, List<ItemTable> allTables, List<ItemTask> tasks)
        {
            ItemRel rel = new ItemRel();
            var task1 = tasks.First(ii => ii.seq_id == seq_id);
            rel.srcId = 2;
            rel.Name1Table = task1.outputPath;
            ItemTable table1 = new ItemTable();
            table1.Name =task1.outputPath;
            table1.seq_id=new_seq_id;

            ItemTable table2 = new ItemTable();
            table2.seq_id = new_seq_id;

            bool found = false;
            foreach (var item in list.Where(ii => ii.seq_id == task1.seq_id))
            {
                string Table1="";
                string Alias1 = "";
                string NameColumns1 = "";
                string Table2 = "";
                string Alias2 = "";
                string NameColumns2 = "";

                if (item.is1Skip && !item.is2Skip)
                {
                    Table1 = item.Name2Table;
                    Alias1 = item.Alias2Table;
                    NameColumns1 = item.NameColumns2;
                    Table2 = item.Name1Table;
                    Alias2 = item.Alias1Table;
                    NameColumns2 = item.NameColumns1;
                }
                if (!item.is1Skip && item.is2Skip)
                {
                    Table1 = item.Name1Table;
                    Alias1 = item.Alias1Table;
                    NameColumns1 = item.NameColumns1;
                    Table2 = item.Name2Table;
                    Alias2 = item.Alias2Table;
                    NameColumns2 = item.NameColumns2;
                }
                if (Table1 != "")
                {
                    rel.NameColumns1 = NameColumns1;
                    rel.NameColumns2 = NameColumns2;
                    found = true;
                    loadInfoFromTableToExternalTable(table1, Table1, Alias1, allTables, NameColumns1);

                    loadInfoFromTableToExternalTable(table2, Table2, Alias2, allTables, NameColumns2);

                    var second_seq_id = list.First(ii => ii.seq_id != item.seq_id && ((ii.Name1Table == Table2 && ii.Alias1Table == Alias2) || (ii.Name2Table == Table2 && ii.Alias2Table == Alias2))).seq_id;
                    var task2 = tasks.First(ii => ii.seq_id == second_seq_id);
                    rel.Name2Table = task2.outputPath;
                    table2.Name = task2.outputPath;
            //        table2.seq_id = seq_id;
                    foreach (var item1 in list.Where(ii => ii.seq_id == second_seq_id))
                    {
                        if (!item1.is1Skip && !(item1.Name1Table == Table2 && item1.Alias1Table == Alias2))
                            loadInfoFromTableToExternalTable(table2, item1.Name1Table, item1.Alias1Table, allTables, "");
                        if (!item1.is2Skip && !(item1.Name2Table == Table2 && item1.Alias2Table == Alias2))
                            loadInfoFromTableToExternalTable(table2, item1.Name2Table, item1.Alias2Table, allTables, "");
                    }
                }

                //     var second = list.FirstOrDefault(ii => ii.seq_id <= countTask && (allTable.Count(i1 => i1.Name == ii.Name1Table && i1.Alias == ii.Alias1Table) > 0 || allTable.Count(i1 => i1.Name == ii.Name2Table && i1.Alias == ii.Alias2Table) > 0));
            }
            if (!found)
                return null;
            allTables.Add(table1);
            allTables.Add(table2);
            return rel;
        }

        private static void SearchPaths(List<ItemRel> list)
        {
            bool found = false;
            int seq_id = 0;

            do
            {
                var first = list.FirstOrDefault(ii => ii.seq_id < 0);
                if (first != null)
                {
                    seq_id++;
                    found = true;
                    first.seq_id = seq_id;
                    List<ItemTable> allTable = new List<ItemTable>();
                    first.ToTableList(allTable);
                    if (!first.isExternal)
                    {
                        ItemRel second = null;
                        do
                        {
                            second = list.FirstOrDefault(ii => ii.seq_id < 0 && ii.isExternal == false && (allTable.Count(i1 => i1.Name == ii.Name1Table && i1.Alias == ii.Alias1Table) > 0 || allTable.Count(i1 => i1.Name == ii.Name2Table && i1.Alias == ii.Alias2Table) > 0));
                            if (second != null)
                            {
                                second.ToTableList(allTable);
                                second.seq_id = seq_id;
                            }

                        } while (second != null);
                    }
                }
                else
                    found = false;

                foreach (var item in list)
                {
                    if (item.seq_id < 0)
                    {

                    }
                }
            } while (found);
        }

        public class ItemTask
        {
            public int seq_id;
            public int source_id = 1;
            public int dest_id = 5;
            public string outputPath;
            public List<List<ItemTable.ColumnItem>> indexes =  new List<List<ItemTable.ColumnItem>>();
            public string sqlExec;
            public ItemTable outputTable;
            public int keyCount = 1;
            string indexesDescription()
            {
                if (indexes.Count == 0)
                    return "";
                string retValue = $"[";
                bool first = true;
                foreach(var item in indexes)
                {
                    retValue += (first?"":",")+"\""+String.Join(',', item.Select(ii => ii.Name).ToArray())+ "\"";
                    first = false;
                }
                return retValue + "]";
            }
            string columnsDescription(List<ItemTable.ColumnItem> columns)
            {
                string retValue = "{";
                /*    if (indexes.Count == 0)
                        return "";
                    string retValue = "{";
                    foreach (var item in indexes)
                    {*/
                retValue +=  String.Join(',', columns.Select(ii =>$"\"{ ii.Name}\":\"{ii.Type}\"")) ;
               /* }*/
                return retValue + "}";
            }

            CamundaProcess.ExternalTask urlTask(string ConnectionString, string ConnectionAdm, ItemTable table)
            {
                CamundaProcess.ExternalTask retValue = new CamundaProcess.ExternalTask();
                retValue.id = $"Id{table.Name}";
                retValue.name = $"ETL_Task_{table.Name}";
                retValue.topic = "url_crowler";
                retValue.parameters.Clear();
                retValue.Annotation = $"Get data from {"external url"} to {table.Name} ";
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("ConnSelect", ConnectionString));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("ConnAdm", ConnectionAdm));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("URL", table.url));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQL",table.sqlurl));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Table", table.Name));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("UpdateTimeout", table.IntervalUpdateInSec.ToString()));
                return retValue;

            }
            public static async Task SaveDictionaryMeta(int srcid,DictionaryDefiner def, NpgsqlConnection conn)
            {
                var command = @"select* from md_merge_dictionary(@srcid,@body)";
                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    cmd.Parameters.AddWithValue("@srcid", srcid);
                    cmd.Parameters.AddWithValue("@body",NpgsqlTypes.NpgsqlDbType.Jsonb, JsonSerializer.Serialize<DictionaryDefiner>(def));
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                        }
                    }


                }
            }

            

            public class DictionaryDefiner
            {
                public class Field
                {
                    public string Name { get; set; }
                    public string Type { get; set; }
                    public string Detail { get; set; }
                    public string SensData { get; set; }
                    public int? synonym { get; set; }
                }

                public string Name { get; set; }
                public string Description { get; set; }
                public List<Field> Fields { get; set; }
                public string Key { get; set; }
            }
            public async Task<List<CamundaProcess.ExternalTask>> toExternalTask(ETL_Package package,NpgsqlConnection conn,int index,string description, List<ItemTable.ColumnItem> columnList,List<ItemVar> variables, IEnumerable<ItemTable> allTables)
            {
                List<CamundaProcess.ExternalTask> listValue = new List<CamundaProcess.ExternalTask>();
                var src=await GenerateStatement.getSrcInfo(source_id, conn);
                var dest = await GenerateStatement.getSrcInfo(dest_id, conn);
                foreach( var table in allTables.Where(ii=>!string.IsNullOrEmpty(ii.url)))
                {
                    listValue.Add(urlTask(src.connectionString, GenerateStatement.ConnectionStringAdm, table));
                }

                CamundaProcess.ExternalTask retValue = new CamundaProcess.ExternalTask();
                retValue.id = $"Id{index}";
                retValue.name = $"ETL_Task_{index}";
                retValue.topic = "LoginDB";
                retValue.parameters.Clear();
                retValue.Annotation = $"{description} data from {src.description} to {dest.description} ";
                retValue.author = "Alexey Rubtsov";
                retValue.service_location = "unknown";

                retValue.description = "Collecting data from one sources(SQL) and put to another destination( SQL too)";

                if (dest_id == 11)
                {
                    DictionaryDefiner def= new DictionaryDefiner();
                    def.Name = this.outputPath;
                    def.Key = string.Join(',', columnList.GetRange(0, this.keyCount));
                    def.Fields = new List<DictionaryDefiner.Field>();
                    foreach( var col in columnList)
                        def.Fields.Add( new DictionaryDefiner.Field() {  Name=col.Name, Type=col.Type, SensData=col.SensitiveData, synonym=col.synonym });
                    await SaveDictionaryMeta(dest_id, def,conn);
                    int posBase = src.dsn.LastIndexOf("/");
                    int posPort = src.dsn.LastIndexOf(":");
                    
                    
                    retValue.description = "Transfer data from one sources(SQL) and  to NoSQL(key/value ) destination";
                    //                    var admSet = await GenerateStatement.getSrcInfo(2, conn);


                    string adm = GenerateStatement.ConnectionStringAdm;// $"User ID={admSet.login};Password={admSet.password};Host={src.dsn.Substring(0,posPort)};Port={src.dsn.Substring(posPort+1,4)};Database={src.dsn.Substring(posBase+1)};";
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("DictName", this.outputPath, "Name of dictionary"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("ConnSelect", src.connectionString,"Connection string of source database"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("ConnAdm", adm, "Connection string of fraud processing database"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("DictAddr", dest.dsn, "Connection string of target kv database"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("MaxRecords","1000","Count of one moment count of transferred records"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLText", sqlExec,"SQL statement"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SensitiveData",String.Join(", " ,columnList.Select(ii=>ii.SensitiveData),"Names of columns with sensitive data (',' delimiter)")));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("CountInKey", keyCount.ToString(),"Count of first columns, which includes to kv key(by concatenation)"));
                    
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Variables", String.Join(", ", variables.Select(ii => ii.Name),"List of package variables(',' delimiter)")));

                    retValue.topic = "to_dict_sender";
                }
                else
                {



                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SNAME", src.name));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TNAME", dest.name));
/*                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SDSN", src.dsn));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SLogin", src.login));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SPassword", src.password));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SDriver", src.driver));

                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TDSN", dest.dsn));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TLogin", dest.login));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TPassword", dest.password));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TDriver", dest.driver));*/


                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLTable", JsonSerializer.Serialize<List<JsonItem>>( await getJsonDefs(conn,columnList,dest_id) /*this.outputPath*/)));
//                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLParams", ""));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLText", sqlExec));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Oper", "None"));
 //                   retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLIndexes", indexesDescription()));
//                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLColumns", columnsDescription(columnList)));
 //                   retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLParams", String.Join(", ",variables.Select(ii=>ii.Name))));
                }
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Signature", "569074234566666","Parameters signature( for control package integrity)"));
                listValue.Add(retValue);
                package.usedExternalTasks.Add(retValue);
                return listValue;
            }
            

            public class JsonItem
            {
                public class ExtID
                {
                    public string Column { get; set; }
                    public string Table { get; set; }
                }
                public string Table { get; set; }
                public class ColumnItem
                {
                    public int ind { get;set; }
                    public string Name { get; set; }
                    public string Type { get; set; }
                }

                public List<ColumnItem> Columns { get; set; }
                public List<ExtID> ExtIDs { get; set; }
            }
            async Task<List<JsonItem>> getJsonDefs(NpgsqlConnection conn, List<ItemTable.ColumnItem> columnList,int dest_id)
            {
                List<JsonItem> retValue = new List<JsonItem>();
                var items=await DBInterface.GetAllRelation(conn, outputPath.Split(','), dest_id);
                var allTables=items.Select(ii=>ii.MainTable).Union(items.Select(ii=>ii.SecondTable)).Distinct().ToList();
                var allTablesOutput = allTables.ToList();
                for(int i=0; i< allTables.Count;i++)
                {
                    var table = allTables[i];
                    foreach(var table1 in items.Where(ii=>ii.SecondTable==table).Select(i1=>i1.MainTable))
                    {
                        int index1=allTablesOutput.IndexOf(table);
                        int index2 = allTablesOutput.IndexOf(table1);
                        if(index1<index2)
                        {

                            allTablesOutput.Insert(index1, table1);
                            allTablesOutput.RemoveAt(index2+1);
                        }
                    }

                }
                foreach(var table in allTablesOutput)
                {
                    JsonItem newItem= new JsonItem();
                    newItem.Columns = columnList.Where(ii => ii.OutputTable == table).Select(ii =>new JsonItem.ColumnItem() { ind = columnList.IndexOf(ii) + 1 , Name=ii.Name, Type=ii.Type}).ToList();
                    newItem.ExtIDs = items.Where(ii => ii.SecondTable == table).Select(ii => new JsonItem.ExtID() { Table = ii.MainTable, Column = ii.ColumnSecond }).ToList();
                    newItem.Table = table;
                    retValue.Add(newItem);
                }
                return retValue;
            }

        }
         

        static void test()
        {
            var val = @"select power(99999999999, rownum) as double_  
     , rownum as int_
     , case rownum when 1 then 1.19 when 2 then 5 else 10 end as float_
     , pan
     , mbr
     , case rownum when 1 then null else 123 end as servicecode
     , lastupdated
     , t.blobvalue as tran_blobvalue
   from card c, tranadditionalfields t
where pan>:pan and orderedstatustime > to_date(:ost, 'DD.MM.YYYY Hh24:Mi:SS') and t.blobvalue is not null and seq = 4651216 and fieldid = 51";

           foreach(var match in Regex.Matches(val, ":\b*\b"))
            {

            }

            while(0==0)
            {
                val = " a1.Vasya";
                val = Regex.Replace(val, @"(?<!\.)" + "Vasya" + @"\b", "Aa.Vasya");

            }
        }
        public static string prepareSQLString(string val,ItemTable item)
        {


            var kvalPrefix = ItemRel.getKvalPrefix(item.Name, item.Alias);
            foreach (var col in item.columns)
            {
                val = Regex.Replace(val, @"(?<![\.:])\b" + col.Name + @"\b", kvalPrefix + col.Name);
//                val = Regex.Replace(val, @"\b^(.){0}" + col + @"\b", kvalPrefix + col);
//                val = val.Replace(col, kvalPrefix + col);
            }
            return val; 
        }


        public class SrcItem
        {
            public string driver;
            public string dsn;
            public string login;
            public string password;
            public string description;
            public string name;
            public string connectionString
            {
                get
                {
                    int posBase = dsn.LastIndexOf("/");
                    int posPort = dsn.LastIndexOf(":");

                    return $"User ID={login};Password={password};Host={dsn.Substring(0, posPort)};Port={dsn.Substring(posPort + 1, 4)};Database={dsn.Substring(posBase + 1)};";
                }
            }
        }
        public static async Task<SrcItem> getSrcInfo(int id, NpgsqlConnection conn)
        {
            SrcItem retValue = new SrcItem();
            var command = @"SELECT a.driver,b.dsn,b.login,b.pass,b.descr,a.descr,b.name FROM md_src_driver a,md_src b where a.driverid=b.driverid
and b.srcid=@id
  ";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        
                        retValue.driver=reader.GetString(0);
                        retValue.dsn = reader.GetString(1);
                        retValue.login = reader.GetString(2);
                        retValue.password = reader.GetString(3);
                        retValue.description = reader.GetString(4);
                        retValue.name = reader.GetString(6);
                    }
                }


            }
            return retValue;
        }

        public static async Task<List<ItemTable.ColumnItem>> getColumns(long id, NpgsqlConnection conn)
        {
            List<ItemTable.ColumnItem> retValue = new List<ItemTable.ColumnItem>();
            var command = @"select  n2.Name,a2.key,nav3.val,n2.synonym,sens.val
    from md_node n1, md_arc a1, 
md_node n2 
left join md_node_attr_val nav3 on(n2.NodeID=nav3.NodeID and nav3.AttrID=29 )
left join md_node_attr_val sens on(n2.NodeID=sens.NodeID and sens.AttrID=md_get_attr('Addon','SensData') )


, md_node_attr_val nav2,
    md_attr a2
    where n1.typeid=md_get_type('Table') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and n2.typeid=md_get_type('Column')
      and n2.NodeID=nav2.NodeID
      and nav2.AttrID=a2.AttrID
      and a2.attrPID=1
      and n1.nodeid=@id and n1.isdeleted=false and n2.isdeleted=false  and a1.isdeleted=false 
  ";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new ItemTable.ColumnItem();
                        item.Name=reader.GetString(0);
                        item.Type=reader.GetString(1);
                        item.Lengtn = 1;
                        if(!reader.IsDBNull(2))
                            item.Lengtn=Convert.ToInt32(reader.GetString(2));
                        if (!reader.IsDBNull(3))
                            item.synonym = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            item.SensitiveData = reader.GetString(4);

                        retValue.Add( item);
                    }
                }


            }
            return retValue;
        }
    }
}
