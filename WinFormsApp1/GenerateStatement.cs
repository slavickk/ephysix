using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CamundaInterfaces;
using Npgsql;


namespace WinFormsApp1
{
    public class GenerateStatement
    {
       static HttpClient client;

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




        public async static Task SendToCamunda(string path,string processId,List<ItemVar> variables)
        {
//                    const string camundaPath = @"http://localhost:8080/engine-rest/";
            const string camundaPath = @"http://192.168.75.217:18080/engine-rest/";
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
                                MessageBox.Show($"Task  {processId} {((res.isSucess)?"finished successfully":"failed")} with message: {res.Description}");

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
            public int seq_id;
            public string Name = "";
            public string Alias="";
            public string Condition="";
            public class SelectListItem
            {
                public string expression = "";
                public string alias = "";
                public bool fromOriginalSelect = false;
                public SelectListItem(string Expression)
                {
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
                        if (expression.Contains(col.Name) && !retValue.Contains(col))
                            retValue.Add(col);
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
                public string SensitiveData = "";
            }
            public List<ColumnItem> columns= new List<ColumnItem>();

        }
        public static  string formSQLStatement(IEnumerable<ItemRel> items,IEnumerable<ItemTable> itemTables,out List<ItemTable.ColumnItem> columns,List<ItemVar> variables ,int dest_id)
        {
            bool onlyOneTable = (items.Count() == 0 && itemTables.Count() == 1);
//                test();
            //    columnList = new List<ItemTable.ColumnItem>();
            var columns1 = new List<ItemTable.ColumnItem>();
            string selectList = "";
            string whereCondition = "";
            foreach (var item in itemTables)
            {
                if (onlyOneTable || items.Count(ii => (!ii.is1Skip && ii.Name1Table == item.Name && ii.Alias1Table == item.Alias) || (!ii.is2Skip && ii.Name2Table == item.Name && ii.Alias2Table == item.Alias)) > 0)
                {
                    if (item.SelectList.Count >0 )
                    {

                        columns1.AddRange(item.SelectList.Where(ii=>columns1.Count(ii1=>ii1.Name==ii.expression)==0 && ii.alias != "" && ii.expression != "").Select(ii =>  new ItemTable.ColumnItem() {  Name = ii.alias, Type = ii.getAllColumnsFromExpression(item).First().Type, Lengtn = ii.getAllColumnsFromExpression(item).First().Lengtn }));
                        var str= item.getSelectList().Trim();
                        if(str.Length>0)
                        selectList +=((selectList=="")?"":",")+ str;//  prepareSQLString(item.SelectList, item);

                    }
                    if (item.Condition != "")
                    {
                        if (whereCondition != "")
                            whereCondition += " AND ";
                        whereCondition += "(" + prepareSQLString(item.Condition, item) + ")";

                    }
                }
            }
            columns = columns1;
            string returnValue = $"select {selectList} from ";





            List<ItemTable> list=new List<ItemTable>();
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
                    if (!item.isExternal)
                    {
                        if (!second)
                            returnValue += $" inner join  {item.Name1Table} {item.Alias1Table} on ({item.getOnClause()})  \r\n";
                        else
                            returnValue += $" inner join   {item.Name2Table} {item.Alias2Table} on ({item.getOnClause()})  \r\n";
                    }
                    item.ToTableList(list);
                }
            }
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

