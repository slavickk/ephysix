/******************************************************************
 * File: FormDefineETL.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using BlazorAppCreateETL.Shared;
using ETL_DB_Interface;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class FormDefineETL : Form
    {
        NpgsqlConnection conn;
        ETL_Package package;
        public FormDefineETL(NpgsqlConnection conn1,ETL_Package package)
        {
            conn = conn1;
            this.package = package;
            InitializeComponent();
        }
        public string ETLName;
        public List<string> OutputTableName;
        public string ETLDescription;
        public int ETL_dest_id;
        public string ETLAddPar;

        private void button1_Click(object sender, EventArgs e)
        {
            ETLName = textBoxETLName.Text;
          //  OutputTableName = textBoxOutputName.Text;
            ETLDescription = textBox1.Text;
            ETL_dest_id = (comboBox1.SelectedItem as ItemSelect).id;
            ETLAddPar = textBoxAddParameter.Text;
        }

        public class ItemSelect
        {
            public int id;
            public string description;
            public override string ToString()
            {
                return description;
            }
        }

        private async void FormDefineETL_Load(object sender, EventArgs e)
        {
            int selectedIndex = 1;
            await using (var cmd = new NpgsqlCommand(@"select srcid,descr from md_src order by srcid", conn))
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int index = 0;
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetInt32(0);
                        if (id == ETL_dest_id)
                            selectedIndex = index;
                        comboBox1.Items.Add(new ItemSelect() { id = reader.GetInt32(0), description = reader.GetString(1) });
                        index++;
                    }
                }
            }
            comboBox1.SelectedIndex = selectedIndex;
            textBoxETLName.Text = ETLName;
            if (OutputTableName == null)
                OutputTableName = new List<string>();
            textBoxOutputName.Text = "";// OutputTableName;
            refreshOutputTables();
            textBox1.Text = ETLDescription;
            if (!string.IsNullOrEmpty(ETLAddPar))
                textBoxAddParameter.Text = (ETLAddPar);
            refreshAddParameter();

        }
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            refreshAddParameter((int)numericUpDown1.Value);
        }

        private void refreshAddParameter(int countKey=1)
        {
            if (this.ETL_dest_id == 11)
            {
                GenerateStatement.ItemTask.BriefDictionaryDef def = null;
                //                int countKey = 1;
                double result;
                if (!string.IsNullOrEmpty(textBoxAddParameter.Text) && !double.TryParse(textBoxAddParameter.Text, out result) ) 
                {
                    def = JsonSerializer.Deserialize<GenerateStatement.ItemTask.BriefDictionaryDef>(textBoxAddParameter.Text);
                    if (def.keys.Length != countKey)
                        def = null;
                    else
                        countKey = def.keys.Length;
                }
                if (def == null || def.keys.Length + def.otherfield.Length != package.selectedFields.Count)
                {
                    def = new GenerateStatement.ItemTask.BriefDictionaryDef();
                    var selectList = package.selectedFields.Where(ii => !string.IsNullOrEmpty(ii.sourceColumn.alias) || !string.IsNullOrEmpty(ii.sourceColumn.col_name)).Select(ii => (!string.IsNullOrEmpty(ii.sourceColumn.alias) ? ii.sourceColumn.alias : ii.sourceColumn.col_name)).ToList();
                    def.keys = selectList.GetRange(0, countKey).ToArray();
                    def.otherfield = selectList.GetRange(countKey,selectList.Count - def.keys.Length).ToArray();
                    textBoxAddParameter.Text = JsonSerializer.Serialize<GenerateStatement.ItemTask.BriefDictionaryDef>(def);
                }
                numericUpDown1.Value = countKey;    

            }
        }

        void refreshOutputTables()
        {
            comboBoxDestTables.Items.Clear();
            comboBoxDestTables.Items.AddRange(OutputTableName.ToArray());
            if (comboBoxDestTables.Items.Count > 0)
                comboBoxDestTables.SelectedIndex = 0;

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selItem = (comboBox1.SelectedItem as ItemSelect);
            if (selItem != null)
            {
                if (selItem.description == "CCFA Dictionary exporter")
                    label3.Visible = numericUpDown1.Visible = true;
                else
                    label3.Visible = numericUpDown1.Visible = false;

            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            OutputTableName.Add(textBoxOutputName.Text);
            refreshOutputTables();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {

        }
    }
}
