using MXGraphHelperLibrary;
using Npgsql;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using System.Text.Json;
using static FrontInterfaceSupport.DBTable;

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
        public class DBTableConfig
        {
            public long tableId { get; set; }
            public long? tableExistedId { get; set; }
            public List<string> conditions { get; set; } = new List<string>();
            public List<List<string>> relation { get; set; }= new List<List<string>>();
            public int depth { get; set; } = 4;
        }

        public class ItemColumn
        {
            public string col_name { get; set; }
            public string alias { get; set; }
            public long col_id { get; set; }
            public ItemTable table { get; set; }

            public override string ToString()
            {
                return $"{col_name}:{table}";

            }
        }


        public static async Task<string> CreateOrModifyTables(string jsonMXGrapth = "", string tableDefJson = "{\r\n  \"tableId\":550119,\r\n  \"tableExistedId\":550079,\r\n  \"conditions\":[\"OriginalTime>@timeBegin\",\"OriginalTime<@timeEnd\",\"OriginalTime is not null\"],\r\n  \"relation\":[[\"Table\",\"550119\",\"account\",\"\"],[\"Column\",\"550130\",\"branchid\",\"account\"],[\"ForeignKey\",\"2163920\",\"branchid\",\"2163595\",\"branchid\",\"550130\"],[\"Column\",\"2163595\",\"branchid\",\"branch\"],[\"ForeignKey\",\"2163944\",\"branchid\",\"2163595\",\"branchid\",\"550095\"],[\"Column\",\"550095\",\"branchid\",\"card\"],[\"Table\",\"550079\",\"card\",\"\"]]\r\n\r\n\r\n\r\n,\r\n  \"depth\":6\r\n}", string ConnectionString= "User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=md;",bool isNew=true)
        {
            if (!isNew)
                return jsonMXGrapth;
            MXGraphHelperLibrary.MXGraphDoc retDoc = new MXGraphHelperLibrary.MXGraphDoc();

            if (!string.IsNullOrEmpty(jsonMXGrapth))
                retDoc = JsonSerializer.Deserialize<MXGraphHelperLibrary.MXGraphDoc>(jsonMXGrapth);
            else
                retDoc.boxes = new List<MXGraphHelperLibrary.MXGraphDoc.Box>();
            DBTableConfig dbTableConfig = JsonSerializer.Deserialize<DBTableConfig>(tableDefJson);
            if (dbTableConfig.relation?.Count > 0 && (dbTableConfig.relation.Count < 5 || dbTableConfig.relation.Count % 2 == 0))
                throw new Exception($"invalid relation length:{dbTableConfig.relation.Count}");
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            var box1 = await AddTable(  retDoc, dbTableConfig, conn);
            var lastBox = box1;
            var box2 = box1;
            var lastIndex = 1;
            if (dbTableConfig.relation?.Count > 0)
            {
                if(dbTableConfig.relation?.Count>5)
                {
                    for(int i=3; i<dbTableConfig.relation?.Count-2;i+=2)
                    {
                        var temp = await getTableConfig(dbTableConfig.relation, i, conn);

                        if (dbTableConfig.relation[i][3] != dbTableConfig.relation[i - 2][3])
                        {
                            box2 = await AddTable( retDoc, temp, conn);
                            AddLink(retDoc, lastBox, box2, dbTableConfig.relation[i], dbTableConfig.relation[i - 2]);
                        }
                        lastIndex = i;
                        lastBox = box2;
                    }
                }
                var destTables = findTable(retDoc, Convert.ToInt64(dbTableConfig.relation.Last()[1])).ToArray();
                if (destTables.Length > 0)
                {
                    AddLink(retDoc,lastBox, destTables.First(), dbTableConfig.relation[1], dbTableConfig.relation[dbTableConfig.relation.Count - 2]);
                }


               /* if (dbTableConfig.relation?.Count > 0)
                {
                    var temp = await getTableConfig(dbTableConfig.relation, 1, conn);
                }*/
            }

            conn.Close();
            JsonSerializerOptions options = new JsonSerializerOptions() { IgnoreNullValues = true };

            return JsonSerializer.Serialize<MXGraphHelperLibrary.MXGraphDoc>(retDoc,options);

        }

        static IEnumerable<MXGraphHelperLibrary.MXGraphDoc.Box> findTable(MXGraphDoc retDoc,long tableId)
        {
            foreach(var tabl_box in retDoc.boxes.Where(ii=>ii.type=="table"))
            {
                var conf=JsonSerializer.Deserialize<DBTableConfig>((JsonElement)tabl_box.AppData);
                if (conf.tableId == tableId)
                    yield return tabl_box;
            }
        }
        const int heigthHeaderBox = 64;
        const int heigthRow = 34;

        private static async Task<DBTableConfig> getTableConfig(List<List<string>> relation, int indexCol, NpgsqlConnection conn)
        {
            long colId = Convert.ToInt64(relation[indexCol][1]);
            await using (var cmd = new NpgsqlCommand(@"select nt.name tablename,nt.nodeid,s.name,s.pci_dss_zone tableid from MD_node nc 
inner join MD_type tc on nc.typeid = tc.typeid and tc.key = 'Column'
inner join MD_arc ac on (ac.fromid = nc.nodeid  and ac.isdeleted=false)
inner join md_Node nt on ac.toid = nt.nodeid  and nt.typeid = 1 and nt.isdeleted=false
inner join md_src s on (nt.srcid=s.srcid)
where nc.nodeid=@nodeid and nc.isdeleted=false", conn))
            {
                cmd.Parameters.AddWithValue("@nodeid", colId);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetInt64(1);
                        return new DBTableConfig() { tableId =id  };
                        //                        list.Add(new ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1)/*, table = new ETL_Package.ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3), src_name = reader.GetString(4) }*/ });
                    }
                }
            }
            return null;

        }

        private static async Task<MXGraphHelperLibrary.MXGraphDoc.Box> AddTable(  MXGraphDoc retDoc, DBTableConfig dbTableConfig, NpgsqlConnection conn)
        {
            MXGraphHelperLibrary.MXGraphDoc.Box retBox = new MXGraphHelperLibrary.MXGraphDoc.Box();
            retBox.AppData =   JsonDocument.Parse(JsonSerializer.Serialize<DBTableConfig>( dbTableConfig)).RootElement;
            retBox.header = new MXGraphHelperLibrary.MXGraphDoc.Box.Header();
            if (retDoc.boxes.Count == 0)
            {
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = 100, top = 100 };
            }
            else
            {
                int delta = 15;
                int left = retDoc.boxes.Min(ii => ii.header.position.left);
                int top = retDoc.boxes.Max(ii => ii.header.position.top + ii.header.size.height) + delta;
                retBox.header.position = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Position() { left = left, top = top };
            }

            retBox.header.size = new MXGraphHelperLibrary.MXGraphDoc.Box.Header.Size() { width = 300, height = heigthHeaderBox + heigthRow };