        public static async Task Generate(NpgsqlConnection conn, long id)
        {
            List<ItemVar> variables = new List<ItemVar>();
            CamundaProcess process = new CamundaProcess();

            List<ItemRel> list = new List<ItemRel>();
            //            List<ItemRel> listExternal = new List<ItemRel>();
            string NamePacket = "";
            string outputTable = "output_Table";
            string description = "";
            int dest_id = 2;
            int keyCount = 1;
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
                            NamePacket = reader.GetString(0);
                            if (!reader.IsDBNull(1))
                                outputTable = reader.GetString(1);
                            if (!reader.IsDBNull(2))
                                description = reader.GetString(2);
                            if (!reader.IsDBNull(3))
                                dest_id =Convert.ToInt32( reader.GetString(3));
                            if (!reader.IsDBNull(4))
                                keyCount = Convert.ToInt32(reader.GetString(4));
                        }
                    }
                }


            }
            //Variables

            {
                var command = @"select a.name,at1.val,at2.val from md_node a 
inner join md_arc a1 on (a.nodeid=a1.toid and a1.fromid=@id  and a1.isdeleted=false)
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
                            variables.Add(new ItemVar() { Name = reader.GetString(0),  Type=reader.GetString(1), DefaultValue=reader.GetString(2)});

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
                            list.Add(rel);
                        }
                    }
                }
            }

            List<ItemTable> allTables = new List<ItemTable>();
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
                            list.Add(rel);
                        }
                    }
                }
            }
            if(list.Count == 0)
            {
                await FillTableInfo(conn, allTables, null, id);

            }
            foreach (var item in list)
            {
                //string a1 = @"";
                await FillTableInfo(conn, allTables, item, item.IdEtlRelation);

                {
                    string command = @"select t.name1table,t.columns1table,t.name2table,t.columns2table from ccfa_fk_info_as_table(@idFK) t";
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
                                    var t1 = allTables.First(ii => ii.Name == table1);
                                    t1.needed_indexes = columns1.Split(',').Select(ii => t1.columns.First(i1 => i1.Name == ii.Trim())).ToList();//
                                    t1.SelectList.AddRange(columns1.Split(',').Select(ii => new ItemTable.SelectListItem(ii)));
                                    var t2 = allTables.First(ii => ii.Name == table2);
                                    t2.needed_indexes = columns2.Split(',').Select(ii => t1.columns.First(i1 => i1.Name == ii.Trim())).ToList();//
                                    t2.SelectList.AddRange(columns2.Split(',').Select(ii => new ItemTable.SelectListItem(ii)));
                                }
                            }
                        }
                    }

                }


            }
            SearchPaths(list);

            foreach (var item in list)
            {
                bool skipFirst = false;
                bool skipSecond = false;
                if (item.isExternal)
                {
                    if (list.Count(ii => ii.seq_id != item.seq_id && ((ii.Name1Table == item.Name1Table && ii.Alias1Table == item.Alias1Table) || ((ii.Name2Table == item.Name1Table && ii.Alias2Table == item.Alias1Table)))) == 0)
                    {
                        skipSecond = true;
                        //                        item.Name2Table = "";
                    }
                    if (list.Count(ii => ii.seq_id != item.seq_id && ((ii.Name1Table == item.Name2Table && ii.Alias1Table == item.Alias2Table) || ((ii.Name2Table == item.Name2Table && ii.Alias2Table == item.Alias2Table)))) == 0)
                    {
                        skipFirst = true;
                    }
                    if (skipFirst)
                        item.is1Skip = true;
                    if (skipSecond)
                        item.is2Skip = true;
                }

                if (item.Name1Table != "")
                    allTables.First(ii => ii.Name == item.Name1Table && ii.Alias == item.Alias1Table).seq_id = item.seq_id;
                if (item.Name2Table != "")
                    allTables.First(ii => ii.Name == item.Name2Table && ii.Alias == item.Alias2Table).seq_id = item.seq_id;

            }

            //if()

/*            foreach (var item in allTables)
            {
                item.columns = await getColumns(item.TableId, conn);
            }
*/
            List<ItemTask> tasks = new List<ItemTask>();
            process.ProcessID = $"ETL_Process{id}";
            process.ProcessName = $"{NamePacket}{id}";
            process.documentation = $"{description}\r\n  Not contain input variables!";
            //            process.save($"c:\\Camunda\\{NamePacket}.bpmn");
            process.tasks.Clear();
            if(list.Count== 0)
            {

                if (allTables.Count == 0)
                {
                    MessageBox.Show($"The package {id} is empty");
                    return;
                } 
            }

            int countTask = 0;
            if(list.Count>0)
            countTask = list.Max(ii => ii.seq_id);

            //            string outputPath = "outputTable";
            bool isExternalDest = countTask == 1 && list[0].srcId != 2 && dest_id != 2;
            for (int i = 1; i <= countTask; i++)
            {
                await AddTask(conn, process, list, allTables, tasks, (countTask==1 && !isExternalDest),outputTable, i,dest_id,id,variables,keyCount);
            }
            if(countTask == 0)
            {
                var s_id = 1;
                allTables.First().seq_id = s_id;//1;??? 1;
                await AddTask(conn, process, list, allTables, tasks, false, outputTable, s_id, dest_id, id,variables,keyCount);
                countTask = 1;
                isExternalDest = countTask == 1  && dest_id != 2;
            }
            /* if (isExternalDest)
                 countTask++;
            */

            if (countTask > 1 )
            {
                int seq_id = countTask;

                for (int i=0; i < countTask;i++)
                {
                    var task = tasks[i];
                    ItemRel rel = FillTableRel(seq_id+1,task.seq_id,list, allTables, tasks);
                    if(rel != null)
                    {
                        seq_id++;
                        rel.seq_id = seq_id;
//                        rel.isExternal = true; //No!!!!
                        list.Add(rel);

                        await AddTask(conn, process, list, allTables, tasks, true, outputTable, rel.seq_id,dest_id,id, variables, keyCount);

                    }

                }
            }
            if(isExternalDest)
            {
                tasks[0].seq_id = 1;
                tasks[0].outputTable.seq_id = 2;
                await  AddTask(conn, process, new List<ItemRel>() { }, new List<ItemTable>() { tasks[0].outputTable }, tasks, true, outputTable, 2, dest_id, id,variables,keyCount);

            }

            var path1 = $"c:\\Camunda\\{NamePacket}.bpmn";
            process.save(path1);
            
