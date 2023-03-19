using BlazorAppCreateETL.Shared;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Core.Tokens;

namespace ETL_DB_Interface
{
    public class DBInterface
    {
        public static async Task<ETL_Package> FillPackageContent(NpgsqlConnection conn, ItemPackage? pack)
        {
            ETL_Package package = new ETL_Package();
            package.idPackage = pack.id;
            {
                await using (var cmd = new NpgsqlCommand(@"select n.name,a1.val out_table,a2.val description,a3.val type_src,a4.val add_par  from md_Node n
left join md_node_attr_val a1  on( a1.attrid=42 and a1.nodeid=n.nodeid)
left join md_node_attr_val a2  on( a2.attrid=43 and a2.nodeid=n.nodeid)
left join md_node_attr_val a3  on( a3.attrid=44 and a3.nodeid=n.nodeid)
left join md_node_attr_val a4  on( a4.attrid=57 and a4.nodeid=n.nodeid)
  where n.nodeid= @id and n.isdeleted=false", conn))
                {
                    cmd.Parameters.AddWithValue("@id", pack.id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            package.ETLName = reader.GetString(0);
                            if (!reader.IsDBNull(1))
                                package.OutputTables.AddRange(reader.GetString(1).Split(','));
                            if (!reader.IsDBNull(2))
                                package.ETLDescription = reader.GetString(2);
                            if (reader.FieldCount > 3 && !reader.IsDBNull(3))
                                package.ETL_dest_id = Convert.ToInt32(reader.GetString(3));
                            package.ETL_add_par = "";
                            if (!reader.IsDBNull(4))
                                package.ETL_add_par = reader.GetString(4);


                        }
                    }
                }

            }

            {
                await using (var cmd = new NpgsqlCommand(@"select a.name name_variable, b.val description_var,b1.val,b2.val from md_node a
inner join md_node_attr_val b on (a.nodeid = b.nodeid and b.attrid = 46)
inner join md_node_attr_val b1 on (a.nodeid = b1.nodeid and b1.attrid = 49)
inner join md_node_attr_val b2 on (a.nodeid = b2.nodeid and b2.attrid = 50)
inner join md_arc c on(c.toid = a.nodeid and c.fromid = @id and c.isdeleted = false)


", conn))
                {
                    package.variables.Clear();
                    cmd.Parameters.AddWithValue("@id", pack.id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            package.variables.Add(new ETL_Package.VariableItem() { Name = reader.GetString(0), Description = reader.GetString(1), VariableType = reader.GetString(2), VariableDefaultValue = reader.GetString(3) });

                        }
                    }
                }

            }

            {
                await using (var cmd = new NpgsqlCommand(@"select t1.name,at1.val alias1, at2.val selectList1, att1.toid idTable, t1.nodeid idetlTable,f.val as condition,at3.val from md_node t1 
 inner join md_arc a1 on (a1.fromid = @id and t1.nodeid = a1.toid)
     inner join md_arc att1 on(att1.fromid = t1.nodeid)
     left join md_node_attr_val at1 on(t1.nodeid = at1.nodeid and at1.attrid = 39)
     left join md_node_attr_val at2 on(t1.nodeid = at2.nodeid and at2.attrid = 41)
     left join md_node_attr_val at3 on(t1.nodeid = at3.nodeid and at3.attrid = 115)
     left join md_node_attr_val f on (t1.nodeid = f.nodeid and f.attrid = 40)
     where t1.typeid = md_get_type('ETLTable') and t1.isdeleted=false
", conn))
                {
                    package.conditions.Clear();
                    package.selectedFields.Clear();
                    package.allTables.Clear();
                    /*                        listViewSelectedField.Items.Clear();
                                            foreach (var field in selectedFields)
                                                listViewSelectedField.Items.Add(new ListViewItem(new String[] { field.col_name, field.alias, field.table.table_name, field.table.alias }));*/
                    cmd.Parameters.AddWithValue("@id", pack.id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var table_id = reader.GetInt64(3);
                            var table_name = reader.GetString(0);
                            string alias = "";
                            if (!reader.IsDBNull(1))
                                alias = reader.GetString(1);
                            var etl_id = reader.GetInt64(4);
                            ETL_Package.ItemTable table = package.allTables.FirstOrDefault(ii => ii.etl_id == etl_id);
                            if (table == null)
                            {
                                table = new ETL_Package.ItemTable() { alias = alias, table_name = table_name, etl_id = etl_id, table_id = table_id };
                                package.allTables.Add(table);
                            }
                            string outTablesList = "";
                            string[] outTables = null;
                            if (!reader.IsDBNull(6))
                            {
                                outTablesList = reader.GetString(6);
                                outTables = outTablesList.Split(',');
                            }

                            var selectList = reader.GetString(2);
                            var selListSplit = selectList.Split(",");
                            for (int i=0;i <selListSplit.Length;i++)
                            {
                                var it = selListSplit[i];
                                var outTable = "";// package.TableOutputName.First();
                                if(outTables?.Length>i)
                                    outTable= outTables[i];
                                GenerateStatement.ItemTable.SelectListItem selectItem = new GenerateStatement.ItemTable.SelectListItem(it,outTable);

                                package.selectedFields.Add(new ETL_Package.ItemSelectedList()
                                {
                                    sourceColumn = new ETL_Package.ItemColumn()
                                    { table = table, col_name = selectItem.expression, alias = selectItem.alias }
                                    ,
                                    outputTable = ((outTables == null) ? package.OutputTables.First() : outTables[i])
                                });
                            }
                            if (!reader.IsDBNull(5))
                            {
                                package.conditions.Add(new ETL_Package.ItemAddCondition() { condition = reader.GetString(5), table = table });
                            }

                            //                                variables.Add(new VariableItem() { Name = reader.GetString(0), Description = reader.GetString(1) });

                        }
                    }
                }
            }


            {
                await using (var cmd = new NpgsqlCommand(@"select distinct r.Name,fk.nodeid RelID,lt.nodeid idtable1,rt.nodeid idtable2 from md_Node r 
inner join md_arc ml on(ml.toid = r.nodeid and ml.fromid = @id)
inner join md_arc rfk on(rfk.fromid = r.nodeid)
inner join md_node fk on(fk.nodeid = rfk.toid and(fk.typeid = md_get_type('ForeignKey') or(fk.typeid = md_get_type('VForeignKey'))))
inner join md_arc lr on(lr.fromid = r.nodeid)
inner join md_node lt on(lt.nodeid = lr.toid and lt.typeid = md_get_type('ETLTable'))
inner join md_arc rr on(rr.fromid = r.nodeid and rr.toid <> lr.toid)
inner join md_node rt on(rt.nodeid = rr.toid and rt.typeid = md_get_type('ETLTable'))
where r.isdeleted = false
", conn))
                {
                    package.relations.Clear();
                    cmd.Parameters.AddWithValue("@id", pack.id);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var fk_name = reader.GetString(0);
                            var fk_id = reader.GetInt64(1);
                            var table_id1 = reader.GetInt64(2);
                            var table_id2 = reader.GetInt64(3);
                            if (package.relations.Count(ii => ii.relationID == fk_id) == 0)
                            {
                                package.relations.Add(new ETL_Package.ItemRelation() { relationID = fk_id, relationName = fk_name, table1 = package.allTables.First(ii => ii.etl_id == table_id1), table2 = package.allTables.First(ii => ii.etl_id == table_id2) });
                            }


                        }
                    }
                }
            }
            return package;
        }
        public static async Task<List<ItemPackage>> GetSrcItems(NpgsqlConnection conn)
        {
            List<ItemPackage> packages = new List<ItemPackage>();
            await using (var cmd = new NpgsqlCommand(@"select srcid, descr from md_src", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    packages.Add(new ItemPackage() { Name = reader.GetString(1), id = reader.GetInt64(0) });
                }
            }

            return packages;
        }

        public static async Task<ETL_Package.ItemTable> enrichTable(NpgsqlConnection conn, ETL_Package.ItemTable table)
        {
            await using (var cmd = new NpgsqlCommand(@"        select nc.name colname, nc.nodeid colid, nt.name tablename, s.name, s.srcid, s.pci_dss_zone from MD_node nc
inner join MD_type tc on nc.typeid = tc.typeid and tc.key in ('Column','DictionaryColumn')
inner join MD_arc ac on(ac.fromid = nc.nodeid  and ac.isdeleted= false)
inner join md_Node nt on ac.toid = nt.nodeid and nt.isdeleted= false
inner join md_src s on (nt.srcid= s.srcid)
where nt.nodeid=@id and nc.isdeleted=false
", conn))
            {
                cmd.Parameters.AddWithValue("@id", table.table_id);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    table.columns = new List<ETL_Package.ItemColumn>();
                    while (await reader.ReadAsync())
                    {
                        table.columns.Add(new ETL_Package.ItemColumn() { table = table, col_name = reader.GetString(0), col_id = reader.GetInt64(1) });
                        table.table_name= reader.GetString(2);
                        table.scema= reader.GetString(3);
                        table.src_id=reader.GetInt32(4);
                        table.pci_dss_zone= reader.GetBoolean(5);
//                        list.Add(new ETL_Package.ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1), table = new ETL_Package.ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3), scema = reader.GetString(4) } });
                    }
                }
            }

            return table;
        }
       

        public class SourceTableItem
        {
            public long table_id;
            public string table_name;
            public class ItemColumnRef
            {
                public long source_col_id;
                public string source_col_name;
                public long dest_col_id;
                public string dest_col_name;
            }
            public List<ItemColumnRef> columns= new List<ItemColumnRef>();
        }

        
        public static async Task<List<SourceTableItem>> getSrcForTable(NpgsqlConnection conn, string tableName,int dest_id,int src_id,string srcTableNameOrig)
        {
            List<SourceTableItem> retValue = new List<SourceTableItem>();
            await using (var cmd = new NpgsqlCommand(@"
        select t1.name src_table,snode.name src_column,nt.name dest_table,c.name dest_column
,t1.nodeid src_table_id,snode.nodeid src_column_id,nt.nodeid dest_table_id,c.nodeid dest_table_id
from md_node c
inner join md_arc l2 on (l2.fromid = c.nodeid and l2.typeid=md_get_type('Column2Table'))
inner join md_node nt on (l2.toid=nt.nodeid )
inner join md_node snode on (snode.synonym = c.synonym and snode.srcid=@srcid)
inner join md_arc l1 on (l1.fromid = snode.nodeid and l1.typeid=md_get_type('Column2Table'))
inner join md_node t1 on (l1.toid=t1.nodeid)
where c.srcid=@destid and nt.name=@tablename and t1.name=@srctablename
order by t1.nodeid
 ", conn))
            {
                cmd.Parameters.AddWithValue("@srcid", src_id);
                cmd.Parameters.AddWithValue("@destid", dest_id);
                cmd.Parameters.AddWithValue("@tablename", tableName);
                cmd.Parameters.AddWithValue("@srctablename", srcTableNameOrig);
                long lastId = -1;
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string srcTableName=reader.GetString(0);
                        string srcColumnName = reader.GetString(1);
                        string destTableName = reader.GetString(2);
                        string destColumnName = reader.GetString(3);
                        long src_table_id=reader.GetInt64(4);
                        long src_column_id = reader.GetInt64(5);
                        long dest_table_id = reader.GetInt64(6);
                        long dest_column_id = reader.GetInt64(7);
                        if(lastId != src_table_id)
                        {
                            lastId=src_table_id;
                            retValue.Add(new SourceTableItem { table_id = src_table_id, table_name = srcTableName });
                        }
                        retValue.Last().columns.Add(new SourceTableItem.ItemColumnRef() {  source_col_id=src_column_id, source_col_name= srcColumnName , dest_col_id=dest_column_id, dest_col_name=destColumnName });
                    }
                }
            }

            return retValue;
        }

        public static async Task<ETL_Package.ItemRelation> enrichRelation(NpgsqlConnection conn, ETL_Package.ItemRelation rel)
        {
            await using (var cmd = new NpgsqlCommand(@"select s.nodeid as source_tid,s.name as source_tname,f.nodeid as source_cid, f.name as source_cname,
       t.nodeid as target_tid,t.name as target_tname,tf.nodeid as target_cid, tf.name as target_cname
    from md.md_node s, md.md_type st, md.md_arc slf, md.md_type slft, md.md_node f, md.md_type ft,
         md.md_node fkn, md.md_arc fka, md.md_arc tfka,
         md.md_node t, md.md_type tt, md.md_arc tlf, md.md_type tlft, md.md_node tf, md.md_type tft
    where s.typeid=st.typeid and st.key='Table' and not s.isdeleted
      and slf.typeid=slft.typeid and slft.key='Column2Table'  and not slf.isdeleted
      and f.typeid=ft.typeid and ft.key='Column' and not f.isdeleted
      and s.nodeid=slf.toid and f.nodeid=slf.fromid
--      and s.srcid=5 and slf.srcid=5 and f.srcid=5 and fkn.srcid=5 and fka.srcid=5
      and (fkn.typeid=md_get_type('ForeignKey')
        or fkn.typeid=md_get_type('VForeignKey')
          )
      and fkn.nodeid=@id
   ---fkn.nodeid=2057121-- fkn.name='terminal_country_fk'
      and fka.fromid=f.nodeid and fka.toid=fkn.nodeid
      and tfka.toid=tf.nodeid and tfka.fromid=fkn.nodeid
      and t.typeid=tt.typeid and tt.key='Table' and not t.isdeleted
      and tlf.typeid=tlft.typeid and tlft.key='Column2Table'  and not tlf.isdeleted
      and tf.typeid=tft.typeid and tft.key='Column' and not tf.isdeleted
      and t.nodeid=tlf.toid and tf.nodeid=tlf.fromid
", conn))
            {
                cmd.Parameters.AddWithValue("@id", rel.relationID);
                rel.column1Name = new List<string>();
                rel.column2Name= new List<string>();    
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        rel.column1Name.Add(reader.GetString(3));
                        rel.column2Name.Add(reader.GetString(7));
                    }
                }
            }

            return rel;
        }

        public static async Task<List<ETL_Package.ItemColumn>> GetColumnsForPattern(NpgsqlConnection conn, string findString)
        {
            List<ETL_Package.ItemColumn> list = new List<ETL_Package.ItemColumn>();
            await using (var cmd = new NpgsqlCommand(@"select nc.name colname,nc.nodeid colid,nt.name tablename,nt.nodeid tableid,s.name from MD_node nc 
inner join MD_type tc on nc.typeid = tc.typeid and tc.key = 'Column'
inner join MD_arc ac on (ac.fromid = nc.nodeid  and ac.isdeleted=false)
inner join md_Node nt on ac.toid = nt.nodeid  and nt.typeid = 1 and nt.isdeleted=false
inner join md_src s on (nt.srcid=s.srcid)


where nc.name like '%" + findString + "%' and nc.isdeleted=false", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ETL_Package.ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1), table = new ETL_Package.ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3), scema = reader.GetString(4) } });
                }
            }

            return list;
        }
        public static async Task<List<ETL_Package.ItemColumn>> GetColumnsForTablePattern(NpgsqlConnection conn, string findString)
        {
            List<ETL_Package.ItemColumn> list = new List<ETL_Package.ItemColumn>();
            await using (var cmd = new NpgsqlCommand(@"select nc.name colname,nc.nodeid colid,nt.name tablename,nt.nodeid tableid,s.name from MD_node nc 
inner join MD_type tc on nc.typeid = tc.typeid and tc.key = 'Column'
inner join MD_arc ac on (ac.fromid = nc.nodeid  and ac.isdeleted=false)
inner join md_Node nt on ac.toid = nt.nodeid  and nt.typeid = 1 and nt.isdeleted=false
inner join md_src s on (nt.srcid=s.srcid)


where nt.name like '%" + findString + "%' and nc.isdeleted=false", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new ETL_Package.ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1), table = new ETL_Package.ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3), scema = reader.GetString(4) } });
                }
            }

            return list;
        }
        public static async Task<List<ItemPackage>> GetTablesForPatternAndSrc(NpgsqlConnection conn, string findString,int id_src)
        {
            List<ItemPackage> list = new List<ItemPackage>();
            await using (var cmd = new NpgsqlCommand(@"select nt.name tablename,nt.nodeid tableid from md_Node nt 
where nt.name like '%" + findString + "%' and nt.srcid=@src and typeid in (1,10,19) and nt.isdeleted=false", conn))
            {
                cmd.Parameters.AddWithValue("@src", id_src);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new ItemPackage() { Name = reader.GetString(0), id = reader.GetInt64(1) });
                    }
                }
            }

            return list;
        }

        public static async Task<List<ItemPackage>> GetPackagesItems(NpgsqlConnection conn)
        {
            List<ItemPackage> packages = new List<ItemPackage>();
            await using (var cmd = new NpgsqlCommand(@"select nodeid, name from md_node where typeid = md_get_type('ETLPackage') and isdeleted=false", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    packages.Add(new ItemPackage() { Name = reader.GetString(1), id = reader.GetInt64(0) });
                }
            }

            return packages;
        }

        public static async Task<List<ETL_Package.ItemTable>> GetOutputTables(NpgsqlConnection conn, ETL_Package package)
        {
            List<ETL_Package.ItemTable> retValue = new List<ETL_Package.ItemTable>();
            var command = @"select n1.NodeID, n1.Name, n2.NodeID, n2.Name, a2.key, s.name, s.srcid, s.pci_dss_zone
    from md_node n1 inner join md_src s on (n1.srcid= s.srcid), md_arc a1, md_node n2, md_node_attr_val nav2, md_attr a2

    where (n1.typeid=md_get_type('Table') or n1.typeid=md_get_type('Dictionary') ) and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and (n2.typeid=md_get_type('Column') or n2.typeid=md_get_type('DictionaryColumn'))
      and n2.NodeID=nav2.NodeID
      and nav2.AttrID=a2.AttrID
  and a2.attrPID=1
  and n2.isdeleted=false and n1.isdeleted=false
      and n1.name=ANY(@names) and n1.srcid=@srcid
";
            string tableName = "";
            long lastId = -1;
            ETL_Package.ItemTable lastTable=null;
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@names", package.OutputTables);
                cmd.Parameters.AddWithValue("@srcid", package.ETL_dest_id);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id=reader.GetInt64(0);
                        if(id != lastId)
                        {
                            lastTable = new ETL_Package.ItemTable();
                            lastTable.table_name = reader.GetString(1);
                            lastTable.src_id = package.ETL_dest_id;
                            lastTable.scema = reader.GetString(5);
                            lastTable.columns = new List<ETL_Package.ItemColumn>();
                            retValue.Add(lastTable);
                            lastId= id;
                            //                            var idCol = reader.GetInt64(2);
                        }

                        var idCol = reader.GetInt64(2);
                        var column = reader.GetString(3);
                        var type = reader.GetString(4);
                        lastTable.columns.Add(new ETL_Package.ItemColumn() { col_name = column,  col_id = idCol, table=lastTable });
                    }
                }


            }
            return retValue;
        }

