using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                public string Name { get; set; }
                public string Description { get; set; }
                public string Type { get; set; }
                public string SensitiveData { get; set; }
                public bool Calculated { get; set; }
            }
            public List<Item> fields { get; set; }  = new List<Item>();
        }
        public static async Task<List<StreamDescr>> GetAllStreams(string ConnectionString, bool withFields=false)
        {
            var retValue= new List<StreamDescr>();
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            using (var cmd = new NpgsqlCommand(@"select n.nodeid,n.name,a.val,np.name,'String',ap.val,asd.val,rl.toid,ascalc.val from md_node n
inner join md_node_attr_val a  
on ( a.nodeid=n.nodeid and attrid=22)
inner join md_arc l on (l.toid=n.nodeid and l.isdeleted=false)
inner join md_node np on (l.fromid=np.nodeid and np.isdeleted=false)
inner join md_node_attr_val ap on ( ap.nodeid=np.nodeid and ap.attrid=22)
left join md_node_attr_val asd on ( asd.nodeid=np.nodeid and asd.attrid=51)
left join md_node_attr_val ascalc on ( ascalc.nodeid=np.nodeid and ascalc.attrid=100)
left join md_arc rl on ( rl.fromid=np.nodeid and rl.typeid=16)
where n.typeid=md_get_type('Stream')  and n.isdeleted=false
order by n.nodeid
", conn))
            {
                //cmd.Parameters.AddWithValue("@name", streamName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Name = reader.GetString(1);
                        if (retValue.Count==0 || retValue.Last().Name!= Name)
                        {
                            retValue.Add(new StreamDescr() { Name = Name, Description = reader.GetString(2) });
//                            stream.Name = reader.GetString(1);
//                            stream.Description = reader.GetString(2);
                        }
                        if(withFields)
                            retValue.Last().fields.Add(new StreamDescr.Item() { Name = reader.GetString(3), Type = reader.GetString(4), Description = reader.IsDBNull(5) ? "" : reader.GetString(5), SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6)/*, linkedColumn = reader.IsDBNull(7) ? null : reader.GetInt64(7)*/, Calculated = reader.IsDBNull(8) ? false : true });
                    }
                }
            }

            conn.Close();
            return retValue;
        }
        public static async Task<StreamDescr> GetStreamDescription(string ConnectionString, string streamName)
        {
            var stream = new StreamDescr();
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
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
                cmd.Parameters.AddWithValue("@name", streamName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       // if (stream.Name == "")
                        {
                            stream.Name = reader.GetString(1);
                            stream.Description = reader.GetString(2);
                        }
                        stream.fields.Add(new StreamDescr.Item() { Name = reader.GetString(3), Type = reader.GetString(4), Description = reader.IsDBNull(5) ? "" : reader.GetString(5), SensitiveData = reader.IsDBNull(6) ? null : reader.GetString(6)/*, linkedColumn = reader.IsDBNull(7) ? null : reader.GetInt64(7)*/, Calculated = reader.IsDBNull(8) ? false : true });
                    }
                }
            }

            conn.Close();
            return stream;
        }
    }
}
