using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ParserLibrary;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CamundaInterface
{
    public class ExecProcExecutor
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
/*        examplefunction(
    timeExample date, IDEvent integer, comment1 text)*/
        public class ProcCalls
        {
            public string procName { get; set; }
            public ItemParams[] param_proc { get; set; }


        }
        public class ItemParams
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public object Value { get; set; }
        }
        static List<NpgsqlTypes.NpgsqlDbType> datatypes = new List<NpgsqlTypes.NpgsqlDbType> { NpgsqlTypes.NpgsqlDbType.Date, NpgsqlTypes.NpgsqlDbType.Time, NpgsqlTypes.NpgsqlDbType.Timestamp, NpgsqlTypes.NpgsqlDbType.TimestampTZ, NpgsqlTypes.NpgsqlDbType.TimestampTz, NpgsqlTypes.NpgsqlDbType.TimeTz, NpgsqlTypes.NpgsqlDbType.TimeTZ };
        public static async Task<SendToRefDataLoader.ExportItem> execRequestProcedure(HttpClient client, string processId = "asdfa", string connectionStringAdmin = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;",
            string procJob = "{\"procName\":\"examplefunction\",\"param_proc\":[{\"Name\":\"timeExample\",\"Type\":\"Date\", \"Value\":\"2023-03-11T00:00:00Z\"},{\"Name\":\"IDEvent\",\"Value\":5,\"Type\":\"Integer\"},{\"Name\":\"comment1\",\"Type\":\"Text\",\"Value\":\"all right\"}]}")
        {
            var pars = JsonSerializer.Deserialize<CamundaInterface.ExecProcExecutor.ProcCalls>(procJob);
            var conn = new NpgsqlConnection(connectionStringAdmin);
            await conn.OpenAsync();
            await using (var cmd1 = new NpgsqlCommand(pars.procName, conn))
            {
                cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (var par1 in pars.param_proc)
                {
                    NpgsqlTypes.NpgsqlDbType type;
                    if (!Enum.TryParse<NpgsqlTypes.NpgsqlDbType>(par1.Type, out type))
                        throw new Exception("Can't convertt to type " + par1.Type);

                    if (datatypes.Contains(type))
                        cmd1.Parameters.AddWithValue(par1.Name.ToLower(), type, DateTime.Parse(par1.Value.ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind));
                    else
                    {
                        cmd1.Parameters.AddWithValue(par1.Name.ToLower(), type, ((JsonElement)par1.Value).ValueKind switch
                        {
                            JsonValueKind.String => par1.Value.ToString(),
                            JsonValueKind.Number => Convert.ToInt64(par1.Value.ToString()),
                            _ => par1.Value.ToString()
                        });
                    }
                }
              //  cmd1.ExecuteScalar();
                await using (var reader = await cmd1.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
//                        hash = reader.GetString(0);
                    }
                }
            }


            await conn.CloseAsync();

                 return new SendToRefDataLoader.ExportItem() { all = 1, errors = 0 };
        }

        private static void CloseConns(NpgsqlConnection? conn, NpgsqlConnection connAdm)
        {
            if (conn != null)
                conn.Close();
            if (connAdm != conn && connAdm != null)
                connAdm.Close();
        }

        private static async Task SetHashToBase(string processId, string connectionStringBase, string connectionStringAdmin, string dictName, NpgsqlConnection connAdm,string new_hash)
        {
//            if (hash != new_hash)
            {
                await using (var cmd1 = new NpgsqlCommand("delete from MD_Camunda_Hash where key=@key", connAdm))
                {
                    cmd1.Parameters.AddWithValue("@key", dictName + processId);
                    cmd1.ExecuteNonQuery();
                }
                await using (var cmd1 = new NpgsqlCommand("insert into  MD_Camunda_Hash (hash,key)  values(@hash,@key)", connAdm))
                {
                    cmd1.Parameters.AddWithValue("@hash", new_hash);
                    cmd1.Parameters.AddWithValue("@key", dictName + processId);
                    cmd1.ExecuteNonQuery();
                }
            }
            if (connectionStringBase != connectionStringAdmin)
            {
                connAdm.Close();
            }
        }

        private static async Task<int> SendToCCfa(HttpClient client, string dictName, string FID, string baseAddr, int kolRecord, StringWriter sw)
        {
            using (StreamWriter sw1 = new StreamWriter(@"C:\d\ex.csv"))
            {
                sw1.Write(sw.GetStringBuilder());
            }
            if (kolRecord != 0)
            {
                Log.Information("Send record to" + kolRecord);
                var str = sw.GetStringBuilder().ToString();
                var arr = StringToByteArray(str);
                await SendDictToCcfa(baseAddr, dictName, FID, client, arr, arr.Length);
                sw.Flush();
                kolRecord = 0;
            }
            return kolRecord;
        }

        private static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }
        public static int i1 = 0;
        public static async Task UploadFileAsync(string dictName = "People", string FID = "TEST", int maxByteLen = 2000)
        {
            string path = @"C:\Camunda\200_people.csv";
            var client = new HttpClient() { BaseAddress = new Uri("http://192.168.75.212:30281") };
            byte[] buffer = new byte[maxByteLen];
            int offsetFile = 0;
            int offsetArray = 0;
            using (FileStream fs = File.OpenRead(path))
            {

                int kol;
                while ((kol = fs.Read(buffer, offsetArray, maxByteLen - offsetArray)) > 0 || offsetArray > 0)
                {
                    if (i1 >= 10)
                    {
                        int yy = 0;
                    }
                    i1++;
                    Console.WriteLine(i1);
                    int i;
                    for (i = kol + offsetArray - 1; i >= 0; i--)
                        if (buffer[i] == 10)
                            break;
                    if (i > 0)
                        await SendDictToCcfa("", dictName, FID, client, buffer, i + 1);

                    for (int j = i + 1; j < kol + offsetArray; j++)
                        buffer[j - i - 1] = buffer[j];
                    offsetArray = kol + offsetArray - i - 1;
                    if (offsetArray < 0)
                    {
                        int yy = 0;
                    }
                }


            }

            /*            HttpClient client = new HttpClient();
                        // we need to send a request with multipart/form-data
                        var multiForm = new MultipartFormDataContent();

                        string path = @"C:\Camunda\200_people.csv";

                        // add file and directly upload it
                        FileStream fs = File.OpenRead(path);
                        var streamContent = new StreamContent(fs);
                        streamContent.Headers.Add("Content-Type", "text/csv");
                        streamContent.Headers.Add("Content-Disposition", $"form-data; name=\"aa.csv\"; filename=\"aa.csv\"");

                        multiForm.Add(streamContent, "file", Path.GetFileName(path));


                        var request = new HttpRequestMessage(HttpMethod.Post, @"http://192.168.75.212:30281/referencedata/" + FID + "/" + dictName + @"/append?delimiter=%2C")
                        {
                            Content = multiForm,
                        };

                        request.Headers.Add("accept", "application/json");

                        var response = await client.SendAsync(request);

                        //            multiForm.Headers["Accept"]= "application/json";
                        // send request to API
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Success");
                        }
                        else
                        {
                            var respBody=await response.Content.ReadAsStringAsync();
                            MessageBox.Show(response.ToString());
                        }
            */
        }

        private static async Task SendDictToCcfa(string baseAddr, string dictName, string FID, HttpClient client, byte[] buffer, int kol)
        {
            using (MemoryStream sw = new MemoryStream())
            {
                //                sw.Flush();
                sw.Write(buffer, 0, kol);
                //   File.OpenRead(path).CopyTo(sw);
                using (var multiPartFormContent = new MultipartFormDataContent())
                {
                    var sc = new StreamContent(sw);
                    //                    var sc = new StreamContent(File.OpenRead(path));
                    sc.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                    multiPartFormContent.Add(sc, "file", "people.csv");

                    var response = await client.PostAsync($"{baseAddr}/api/v0/referencedata/{FID}/{dictName}/append?delimeter=;", multiPartFormContent);
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        var ans = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"OK: {ans}");
                        Log.Information($"OK: {ans}");
                    }
                    catch (HttpRequestException)
                    {
                        Log.Error($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
                        throw;
                        //                        Console.WriteLine($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        Console.WriteLine(e);
                        throw;
                    }
                    // if (response.IsSuccessStatusCode)
                    //    MessageBox.Show("Success");
                }
            }
        }
    }
}
