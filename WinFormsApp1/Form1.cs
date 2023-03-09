using BlazorAppCreateETL.Shared;
using CamundaInterface;
using ETL_DB_Interface;
using MaxMind.Db;
using Microsoft.Web.WebView2.Core;
using Npgsql;
using System;
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

 
        Task runner;
        private async void Form1_Load(object sender, EventArgs e)

        {
            string body;
            using(StreamReader sr= new StreamReader(@"Data\example.txt"))
            {
                body = sr.ReadToEnd();
            }
//            Path path = new Path(@"HTML\testInteractive.html");

           // this.webView21.Source = new Uri(Path.GetFullPath(@"HTML/testInteractive.html"));
//            this.pictureBox1.Image = GraphvizTest.toGraphviz(body);
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
            List<ItemPackage> packages;
            packages = await DBInterface.GetPackagesItems(conn);

            comboBoxPackage.Items.AddRange(packages.ToArray());
        }

 
        private void TableViewControl1_OnTableDoubleClicked(TableViewControl.ItemColumn column)
        {
            textBoxColumnAlias.Text = textBoxFieldName.Text = column.Name;
            
//            throw new NotImplementedException();
        }

        public ETL_Package.ItemTable getTable(out bool isNew,string Name, string Alias, long id)
        {
            isNew = package.allTables.Count(ii => ii.table_name == Name && ii.alias == Alias)==0;
            return getTable(Name, Alias, id);
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
                listViewSelectedField.Items.Add(new ListViewItem(new String[] { field.sourceColumn.col_name,field.sourceColumn.alias, field.sourceColumn.table.table_name, field.sourceColumn.table.alias,field.outputTable }));
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
            var findString = textBox1.Text;
            List<ETL_Package.ItemColumn> list = await DBInterface.GetColumnsForPattern(conn,findString);
            comboBox1.Items.AddRange(list.ToArray());
        }

  
        private async void button2_Click(object sender, EventArgs e)
        {
            var cols = comboBox1.SelectedItem as ETL_Package.ItemColumn;
            bool isNew;
            var table = getTable(out isNew, textBoxTableName.Text, textBoxTableAlias.Text, cols.table.table_id);
            await AddCols(cols, new ETL_Package.ItemSelectedList() { sourceColumn = new ETL_Package.ItemColumn() { table = table, alias = textBoxColumnAlias.Text, col_name = textBoxFieldName.Text }, outputTable = package.OutputTables.First() },isNew,package.allTables.Last().table_name);
        }

        private async Task AddCols(ETL_Package.ItemColumn? cols, ETL_Package.ItemSelectedList newItem,bool isNew,string newTableName)
        {
            if (cols != null)
            {
                if (package.selectedFields.Count < 1)
                {
                    //add new table and new column
                    package.selectedFields.Add(newItem);
                    RefreshListViewTablesSelected();
                }
                else
                {
                    if (!isNew && package.allTables.Count(ii => ii.table_name ==cols.table.table_name && ii.alias == cols.table.alias) > 0)
                    {
                        //add column in existing table
                        package.selectedFields.Add(newItem);
                        RefreshListViewTablesSelected();

                    }
                    else
                    {
                        try
                        {
                            await AddNewTableRel(cols,newTableName);
                        }
                        catch (Exception e77)
                        {
                            MessageBox.Show(e77.Message);
                        }
                    }
                }
            }
        }

        private async Task AddNewTableRel(ETL_Package.ItemColumn? cols,string newTableName)
        {
            FormAddTable frm = new FormAddTable(package.allTables.Select(ii=>ii.table_id).ToArray(),package.selectedFields.First().sourceColumn.table.table_id, cols.table.table_id, conn);
            if (frm.ShowDialog() == DialogResult.OK)
            {
               // package.allTables.Add(cols.table);

                foreach (var item in frm.returnedItems)
                {
                    if (item.itemName == "Table")
                    {
                        if (package.selectedFields.Count(ii => ii.sourceColumn.table.table_id == item.itemId) > 0)
                        {
                            // add rel in dublicate table
                            var alias = ((char)('a' + package.selectedFields.Count)).ToString();
                            package.selectedFields.Add(new ETL_Package.ItemSelectedList()
                            {
                                sourceColumn ={ col_id = cols.col_id,alias=textBoxColumnAlias.Text,
                                                col_name = cols.col_name,
                                                table =getTable(cols.table.table_name,alias,cols.table.table_id)/* new  ETL_Package.ItemTable() { alias = alias, table_id = cols.table.table_id, table_name = cols.table.table_name } }*/
                                }
                            });
                        }
                        else
                        {
                            if (cols.table.table_id != item.itemId)
                                // add relation
                                package.selectedFields.Add(new ETL_Package.ItemSelectedList()
                                { sourceColumn = new ETL_Package.ItemColumn() { col_id = -1, col_name = "", table =getTable(await GetNodeName(item.itemId),"", item.itemId)/* new ETL_Package.ItemTable() { alias = "", table_id = item.itemId, table_name = await GetNodeName(item.itemId) }*/ } });
                            else
                                //add new fileld in new table
                                package.selectedFields.Add(new ETL_Package.ItemSelectedList()
                                {
                                    sourceColumn =new ETL_Package.ItemColumn() { table = getTable(cols.table.table_name, cols.table.alias, cols.table.table_id), alias =cols.alias/* textBoxColumnAlias.Text*/, col_name =cols.col_name/* textBoxFieldName.Text*/ }
                                    ,
                                    outputTable = newTableName
                                });
                            //                                            selectedFields.Add(cols);
                        }
                    }

                }
                foreach (var item in frm.returnedItems)
                {
                    if (item.itemName == "ForeignKey" || item.itemName == "VForeignKey")
                    {
                        package.relations.Add(new ETL_Package.ItemRelation() { relationID = item.itemId, relationName = item.itemName, table1 = package.selectedFields.Where(ii => ii.sourceColumn.table.table_name == item.additionalInfo[0]).Last().sourceColumn.table, table2 = package.selectedFields.Where(ii => ii.sourceColumn.table.table_name == item.additionalInfo[1]).Last().sourceColumn.table });
                    }
                }
                RefreshListViewTablesSelected();
                RefreshListViewLinksSelected();
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
                var id = package.selectedFields[index].sourceColumn.table.table_id;
                buttonEditField.Enabled = true;
                textBoxFieldName.Text = package.selectedFields[index].sourceColumn.col_name;
                textBoxColumnAlias.Text = package.selectedFields[index].sourceColumn.alias;
                textBoxTableName.Text = package.selectedFields[index].sourceColumn.table.table_name;
                textBoxTableAlias.Text = package.selectedFields[index].sourceColumn.table.alias;
                await fillTableInfo(package.selectedFields[index].sourceColumn.table);
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
        
    
        ETL_Package package= new ETL_Package();
        private async void button3_Click(object sender, EventArgs e)
        {
//            await DBInterface.GetAllRelation(conn);
            if (package.ETLName == "")
            {
                MessageBox.Show(" Не заполнены параметры ETL пакета");
                return;

            }
            try
            {
                await DBInterface.SaveAndExecuteETL(conn,package);
            }
            catch (Exception e88)
            {
                MessageBox.Show(e88.Message);
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
        void RefreshComboBoxDestTables()
        {
            comboBoxDestTable.Items.Clear();
            comboBoxDestTable.Items.AddRange(package.OutputTables.ToArray());
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            FormDefineETL frm = new FormDefineETL(conn);
            frm.ETLName= package.ETLName;
             frm.OutputTableName= package.OutputTables.ToList();
             frm.ETLDescription= package.ETLDescription;
            frm.ETL_dest_id= package.ETL_dest_id;
            frm.ETLAddPar = package.ETL_add_par;

            if ( frm.ShowDialog() == DialogResult.OK )
            {
                package.ETLName = frm.ETLName;
                package.ETL_dest_id = frm.ETL_dest_id;
                if (frm.OutputTableName.Count > 0 && !string.IsNullOrEmpty(textBoxFromSrc.Text))
                {
                    var src_id = Convert.ToInt32(textBoxFromSrc.Text);
                    var diff = frm.OutputTableName.Except(package.OutputTables);// Intersect
                    package.OutputTables=frm.OutputTableName.ToList();
                    foreach(var item in diff ) 
                    {
                        var items=await DBInterface.getSrcForTable(conn, item, package.ETL_dest_id,src_id);
                        string lastTableName = "";
                        foreach (var it1 in items)
                        {
                            //if (lastTableName != it1.table_name)
                            {
                                lastTableName = it1.table_name;
                                bool isFirst = true;
                                foreach (var cols in it1.columns)
                                {
                                    bool isNew;
                                    var table=getTable(out isNew,it1.table_name, "", it1.table_id);
                                    var col = new ETL_Package.ItemColumn() { col_id = cols.source_col_id, alias = cols.source_col_name, col_name = cols.source_col_name, table = table /*new ETL_Package.ItemTable() { table_name = it1.table_name, alias = "", src_id = src_id, table_id = it1.table_id }*/ };
                                    table.src_id = src_id;
//                                    cols.source_col_id
                                    await AddCols(col, new ETL_Package.ItemSelectedList() { outputTable=it1.table_name, sourceColumn= col},isNew,item);
                                    //getTable(it1.table_name, "", it1.table_id);
                                }
                            }
                        }
                    }
                }
                

                package.OutputTables=(frm.OutputTableName);
                package.ETLDescription = frm.ETLDescription;
                package.ETL_add_par = frm.ETLAddPar;
                textBoxEtlDescr.Text = $"Etl:{package.ETLName} outFile:{package.OutputTables}";
                RefreshComboBoxDestTables();
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
                package=await DBInterface.FillPackageContent(conn,pack);
                textBoxEtlDescr.Text = $"Etl:{package.ETLName} outFile:{package.OutputTables}";

                RefreshVariableList();
                RefreshListViewLinksSelected();
                RefreshListViewTablesSelected();
                RefreshListViewCondition();
                RefreshComboBoxDestTables();
                var package1 = await GenerateStatement.getPackage(conn, package.idPackage);
//                var ans = await webView21.CoreWebView2.ExecuteScriptAsync($"render('{GraphvizTest.drawContent(package1)}')");

                //    pictureBox1.Image= GraphvizTest.drawContent(package1);
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
                package.selectedFields[index] = new ETL_Package.ItemSelectedList()
                {
                    sourceColumn = { table = getTable(textBoxTableName.Text, textBoxTableAlias.Text, package.selectedFields[index].sourceColumn.table.table_id), alias = textBoxColumnAlias.Text, col_name = textBoxFieldName.Text },
                    outputTable = ((comboBoxDestTable.SelectedIndex < 0) ? package.OutputTables.First() : comboBoxDestTable.Items[comboBoxDestTable.SelectedIndex].ToString())
                }  ;
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