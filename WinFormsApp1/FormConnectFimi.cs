using CamundaInterface;
using BlazorAppCreateETL.Shared;
using ETL_DB_Interface;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Packaging;
using WinFormsApp1;
using static CamundaInterface.APIExecutor;
using CamundaInterfaces;
//using static ETL_DB_Interface.GenerateStatement;
using NUnit.Framework.Internal;
using static CamundaInterface.CamundaExecutor.ExternalTaskAnswer;
//using static ETL_DB_Interface.GenerateStatement.ItemTask;
using System.Text.Json;

namespace WinFormsETLPackagedCreator
{
    public partial class FormConnectFimi : Form
    {
        public FormConnectFimi()
        {
            InitializeComponent();
        }

        private void comboBoxFimiCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comm = comboBoxFimiCommand.SelectedItem as FIMIHelper.ItemCommand;
            if (comm != null)
            {
                listViewFIMIInputParams.Items.Clear();
                foreach(var item in comm.parameters)
                {
                    listViewFIMIInputParams.Items.Add(new ListViewItem(new string[] { item.name, "", "" }));
                }
//                listView
          //     this.listVie
          listViewFimiOutputParam.Items.Clear();
//                listBoxFimiOutputParam.Items.Clear();
                foreach(var item in comm.outputItems)
                {
                    listViewFimiOutputParam.Items.Add(new ListViewItem(new string[] {item.path,"" }));
                }

            }
        }

        private void listBoxFimiParam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public class ItemKeyColumns
        {
            public string table;
            public List<string> keyColumns;
        }

        List<ItemKeyColumns> listKeyColumns= new List<ItemKeyColumns>();

