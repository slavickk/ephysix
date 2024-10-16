/******************************************************************
 * File: StreamSender.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Npgsql;
using YamlDotNet.Core.Tokens;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using System.Globalization;
using PluginBase;
using UniElLib;
using System.IO;
using NUnit.Framework;
using Serilog;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ParserLibrary;

public class StreamSender:HTTPSender,ISelfTested
{
    public static TimeSpan Interval; 
    public string streamName  = "checkRegistration5";
    public static long countOpenRexRequest = 0;

    public static Metrics.MetricCount metricStreamConcurrent = new Metrics.MetricCount("StreamConcurrCount", "Some time concurrent count");
    public static Metrics.MetricCount metricPerformanceStreams = new Metrics.MetricCount("StreamTime", "Stream peformance time");
    public async Task<(bool, string, Exception)> isOK()
    {
        string details;
        bool isSuccess = true;
        Exception exc = null;
        details = "Make http request to " + this.urls[0];
        try
        {
            DateTime time1 = DateTime.Now;
            var ans = await internSend("{\"stream\":\"checkTest\",\"originalTime\":\"" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture) + "\"}");
            Logger.log(time1, "{Sender}-Send:{ans}", "SelfTest", Serilog.Events.LogEventLevel.Information, this, ans);
            if (ans == "")
                isSuccess = false;
        }
        catch (Exception e77)
        {
            isSuccess = false;
            exc = e77;
        }
        //            if(ans)
        return (isSuccess, details, exc);
    }
    string getVal1(AbstrParser.UniEl el,Stream stream)
    {
        if (stream == null)
            return getVal(el);
        var conv = stream.fieldsDict[el.Name].SensitiveConverter;
        if (conv != null)
            return getVal(conv.ConvertToNew(el));
        else
            return getVal(el);

    }
    protected override string formBody(AbstrParser.UniEl root)
    {
        string currentStream = streamName;
        //         getStream()
        var it = root.childs.FirstOrDefault(ii => ii.Name == "stream");
        if (it != null)
            currentStream = it.Value.ToString();
        //!!!!!!temporary 
       // var stream = getStream(currentStream);
        Stream stream = null;
        string str = "{";
        str = "{" + String.Join(",", root.childs.Select(ii => $"\"{ii.Name}\":{getVal1(ii,stream)}")) + "}";
        return str;
    }

    public async override Task<string> sendInternal(AbstrParser.UniEl root, ContextItem context   )
    {
        metricStreamConcurrent.Increment();
        Interlocked.Increment(ref countOpenRexRequest);
        string currentStream = streamName;
        //         getStream()
        var it=root.childs.FirstOrDefault(ii => ii.Name == "stream");
        if (it == null)
        {
            root.childs.Add(new AbstrParser.UniEl() { Name = "stream", Value = streamName });
        } else
            currentStream = it.Value.ToString();
           
        //   foreach(var item in stream.fields.Where(ii => ii.SensitiveData.IsNotEmpty()))

        DateTime time1=DateTime.Now;
        var ret= await base.sendInternal(root, context);
        Interval+=(DateTime.Now-time1); 
        Interlocked.Decrement(ref countOpenRexRequest);
        metricStreamConcurrent.Decrement();
        metricPerformanceStreams.Add(time1);
        return ret;
    }

    public class Stream
    {
        public class Field
        {
            public string Name { get; set; } = "";
            public string Type { get; set; }
            public string Detail { get; set; }

            public string SensitiveData { get; set; }
            HashOutput converter = null;
            public bool? Calculated { get; set; }
            [YamlIgnore]
            public HashOutput SensitiveConverter
            {
                get
                {
                    if(!String.IsNullOrEmpty(SensitiveData) && converter==null)
                    {
                        Type typeProducer= Assembly.GetAssembly(typeof(AliasProducer)).GetTypes().First(t => t.IsAssignableTo(typeof(AliasProducer)) && !t.IsAbstract && t.CustomAttributes.Count(ii => ii.AttributeType == typeof(SensitiveAttribute) && ii.ConstructorArguments[0].Value.ToString()== SensitiveData) >0);
                        converter = new HashOutput() { hashConverter = new CryptoHash(), aliasProducer = Activator.CreateInstance(typeProducer) as AliasProducer };
                    }
                    return converter;
                }
            }
                
            //converter = new HashOutput() { hashConverter = new CryptoHash(), aliasProducer = Activator.CreateInstance(comboBoxTypeAlias.SelectedItem as Type) as AliasProducer };

            public long? linkedColumn { get; set; }
        }
        public string Name { get; set; }
        public string Description { get; set; }

        public Dictionary<string,Field> fieldsDict { get; set; } = new Dictionary<string,Field>();
        public Field[] fields
        {
            get
            {
                return fieldsDict.Values.ToArray(); 
            }
            set
            {
                fieldsDict = value.ToDictionary(p => p.Name);
            }
        }
    }
    Dictionary<string, Stream> streams;// = new Dictionary<string, Stream>();  


//    public string db_connection_string = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.consul;Port=5432;Database=fpdb;SearchPath=md;";
    public string db_connection_string = "User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;";

    public static List<Stream> GetAllStreams()
    {
        List<Stream> streams= new List<Stream>();
        try
        {
            var stream = new Stream();
            stream.Name = "";
            Npgsql.NpgsqlConnectionStringBuilder csb = new NpgsqlConnectionStringBuilder();
            csb.Host = Environment.GetEnvironmentVariable("DB_URL_FPDB");
            csb.Port = Int32.Parse(Environment.GetEnvironmentVariable("DB_PORT_FPDB"));
            csb.Database = Environment.GetEnvironmentVariable("DB_NAME_FPDB");
            csb.SearchPath = Environment.GetEnvironmentVariable("DB_SCHEMA_FPDB");
            csb.Username = Environment.GetEnvironmentVariable("DB_USER_FPDB");
            csb.Password = Environment.GetEnvironmentVariable("DB_PASSWORD_FPDB");
            
            

            //var conn = new NpgsqlConnection("User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;"/*db_connection_string*/);
            Npgsql.NpgsqlConnection conn = new NpgsqlConnection(csb.ToString());
            conn.Open();
            string oldName = "";
            using (var cmd = new NpgsqlCommand(@"select n.nodeid,n.name,a.val,np.name,'String',ap.val,asd.val,rl.toid,ascalc.val from md_node n
inner join md_node_attr_val a  
on ( a.nodeid=n.nodeid and attrid=22)
inner join md_arc l on (l.toid=n.nodeid and l.isdeleted=false)
inner join md_node np on (l.fromid=np.nodeid and np.isdeleted=false)
inner join md_node_attr_val ap on ( ap.nodeid=np.nodeid and ap.attrid=22)
left join md_node_attr_val asd on ( asd.nodeid=np.nodeid and asd.attrid=51)
left join md_node_attr_val ascalc on ( ascalc.nodeid=np.nodeid and ascalc.attrid=100)
left join md_arc rl on ( rl.fromid=np.nodeid and rl.typeid=16)
where n.typeid=md_get_type('Stream') /*and n.name =@name*/ and n.isdeleted=false
order by n.name
", conn))
            {
               // cmd.Parameters.AddWithValue("@name", key);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       // if (stream.Name == "")
                        //{
                            var Name = reader.GetString(1);
                            stream.Description = reader.GetString(2);
                        //}
                        if(oldName != Name)
                        {
                              
                            stream = new Stream();
                            stream.Name = Name;
                            streams.Add(stream);
                            oldName = Name;
                        }
                        if (reader.GetString(3) == "mbr")
                        {

                        }
                        stream.fieldsDict.Add(reader.GetString(3), new Stream.Field() { Name = reader.GetString(3), Type = reader.GetString(4), Detail = reader.IsDBNull(5) ? "" : reader.GetString(5), SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6), linkedColumn = reader.IsDBNull(7) ? null : reader.GetInt64(7), Calculated = reader.IsDBNull(8) ? false : true });
                    }
                }
            }

            conn.Close();
        }
        catch (Exception ex)
        {
            
            Logger.log($"DB open failed", ex);
    /*        stream = this.streamSender;
            streams.TryAdd(key, stream);
            return (stream);*/
        }
       /* if (stream.Name == "")
        {
            stream.Name = key;
            stream.fieldsDict.Add("stream", new Stream.Field() { Name = "stream", Detail = "Name of stream", Type = "String" });
            stream.fieldsDict.Add("originalTime", new Stream.Field() { Name = "originalTime", Detail = "Time of stream", Type = "DateTime" });
        }*
        stream.fieldsDict.Where(ii => !string.IsNullOrEmpty(ii.Value.SensitiveData)).Select(ii1 => ii1.Value.SensitiveConverter);
        streamSender = stream;
        streams.TryAdd(key, stream);
       */
       return streams;
    }

    public override string getTemplate(string key)
    {
  /*      if (string.IsNullOrEmpty(key))
            return "{}";*/
        return formJson(getStream(streamName));
    }

    public Stream streamSender;
    bool first = true;
    public Stream getStream(string key)
    {
        // TODO: figure out the best approach to mocking here
        if (MocMode)
        {
            Logger.log("getStream() is called in mock mode; exiting early to prevent database access (TODO: figure out a better approach)", Serilog.Events.LogEventLevel.Warning);
            return null;
        }
        
        if(streams== null)
            streams=FromLocalStorage().ToDictionary(str => str.Name, str => str);
        Stream stream;
        if (streams.TryGetValue(key, out stream))
            return (stream);

        //                return JsonSerializer.Serialize<IEnumerable<string>>(stream.fields.Select(ii=>"\"ii.Name)); 
        try
        {
            stream = new Stream();
            stream.Name = "";
            var conn = new NpgsqlConnection(db_connection_string);
            conn.Open();
            using (var cmd = new NpgsqlCommand(@"select n.nodeid,n.name,a.val,np.name,'String',ap.val,asd.val,rl.toid,ascalc.val from md_node n
inner join md_node_attr_val a  
on ( a.nodeid=n.nodeid and attrid=22)
inner join md_arc l on (l.toid=n.nodeid and l.isdeleted=false)
inner join md_node np on (l.fromid=np.nodeid and np.isdeleted=false)
inner join md_node_attr_val ap on ( ap.nodeid=np.nodeid and ap.attrid=22)
left join md_node_attr_val asd on ( asd.nodeid=np.nodeid and asd.attrid=51)
left join md_node_attr_val ascalc on ( ascalc.nodeid=np.nodeid and ascalc.attrid=100)
left join md_arc rl on ( rl.fromid=np.nodeid and rl.typeid=16)
where n.typeid=md_get_type('Stream') and n.name =@name and n.isdeleted=false
", conn))
            {
                cmd.Parameters.AddWithValue("@name", key);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (stream.Name == "")
                        {
                            stream.Name = reader.GetString(1);
                            stream.Description = reader.GetString(2);
                        }
                        if (reader.GetString(3) == "mbr")
                        {

                        }
                        stream.fieldsDict.Add(reader.GetString(3), new Stream.Field() { Name = reader.GetString(3), Type = reader.GetString(4), Detail = reader.IsDBNull(5) ? "" : reader.GetString(5), SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6), linkedColumn = reader.IsDBNull(7) ? null : reader.GetInt64(7), Calculated = reader.IsDBNull(8) ? false : true });
                    }
                }
            }

            conn.Close();
        }
        catch(Exception ex)
        {
            Logger.log($"DB open failed",ex);
            stream = this.streamSender;
            streams.TryAdd(key, stream);
            return (stream);
        }
        if (stream.Name =="")
        {
            stream.Name = key;
            stream.fieldsDict.Add("stream",new Stream.Field() { Name = "stream", Detail = "Name of stream", Type = "String" });
            stream.fieldsDict.Add("originalTime",new Stream.Field() { Name = "originalTime", Detail = "Time of stream", Type = "DateTime" });
        }
        stream.fieldsDict.Where(ii => !string.IsNullOrEmpty(ii.Value.SensitiveData)).Select(ii1 => ii1.Value.SensitiveConverter);
        streamSender = stream;
        streams.TryAdd(key, stream);
        return (stream);
    }

    public override void Init(Pipeline owner)
    {
        getStream(streamName);
        base.Init(owner);
    }

 
    public class StreamConsul
    {
        public class Field
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Detail { get; set; }
            public bool Calculated { get; set; }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Field> fields { get; set; }
    }


    private static readonly JsonSerializerOptions jsonSerializerOptions
      = new JsonSerializerOptions
      {
          PropertyNameCaseInsensitive = true,
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };
    private static async Task SendPutAsync(HttpClient httpClient,string addr,StreamConsul post)
    {
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(post, jsonSerializerOptions);
        using var httpContent = new StringContent
            (jsonContent, Encoding.UTF8, "text/json");
        
        using var response = await httpClient.PutAsync(addr, httpContent);

        response.EnsureSuccessStatusCode();
    }

    static public void ToLocalStorage(Stream stream)
    {
        var stor = FromLocalStorage();
        if (stor.Count(ii => ii.Name == stream.Name) == 0)
            stor.Add(stream);
        else
        {
            stor.RemoveAll(ii => ii.Name == stream.Name);
            stor.Add(stream);
        }
        using (StreamWriter sw= new StreamWriter(GetStoragePath()))
        {
            sw.Write(System.Text.Json.JsonSerializer.Serialize<List<Stream>>(stor));

        }
    }