//            await SendToCamunda(@"C:\Camunda\Temp6.bpmn", "ETL_Process532730");

            await SendToCamunda(path1, process.ProcessID,variables);

        }

        private static async Task FillTableInfo(NpgsqlConnection conn, List<ItemTable> allTables, ItemRel item,long id)
        {
            {
                string command = @"select at.val,n.name,st.val,st1.val,a1.toid from md_arc a
inner join md_node n on(n.nodeid = a.toid and n.typeid = md_get_type('ETLTable') and n.isdeleted=false)
inner join md_arc a1 on (n.nodeid=a1.fromid  and a1.isdeleted=false)
left join md_node_attr_val at on(at.nodeid = n.nodeid and at.attrid = 39/*md_get_attr('Alias')*/)
left join md_node_attr_val st on(st.nodeid = n.nodeid and st.attrid = 41/*md_get_attr('SelectList')*/)
left join md_node_attr_val st1 on(st1.nodeid = n.nodeid and st1.attrid =40/* md_get_attr('Condition')*/)
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

                            if (allTables.Count(ii => ii.Name == table && ii.Alias == alias) == 0)
                            {
                                allTables.Add(new ItemTable() { Name = table, Alias = alias, Condition = condition, SelectList = selectList.Split(',').Where(ii2 => ii2.Trim().Length > 0).Select(ii => new ItemTable.SelectListItem(ii) { fromOriginalSelect = true }).ToList(), TableId = table_id });
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

        private static async Task AddTask(NpgsqlConnection conn, CamundaProcess process, List<ItemRel> list, List<ItemTable> allTables, List<ItemTask> tasks, bool isFinishTask, string outputPath, int i,int dest_id,long package_id,List<ItemVar> variables,int keyCount)
        {
            ItemTask task = new ItemTask() {  keyCount=keyCount};
            task.seq_id = i;
            if (isFinishTask)
                task.outputPath = outputPath.ToLower();
            else
                task.outputPath = $"tmp_table{i}_{package_id}";
            task.indexes.AddRange(allTables.Where(ii => ii.seq_id == i && ii.needed_indexes.Count > 0).Select(ii => ii.needed_indexes));
            if (isFinishTask && i >1)
                task.source_id = 2; //Temp decision
            if (isFinishTask && i > 1)
                task.dest_id = dest_id; //Temp decision
//            List<ItemTable.ColumnItem> selectList;
            List<ItemTable.ColumnItem> columns;
            task.sqlExec = formSQLStatement(list.Where(ii => ii.seq_id == i), allTables.Where(ii => ii.seq_id == i),out columns,variables,task.dest_id);
            task.outputTable = new ItemTable() {  Name=task.outputPath, columns = columns, seq_id=i, SelectList=  columns.Select(ii=>new ItemTable.SelectListItem(ii.Name)).ToList()};
       //     ItemTable outputTable = new ItemTable() { Name= outputTable.columns.Add(new ItemTable.ColumnItem() { Name = item.alias, Lengtn = it.Lengtn, Type = it.Type });

/*            var table = allTables.FirstOrDefault(ii => ii.Name == outputPath);
            if( table == null)
            {
                table = new ItemTable() { Name = task.outputPath };
                FillOutputTableStructure(table, table2, Columns);

            }*/

            process.tasks.Add(await task.toExternalTask(conn, i,isFinishTask?"Merge":"Extract",columns,variables ));
            tasks.Add(task);
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

                outputTable.SelectList.AddRange(table2.SelectList.Where(i1 => i1.fromOriginalSelect).Select(ii => new ItemTable.SelectListItem(ii.alias)));// += ((outputTable.SelectList == "") ? "" : ",") + table2.SelectList;
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
            public int dest_id = 2;
            public string outputPath;
            public List<List<ItemTable.ColumnItem>> indexes =  new List<List<ItemTable.ColumnItem>>();
            public string sqlExec;
            public ItemTable outputTable;
            public int keyCount = 1;
            string indexesDescription()
            {
                if (indexes.Count == 0)
                    return "";
                string retValue = "[";
                foreach(var item in indexes)
                {
                    retValue += "\""+String.Join(',', item.Select(ii => ii.Name).ToArray())+ "\"";
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
            public async Task<CamundaProcess.ExternalTask> toExternalTask(NpgsqlConnection conn,int index,string description, List<ItemTable.ColumnItem> columnList,List<ItemVar> variables)
            {
                var src=await GenerateStatement.getSrcInfo(source_id, conn);
                var dest = await GenerateStatement.getSrcInfo(dest_id, conn);
                CamundaProcess.ExternalTask retValue = new CamundaProcess.ExternalTask();
                retValue.id = $"Id{index}";
                retValue.name = $"ETL_Task_{index}";
                retValue.topic = "LoginDB";
                retValue.parameters.Clear();
                retValue.Annotation = $"{description} data from {src.description} to {dest.description} ";

                if (dest_id == 11)
                {
                    int posBase = src.dsn.LastIndexOf("/");
                    int posPort = src.dsn.LastIndexOf(":");
                    string adm = $"User ID={src.login};Password={src.password};Host={src.dsn.Substring(0,posPort)};Port={src.dsn.Substring(posPort+1,4)};Database={src.dsn.Substring(posBase+1)};";
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("DictName", this.outputPath));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("ConnSelect", adm));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("ConnAdm", adm));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("DictAddr", dest.dsn));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("MaxRecords","1000"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLText", sqlExec));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SensitiveData",String.Join(", " ,columnList.Select(ii=>ii.SensitiveData))));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("CountInKey", keyCount.ToString()));
                    
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Variables", String.Join(", ", variables.Select(ii => ii.Name))));

                    retValue.topic = "to_dict_sender";
                }
                else
                {

                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SDSN", src.dsn));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SLogin", src.login));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SPassword", src.password));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SDriver", src.driver));

                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TDSN", dest.dsn));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TLogin", dest.login));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TPassword", dest.password));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("TDriver", dest.driver));


                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLTable", this.outputPath));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLParams", ""));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLText", sqlExec));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Oper", "Refill"));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLIndexes", indexesDescription()));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLColumns", columnsDescription(columnList)));
                    retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLParams", String.Join(", ",variables.Select(ii=>ii.Name))));
                }
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Signature", "569074234566666"));
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
                val = Regex.Replace(val, @"(?<!\.)\b" + col.Name + @"\b", kvalPrefix + col.Name);
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

        }
        public static async Task<SrcItem> getSrcInfo(int id, NpgsqlConnection conn)
        {
            SrcItem retValue = new SrcItem();
            var command = @"SELECT a.driver,b.dsn,b.login,b.pass,a.descr FROM md_src_driver a,md_src b where a.driverid=b.driverid
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
                    }
                }


            }
            return retValue;
        }

        public static async Task<List<ItemTable.ColumnItem>> getColumns(long id, NpgsqlConnection conn)
        {
            List<ItemTable.ColumnItem> retValue = new List<ItemTable.ColumnItem>();
            var command = @"select  n2.Name,a2.key,nav3.val
    from md_node n1, md_arc a1, md_node n2 left join md_node_attr_val nav3 on(n2.NodeID=nav3.NodeID and nav3.AttrID=29 ) , md_node_attr_val nav2,
    md_attr a2
    where n1.typeid=md_get_type('Table') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and n2.typeid=md_get_type('Column')
      and n2.NodeID=nav2.NodeID
      and nav2.AttrID=a2.AttrID
      and a2.attrid>=5 and a2.attrid<=17 
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
                        retValue.Add( item);
                    }
                }


            }
            return retValue;
        }
    }
}
