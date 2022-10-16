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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp1
{
    public partial class FormExploreFK : Form
    {
        long tableId1;
        long tableId2; 
        NpgsqlConnection conn;
        public FormExploreFK(NpgsqlConnection conn1)
        {
            /*tableId1 = TableId1;
            tableId2 = TableId2;*/
            conn = conn1;
            InitializeComponent();
        }

        private async void FormConstructFK_Load(object sender, EventArgs e)
        {
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
            items.Add(new Item() { id1 = tableViewControl1.selectedColumn.Id, name1 = tableViewControl1.selectedColumn.Name, id2 = tableViewControl2.selectedColumn.Id, name2 = tableViewControl2.selectedColumn.Name });

            RefreshRelMembers();
        }

        private void RefreshRelMembers()
        {
            listView1.Items.Clear();
            listView1.Items.AddRange(items.Select(ii => new ListViewItem(new String[] { ii.name1, ii.name2 })).ToArray());
        }

        public long IDRelation = 0;
        public string nameRelation;

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {

                var command = @"select * from md_insert_newFK(@detail,2,'VForeignKey')
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
                    await AddArc(it.id1, IDRelation, 2);
                    await AddArc(IDRelation, it.id2, 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        public class ItemTable
        {
            public string table_name;
            public long table_id;
            public string alias = "";
            public long etl_id;
            public override string ToString()
            {
                if (alias != "")
                    return $"{table_name}({alias})";
                else
                    return $"{table_name}";
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
                if (table.alias != "")
                    return $"{col_name}:{table.table_name}({table.alias})";
                else
                    return $"{col_name}:{table.table_name}";

            }
        }

        private async void buttonFind_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            await using (var cmd = new NpgsqlCommand(@"select distinct nodeid tableid,name tablename from md_node n1 
inner join md_arc a1 on (a1.isdeleted = false and ( a1.toid=n1.nodeid /*and ((n1.srcid=5 and a1.typeid=9) or a1.typeid=15)*/ ) )
and n1.name like '%" + textBox2.Text + "%' and n1.isdeleted=false", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    comboBox1.Items.Add(new ItemTable() { table_id = reader.GetInt64(0),table_name = reader.GetString(1)});
                }
            }

        }

        public class RelItem
        {
            public long id;
            public string name;
            public override string ToString()
            {
                return name;
            }
        }
        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selItem= comboBox1.SelectedItem as ItemTable;
            if (selItem != null)
            {
                if (checkBox1.Checked)
                    await tableViewControl1.setContent(selItem.table_id, conn);
                if (checkBox2.Checked)
                    await tableViewControl2.setContent(selItem.table_id, conn);
                await RefreshRels();

            }

        }

        private async Task RefreshRels()
        {
            if (tableViewControl1.selectedTableId >= 0 && tableViewControl2.selectedTableId >= 0)
            {
                await using (var cmd = new NpgsqlCommand(@"select distinct n.nodeid, n.name from md_node n, md_arc a1, md_arc a2
where n.nodeid = a1.fromid and a1.typeid = -6 and a2.fromid = a1.toid and a2.typeid in (9, 15)
and n.typeid = -6 and n.isdeleted = false
and a2.toid in (@id1, @id2)
union
select distinct  n.nodeid, n.name from md_node n, md_arc a1, md_arc a2
where n.nodeid = a1.toid and a1.typeid = -6 and a2.fromid = a1.fromid and a2.typeid in (9, 15)
and n.typeid = -6 and n.isdeleted = false
and a2.toid in (@id1, @id2)
", conn))
                {
                    cmd.Parameters.AddWithValue("@id1", tableViewControl1.selectedTableId);
                    cmd.Parameters.AddWithValue("@id2", tableViewControl2.selectedTableId);
                    listBox1.Items.Clear();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            listBox1.Items.Add(new RelItem() { id = reader.GetInt64(0), name = reader.GetString(1) });
                        }
                    }
                }


            }
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected=listBox1.SelectedItem as RelItem;
            if(selected!= null)
            {
                await using (var cmd = new NpgsqlCommand(@"select n2.nodeid ,n2.name,n1.nodeid,n1.name,nm.name from md_node nm,md_node n1,md_arc a1,md_node n2,md_arc a2
where nm.nodeid=@id and a1.fromid=@id and a1.toid=n1.nodeid and a2.toid=a1.fromid and a2.fromid=n2.nodeid
", conn))
                {
                    cmd.Parameters.AddWithValue("@id", selected.id);
//                    cmd.Parameters.AddWithValue("@id2", tableViewControl2.selectedTableId);
                    items.Clear();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            textBox1.Text = reader.GetString(4);
                            items.Add(new Item() {  id1 = reader.GetInt64(0),  name1 = reader.GetString(1), id2 = reader.GetInt64(2), name2 = reader.GetString(3) });
                        }
                    }
                    RefreshRelMembers();
                }

            }
        }
    }
}
