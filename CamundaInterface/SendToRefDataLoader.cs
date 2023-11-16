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
using UniElLib;
using System.IO;
using System.Xml.Linq;
using System.Globalization;

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
                public string Detail { get; set; } = "Exported from ETL package";

                public static string ConvertType(string type)
                {
                    int index1=type.IndexOf('(');
                    if(index1>=0)
                        type=type.Substring(0, index1);
                    switch (type)
                    {
                        case "timestamp":
                        case "timestamp with time zone":
                        case "date":
                            return "DateTime";
                        case "boolean":
                            return "Boolean";
                        case "character varying":
                            return "String";
                            break;
                        case "bigint":
                        case "integer":
                        case "numeric":
                        case "double precision":
                            return "Double";
                            break;
                        default:
                            throw new Exception($"inknown type {type}");
                    }
                }
            }

            public string Name { get; set; }
            public string Description { get; set; } = "Dictionary exported from ETL";
            public List<Field> Fields { get; set; } = new List<Field>();
            public string Key { get; set; } = "";
            public string Type { get; set; } = "DICTIONARY";
            bool ReadOnce { get; set; } = false;
            

        }
        public static string StringSha256Hash(string text) =>
       string.IsNullOrEmpty(text) ? string.Empty : BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().ComputeHash(System.Text.Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty);


        public class  ExportItem
        {
            public int all = 0;
            public int errors = 0;
        }



        static CryptoHash cryptoHash = new CryptoHash();
        static IEnumerable<int> getColIndex(string[] columns,NpgsqlDataReader reader)
        {
            foreach (var col in columns)
            {
                for(int i=0; i < reader.FieldCount;i++)
                    if(reader.GetName(i)== col)
                        yield return i;
            }
        }
