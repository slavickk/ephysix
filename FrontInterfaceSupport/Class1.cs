using Npgsql;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using System.Text.Json;

namespace FrontInterfaceSupport
{
    public class DBTable
    {
        public class AppDataTable
        {
            public int tableId { get; set; }
            public int tableExistedId { get; set; }
            public List<string> conditions { get; set; }
            public List<string[]> relation { get; set; }
            public int depth { get; set; }
        }

        public class ItemTable
        {
            public string table_name { get; set; }
            public long table_id { get; set; }
            public string src_name { get; set; }
        }
        static async Task<List<ItemTable>> GetTablesForTablePattern(NpgsqlConnection conn, string findString, int[] excludeSrc = null)
        {
            List<ItemTable> list = new List<ItemTable>();
            await using (var cmd = new NpgsqlCommand(@"select nt.name tablename,nt.nodeid tableid,s.name from 
md_Node nt 
inner join md_src s on (nt.srcid=s.srcid)
where  nt.typeid = 1 and nt.isdeleted=false and nt.name like '%" + findString + "%' " + ((excludeSrc != null) ? string.Join("", excludeSrc.Select(ii => $" AND nt.srcid!={ii}")) : ""), conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ItemTable() { table_name = reader.GetString(0), table_id = reader.GetInt64(1), src_name = reader.GetString(2) });
                }
            }

            return list;
        }

        public static async Task<List<ItemTable>> FindTable(string ConnectionString,string pattern, int[] excludeSrc=null)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            var ret =await  GetTablesForTablePattern(conn, pattern, excludeSrc);
            conn.Close();
            return ret;
        }

        public static async Task<List<List<string[]>>> FindLinkBetween2Tables(string ConnectionString,long fromId,long toId,int findDepth)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            List<List<string[]>> allPaths= new List<List<string[]>>();
            var command = @"select pathlen
,case when(md_nodeinfo(p.n2) like 'Column%') then '' else COALESCE(CAST(p.n2 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n3) like 'Column%') then '' else COALESCE(CAST(p.n3 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n4) like 'Column%') then '' else COALESCE(CAST(p.n4 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n5) like 'Column%') then '' else COALESCE(CAST(p.n5 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n6) like 'Column%') then '' else COALESCE(CAST(p.n6 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n7) like 'Column%') then '' else COALESCE(CAST(p.n7 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n8) like 'Column%') then '' else COALESCE(CAST(p.n8 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n9) like 'Column%') then '' else COALESCE(CAST(p.n9 as varchar(30)), '') end
|| case when(md_nodeinfo(p.n10) like 'Column%') then '' else COALESCE(CAST(p.n10 as varchar(30)), '') end
,md_nodeinfo(p.n1),md_nodeinfo(p.n2),md_nodeinfo(p.n3),md_nodeinfo(p.n4),md_nodeinfo(p.n5),md_nodeinfo(p.n6),md_nodeinfo(p.n7),md_nodeinfo(p.n8),md_nodeinfo(p.n9),md_nodeinfo(p.n10)
from MD_allpaths_5(@FROMID,@TOID,@DEPTH,@EXCLUDE, @EXCLUDE_TYPES) p order by 1,2";

            int i1 = 0;
            allPaths = new List<List<string[]>>();
            //            rightToLeft.Clear();
            string oldKey = "";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@FROMID", fromId);
                cmd.Parameters.AddWithValue("@TOID", toId);
                cmd.Parameters.AddWithValue("@DEPTH", findDepth);
                cmd.Parameters.AddWithValue("@EXCLUDE", "");
                cmd.Parameters.AddWithValue("@EXCLUDE_TYPES", "4,3,5,7,102,103,104,105");
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var Key = reader.GetString(1);
                        if (Key != oldKey)
                        {
                            allPaths.Add(new List<string[]>());
                            for (int i = 2; i < reader.FieldCount; i++)
                            {
                                if (!reader.IsDBNull(i))
                                {
                                    var st = reader.GetString(i);
                                    if (!string.IsNullOrEmpty(st))
                                    {
                                        var arr = st.Split(':');
                                        allPaths.Last().Add(arr);
                                    }
                                }
                            }
                        }
                        oldKey = Key;
                        i1++;
                    }
                }
            }
            conn.Close();
            return allPaths;

        }


    //    public JsonElement 

    }
}