//    static string  storagePath = @"Data\LocalStor.json";
    static List<Stream> FromLocalStorage()
    {
//        "curl -X POST http://localhost:8080 -d"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\"><SOAP-ENV:Header/><SOAP-ENV:Body><Tran><Request InitiatorRid=\"RcScheme Test\" Kind=\"TdsProcessAuth\" LifePhase=\"Single\" NetworkRid=\"VISA\" OrigTime=\"2022-10-11T02:02:01\" ProcessorInstRid=\"ACS\"><Parties><Cust Presence=\"true\"><Token Kind=\"Card\"><Card ExpDate=\"2025-12-01T00:00:00\" Pan=\"2220000000000200\"><Tds NetworkType=\"MIR\"/></Card></Token></Cust><Term AcquirerRid=\"2201380101\"><Caps Interactive=\"true\"/><Owner Country=\"643\" Rid=\"compass_mrc_1\" Title=\"compass_mrc_1\" Url=\"http://compassplus.ru\"/></Term></Parties><Link Id=\"5802262\" Kind=\"PrevStep2NextStep\"/><Match Key=\"9D313DB97181F159505725BF06A1928421E9794CEC10731E48C71F84AF0890E8b1a03daa-352f-4909-87c6-a33868162c19\" Nrn=\"b1a03daa-352f-4909-87c6-a33868162c19\" Rrn=\"bbc44d41-71ee-4eff-b0f0-49e2308be233\"/><Moneys><Cust Amt=\"25.0\" Ccy=\"840\"/></Moneys><Specific><Tds Version=\"2.2.0\"><Extensions><Extension criticalityIndicator=\"false\" id=\"ID1\" name=\"extensionField1\">{\"valueOne\":\"value\"}</Extension></Extensions></Tds></Specific></Request></Tran></SOAP-ENV:Body></SOAP-ENV:Envelope>"
       // GetStoragePath();
        if (File.Exists(GetStoragePath()))
            using (StreamReader sr = new StreamReader(GetStoragePath()))
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<Stream>>(sr.ReadToEnd());
            }

        Log.Information("Local storage file not found: {path}", GetStoragePath());
        return new List<Stream>();
    }

    private static string GetStoragePath()
    {
        //        Path.GetFullPath(@"..\..\");
        if (Environment.GetEnvironmentVariable("YAML_PATH") == null)
            return "aa.nl";
        return Path.Combine(Path.GetDirectoryName(Environment.GetEnvironmentVariable("YAML_PATH")), "LocalStor.json");
    }

    static public void ToConsul(Stream stream)
    {
        ToLocalStorage(stream);
        HttpClient httpClient = new HttpClient();
        StreamConsul consul = new StreamConsul() { Name = stream.Name, Description = stream.Description, fields = stream.fields.Select(ii => new StreamConsul.Field() { Name = ii.Name, Detail = ii.Detail, Type = ii.Type,Calculated=(bool)((ii.Calculated==null)?false:ii.Calculated) }).ToList() };
        string CONSUL_ADDR = Environment.GetEnvironmentVariable("CONSUL_ADDR");// "http://192.168.75.204:8500";
        var res =httpClient.PutAsJsonAsync<StreamConsul>($"{CONSUL_ADDR}/v1/kv/ROOT/STREAMS/Schemas/{stream.Name}", consul,new JsonSerializerOptions()).ContinueWith(ii =>
        {
            if (ii.IsCompletedSuccessfully)
            { }
        });

        /*
        if (stream.fields.Count(ii => ii.Calculated == true) == 0)
            httpClient.DeleteAsync($"{CONSUL_ADDR}/v1/kv/ROOT/STREAMS/Schemas/Calculated/{stream.Name}").ContinueWith(ii =>
            {
                if (ii.IsCompletedSuccessfully)
                { }
            });
        else
        {
            httpClient.PutAsJsonAsync<string[]>($"{CONSUL_ADDR}/v1/kv/ROOT/STREAMS/Schemas/Calculated/{stream.Name}", stream.fields.Where(ii=>ii.Calculated==true).Select(i1=>i1.Name).ToArray(), new JsonSerializerOptions()).ContinueWith(ii =>
            {
                if (ii.IsCompletedSuccessfully)
                { }
            });
        }
        */
    }
    private static string formJson(Stream stream)
    {
        return "{" + string.Join<string>(",", stream.fieldsDict.Select(ii => $"\"{ii.Value.Name}\":\"{((ii.Value.Name=="stream")?stream.Name:"")}\"")) + "}";
    }

    public override void setTemplate(string key, string body)
    {
//            setStream(body);

        base.setTemplate(key, body);
    }

    public void setStream(string body)
    {
        var conn = new NpgsqlConnection(db_connection_string);
        conn.Open();
        using (var cmd = new NpgsqlCommand(@"select * from md.md_add_stream_description(@json)",conn))
        {
            cmd.Parameters.AddWithValue("@json", NpgsqlTypes.NpgsqlDbType.Json, body);
            cmd.ExecuteNonQuery();
        }
    }
}