        public static async Task SaveAndExecuteETL(NpgsqlConnection conn, ETL_Package package)
        {
            await using (var cmd = new NpgsqlCommand(@"select * from md_add_etl_package(5,@id,@title,@output_name,@description,@dest_id,@add_par)", conn))
            {
                cmd.Parameters.AddWithValue("@id", package.idPackage);
                cmd.Parameters.AddWithValue("@title", package.ETLName);
                cmd.Parameters.AddWithValue("@output_name", string.Join(",",package.OutputTables));
                cmd.Parameters.AddWithValue("@description", package.ETLDescription);
                cmd.Parameters.AddWithValue("@dest_id", package.ETL_dest_id);
                cmd.Parameters.AddWithValue("@add_par", package.ETL_add_par);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        package.idPackage = reader.GetInt64(0);
                    }
                }
            }
            /*if(package.TableOutputName.Count>1)
            {
                for(int i=1; i < package.TableOutputName.Count;i++)
                {
                    await using (var cmd = new NpgsqlCommand(@"select * from md_add_etl_dest_table(@id,@output_name)", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", package.idPackage);
//                        cmd.Parameters.AddWithValue("@scema_id", package.idPackage);

                        cmd.Parameters.AddWithValue("@output_name", package.TableOutputName[i]);
                    
                        await using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                package.idPackage = reader.GetInt64(0);
                            }
                        }
                    }

                }
            }
            */
            foreach (var item in package.variables)
            {
                await using (var cmd = new NpgsqlCommand(@"select * from ccfa_add_etl_variable(5,@id,@name,@description,@type,@defaultValue)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", package.idPackage);
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.Parameters.AddWithValue("@type", item.VariableType);
                    cmd.Parameters.AddWithValue("@defaultValue", item.VariableDefaultValue);


                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reader.GetInt64(0);
                        }
                    }
                }

            }






            string selectList = "";
            string outputList = "";

            string lastKey = "";
            ETL_Package.ItemSelectedList lastItem = null;
            foreach (var item in package.selectedFields.OrderBy(ii => ii.sourceColumn.table.table_name + ii.sourceColumn.table.alias))
            {
                if (item.sourceColumn.table.table_name + item.sourceColumn.table.alias != lastKey)
                {
                    lastKey = item.sourceColumn.table.table_name + item.sourceColumn.table.alias;
                    await SaveItem(conn, package.idPackage, selectList, lastItem,outputList);
                    selectList = "";
                    outputList = "";

                }
                selectList += ((selectList == "") ? "" : ",") + item.sourceColumn.col_name + " " + item.sourceColumn.alias;
                outputList += ((string.IsNullOrEmpty(outputList)) ? "" : ",") + item.outputTable;
                lastItem = item;
            }
            await SaveItem(conn, package.idPackage, selectList, lastItem,outputList);


            /*                foreach (var group in selectedFields.GroupBy(ii => ii.table.table_name + ii.table.alias,i1=>i1.).Select(i1=>i1.Key.)
                            {
                                await using (var cmd = new NpgsqlCommand(@"select * from ccfa_addetltable(2,@etlid,@tableid,@alias,@select_list)", conn))
                                {
                                    cmd.Parameters.AddWithValue("@etlid", idPackage);
                                    cmd.Parameters.AddWithValue("@tableid", item.table.table_id);
                                    cmd.Parameters.AddWithValue("@alias", item.table.alias);
                                    cmd.Parameters.AddWithValue("@select_list", item.col_name);
                                    await using (var reader = await cmd.ExecuteReaderAsync())
                                    {
                                        while (await reader.ReadAsync())
                                        {
                                            item.table.etl_id = reader.GetInt64(0);
                                        }
                                    }
                                }
                            } 
            */
            foreach (var item in package.relations)
                await using (var cmd = new NpgsqlCommand(@"select * from ccfa_addetlrelation(5,@etlid,@fk_id,@table1id,@table2id)", conn))
                {
                    cmd.Parameters.AddWithValue("@etlid", package.idPackage);
                    cmd.Parameters.AddWithValue("@table1id", item.table1.etl_id);
                    cmd.Parameters.AddWithValue("@table2id", item.table2.etl_id);
                    cmd.Parameters.AddWithValue("@fk_id", item.relationID);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reader.GetInt64(0);
                        }
                    }
                }
            foreach (var item in package.conditions)
                await using (var cmd = new NpgsqlCommand(@"select * from ccfa_addetlcondition(5,@tableid,@condition)", conn))
                {
                    cmd.Parameters.AddWithValue("@tableid", item.table.etl_id);
                    cmd.Parameters.AddWithValue("@condition", item.condition);
                    //cmd.ExecuteNonQuery();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            //       reader.GetInt64(0);
                        }
                    }
                }
            var options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            using (StreamWriter sw = new StreamWriter(@"c:\d\pack.json"))
            {
                sw.Write(JsonSerializer.Serialize<ETL_Package>(package,options));
            }
            await GenerateStatement.Generate(conn, package.idPackage);
        }
        public class TableRelsItem
        {
            public string MainTable;
            public string SecondTable;
            public string ColumnSecond;
        }
        public static async Task<IEnumerable<TableRelsItem>> GetAllRelation(NpgsqlConnection conn, string[] tables,int srcid)
        {
            List<TableRelsItem> retValue= new List<TableRelsItem>();
            /*string[] tables = new string[] { "card", "account","customer","branch" };
            int srcid = 5;*/
            await using (var cmd = new NpgsqlCommand(@"
select distinct lt.name,rt.name,rk.name from md_node a,md_node b,md_arc lc,md_arc rc--,md_node lk,md_node rk
,md_arc fkl,md_arc fkr,
md_node lt,md_node rt ,md_node rk
where a.name  =any(@arr)
and a.typeid=1 and a.isdeleted=false and a.srcid=@srcid
and b.name =any(@arr)
and b.typeid=1  and b.isdeleted=false  and b.srcid=@srcid
and ((lc.toid=a.nodeid or lc.toid=b.nodeid) and lc.typeid=9 and lc.isdeleted=false)
and ((rc.toid=a.nodeid or rc.toid=b.nodeid) and rc.typeid=9 and rc.isdeleted=false)
--and lc.fromid=lk.nodeid
--and rc.fromid=rk.nodeid
and fkl.typeid=6
and fkl.fromid=lc.fromid
--and fkl.toid=fkn.nodeid
and fkr.fromid=fkl.toid
and fkr.toid=rc.fromid
and lt.nodeid=lc.toid
and rt.nodeid=rc.toid
and rk.nodeid=rc.fromid
", conn))
            {
                cmd.Parameters.AddWithValue("@arr", tables);
                cmd.Parameters.AddWithValue("@srcid", srcid);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        retValue.Add(new TableRelsItem() { MainTable = reader.GetString(1), SecondTable = reader.GetString(0), ColumnSecond = reader.GetString(2) });
                        /* var mainTable=reader.GetString(0);
                       var secondTable = reader.GetString(1);
                       var thirdTable = reader.GetString(2);
                        */
                        //                            lastItem.sourceColumn.table.etl_id = reader.GetInt64(0);
                    }
                }
            }
            return retValue;
        }



        private static async Task SaveItem(NpgsqlConnection conn, long idPackage, string selectList, ETL_Package.ItemSelectedList lastItem,string outputList)
        {
            if (lastItem != null)
            {
                await using (var cmd = new NpgsqlCommand(@"select * from md_addetltable(5,@etlid,@tableid,@alias,@select_list,@output_list)", conn))
                {
                    cmd.Parameters.AddWithValue("@etlid", idPackage);
                    cmd.Parameters.AddWithValue("@tableid", lastItem.sourceColumn.table.table_id);
                    cmd.Parameters.AddWithValue("@alias", lastItem.sourceColumn.table.alias);
        
                    cmd.Parameters.AddWithValue("@select_list", selectList);
                    cmd.Parameters.AddWithValue("@output_list", outputList);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lastItem.sourceColumn.table.etl_id = reader.GetInt64(0);
                        }
                    }
                }
            }
        }


    }
}