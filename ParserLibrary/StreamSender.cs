using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace ParserLibrary;

public class StreamSender:HTTPSender
{
    public static TimeSpan Interval; 
    public string streamName  = "checkRegistration5";
    public static long countOpenRexRequest = 0;

    public static Metrics.MetricCount metricStreamConcurrent = new Metrics.MetricCount("StreamConcurrCount", "Some time concurrent count");
    public static Metrics.MetricCount metricPerformanceStreams = new Metrics.MetricCount("StreamTime", "Stream peformance time");
    string getVal1(AbstrParser.UniEl el,Stream stream)
    {
        var conv = stream.fields[el.Name].SensitiveConverter;
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

        var stream = getStream(currentStream);
        string str = "{";
        str = "{" + String.Join(",", root.childs.Select(ii => $"\"{ii.Name}\":{getVal1(ii,stream)}")) + "}";
        return str;
    }

    public async override Task<string> sendInternal(AbstrParser.UniEl root)
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
        var ret= await base.sendInternal(root);
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
        public Dictionary<string,Field> fields { get; set; } = new Dictionary<string,Field>();
    }
    Dictionary<string, Stream> streams = new Dictionary<string, Stream>();  


    public string db_connection_string = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
    public override string getTemplate(string key)
    {
        return formJson(getStream(streamName));
    }

    public Stream getStream(string key)
    {
        Stream stream;
        if (streams.TryGetValue(key, out stream))
            return (stream);
        //                return JsonSerializer.Serialize<IEnumerable<string>>(stream.fields.Select(ii=>"\"ii.Name)); 
        stream = new Stream();
        stream.Name = "";
        var conn = new NpgsqlConnection(db_connection_string);
        conn.Open();
        using (var cmd = new NpgsqlCommand(@"select n.nodeid,n.name,a.val,np.name,'String',ap.val,asd.val,rl.toid from md_node n
inner join md_node_attr_val a  
on ( a.nodeid=n.nodeid and attrid=22)
inner join md_arc l on (l.toid=n.nodeid and l.isdeleted=false)
inner join md_node np on (l.fromid=np.nodeid and np.isdeleted=false)
inner join md_node_attr_val ap on ( ap.nodeid=np.nodeid and ap.attrid=22)
left join md_node_attr_val asd on ( asd.nodeid=np.nodeid and asd.attrid=51)
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
                    stream.fields.Add(reader.GetString(3), new Stream.Field() { Name = reader.GetString(3), Type = reader.GetString(4), Detail =reader.IsDBNull(5)?"": reader.GetString(5),SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6), linkedColumn = reader.IsDBNull(7) ? null : reader.GetInt64(7) });
                }
            }
        }

        conn.Close();
        if(stream.Name =="")
        {
            stream.Name = key;
            stream.fields.Add("stream",new Stream.Field() { Name = "stream", Detail = "Name of stream", Type = "String" });
            stream.fields.Add("originalTime",new Stream.Field() { Name = "originalTime", Detail = "Time of stream", Type = "DateTime" });
        }
        stream.fields.Where(ii => !string.IsNullOrEmpty(ii.Value.SensitiveData)).Select(ii1 => ii1.Value.SensitiveConverter);

        streams.TryAdd(key, stream);
        return (stream);
    }

    public override void Init(Pipeline owner)
    {
        getStream(streamName);
        base.Init(owner);
    }
    private static string formJson(Stream stream)
    {
        return "{" + string.Join<string>(",", stream.fields.Select(ii => $"\"{ii.Value.Name}\":\"{((ii.Value.Name=="stream")?stream.Name:"")}\"")) + "}";
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