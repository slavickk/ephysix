using Confluent.Kafka;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using MXGraphHelperLibrary;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;


namespace FrontInterfaceSupport
{
    public class StreamHelper
    {

        public class StreamDescr
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public class Item
            {
                public class LinkItem
                {
                    public string Stream { get; set; }
                    public string Field { get; set; }
                }
                public string Name { get; set; }
                public string Detail { get; set; }
                public string Type { get; set; }
                public string SensitiveData { get; set; }
                public bool Calculated { get; set; }
                public List<LinkItem> linkedColumn { get; set; }
            }
            public List<Item> fields { get; set; }  = new List<Item>();
        }
        public class StreamConfig
        {
            public string StreamName { get; set; }
        }

        public static string buildConnString(IConfiguration conf)
        {
            return conf.GetSection("MxGraphConnection")["ConnectionString"];
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Host = conf["DB_URL_FPDB"];
            builder.SearchPath = conf["DB_SCHEMA_FPDB"];
            builder.Username = conf["DB_USER_FPDB"];
            builder.Password = conf["DB_PASSWORD_FPDB"];
            builder.Database = conf["DB_NAME_FPDB"];
            return builder.ConnectionString;

        }

        public async static Task<string> getStreamBox(IConfiguration conf, string jsonMXGrapth, string streamDefJson)
        {
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();
            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            await createStreamBox(conf,retDoc, streamDefJson,null);
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };

