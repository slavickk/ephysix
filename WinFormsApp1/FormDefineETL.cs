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
        public string OutputTableName;
        public string ETLDescription;
        public int ETL_dest_id;

        private void button1_Click(object sender, EventArgs e)
        {
            ETLName = textBoxETLName.Text;
            OutputTableName = textBoxOutputName.Text;
            ETLDescription = textBox1.Text;
            ETL_dest_id = (comboBox1.SelectedItem as ItemSelect).id;
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
            textBoxOutputName.Text = OutputTableName;
            textBox1.Text=ETLDescription;
         
        }
    }
}