        public class ItemRel
        {
            public string pktable;
            public string pkcolumns;
            public string fktable;
            public string fkcolumn;
        }
        List<ItemRel> relations= new List<ItemRel>();
        List<ETL_Package.ItemColumn> listAllColumns = new List<ETL_Package.ItemColumn>();
        private async void comboBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            var table = comboBoxTable.SelectedItem as ETL_Package.ItemTable;
            if (table != null)
            {
/*                ETL_Package.ItemColumn[] arr = new ETL_Package.ItemColumn[listBoxTableColumns.Items.Count];
                listBoxTableColumns.Items.CopyTo(arr, 0);*/
                var arr1=listAllColumns.Select(ii=>ii.table).DistinctBy(i1=>i1.table_name).Select(ii=>ii.table_id).ToArray();
                if (arr1.Length > 0)
                {
                    FormAddTable frm = new FormAddTable(arr1,arr1.First() , table.table_id, conn);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // package.allTables.Add(cols.table);
                        var rr=frm.fromLeftToRight;
                        foreach (var item in frm.returnedItems.Where(ii=>ii.itemName=="ForeignKey"))
                        {

                            await using (var cmdCommand = new NpgsqlCommand("select pktable,pkcolumn,fktable,fkcolumn from md_get_fk_info(@idrel)", conn))
                            {
                                cmdCommand.Parameters.AddWithValue("@idrel", item.itemId);
                                await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                                {
                                    while (await readerCom.ReadAsync())
                                    {
                                        relations.Add(new ItemRel() { pktable = readerCom.GetString(0), pkcolumns = readerCom.GetString(1), fktable = readerCom.GetString(2), fkcolumn = readerCom.GetString(3) });


                                        //                            "select pktable,pkcolumn,fktable,fkcolumn from md_get_fk_info(2163938)"
                                        /*                          if (item.itemName == "Table")
                                                      {
                                                      }*/
                                    }
                                }
                            }
                        }
                    }
                }
                FormSelectKeyColumns frm1 = new FormSelectKeyColumns(list.Where(ii => ii.table.table_name == table.table_name).Select(ii => ii.col_name).ToArray());
                if (frm1.ShowDialog() == DialogResult.OK)
                {
                    listKeyColumns.Add(new ItemKeyColumns() { table = table.table_name, keyColumns = frm1.keyColumns });
                    /*                    var keyColumns = frm1.keyColumns;
                                        listBoxTableColumns.Items.Clear();*/
                    var ll = list.Where(ii => ii.table.table_name == table.table_name).ToArray();
                    listAllColumns.AddRange(ll);
                    listViewTableColumns.Items.AddRange(ll.Select(ii => new ListViewItem(new string[] { "", "", "", ii.col_name, ii.table.table_name })).ToArray());
                    foreach(var key in frm1.keyColumns)
                    {
                        var index= listAllColumns.IndexOf(listAllColumns.First(ii=>ii.col_name== key && ii.table.table_name==table.table_name));
                        listViewTableColumns.Items[index].ForeColor= Color.Red;
                    }

//                    listBoxTableColumns.Items.AddRange(list.Where(ii => ii.table.table_name == table.table_name).ToArray());
                }

            }
        }

        private void listBoxTableColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        List<ETL_Package.ItemColumn> list;

        bool busyText = false;
        private async void textBoxPrefix_TextChanged(object sender, EventArgs e)
        {
/*            await Task.Delay(300);
            if (!busyText)
            {
                busyText = true;
                if (!string.IsNullOrEmpty(textBoxPrefix.Text))
                {
                    list = await DBInterface.GetColumnsForTablePattern(conn, textBoxPrefix.Text);
                    comboBoxTable.Items.Clear();
                    comboBoxTable.Items.AddRange(list.Select(ii => ii.table).DistinctBy(i1 => i1.table_name).ToArray());



                }
            }*/
        }

        private void buttonLink_Click(object sender, EventArgs e)
        {
            if(listViewFimiOutputParam.SelectedItems.Count > 0 && listViewTableColumns.SelectedItems.Count>0) 
            {
                int index1 = listViewFimiOutputParam.SelectedIndices[0];
                int index2 = listViewTableColumns.SelectedIndices[0];
                if (!string.IsNullOrEmpty(listViewFimiOutputParam.Items[index1].SubItems[0].Text))
                {
                    listViewTableColumns.Items[index2].SubItems[0].Text = listViewFimiOutputParam.Items[index1].SubItems[0].Text;
                }
            }

        }

        private async void buttonTest_Click(object sender, EventArgs e)
        {
            APIExecutor.ExecContextItem[] commands = new APIExecutor.ExecContextItem[1];
            commands[0] = new APIExecutor.ExecContextItem();
            commands[0].Command = (comboBoxFimiCommand.SelectedItem as FIMIHelper.ItemCommand).Name;
            commands[0].Params = new List<APIExecutor.ExecContextItem.ItemParam>();
            for (int i = 0; i < listViewFIMIInputParams.Items.Count; i++)
            {
                var subitems = listViewFIMIInputParams.Items[i].SubItems;
                if (subitems[1].Text.Length > 0 || subitems[2].Text.Length > 0)
                {
                    commands[0].Params.Add(new APIExecutor.ExecContextItem.ItemParam() { Key = subitems[0].Text, Value = subitems[2].Text, Variable = subitems[1].Text });
                }
            }
            var trans = new FimiXmlTransport();
            var ans = await APIExecutor.ExecuteApiRequestOnly(trans, commands);
            if (ans == null)
            {
                MessageBox.Show(trans.getError().Reason.Text, "Error");
                return;
            }
            try
            {
                for (int i = 0; i < listViewFimiOutputParam.Items.Count; i++)
                {
                    var subitems = listViewFimiOutputParam.Items[i].SubItems;
                    subitems[1].Text = string.Join(',', ans.filter(subitems[0].Text));
                }
            }
            catch (Exception ex)
            {
            }
        }
            
            List<FIMIHelper.ItemCommand> def;
        private NpgsqlConnection conn;

        private async void FormConnectFimi_Load(object sender, EventArgs e)
        {
           // DBInterface.SaveAndExecuteETL(conn, pack);
            textBoxSQL.Text = "select '2220000000000200' PAN";
            conn = new NpgsqlConnection(GenerateStatement.ConnectionStringAdm);
            conn.Open();
            GenerateStatement.ETL_Package pack = null;
            using (StreamReader sr = new StreamReader(@"c:\d\packageTemp.json"))
            {
                var content = sr.ReadToEnd();
                pack = JsonSerializer.Deserialize<GenerateStatement.ETL_Package>(content);
                foreach(var rel in pack.relations)
                {
                    rel.table1 = pack.allTables.First(ii => ii.etl_id == rel.table1.etl_id);
                    rel.table2 = pack.allTables.First(ii => ii.etl_id == rel.table2.etl_id);
                }

            }
            await DBInterface.SavePackage(conn, pack);

            def = FIMIHelper.getDefine();
            foreach(var item in def)
            {
                comboBoxFimiCommand.Items.Add(item);
            }
        }

        private async void textBoxSQL_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxSQL.Text))
            {
                comboBoxSqlColumn.Items.Clear();
                await using (var cmdCommand = new NpgsqlCommand(textBoxSQL.Text, conn))
                {
                    await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                    {
                        while (await readerCom.ReadAsync())
                        {
                            // Initialize commands variables
                            for (int i = 0; i < readerCom.FieldCount; i++)
                            {
                                comboBoxSqlColumn.Items.Add(readerCom.GetName(i));
                            }
                            return;
                        }
                    }
                }
            }
        }

        private void textBoxCommandPrefix_Leave(object sender, EventArgs e)
        {
            comboBoxFimiCommand.Items.Clear();
            foreach (var item in def.Where(ii=>ii.Name.Contains(textBoxCommandPrefix.Text)))
            {
                comboBoxFimiCommand.Items.Add(item);
            }

        }

        private void buttonAddConst_Click(object sender, EventArgs e)
        {
            if(textBoxConstant.Text.Length > 0 && listViewFIMIInputParams.SelectedIndices.Count>0)
            {
                int index= listViewFIMIInputParams.SelectedIndices[0];
                listViewFIMIInputParams.Items[index].SubItems[2].Text=textBoxConstant.Text;
            }
        }

        private void buttonAddSQLColumn_Click(object sender, EventArgs e)
        {
            var val=comboBoxSqlColumn.SelectedItem?.ToString(); 
            if(val!= null && val.Length>0)
            {
                int index = listViewFIMIInputParams.SelectedIndices[0];
                listViewFIMIInputParams.Items[index].SubItems[1].Text = val;

            }
        }

        private async void textBoxPrefix_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxPrefix.Text))
            {
                list = await DBInterface.GetColumnsForTablePattern(conn, textBoxPrefix.Text,new int[] { 1 });
                comboBoxTable.Items.Clear();
                comboBoxTable.Items.AddRange(list.Select(ii => ii.table).DistinctBy(i1 => i1.table_name).ToArray());



            }

        }

        async Task saveToCamunda(TableDefine[] tables, APIExecutor.ExecContextItem[] commands,string SQLQuery,string connectionString)
        {
   /*         CamundaProcess process = new CamundaProcess();
            string CamundaID = $"ETL_Process{id}";



        
            int index = 999;
            List<ETL_DB_Interface.GenerateStatement.ItemTask> tasks = new List<ETL_DB_Interface.GenerateStatement.ItemTask>();
            process.ProcessID = CamundaID;
            process.ProcessName = $"{"Example"}{123}";
            process.documentation = $"{"Some example"}\r\n  Not contain input variables!";
            process.tasks.Clear();
            CamundaProcess.ExternalTask retValue = new CamundaProcess.ExternalTask();
            retValue.id = $"Id{index}";
            retValue.name = $"ETL_Task_{index}";
            retValue.topic = "LoginDB";
            retValue.parameters.Clear();
            retValue.Annotation = $"FIMI executor ";
            retValue.author = "Yury Gasnikov";
            retValue.service_location = "unknown";

            retValue.description = "Collecting data from one sources(SQL) and put to another destination( SQL too)";
            {
                retValue.noDescribe = true;

                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLTable", JsonSerializer.Serialize<List<JsonItem>>(await getJsonDefs(conn, columnList, dest_id) )));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("SQLText", sqlExec));
                retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Oper", "None"));
            }
            retValue.parameters.Add(new CamundaProcess.ExternalTask.Parameter("Signature", "569074234566666", "Parameters signature( for control package integrity)"));
            process.tasks.Add(retValue);

            var path1 = $"{GenerateStatement.pathToSaveETL}{111}.bpmn";
            process.save(path1);
            */
        }

        private void buttonAddColumnToTable_Click(object sender, EventArgs e)
        {
            if (comboBoxSqlColumn.SelectedItem != null &&   listViewTableColumns.SelectedItems.Count > 0)
            {

//                int index1 = listViewFimiOutputParam.SelectedIndices[0];
                int index2 = listViewTableColumns.SelectedIndices[0];
//                if (!string.IsNullOrEmpty(listViewFimiOutputParam.Items[index1].SubItems[0].Text))
                {
                    listViewTableColumns.Items[index2].SubItems[2].Text = comboBoxSqlColumn.SelectedItem.ToString();
                }
            }

        }

        private void buttonAddConstantToTable_Click(object sender, EventArgs e)
        {
            if (textBoxConstant.Text.Length>0 && listViewTableColumns.SelectedItems.Count > 0)
            {

                //                int index1 = listViewFimiOutputParam.SelectedIndices[0];
                int index2 = listViewTableColumns.SelectedIndices[0];
                //                if (!string.IsNullOrEmpty(listViewFimiOutputParam.Items[index1].SubItems[0].Text))
                {
                    listViewTableColumns.Items[index2].SubItems[1].Text = textBoxConstant.Text;
                }
            }

        }

        private void listViewTableColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void buttonFullTest_Click(object sender, EventArgs e)
        {
            try
            {
                TableDefine[] tables = listAllColumns.Select(ii => ii.table.table_name).Distinct().Select(i1 => new TableDefine() { Table = i1, Columns = new List<TableDefine.Column>() }).ToArray();
                foreach (ListViewItem item in listViewTableColumns.Items)
                {
                    if (item.SubItems[1].Text.Length > 0 || item.SubItems[0].Text.Length > 0 || item.SubItems[2].Text.Length > 0 || relations.Count(ii=>ii.pktable == item.SubItems[4].Text && ii.pkcolumns==item.SubItems[3].Text)> 0 || relations.Count(ii => ii.fktable == item.SubItems[4].Text && ii.fkcolumn == item.SubItems[3].Text) > 0)
                    {
                        var col= listAllColumns.First(ii=>ii.table.table_name== item.SubItems[4].Text && ii.col_name == item.SubItems[3].Text);
                        string type_col="";
                        var cmd = @"select  n2.Name,a2.key,nav3.val,n2.synonym,md.md_find_sensitive(n2.nodeid)
    from md_node n1, md_arc a1, 
md_node n2 
left join md_node_attr_val nav3 on(n2.NodeID=nav3.NodeID and nav3.AttrID=29 )
left join md_node_attr_val sens on(n2.NodeID=sens.NodeID and sens.AttrID=md_get_attr('Addon','SensData') )


, md_node_attr_val nav2,
    md_attr a2
    where n1.typeid=md_get_type('Table') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and n2.typeid=md_get_type('Column')
      and n2.NodeID=nav2.NodeID
      and nav2.AttrID=a2.AttrID
      and a2.attrPID=1
      and n2.nodeid=@id and n1.isdeleted=false and n2.isdeleted=false  and a1.isdeleted=false";
                    await using (var cmdCommand = new NpgsqlCommand(cmd, conn))
                        {
                            cmdCommand.Parameters.AddWithValue("@id", col.col_id);
                            await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                            {
                                while (await readerCom.ReadAsync())
                                {
                                    type_col = readerCom.GetString(1);
                                    break;


                                    //                            "select pktable,pkcolumn,fktable,fkcolumn from md_get_fk_info(2163938)"
                                    /*                          if (item.itemName == "Table")
                                                  {
                                                  }*/
                                }
                            }
                        }
                        tables.First(ii => ii.Table == item.SubItems[4].Text).Columns.Add(new TableDefine.Column() { Type=type_col, Name = item.SubItems[3].Text, constant = item.SubItems[1].Text, path = item.SubItems[0].Text, variable = item.SubItems[2].Text });
                    }
                    else
                    {
                        if (item.ForeColor == Color.Red)
                        {
                            MessageBox.Show($"Column {item.SubItems[3].Text} on table {item.SubItems[4].Text} must have source");
                            return;
                        }
                    }
                }
                foreach (var table in tables)
                {
                    table.KeyColumns = this.listKeyColumns.First(ii => ii.table == table.Table).keyColumns.ToArray();

                    //var t=this.relations.Where(ii => ii.fktable == table.Table).Select(i1 => new TableDefine.ExtID() { Column = i1.pkcolumns, Table = i1.pktable }).ToArray();
                    table.ExtIDs = this.relations.Where(ii => ii.fktable == table.Table).Select(i1 => new TableDefine.ExtID() { Column = i1.pkcolumns, Table = i1.pktable }).ToList();
                }
                APIExecutor.ExecContextItem[] commands = new APIExecutor.ExecContextItem[1];
                commands[0] = new APIExecutor.ExecContextItem();
                commands[0].Command = (comboBoxFimiCommand.SelectedItem as FIMIHelper.ItemCommand).Name;
                commands[0].Params = new List<APIExecutor.ExecContextItem.ItemParam>();
                for (int i = 0; i < listViewFIMIInputParams.Items.Count; i++)
                {
                    var subitems = listViewFIMIInputParams.Items[i].SubItems;
                    if (subitems[1].Text.Length > 0 || subitems[2].Text.Length > 0)
                    {
                        commands[0].Params.Add(new APIExecutor.ExecContextItem.ItemParam() { Key = subitems[0].Text, Value = subitems[2].Text, Variable = subitems[1].Text });
                    }
                }
                await saveToCamunda(tables, commands, textBoxSQL.Text, "User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;");
                var trans = new FimiXmlTransport();
                var ans1=await new APIExecutor().ExecuteApiRequest(trans, commands,tables,textBoxSQL.Text, "User ID=dm;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;");
                if(!ans1)
                {
                    MessageBox.Show(trans.getError().Reason.Text, "Error");
                    return;

                }
                /*                var ans = await APIExecutor.ExecuteApiRequestOnly(trans, commands);
                                if (ans == null)
                                {
                                    MessageBox.Show(trans.getError().Reason.Text, "Error");
                                    return;
                                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }
    }
}
