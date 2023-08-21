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

        private void FormConnectFimi_Load(object sender, EventArgs e)
        {
            textBoxSQL.Text = "select '2220000000000200' PAN";
            conn = new NpgsqlConnection(GenerateStatement.ConnectionStringAdm);
            conn.Open();

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

        private void buttonFullTest_Click(object sender, EventArgs e)
        {

        }
    }
}