            var st = JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);
            return st;
            //            return (type as _ApiExecutor).getDefine().Select(ii=>ii.Name).ToList();
            //            return null;
        }

        public static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> createStreamBox(IConfiguration conf, MXGraphDoc retDoc, string streamDefJson, MXGraphDoc.Box oldbox)
        {
            var ConnectionString = buildConnString(conf);
            StreamConfig dbTableConfig = JsonSerializer.Deserialize<StreamConfig>(streamDefJson);


            StreamDescr descr = await GetStreamDescription(conf, dbTableConfig.StreamName);

            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            //   retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<DBTableConfig>(dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();

            int mx = retDoc.boxes.Count(ii => ii.type == "stream") + 1;
            retBox.category = "receiving";

            retBox.header.caption = "Stream " + dbTableConfig.StreamName;
            retBox.header.description = "Stream " + dbTableConfig.StreamName;
            retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<StreamConfig>(dbTableConfig)).RootElement;
            if (oldbox == null)
            {
                retBox.id = "stream" + "_" + "_" + mx;
                if (retDoc.boxes.Count == 0)
                {
                    retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = 100, top = 100 };
                }
                else
                {
                    int delta = 15;
                    int left = retDoc.boxes.Max(ii => ii.header.position.left + ii.header.size.width) + delta;
                    int top = retDoc.boxes.Min(ii => ii.header.position.top);
                    retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = left, top = top };
                }

                retBox.header.size = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Size() { width = 300, height = 300 };
            } else
            {
                retBox.id = oldbox.id;
                retBox.header.position=oldbox.header.position;
            }
            //            retBox.id = mxGraphID;
            retBox.type = "stream";

            retDoc.boxes.Add(retBox);
            MXGraphDoc.Box.Body body = GetBody(descr);
            retBox.body = body;
            return retBox;
        }

        public static MXGraphDoc.Box.Body GetBody(StreamDescr descr)
        {
            MXGraphDoc.Box.Body body = new MXGraphDoc.Box.Body();
            body.header = new List<MXGraphDoc.Box.Header>() { new MXGraphDoc.Box.Header() { value = "Name" }, new MXGraphDoc.Box.Header() { value = "Type" } };
            //            var body = new MXGraphDoc.Box.Body();
            body.rows = new List<MXGraphDoc.Box.Body.Row>();
            foreach (var it in descr.fields)
            {
                var lastRow = new MXGraphDoc.Box.Body.Row() { tooltip_info = new Dictionary<string, string> { { "name", "Name" }, { "description", "Type" } } };
                lastRow.columns = new List<MXGraphDoc.Box.Body.Row.Column>();
                lastRow.columns.Add(new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { box_id = it.Name, caption = it.Name } });
                lastRow.columns.Add(new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = it.Type } });
                // lastRow.columns.Add(new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = "<div class=\"button-grey-bg-18 edit-icon\"></div>" } });
                body.rows.Add(lastRow);
            }
            {
                var lastRow = new MXGraphDoc.Box.Body.Row() { tooltip_info = new Dictionary<string, string> { { "name", "Name" }, { "description", "Type" } } };
                body.rows.Add(lastRow);
                lastRow.columns = new List<MXGraphDoc.Box.Body.Row.Column>();
                lastRow.columns.Add(new MXGraphDoc.Box.Body.Row.Column()
                {
                    item = FilterHelper.getRedrawItem("New Field", "new-item-connector", new List<int> { 3 },2) /*new MXGraphDoc.Box.Body.Row.Column.Item()
                    {
                        box_id = "new-item-connector",
                        caption = "New Field",
                        style = "position: relative;box-sizing: border-box;width: calc(100% - 20px);height: 30px;margin: 0px auto;border-radius: 8px;border: 1px dashed var(--grey-10);background: var(--grey-1);",
                        colspan = 2,
                        valid_link_type = new List<int> { 3 }
                    }*/
                });
                lastRow.columns.Add(new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { style = "display:none;" } });
            }

            return body;
        }

        public static async Task<List<StreamDescr>> GetAllStreams(IConfiguration conf, bool withFields=false)
        {
            var ConnectionString = buildConnString(conf);

            var retValue = new List<StreamDescr>();
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
     /*       NpgsqlConnection conn1 = new NpgsqlConnection(ConnectionString);
            await conn1.OpenAsync();*/
            using (var cmd = new NpgsqlCommand(@"select get_stream_json('')
", conn))
            {
                //cmd.Parameters.AddWithValue("@name", streamName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retValue.Add(JsonSerializer.Deserialize<StreamDescr>( reader.GetString(0)));
                /*        if (retValue.Count==0 || retValue.Last().Name!= Name)
                        {
                            retValue.Add(new StreamDescr() { Name = Name, Description = reader.GetString(2) });
//                            stream.Name = reader.GetString(1);
//                            stream.Description = reader.GetString(2);
                        }
                        if (withFields)
                        {
                            long linkedColumn = -1;
                            retValue.Last().fields.Add(new StreamDescr.Item() { Name = reader.GetString(3), Type = reader.GetString(4), Detail = reader.IsDBNull(5) ? "" : reader.GetString(5), SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6), Calculated = reader.IsDBNull(8) ? false : true });
                        }*/
                    }
                }
            }

            conn.Close();
            //conn1.Close();
            return retValue;
        }
        public class StreamConsul
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Field> fields { get; set; } = new List<Field>();
            public class Field
            {
                public string Name { get; set; }
                public string Type { get; set; }
                public string Detail { get; set; }
                public bool SensitiveData { get; set; }
            }

        }


        static Dictionary<string, string> replacedFields = new Dictionary<string, string>() { { "Number", "DOUBLE" },{"DateTime","DATETIME" }, { "Boolean", "BOOLEAN" }, { "String", "STRING" } };
        public static async Task saveStream (IConfiguration conf,StreamDescr stream)
        {
            string CONSUL_ADDR = conf["CONSUL_ADDR"];
            var ConnectionString = buildConnString(conf);

            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };
            var body=JsonSerializer.Serialize<StreamDescr>(stream);
            using (var cmd = new NpgsqlCommand(@"select * from md.md_add_stream_description(@json)", conn))
            {
                cmd.Parameters.AddWithValue("@json", NpgsqlTypes.NpgsqlDbType.Json, body);
                cmd.ExecuteNonQuery();
            }
            NpgsqlConnection connRule = new NpgsqlConnection("User ID=rules;Password=rav1234;Host=master.pgep01.service.dev-ep.consul;Port=5432;Database=ruledb;"); //SearchPath=md;
            connRule.Open();
            {
                var cmd1 = new NpgsqlCommand($"delete from streams_descripton where stream ='{stream.Name}'", connRule);
                cmd1.ExecuteNonQuery();
            }
            using (var cmd = new NpgsqlCommand(@"select * from streams_description where stream=@stream;", conn))
            {
                cmd.Parameters.AddWithValue("@stream", stream.Name);
                //  cmd.ExecuteNonQuery();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        //                            var insert= "insert into streams_descripton" +
                        List<string> insertList = new List<string>();
                        List<string> selectList = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            insertList.Add(reader.GetName(i));
                            if (reader.GetName(i) != "synonym")
                            {
                                if (reader.GetFieldType(i) == typeof(bool))
                                    selectList.Add(reader.GetBoolean(i).ToString());
                                else
                                {
                                    if (reader.IsDBNull(i))
                                        selectList.Add("''");
                                    else
                                    {
                                       // selectList.Add("'" + replacedFields[reader.GetString(i)] + "'");
                                        if (reader.GetName(i) == "datatype")
                                            selectList.Add("'" + replacedFields[reader.GetString(i)] + "'");
                                        else
                                            selectList.Add("'" + reader.GetString(i) + "'");

                                    }
                                }

                                //                                    Console.Write(reader.GetValue(i) + ";");
                            }
                            else
                            {
                                if (reader.IsDBNull(i))
                                    selectList.Add("null");
                                else
                                    selectList.Add($"{reader.GetInt32(i)}");
                            }
                        }
                        var cmd1 = new NpgsqlCommand($"insert into streams_descripton ({string.Join(",", insertList)}) values ({string.Join(", ", selectList)})", connRule);
                        cmd1.ExecuteNonQuery();
                        //                            var Name = reader.GetString(1);
                    }

                }

            }



            conn.Close();
            connRule.Close();
            HttpClient client = new HttpClient();

            StreamConsul st = new StreamConsul() {  Name=stream.Name,Description=stream.Description, fields=stream.fields.Select(ii=>new StreamConsul.Field() { Name=ii.Name, Detail=ii.Detail, SensitiveData=!string.IsNullOrEmpty(ii.SensitiveData), Type=ii.Type }).ToList() };
            JsonSerializerOptions options1 = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
              ,
                IgnoreNullValues = true
            };

            var def1 = JsonSerializer.Serialize<StreamConsul>(st, options1);
            var key = $"ROOT/STREAMS/Schemas/{stream.Name}";
            var res =await client.PutAsync(CONSUL_ADDR + "/v1/kv/" + key, new StringContent(def1));
            /*{
                var cmd1 = new NpgsqlCommand($"delete from streams_descripton where stream ='{stream.Name}'", connRule);
                cmd1.ExecuteNonQuery();
            }*/

        }
        public static async Task<StreamDescr> GetStreamDescription(IConfiguration conf, string streamName)
        {
            var ConnectionString = buildConnString(conf);

            var stream = new StreamDescr();
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            using (var cmd = new NpgsqlCommand(@"select get_stream_json(@name)
", conn))
            {
                cmd.Parameters.AddWithValue("@name", streamName);
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                       // if (stream.Name == "")
                        {
                            stream=JsonSerializer.Deserialize<StreamDescr>(reader.GetString(0));
                            /*stream.Name = reader.GetString(1);
                            stream.Description = reader.GetString(2);*/
                        }
                        //stream.fields.Add(new StreamDescr.Item() { Name = reader.GetString(3), Type = reader.GetString(4), Detail = reader.IsDBNull(5) ? "" : reader.GetString(5), SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6)/*, linkedColumn = reader.IsDBNull(7) ? null : reader.GetInt64(7)*/, Calculated = reader.IsDBNull(8) ? false : true });
                    }
                }
            }

            conn.Close();
            return stream;
        }
    }
}
