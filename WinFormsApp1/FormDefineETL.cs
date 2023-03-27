using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class FormDefineETL : Form
    {
        NpgsqlConnection conn;
        public FormDefineETL(NpgsqlConnection conn1)
        {
            conn = conn1;
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
            ETLAddPar = numericUpDown1.Value.ToString();
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
                        comboBox1.Items.Add( new ItemSelect () { id=reader.GetInt32(0), description=reader.GetString(1) });
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
            textBox1.Text=ETLDescription;
            if(!string.IsNullOrEmpty(ETLAddPar))
                textBoxAddPar.Text =(ETLAddPar);   
         
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
