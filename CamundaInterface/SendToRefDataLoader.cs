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

namespace CamundaInterface
{
    public class SendToRefDataLoader
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Answer
        {
            public class Detail
            {
                public List<object> loc { get; set; }
                public string msg { get; set; }
                public string type { get; set; }
            }

            public List<Detail> detail { get; set; }
        }

        public class Dictionary
        {
            public class Field
            {
                public string Name { get; set; }
                public string Type { get; set; }
                public string Detail { get; set; } = "Especcially for Ilya!";

                public static string ConvertType(string type)
                {
                    switch (type)
                    {
                        case "character varying":
                            return "String";
                            break;
                        case "bigint":
                        case "integer":
                        case "numeric":
                            return "Double";
                            break;
                        default:
                            throw new Exception($"inknown type {type}");
                    }
                }
            }

            public string Name { get; set; }
            public string Description { get; set; } = "!!!";
            public List<Field> Fields { get; set; } = new List<Field>();
            public string Key { get; set; } = "";


        }
        public static string StringSha256Hash(string text) =>
       string.IsNullOrEmpty(text) ? string.Empty : BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().ComputeHash(System.Text.Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty);


        public class  ExportItem
        {
            public int all = 0;
            public int errors = 0;
        }



        static CryptoHash cryptoHash = new CryptoHash();

        public static async Task<ExportItem> putRequestToRefDataLoader(HttpClient client, string processId = "asdfa", string connectionStringBase = "User ID=postgres;Password=test;Host=localhost;Port=5432;", string connectionStringAdmin = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;",
            string dictName = "People", string FID = "TEST", string command = "SELECT id  ID1,firstname,middlename,lastname,sex FROM public.aa_person", int maxRecord = 500, string baseAddr = "http://192.168.75.212:20226",string sensitiveDataArray="",int CountInKey=1)
        {
            //            client.BaseAddress=new Uri(baseAddr);
            //            var client = new HttpClient() { BaseAddress = new Uri(baseAddr) };
            ExportItem retValue = new ExportItem();
            var separator = ",";

            List<ParserLibrary.AliasProducer> aliasProducers = new List<ParserLibrary.AliasProducer>();

            if(sensitiveDataArray!="")
            {
                aliasProducers=sensitiveDataArray.Split(",").Select(ii=> ((ii.Trim().Length > 0)?(new AliasPan() as AliasProducer):null)).ToList();
            }


            //            var client = new HttpClient();
            /*            var url = $"'/schema/dict/{FID}/{dictName}";
                        Uri uri =new  Uri(new Uri("http://192.168.75.212:20226"),url);
                        var st=await client.GetAsync(uri);
                        if(st.StatusCode== System.Net.HttpStatusCode.NotFound)
                        {

                        }*/
            ;

            var conn = new NpgsqlConnection(connectionStringBase/*"User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;"*/);
            conn.Open();

            // Check Hash
            var connAdm = conn;
            if (connectionStringBase != connectionStringAdmin)
            {
                connAdm = new NpgsqlConnection(connectionStringAdmin);
                connAdm.Open();
            }
            var hash = "";
            await using (var cmd1 = new NpgsqlCommand("select hash from MD_Camunda_Hash where key=@key", connAdm))
            {
                cmd1.Parameters.AddWithValue("@key", dictName + processId);
                await using (var reader = await cmd1.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        hash = reader.GetString(0);
                    }
                }
            }
            var new_hash = StringSha256Hash(command);
            if (hash != new_hash)
                await SetHashToBase(processId, connectionStringBase, connectionStringAdmin, dictName, connAdm, new_hash);

            //            command = @"SELECT id  ID1,firstname,middlename,lastname,sex FROM public.aa_person";
            //            int maxRecord = 500;
            int kolRecord = 0;
            using (StringWriter sw = new StringWriter())
            {
                await using (var cmd = new NpgsqlCommand(command, conn))
                {
                    //                cmd.Parameters.AddWithValue("@id", id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        string headerString = "";
                        Dictionary dict = new Dictionary() { Name = dictName };
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var type = reader.GetDataTypeName(i);
                            var name = reader.GetName(i);
                            if (i < CountInKey)
                            {
                                dict.Key = (String.IsNullOrEmpty(dict.Key) ? "" : "+") + name;
                                name= dict.Key;
                            }
                            else
                                headerString += separator;
                            if (i >= CountInKey - 1)
                            {
                                headerString += name;
                                dict.Fields.Add(new Dictionary.Field() { Name = name, Type = Dictionary.Field.ConvertType(type) });
                            }
                        }
                        sw.WriteLine(headerString);
                        if (hash != new_hash )
                        {
                            var url1 = $"/schema/dict/{FID}";
                            Uri uri1 = new Uri(new Uri(baseAddr), url1);

                            string dict1 = JsonSerializer.Serialize<Dictionary>(dict);
                            /*                    HttpContent content = new StringContent(dict1);
                                                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");  //  "application/json";
                                                var res = await client.PostAsync(uri1, content);
                            */

                            var options = new JsonSerializerOptions();
                            options.PropertyNameCaseInsensitive = false;
                            var res = await client.PostAsJsonAsync<Dictionary>(uri1, dict, options);


                            if (!res.IsSuccessStatusCode)
                            {
                                var ans1 = res.Content.ReadAsStringAsync();
                            }
                            else
                            {

                            }
                        }
                        while (await reader.ReadAsync())
                        {
                            string bodyString = "";
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var type = reader.GetDataTypeName(i);
                                if (i > CountInKey)
                                    bodyString += separator;
                                /*                            if(Dictionary.Field.ConvertType(type) == "String")
                                                                bodyString+=reader.GetString(i);
                                                            else
                                                            { 
                                                                if(type=="bigint")*/
                                if(aliasProducers.Count> i && aliasProducers[i] != null)
                                    bodyString += cryptoHash.hash(reader.GetValue(i).ToString());
                                else
                                    bodyString += reader.GetValue(i).ToString();

                            }
                            retValue.all++;
                            sw.WriteLine(bodyString);
                            if (++kolRecord >= maxRecord)
                            {
                                kolRecord = await SendToCCfa(client, dictName, FID, baseAddr, kolRecord, sw);

                            }
                        }
                    }


                }
                kolRecord = await SendToCCfa(client, dictName, FID, baseAddr, kolRecord, sw);

            }
            conn.Close();
            return retValue;
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

                    var response = await client.PostAsync($"{baseAddr}/referencedata/{FID}/{dictName}/append", multiPartFormContent);
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        Console.WriteLine($"OK: {await response.Content.ReadAsStringAsync()}");
                        Log.Information($"OK: {await response.Content.ReadAsStringAsync()}");
                    }
                    catch (HttpRequestException)
                    {
                        Log.Error($"{response.StatusCode}:{await response.Content.ReadAsStringAsync()}");
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
