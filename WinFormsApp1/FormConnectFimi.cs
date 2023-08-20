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
                listBoxFimiOutputParam.Items.Clear();
                foreach(var item in comm.outputItems)
                {
                    listBoxFimiOutputParam.Items.Add(item);
                }

            }
        }

        private void listBoxFimiParam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            var table = comboBoxTable.SelectedItem as ETL_Package.ItemTable;
            if (table != null)
            {
                listBoxTableColumns.Items.Clear();

                listBoxTableColumns.Items.AddRange( list.Where(ii=>ii.table.table_name== table.table_name).ToArray());

            }
        }

        private void listBoxTableColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        List<ETL_Package.ItemColumn> list;

        bool busyText = false;
        private async void textBoxPrefix_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);
            if (!busyText)
            {
                busyText = true;
                if (!string.IsNullOrEmpty(textBoxPrefix.Text))
                {
                    list = await DBInterface.GetColumnsForTablePattern(conn, textBoxPrefix.Text);
                    comboBoxTable.Items.Clear();
                    comboBoxTable.Items.AddRange(list.Select(ii => ii.table).DistinctBy(i1 => i1.table_name).ToArray());



                }
            }
        }

        private void buttonLink_Click(object sender, EventArgs e)
        {

        }

        private void buttonTest_Click(object sender, EventArgs e)
        {

        }
        List<FIMIHelper.ItemCommand> def;
        private NpgsqlConnection conn;

        private void FormConnectFimi_Load(object sender, EventArgs e)
        {
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
    }
}
