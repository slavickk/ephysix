using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using MXGraphHelperLibrary;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace FrontInterfaceSupport
{
    public class DictionaryHelper
    {

        public class DictionaryDescr
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public enum DictType
            {
                /// <summary>
                ///     Dictionary
                /// </summary>
                DICTIONARY,

                /// <summary>
                ///     Range
                /// </summary>
                RANGE
            }

            /// <summary>
            ///     Dictionary name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            ///     Dictionary description
            /// </summary>
            public string Description { get; set; }
            public bool isCommon { get; set; } = true;

            /// <summary>
            ///     If <me
            /// </summary>
            public string Key { get; set; }

            [JsonPropertyName("Fields")] public List<DictionaryFields> fields { get; set; }

            /// <summary>
            ///     Describe type of item
            /// </summary>
            [JsonPropertyName("Type")]
            public DictType Type { get; set; }

            public bool ReadOnce { get; set; }


            public class DictionaryFields 
            {
                [JsonConverter(typeof(JsonStringEnumConverter))]
              //  [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumMemberConverter))]
                public enum DicitionaryAvailTypes
                {
                    String,
                    Double,
                    DateTime,
                    Bool,
                    Int,
                    Number
                }

                public string Name { get; set; }
                [JsonIgnore]
                public bool isExpireDate
                {
                    get
                    {
                        return Name == "@EXP";
                    }
                }
                public string Detail { get; set; }
                public DicitionaryAvailTypes Type { get; set; }

            }
        }
        public class DictionaryConfig
        {
            public string DictionaryName { get; set; }
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

        public async static Task<string> getDictionaryBox(IConfiguration conf, string jsonMXGrapth, string DictionaryDefJson)
        {
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();
            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            await createDictionaryBox(conf, retDoc, DictionaryDefJson, null);
            var options = new JsonSerializerOptions() { IgnoreNullValues = true };

            var st = JsonSerializer.Serialize<MXGraphDoc>(retDoc, options);
            return st;
            //            return (type as _ApiExecutor).getDefine().Select(ii=>ii.Name).ToList();
            //            return null;
        }

        public static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> createDictionaryBox(IConfiguration conf, MXGraphDoc retDoc, string DictionaryDefJson, MXGraphDoc.Box oldbox)
        {
            setConsulAddr(conf);

            var ConnectionString = buildConnString(conf);
            DictionaryConfig dbTableConfig = JsonSerializer.Deserialize<DictionaryConfig>(DictionaryDefJson);


            DictionaryDescr descr = await GetDictionaryDescription( dbTableConfig.DictionaryName);

            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            //   retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<DBTableConfig>(dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();

            int mx = retDoc.boxes.Count(ii => ii.type == "Dictionary") + 1;
            retBox.category = "receiving";

            retBox.header.caption = "Dictionary " + dbTableConfig.DictionaryName;
            retBox.header.description = "Dictionary " + dbTableConfig.DictionaryName;
            retBox.AppData = JsonDocument.Parse(JsonSerializer.Serialize<DictionaryConfig>(dbTableConfig)).RootElement;
            if (oldbox == null)
            {
                retBox.id = "Dictionary" + "_" + "_" + mx;
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
            }
            else
            {
                retBox.id = oldbox.id;
                retBox.header.position = oldbox.header.position;
            }
            //            retBox.id = mxGraphID;
            retBox.type = "dictionary";

            retDoc.boxes.Add(retBox);
            MXGraphDoc.Box.Body body = GetBody(descr);
            retBox.body = body;
            return retBox;
        }

        public static MXGraphDoc.Box.Body GetBody(DictionaryDescr descr)
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
                lastRow.columns.Add(new MXGraphDoc.Box.Body.Row.Column() { item = new MXGraphDoc.Box.Body.Row.Column.Item() { caption = Enum.GetName< DictionaryDescr.DictionaryFields.DicitionaryAvailTypes>(it.Type) } });
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
        public class KeyItem
        {
            public string key;
            public string value;
        }
        public class KeyValueAnswer
        {
            public int LockIndex { get; set; }
            public string Key { get; set; }

            public int Flags { get; set; }
            public string Value { get; set; }
            public int CreateIndex { get; set; }
            public int ModifyIndex { get; set; }
            public byte[] ContentInByteArray
            {
                get
                {
                    return ConvertToByte(Value);
                }
            }

            public static byte[] ConvertToByte(string Value)
            {
                var array = new byte[Value.Length];
                var arraySpan = new Span<byte>(array);
                int bytesWritten;
                bool try1 = Convert.TryFromBase64String(Value, array, out bytesWritten);
                return arraySpan.ToArray();
            }

            public string Content
            {
                get
                {
                    return ConvertToString(Value);
                }

            }

            public static string ConvertToString(string Value)
            {
                if (string.IsNullOrEmpty(Value))
                    return "";
                var array = new byte[Value.Length];
                var arraySpan = new Span<byte>(array);
                int bytesWritten;
                Convert.TryFromBase64String(Value, array, out bytesWritten);
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                //            Encoding.RegisterProvider(new EncodingProvider()
                return System.Text.Encoding.UTF8/*.GetEncoding(1251)*/.GetString(arraySpan.Slice(0, bytesWritten).ToArray());
            }

            //            "[{\"LockIndex\":0,\"Key\":\"SampleService_config\",\"Flags\":0,\"Value\":\"eyJzaW1wbGUiOjEyM30=\",\"CreateIndex\":36747,\"ModifyIndex\":36747}]"
        }

      /*  static byte[] ToB(string val)
        {
            return Encoding.UTF8.GetBytes(val);
         
        }*/

        public static IEnumerable<KeyItem> GetKVKeysAsStringArray(string Body)
        {
            KeyValueAnswer[] answer1 = JsonSerializer.Deserialize<KeyValueAnswer[]>(Body);
            return answer1.Where(i1 => i1.Value != null).Select(ii => new KeyItem() { key = ii.Key, value = ii.Content });
        }
        static byte[] PutObject(string Method, string postUrl, byte[] bytes, out string Body)
        {
            try
            {
                Body = "";
                var request = (HttpWebRequest)WebRequest.Create(postUrl);
                request.Method = Method;
                request.ContentType = "application/json";
                request.KeepAlive = false;
                //      request.Connection = "Close";// add for better performance
                if (bytes != null)
                {
                    //                request.Headers.Add("X-Consul-Namespace", "team-1");
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
                            response.Close();
                            stream.Close();
                            return outBuff.ToArray();
                            //           binWriter.Write(buffer1);
                        }
                    }
                }
                else
                {
                   // Log.Error("Web requestError return {StatusCode}  {Method} {Url} ", returnString, Method, postUrl);
                }
                response.Close();
                return new byte[] { };
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "Web requestError {Method} {Url}", Method, postUrl);
                //                new LogConsulWebError(Method, postUrl) { exeption=ex}.log();
                throw;
            }

        }
        static string CONSUL_ADDR = "http://10.74.32.21:8500/";
        public static IEnumerable<KeyItem> GetKeys(string key)
        {
            string Body;
            PutObject("Get", CONSUL_ADDR + "/v1/kv/" + key + "?recurse=true", null, out Body);
            return GetKVKeysAsStringArray(Body);

        }
        static void setConsulAddr(IConfiguration conf)
        {
            CONSUL_ADDR=conf["CONSUL_ADDR"];
        }

        public static async Task<List<DictionaryDescr>> GetAllDictionarys(IConfiguration conf, bool withFields = false)
        {
            setConsulAddr(conf);

            var retValue = new List<DictionaryDescr>();
            //dictionaries.Clear();
            var items = GetKeys("ROOT/DICTIONARIES/Schemas/TEST/");
            foreach (var item in items)
            {
                if (item != null)
                {
                    retValue.Add(JsonSerializer.Deserialize<DictionaryDescr>(item.value));
                }
            }


            return retValue;
        }

        public static async Task saveDictionary(IConfiguration conf, DictionaryDescr Dictionary)
        {
            setConsulAddr(conf);

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
              ,
                IgnoreNullValues = true
            };

            HttpClient client = new HttpClient();
            //string CONSUL_ADDR = args[2]/*"http://10.74.32.21:8500"*/, key = $"ROOT/STREAMS/Schemas/{stream.Name}";

            var def1 = JsonSerializer.Serialize<DictionaryDescr>(Dictionary, options);
            var res =await client.PutAsync(CONSUL_ADDR + "/v1/kv/" + $"ROOT/DICTIONARIES/Schemas/TEST/{Dictionary.Name}", new StringContent(def1));


        }
        public static string GetKVKeyAsString(string Body)
        {
            KeyValueAnswer[] answer1 = JsonSerializer.Deserialize<KeyValueAnswer[]>(Body);
            return answer1.First().Content;
        }

        public static async Task<DictionaryDescr> GetDictionaryDescription( string DictionaryName)
        {
            HttpClient client = new HttpClient();
            try
            {
//                var dd2=JsonSerializer.Deserialize<DictionaryDescr>("{\"Name\": \"EC_11_while_list\", \"Description\": \"\\u0411\\u0435\\u043b\\u044b\\u0439 \\u0441\\u043f\\u0438\\u0441\\u043e\\u043a \\u0422\\u0421\\u041f \\u0434\\u043b\\u044f \\u043f\\u0440\\u0430\\u0432\\u0438\\u043b\\u0430 EC-11\", \"isCommon\": true, \"Type\": \"DICTIONARY\", \"ReadOnce\": false, \"Fields\": [{\"Type\": \"String\", \"Detail\": \"\\u0422\\u0421\\u041f\", \"Name\": \"TSP\"}], \"Key\": \"TSP\"}");
                var dd1 = await client.GetAsync(CONSUL_ADDR + "/v1/kv/" + $"ROOT/DICTIONARIES/Schemas/TEST/{DictionaryName}");
                var dd3= GetKVKeyAsString(await dd1.Content.ReadAsStringAsync());
                return JsonSerializer.Deserialize<DictionaryDescr>(dd3); ;
            }
            catch(Exception ex)
            {
                throw;
            }


        }
    }
}