//            retBox.id = mxGraphID;
            retBox.type = "table";
            retBox.body = new MXGraphHelperLibrary.MXGraphDoc.Box.Body();
            retBox.body.header = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Header>() { new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Name" }, new MXGraphHelperLibrary.MXGraphDoc.Box.Header() { value = "Type" } };
            retBox.body.rows = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row>();
            await using (var cmd = new NpgsqlCommand(@"select nc.name colname,nc.nodeid colid,nt.name tablename,nt.nodeid tableid,s.name,s.pci_dss_zone 
     , (select a.key from md_node_attr_val nav join md_attr a using (attrid) join md_attr ap on (a.attrpid=ap.attrid) where nodeid=nc.nodeid and ap.key='DataType' and not nav.isdeleted) as coltype
     , (select a.key from md_node_attr_val nav join md_attr a using (attrid) join md_attr ap on (a.attrpid=ap.attrid) where nodeid=nc.nodeid and ap.key='Constraint' and a.key='isNull' and not nav.isdeleted) as isNULL
     , (select t.key from md_arc a join md_type t using (typeid) where toid=nc.nodeid and t.key='PrimaryKey' and not a.isdeleted) as PK
     , (select t.key from md_arc a join md_type t using (typeid) where fromid=nc.nodeid and t.key='ForeignKey' and not a.isdeleted) as FK
    from MD_node nc
        inner join MD_type tc on nc.typeid = tc.typeid and tc.key = 'Column'
        inner join MD_arc ac on (ac.fromid = nc.nodeid  and ac.isdeleted=false)
        inner join md_Node nt on ac.toid = nt.nodeid  and nt.isdeleted=false
        inner join md_type tt on nt.typeid=tt.typeid and tt.key='Table'
        inner join md_src s on (nt.srcid=s.srcid)
where nt.nodeid=@nodeid and nc.isdeleted=false", conn))
            {
                cmd.Parameters.AddWithValue("@nodeid", dbTableConfig.tableId);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        
                        retBox.header.size.height += heigthRow;
                        retBox.header.zone_name = reader.GetString(4);
                        bool isPciDss = reader.GetBoolean(5);
                        retBox.header.zone_type = "unknown";
                        retBox.header.caption = reader.GetString(2);
                        retBox.id = reader.GetString(2);
                        retBox.header.description = "unknown";
                        retBox.body.rows.Add(new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row()
                        {
                            tooltip_info = new Dictionary<string, string>() {
                                { "name", "Name" },
                                { "description", "Description" },
                                { "type", "Data type" }
                            },
                            columns = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column>() {
                                new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column()  { item= new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item() {caption=reader.GetString(0) } },
                                new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column()  { item= new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item() {caption=reader.GetString(6), box_id=retBox.id+"_"+reader.GetInt64(1) } }
                            }

                        }
                            );
                        //                        list.Add(new ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1)/*, table = new ETL_Package.ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3), src_name = reader.GetString(4) }*/ });
                    }
                }
            }
            if(dbTableConfig.conditions?.Count>0)
            {
                retBox.body.rows.Add(new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row()
                {
                    columns = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column>() {
                                new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column()  
                                { 
                                    item= new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item() {   
                                        caption="Conditions", 
                                        colspan=2,
                                        style="padding: 5px 30px;border-top: 1px solid var(--grey-10); border-bottom: none; background: var(--global-white);vertical-align: top;border-bottom-right-radius: 0;border-bottom-left-radius: 0; font: var(--font-h3-semibold-14);"
                                    } 
                                }
                            }

            });
                retBox.header.size.height += heigthRow;
                for (int i=0; i <dbTableConfig.conditions.Count;i++)
                {
                    char s = 'b';
/*                    if (i == 0)
                        s = 'b';*/
                    if (i == dbTableConfig.conditions.Count - 1)
                        s = 'e';
                retBox.header.size.height += heigthRow;

                retBox.body.rows.Add(new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row()
                    {
                        columns = new List<MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column>() {
                                new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column()
                                {
                                    item= new MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item() {
                                        caption=dbTableConfig.conditions[i],
                                        colspan=2,
                                        box_id="cond_"+i,
                                        style=s switch {
        'b' => "padding: 5px 30px;background: var(--global-white);vertical-align: top;border-radius:0; border-top: none; border-bottom: none;",
        'e' => "padding: 5px 30px;background: var(--global-white);vertical-align: top;border-bottom: 1px solid var(--grey-10); border-top:none; border-top-right-radius: 0;border-top-left-radius: 0;",
        _ =>   "padding: 5px 30px;background: var(--global-white);vertical-align: top;border-bottom: 1px solid var(--grey-10); border-top:none; border-top-right-radius: 0;border-top-left-radius: 0;"
    }
                }
                                }
                            }

                    }); ;

                }


            }
            retDoc.boxes.Add(retBox);
            return retBox;
        }

        static MXGraphHelperLibrary.MXGraphDoc.Box.Body.Row.Column.Item findCol(MXGraphHelperLibrary.MXGraphDoc.Box box,string colName)
        {
            foreach(var row in box.body.rows)
            {
                foreach(var col in row.columns)
                {
                    if (col.item.caption == colName)
                        return col.item;
                }
            }
            return null;
        }

        static int calcX(MXGraphHelperLibrary.MXGraphDoc retDoc)
        {
            var minX1 = retDoc.boxes.Min(ii => ii.header.position.left);
            foreach(var item in  retDoc.boxes)
            {
                foreach(var it1 in item.body.rows)
                {
                    foreach( var it2 in it1.columns)
                    {
                        
                        int minX2=(it2.item?.box_links?.Min(ii => ii.link?.points.x))??Int32.MaxValue;
                        if(minX2< minX1 ) minX1=minX2;

                    }
                }
            }
            return minX1;
        }

        static void AddLink(MXGraphHelperLibrary.MXGraphDoc retDoc,MXGraphHelperLibrary.MXGraphDoc.Box box1, MXGraphHelperLibrary.MXGraphDoc.Box box2, List<string> col1, List<string> col2)
        {
            var colItem1 = findCol(box1, col1[2]);
            setBoxId(colItem1, box1, col1[2]);
            var colItem2 = findCol(box2, col2[2]);
            var box_id2=setBoxId(colItem2, box2, col2[2]);
            if (colItem1.box_links == null)
                colItem1.box_links = new List<MXGraphDoc.Box.Body.Row.Column.Item.BoxLink>();
            colItem1.box_links.Add( new MXGraphDoc.Box.Body.Row.Column.Item.BoxLink() { link=new MXGraphDoc.Box.Body.Row.Column.Item.BoxLink.Link()
            {
                box_id=box_id2,
                 typelink=2
                 , points= new MXGraphDoc.Box.Body.Row.Column.Item.BoxLink.Link.Points() { x=calcX(retDoc)-15}
            }
            } );


        }

        private static string  setBoxId(MXGraphDoc.Box.Body.Row.Column.Item colItem1,MXGraphDoc.Box box1, string col_name)
        {
            if (string.IsNullOrEmpty(colItem1.box_id))
                colItem1.box_id = $"{box1.id}_{col_name}";
            return box1.id+":"+colItem1.box_id;
        }


        //    public JsonElement 

    }
}