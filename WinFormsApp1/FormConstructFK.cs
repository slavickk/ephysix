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
    public partial class FormConstructFK : Form
    {
        long tableId1;
        long tableId2; 
        NpgsqlConnection conn;
        public FormConstructFK(long TableId1,long TableId2,NpgsqlConnection conn1)
        {
            tableId1 = TableId1;
            tableId2 = TableId2;
            conn = conn1;
            InitializeComponent();
        }

        private async void FormConstructFK_Load(object sender, EventArgs e)
        {
            await tableViewControl1.setContent(tableId1,conn);
            await tableViewControl2.setContent(tableId2, conn);
            tableViewControl1.OnSelectedChanged += TableViewControl1_OnSelectedChanged;
            tableViewControl2.OnSelectedChanged += TableViewControl1_OnSelectedChanged;
        }

        private void TableViewControl1_OnSelectedChanged()
        {
            if (tableViewControl1.selectedColumn != null && tableViewControl2.selectedColumn != null)
                button1.Enabled = true;
            else
                button1.Enabled = false;

        }

        public class Item
        {
            public long id1;
            public string name1;
            public long id2;
            public string name2;
        }
        List<Item> items= new List<Item>();
        private void button1_Click(object sender, EventArgs e)
        {
            items.Add(new Item() { id1=tableViewControl1.selectedColumn.Id, name1=tableViewControl1.selectedColumn.Name, id2 = tableViewControl2.selectedColumn.Id, name2 = tableViewControl2.selectedColumn.Name });

            listView1.Items.Clear();
            listView1.Items.AddRange(items.Select(ii=>new ListViewItem(new String[] {ii.name1,ii.name2})).ToArray());
        }

        public long IDRelation = 0;
        public string nameRelation;

        private async void button2_Click(object sender, EventArgs e)
        {
            var command = @"select * from ccfa_insert_newFK(@detail,2,'VForeignKey')
  ";
            
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                nameRelation = textBox1.Text;
                cmd.Parameters.AddWithValue("@detail", textBox1.Text);

                
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        IDRelation = reader.GetInt64(0);
                    }
                }


            }
            foreach (var it in items)
            {
                await AddArc(it.id1,IDRelation,2);
                await AddArc( IDRelation,it.id2, 2);
            }



        }

        private async Task AddArc(long fromid,long toid,int scema)
        {
            var command = "insert into md_Arc(fromid,toid,srcid,typeid) values(@fromid,@toid,@scema,md_get_type('VForeignKey'))";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@fromid", fromid);
                cmd.Parameters.AddWithValue("@toid", toid);
                cmd.Parameters.AddWithValue("@scema", scema);
                cmd.ExecuteNonQuery();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