//        static IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "" };
        static IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = ".", NumberGroupSeparator = "" };

        public static async Task<ExportItem> putRequestToRefDataLoader(HttpClient client, string processId = "asdfa", string connectionStringBase = "User ID=postgres;Password=test;Host=localhost;Port=5432;", string connectionStringAdmin = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;",
            string dictName = "People", string FID = "TEST", string command = "SELECT id  ID1,firstname,middlename,lastname,sex FROM public.aa_person", int maxRecord = 500, string baseAddr = "http://192.168.75.212:20226",string sensitiveDataArray="",int CountInKey=1,string columns="" )
        {

       /*     connectionStringAdmin = connectionStringAdmin.Replace("service.consul", "service.dc1.consul");
            connectionStringBase = connectionStringBase.Replace("service.consul", "service.dc1.consul");
            baseAddr = baseAddr.Replace("service.consul", "service.dc1.consul");
            sensitiveDataArray = ", , , , PAN";*/
            var columnList =columns.Split(',');
            if (columnList.Length == 0)
                return null;
            Log.Information($"Start  putRequestToRefDataLoader");

            NpgsqlConnection conn = null, connAdm = null;

            try
            {
                //            client.BaseAddress=new Uri(baseAddr);
                //            var client = new HttpClient() { BaseAddress = new Uri(baseAddr) };
                ExportItem retValue = new ExportItem();
                var separator = ";";

                List<AliasProducer> aliasProducers = new List<AliasProducer>();

                if (sensitiveDataArray != "")
                {
                    aliasProducers = sensitiveDataArray.Split(",").Select(ii => ((ii.Trim().Length > 0) ? (new AliasPan() as AliasProducer) : null)).ToList();
                }


            //            var client = new HttpClient();
            /*            var url = $"'/schema/dict/{FID}/{dictName}";
                        Uri uri =new  Uri(new Uri("http://192.168.75.212:20226"),url);
                        var st=await client.GetAsync(uri);
                        if(st.StatusCode== System.Net.HttpStatusCode.NotFound)
                        {

                        }*/
            ;

                conn = new NpgsqlConnection(connectionStringBase/*"User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;"*/);
                conn.Open();

                // Check Hash
                connAdm = conn;
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
                /*            if (hash != new_hash)
                                await SetHashToBase(processId, connectionStringBase, connectionStringAdmin, dictName, connAdm, new_hash);
                */
                //            command = @"SELECT id  ID1,firstname,middlename,lastname,sex FROM public.aa_person";
                //            int maxRecord = 500;
                int kolRecord = 0;
                using (StringWriter sw = new StringWriter())
//                using (StreamWriter sw = new StreamWriter(@"c:\d\ex.csv"))
                {
                    await using (var cmd = new NpgsqlCommand(command, conn))
                    {
                        //                cmd.Parameters.AddWithValue("@id", id);
                        await using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            string headerString = "";
                            Dictionary dict = new Dictionary() { Name = dictName };
                            int i1 = 0;
                            foreach(var i in getColIndex(columnList,reader))
//                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                               // int i=reader.
                                var type = reader.GetDataTypeName(i);
                                var name = reader.GetName(i);
                                if (i1 < CountInKey)
                                {
                                    dict.Key += (String.IsNullOrEmpty(dict.Key) ? "" : "+") + name;
                                    name = dict.Key;
                                }
                                else
                                    headerString += separator;
                                if (i1 >= CountInKey - 1)
                                {
                                    headerString += name;
                                    if (i1 == CountInKey - 1)
                                        type = "character varying";
                                    dict.Fields.Add(new Dictionary.Field() { Name = name, Type = Dictionary.Field.ConvertType(type) });
                                }
                                i1++;
                            }
                            sw.WriteLine(headerString);
                            if (hash != new_hash)
                            {
                                Log.Information($"Log schema new ");
                                //                                "https://referencedataloader.service.dc1.consul:16666/api/v0/schema/TEST/"
                                var url1 = $"api/v0/schema/{FID}/{dict.Name}";
                                Uri uri2 = new Uri(new Uri(baseAddr), url1);
                                try
                                {
                                    await client.DeleteAsync(uri2);
                                }
                                catch (Exception ex)
                                {

                                }
                                url1 = $"/api/v0/schema/dict/{FID}";
//                                "http://192.168.75.213:16666/api/v0/schema/dict/TEST"
                                Uri uri1 = new Uri(new Uri(baseAddr), url1);

                                string dict1 = JsonSerializer.Serialize<Dictionary>(dict);
                            /*                    HttpContent content = new StringContent(dict1);
                                                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");  //  "application/json";
                                                var res = await client.PostAsync(uri1, content);
                            */
                            //"https://referencedataloader.service.dc1.consul:16666/api/v0/schema/aaa/{catalog}?name=bbb"
                                var options = new JsonSerializerOptions();
                                options.PropertyNameCaseInsensitive = false;
                                Log.Information($"Send request on addr {uri1.ToString()} sended {dict1}");
                                var res = await client.PostAsJsonAsync<Dictionary>(uri1, dict, options);

                                if (!res.IsSuccessStatusCode)
                                {

                                    Log.Error($"Error on web request on addr {uri1.ToString()} sended {dict1} StatueCode {res.StatusCode}");
                                    var ans =await  res.Content.ReadAsStringAsync();
                                }

                                //res.EnsureSuccessStatusCode();

                                if (hash != new_hash)
                                    await SetHashToBase(processId, connectionStringBase, connectionStringAdmin, dictName, connAdm, new_hash);

                                if (!res.IsSuccessStatusCode)
                                {
                                    var ans1 = await res.Content.ReadAsStringAsync();
                                }
                                else
                                {
                                    //throw 
                                }
                            }
                            while (await reader.ReadAsync())
                            {
                                string bodyString = "";
                                int i2 = 0;
                                foreach (var i in getColIndex(columnList, reader))

//                                    for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var type = reader.GetDataTypeName(i);
                                    if (i2 >= CountInKey)
                                        bodyString += separator;
                                    /*                            if(Dictionary.Field.ConvertType(type) == "String")
                                                                    bodyString+=reader.GetString(i);
                                                                else
                                                                { 
                                                                    if(type=="bigint")*/
                                    i2++;
                                    string val;
                                    if(reader.GetFieldType(i) == typeof(double) )
                                        val = reader.GetDouble(i).ToString("F4",formatter);
                                    else
                                    if ( reader.GetFieldType(i) == typeof(decimal))
                                        val = reader.GetDecimal(i).ToString("F4", formatter);
                                    else
                                    if (reader.GetFieldType(i) == typeof(float))
                                        val = reader.GetFloat(i).ToString("F4", formatter);
                                    else
                                        val = reader.GetValue(i).ToString();
                                    if (type == "timestamp with time zone")
                                        val = reader.GetTimeStamp(i).ToDateTime().ToString("O");
                                    if (val=="840")
                                    {
                                        int y = 0;
                                    }
                                    if (aliasProducers.Count > i && aliasProducers[i] != null)
                                        bodyString += cryptoHash.hash(val);
                                    else
                                        bodyString += val;

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
                CloseConns(conn, connAdm);
                return retValue;
            } catch(Exception ex)
            {
                CloseConns(conn, connAdm);
                throw;
            }
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
/*            using (StreamWriter sw1 = new StreamWriter(@"C:\d\ex.csv"))
            {
                sw1.Write(sw.GetStringBuilder());
            }*/
           // if (kolRecord != 0)
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
                    //Console.WriteLine(i1);
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
                    sw.Position = 0;
//                    sw.Flush();
                    var sc = new StreamContent(sw,kol);
//                    sc.
                   
                    //                    var sc = new StreamContent(File.OpenRead(path));
                    sc.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                    //sc.Headers.ContentLength = kol;
                    multiPartFormContent.Add(sc, "file", "people.csv");

                    var response = await client.PostAsync($"{baseAddr}/api/v0/file/{FID}/{dictName}/reload?delimiter=;", multiPartFormContent);
                    //var response = await client.PostAsync($"{baseAddr}/api/v0/referencedata/{FID}/{dictName}/append?delimiter=;", multiPartFormContent);
                    try
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            var ans1 = await response.Content.ReadAsStringAsync();
                            Log.Error($"Error on web request on addr {baseAddr}/api/v0/referencedata/{FID}/{dictName}/reload?delimiter=; StatusCose:{response.StatusCode}");
                        }
                        response.EnsureSuccessStatusCode();
                        var ans = await response.Content.ReadAsStringAsync();
       //                 Console.WriteLine($"OK: {ans}");
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
                       // Console.WriteLine(e);
                        throw;
                    }
                    // if (response.IsSuccessStatusCode)
                    //    MessageBox.Show("Success");
                }
            }
        }
    }
}
