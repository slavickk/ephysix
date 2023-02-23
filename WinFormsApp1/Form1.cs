using CamundaInterface;
using Npgsql;
using WinFormsETLPackagedCreator;
//using Graphviz;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NpgsqlConnection conn;

        public class ItemPackage
        {
            public string Name { get; set; }
            public long id;
            public override string ToString()
            {
                return $"{id}:{Name}";
            }
        }
        Task runner;
        private async void Form1_Load(object sender, EventArgs e)

        {
            string body;
            using(StreamReader sr= new StreamReader(@"Data\example.txt"))
            {
                body = sr.ReadToEnd();
            }
            this.pictureBox1.Image = GraphvizTest.toGraphviz(body);
            runner =CamundaExecutor.runCycle();

            conn = new NpgsqlConnection(GenerateStatement.ConnectionStringAdm);
            conn.Open();
            tableViewControl1.OnTableDoubleClicked += TableViewControl1_OnTableDoubleClicked;
            //            GenerateStatement.Generate(conn, 315721);
//last            await GenerateStatement.Generate(conn, 532746);
            //          await SendToRefDataLoader.putRequestToRefDataLoader(new HttpClient());
            //            SendToRefDataLoader.UploadFileAsync();
            //            await GenerateStatement.Generate(conn, 315745);
            await RedrawAllPackages();

        }

        private async Task RedrawAllPackages()
        {
            comboBoxPackage.Items.Clear();
            await using (var cmd = new NpgsqlCommand(@"select nodeid, name from md_node where typeid = md_get_type('ETLPackage') and isdeleted=false", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    comboBoxPackage.Items.Add(new ItemPackage() { Name = reader.GetString(1), id = reader.GetInt64(0) });
                }
            }
        }

        private void TableViewControl1_OnTableDoubleClicked(TableViewControl.ItemColumn column)
        {
            textBoxColumnAlias.Text = textBoxFieldName.Text = column.Name;
            
//            throw new NotImplementedException();
        }



        public ETL_Package.ItemTable getTable(string Name,string Alias,long id)
        {
            var item=package.allTables.FirstOrDefault(ii => ii.table_name == Name && ii.alias == Alias);
            if(item == null)
            {
                item = new ETL_Package. ItemTable() {  table_name=Name, alias=Alias, table_id=id };
                package.allTables.Add(item);
            }
            return item;
        }
        void RefreshListViewTablesSelected()
        {
            listViewSelectedField.Items.Clear();
            foreach (var field in package.selectedFields)
                listViewSelectedField.Items.Add(new ListViewItem(new String[] { field.col_name,field.alias, field.table.table_name, field.table.alias }));
        }
        void RefreshListViewCondition()
        {
            listViewAddCondition.Items.Clear();
            foreach (var cond in package.conditions)
                listViewAddCondition.Items.Add(new ListViewItem(new String[] { cond.table.ToString(), cond.condition }));
        }
        void RefreshListViewLinksSelected()
        {
            listViewLinks.Items.Clear();
            foreach (var field in package.relations)
                listViewLinks.Items.Add(new ListViewItem(new String[] { field.table1.table_name+" " + field.table1.alias, field.table2.table_name + " " + field.table2.alias, field.relationName }));
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            await using (var cmd = new NpgsqlCommand(@"select nc.name colname,nc.nodeid colid,nt.name tablename,nt.nodeid tableid,s.name from MD_node nc 
inner join MD_type tc on nc.typeid = tc.typeid and tc.key = 'Column'
inner join MD_arc ac on (ac.fromid = nc.nodeid  and ac.isdeleted=false)
inner join md_Node nt on ac.toid = nt.nodeid  and nt.typeid = 1 and nt.isdeleted=false
inner join md_src s on (nt.srcid=s.srcid)


where nc.name like '%" + textBox1.Text + "%' and nc.isdeleted=false", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    comboBox1.Items.Add(new ETL_Package.ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1), table= new ETL_Package.ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3),scema=reader.GetString(4) } });
                }
            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            var cols= comboBox1.SelectedItem as ETL_Package.ItemColumn;
            if (cols != null)
            {
                if (package.selectedFields.Count < 1)
                {
                    package.selectedFields.Add(new ETL_Package.ItemColumn() { table= getTable(textBoxTableName.Text,textBoxTableAlias.Text, cols.table.table_id), alias=textBoxColumnAlias.Text, col_name=textBoxFieldName.Text });
                    RefreshListViewTablesSelected();
                }
                else
                {
                    if (package.allTables.Count(ii => ii.table_name == textBoxTableName.Text && ii.alias == textBoxTableAlias.Text) > 0)
                    {
                        package.selectedFields.Add(new ETL_Package.ItemColumn() { table = getTable(textBoxTableName.Text, textBoxTableAlias.Text, cols.table.table_id), alias = textBoxColumnAlias.Text, col_name = textBoxFieldName.Text });
                        RefreshListViewTablesSelected();

                    }
                    else
                    {
                        try
                        {
                            FormAddTable frm = new FormAddTable(package.selectedFields.First().table.table_id, cols.table.table_id, conn);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                foreach (var item in frm.returnedItems)
                                {
                                    if (item.itemName == "Table")
                                    {
                                        if (package.selectedFields.Count(ii => ii.table.table_id == item.itemId) > 0)
                                        {
                                            var alias = ((char)('a' + package.selectedFields.Count)).ToString();
                                            package.selectedFields.Add(new ETL_Package.ItemColumn() { col_id = cols.col_id,alias=textBoxColumnAlias.Text,
                                                col_name = cols.col_name, table = new  ETL_Package.ItemTable() { alias = alias, table_id = cols.table.table_id, table_name = cols.table.table_name } });
                                        }
                                        else
                                        {
                                            if (cols.table.table_id != item.itemId)
                                                package.selectedFields.Add(new ETL_Package.ItemColumn() { col_id = -1, col_name = "", table = new ETL_Package.ItemTable() { alias = "", table_id = item.itemId, table_name = await GetNodeName(item.itemId) } });
                                            else
                                                package.selectedFields.Add(new ETL_Package.ItemColumn() { table = getTable(textBoxTableName.Text, textBoxTableAlias.Text, cols.table.table_id), alias = textBoxColumnAlias.Text, col_name = textBoxFieldName.Text });
//                                            selectedFields.Add(cols);
                                        }
                                    }

                                }
                                foreach (var item in frm.returnedItems)
                                {
                                    if (item.itemName == "ForeignKey" || item.itemName == "VForeignKey")
                                    {
                                        package.relations.Add(new ETL_Package.ItemRelation() { relationID = item.itemId, relationName = item.itemName, table1 = package.selectedFields.Where(ii => ii.table.table_name == item.additionalInfo[0]).Last().table, table2 = package.selectedFields.Where(ii => ii.table.table_name == item.additionalInfo[1]).Last().table });
                                    }
                                }
                                RefreshListViewTablesSelected();
                                RefreshListViewLinksSelected();
                            }
                        }
                        catch (Exception e77)
                        {
                            MessageBox.Show(e77.Message);
                        }
                    }
                }
            }
        }
        private async Task<string> GetNodeName(Int64 id)
        {
            var command = "select name from md_Node where nodeid=@id";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        return reader.GetString(0);
                    }
                }
            }
            return "";
        }

        private async void listViewSelectedField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listViewSelectedField.SelectedIndices.Count > 0)
            {
                int index = listViewSelectedField.SelectedIndices[0];
                var id = package.selectedFields[index].table.table_id;
                buttonEditField.Enabled = true;
                textBoxFieldName.Text = package.selectedFields[index].col_name;
                textBoxColumnAlias.Text = package.selectedFields[index].alias;
                textBoxTableName.Text = package.selectedFields[index].table.table_name;
                textBoxTableAlias.Text = package.selectedFields[index].table.alias;
                await fillTableInfo(package.selectedFields[index].table);
            }
        }


        ETL_Package.ItemTable selectedTable = null;

        private async Task fillTableInfo( ETL_Package.ItemTable table)
        {
            buttonAddCondition.Enabled = true;
            selectedTable = table;
            await tableViewControl1.setContent(table.table_id, conn);
            textBoxTableAdditional.Text = table.table_name;
        }



        private void listViewSelectedField_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listViewSelectedField.SelectedIndices.Count > 0)
            {
                int index= listViewSelectedField.SelectedIndices[0];




                package.selectedFields.RemoveAt(index);
                RefreshListViewTablesSelected();
                RefreshListViewLinksSelected();

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cols = comboBox1.SelectedItem as ETL_Package.ItemColumn;
            if(cols != null)
            {
//                fillTableInfo(cols.table.table_id, cols.table.table_name);
                tableViewControl1.setContent(cols.table.table_id, conn);
                textBoxFieldName.Text = cols.col_name;
                textBoxColumnAlias.Text = cols.col_name;
                textBoxTableName.Text = cols.table.table_name;
                textBoxTableAlias.Text= cols.table.alias;
            }

        }

        private void buttonAddCondition_Click(object sender, EventArgs e)
        {
            package.conditions.Add(new ETL_Package.ItemAddCondition() {  table=selectedTable, condition=textBoxCondition.Text});
            RefreshListViewCondition();
        }

        private void listViewAddCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAddCondition.SelectedIndices.Count > 0)
            {
                int index = listViewAddCondition.SelectedIndices[0];
                buttonEditCondition.Enabled = true;
                textBoxCondition.Text = package.conditions[index].condition;
                textBoxTableAdditional.Text= package.conditions[index].table.table_name;
            }

        }

        private void listViewAddCondition_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewAddCondition.SelectedIndices.Count > 0)
            {
                int index = listViewAddCondition.SelectedIndices[0];




                 package.conditions.RemoveAt(index);
                RefreshListViewCondition();

            }

        }
        
        public class ETL_Package
        {
            public string ETLName = "";
            public string ETLDescription;
            public int ETL_dest_id;
            public string TableOutputName;
            public long idPackage = -1;
            public string ETL_add_par = "";
            public class VariableItem
            {
                public string Name;
                public string Description;
                public string VariableType;
                public string VariableDefaultValue;

            }

            public List<VariableItem> variables = new List<VariableItem>();
            public class ItemTable
            {
                public string table_name;
                public string scema;
                public long table_id;
                public string alias = "";
                public long etl_id;
                public override string ToString()
                {
                    if (alias != "")
                        return $"{table_name}({alias}) -{scema}";
                    else
                        return $"{table_name}-{scema}";
                }
            }
            public class ItemColumn
            {
                public string col_name;
                public string alias;
                public long col_id;
                public ItemTable table;

                public override string ToString()
                {
                    return $"{col_name}:{table}";

                }
            }

            public List<ItemTable> allTables = new List<ItemTable>();
            public class ItemRelation
            {
                public ItemTable table1;
                public ItemTable table2;
                public long relationID;
                public string relationName;
                public override string ToString()
                {
                    return $"{relationName}-{table1.table_name}:{table2.table_name}";
                }
            }
            public class ItemAddCondition
            {
                public ItemTable table;
                public string condition;
            }
            public List<ItemAddCondition> conditions = new List<ItemAddCondition>();

            public List<ItemRelation> relations = new List<ItemRelation>();


            public List<ItemColumn> selectedFields = new List<ItemColumn>();

        }

        ETL_Package package= new ETL_Package();
        private async void button3_Click(object sender, EventArgs e)
        {
            if(package.ETLName == "")
            {
                MessageBox.Show(" Не заполнены параметры ETL пакета");
                return;

            }
            try
            {
                await using (var cmd = new NpgsqlCommand(@"select * from md_add_etl_package(5,@id,@title,@output_name,@description,@dest_id,@add_par)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", package.idPackage);
                    cmd.Parameters.AddWithValue("@title", package.ETLName);
                    cmd.Parameters.AddWithValue("@output_name", package.TableOutputName);
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


                foreach(var item in package.variables)
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

                string lastKey = "";
                ETL_Package.ItemColumn lastItem = null;
                foreach(var item in package.selectedFields.OrderBy(ii=>ii.table.table_name+ii.table.alias))
                {
                    if(item.table.table_name + item.table.alias !=lastKey)
                    {
                        lastKey = item.table.table_name + item.table.alias;
                        await SaveItem(package.idPackage, selectList, lastItem);
                        selectList = "";

                    }
                    selectList += ((selectList=="")?"":",")+ item.col_name + " " + item.alias;
                    lastItem = item;
                }
                await SaveItem(package.idPackage, selectList, lastItem);


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
                await GenerateStatement.Generate(conn, package.idPackage);
            }
            catch(Exception e88)
            {
                MessageBox.Show(e88.Message);
            }
        }

        private async Task SaveItem(long idPackage, string selectList, ETL_Package.ItemColumn lastItem)
        {
            if (lastItem != null)
            {
                await using (var cmd = new NpgsqlCommand(@"select * from ccfa_addetltable(5,@etlid,@tableid,@alias,@select_list)", conn))
                {
                    cmd.Parameters.AddWithValue("@etlid", idPackage);
                    cmd.Parameters.AddWithValue("@tableid", lastItem.table.table_id);
                    cmd.Parameters.AddWithValue("@alias", lastItem.table.alias);
                    cmd.Parameters.AddWithValue("@select_list", selectList);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lastItem.table.etl_id = reader.GetInt64(0);
                        }
                    }
                }
            }
        }

        
        void RefreshVariableList()
        {
            listViewVariableList.Items.Clear();
            foreach( var item in package.variables)
            {
                listViewVariableList.Items.Add( new ListViewItem(new String[] { item.Name,item.VariableType,item.VariableDefaultValue, item.Description } ) );
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormAddVariable frm = new FormAddVariable();
            if(frm.ShowDialog() == DialogResult.OK)
            {
                package.variables.Add(new ETL_Package.VariableItem() { Name = frm.VariableName, Description = frm.VariableDescription, VariableDefaultValue=frm.VariableDefaultValue, VariableType=frm.VariableType });
                RefreshVariableList();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormDefineETL frm = new FormDefineETL(conn);
            frm.ETLName= package.ETLName;
             frm.OutputTableName= package.TableOutputName;
             frm.ETLDescription= package.ETLDescription;
            frm.ETL_dest_id= package.ETL_dest_id;
            frm.ETLAddPar = package.ETL_add_par;

            if ( frm.ShowDialog() == DialogResult.OK )
            {
                package.ETLName = frm.ETLName;
                package.TableOutputName = frm.OutputTableName;
                package.ETLDescription = frm.ETLDescription;
                package.ETL_dest_id = frm.ETL_dest_id;
                package.ETL_add_par = frm.ETLAddPar;
                textBoxEtlDescr.Text = $"Etl:{package.ETLName} outFile:{package.TableOutputName}";
            }
        }


        bool isBusy = false;
        private async  void comboBoxPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxPackage.SelectedItem != null)
            {
                if (isBusy)
                    return;
                isBusy = true;
                var pack = comboBoxPackage.SelectedItem as ItemPackage;
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
                                if(!reader.IsDBNull(1))
                                package.TableOutputName = reader.GetString(1);
                                if (!reader.IsDBNull(2))
                                    package.ETLDescription = reader.GetString(2);
                                if(reader.FieldCount>3 && !reader.IsDBNull(3))
                                package.ETL_dest_id =Convert.ToInt32( reader.GetString(3));
                                package.ETL_add_par = "";
                                if(!reader.IsDBNull(4))
                                package.ETL_add_par = reader.GetString(4);

                                textBoxEtlDescr.Text = $"Etl:{package.ETLName} outFile:{package.TableOutputName}";

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
                                package.variables.Add(new ETL_Package.VariableItem() { Name = reader.GetString(0), Description = reader.GetString(1), VariableType=reader.GetString(2), VariableDefaultValue=reader.GetString(3) });

                            }
                        }
                        RefreshVariableList();
                    }

                }

                {
                    await using (var cmd = new NpgsqlCommand(@"select t1.name,at1.val alias1, at2.val selectList1, att1.toid idTable, t1.nodeid idetlTable,f.val as condition from md_node t1 
 inner join md_arc a1 on (a1.fromid = @id and t1.nodeid = a1.toid)
     inner join md_arc att1 on(att1.fromid = t1.nodeid)
     left join md_node_attr_val at1 on(t1.nodeid = at1.nodeid and at1.attrid = 39)
     left join md_node_attr_val at2 on(t1.nodeid = at2.nodeid and at2.attrid = 41)
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
                                if(!reader.IsDBNull(1))
                                    alias = reader.GetString(1);
                                var etl_id = reader.GetInt64(4);
                                ETL_Package.ItemTable table = package.allTables.FirstOrDefault(ii => ii.etl_id == etl_id);
                                if (table == null)
                                {
                                    table = new ETL_Package.ItemTable() { alias = alias, table_name = table_name, etl_id = etl_id, table_id = table_id };
                                    package.allTables.Add(table);
                                }
                                var selectList = reader.GetString(2);
                                foreach (var it in selectList.Split(","))
                                {
                                    GenerateStatement.ItemTable.SelectListItem selectItem = new GenerateStatement.ItemTable.SelectListItem(it);

                                    package.selectedFields.Add(new ETL_Package.ItemColumn()
                                    { table = table, col_name = selectItem.expression, alias = selectItem.alias });
                                }
                                if (!reader.IsDBNull(5))
                                {
                                    package.conditions.Add(new ETL_Package.ItemAddCondition() { condition = reader.GetString(5), table=table });
                                }

                                //                                variables.Add(new VariableItem() { Name = reader.GetString(0), Description = reader.GetString(1) });

                            }
                        }
                        RefreshListViewTablesSelected();
                        RefreshListViewCondition();
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
                                var fk_name=reader.GetString(0);
                                var fk_id = reader.GetInt64(1);
                                var table_id1 = reader.GetInt64(2);
                                var table_id2 = reader.GetInt64(3);
                                if(package.relations.Count(ii=>ii.relationID == fk_id) == 0)
                                {
                                    package.relations.Add( new ETL_Package.ItemRelation() { relationID = fk_id, relationName = fk_name,table1= package.allTables.First(ii=>ii.etl_id== table_id1), table2 = package.allTables.First(ii => ii.etl_id == table_id2) });
                                }


                            }
                        }
                        RefreshListViewLinksSelected();
                    }
                }
                var package1=await GenerateStatement.getPackage(conn, package.idPackage);

                pictureBox1.Image= GraphvizTest.drawContent(package1);
                isBusy = false;
            }
        }

        private void buttonEditCondition_Click(object sender, EventArgs e)
        {
            if (listViewAddCondition.SelectedIndices.Count > 0)
            {
                int index = listViewAddCondition.SelectedIndices[0];
                //buttonEditCondition.Enabled = true;
                package.conditions[index].condition = textBoxCondition.Text;
                RefreshListViewCondition(); 
            }

        }

        private void buttonEditField_Click(object sender, EventArgs e)
        {
            if (listViewSelectedField.SelectedIndices.Count > 0)
            {
                int index = listViewSelectedField.SelectedIndices[0];
                package.selectedFields[index] = new ETL_Package.ItemColumn() { table = getTable(textBoxTableName.Text, textBoxTableAlias.Text, package.selectedFields[index].table.table_id), alias = textBoxColumnAlias.Text, col_name = textBoxFieldName.Text };
                RefreshListViewTablesSelected();
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(listViewVariableList.SelectedIndices.Count>0)
            {
                int index = listViewVariableList.SelectedIndices[0];
                FormAddVariable frm = new FormAddVariable();
                frm.VariableName = package.variables[index].Name;
                frm.VariableDescription = package.variables[index].Description;
                frm.VariableType = package.variables[index].VariableType;
                frm.VariableDefaultValue = package.variables[index].VariableDefaultValue;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    package.variables[index]=new ETL_Package.VariableItem() { Name = frm.VariableName, Description = frm.VariableDescription, VariableDefaultValue=frm.VariableDefaultValue, VariableType=frm.VariableType };
                    RefreshVariableList();
                }
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            await using (var cmd = new NpgsqlCommand(@"select * from ccfa_delete_etl_package(@etlid)", conn))
            {
                cmd.Parameters.AddWithValue("@etlid", package.idPackage);
                cmd.ExecuteNonQuery();
            }
            await RedrawAllPackages();          
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormExploreFK form = new FormExploreFK(conn);
            form.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FormAddCustomTables frm = new FormAddCustomTables(conn);
            frm.ShowDialog();
        }
    }
}