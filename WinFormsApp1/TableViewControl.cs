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
    public partial class TableViewControl : UserControl
    {


        public TableViewControl()
        {
            InitializeComponent();
        }
        bool busy = false;

        public class ItemColumn
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }


        List<ItemColumn> list = new List<ItemColumn>();
        public async Task setContent(long id, NpgsqlConnection conn)
        {
            selectedTableId=id;
            list.Clear();
            if (busy)
                return;
            busy = true;
            listView1.Items.Clear();
            var command = @"select n1.NodeID, n1.Name, n2.NodeID, n2.Name, a2.key
    from md_node n1, md_arc a1, md_node n2, md_node_attr_val nav2, md_attr a2
    where n1.typeid=md_get_type('Table') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and n2.typeid=md_get_type('Column')
      and n2.NodeID=nav2.NodeID
      and nav2.AttrID=a2.AttrID
  and a2.attrid>=5 and a2.attrid<=17
      and n1.nodeid=@id
union 
 
select n1.NodeID, n1.Name, n2.NodeID, n2.Name, ' '
    from md_node n1, md_arc a1, md_node n2
    where n1.typeid=md_get_type('Stream') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Field2Stream')
      and n2.typeid=md_get_type('StreamField')
      and n1.nodeid=@id
";
            string tableName = "";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        tableName = reader.GetString(1);
                        var idCol = reader.GetInt64(2);
                        var column = reader.GetString(3);
                        var type = reader.GetString(4);
                        list.Add(new ItemColumn() { Name = column, Id = idCol });
                        listView1.Items.Add(new ListViewItem(new string[] { column, type }));
                    }
                }


            }
            label1.Text = tableName;
            busy = false;

        }
        public long selectedTableId=-1;
        public ItemColumn selectedColumn;

        public delegate void SelectedChanged();
        public event SelectedChanged OnSelectedChanged;

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                selectedColumn = list[listView1.SelectedIndices[0]];
                if (OnSelectedChanged != null)
                    OnSelectedChanged();
            }
            else
                selectedColumn = null;
        }

        private void TableViewControl_Load(object sender, EventArgs e)
        {

        }
        public delegate void TableDoubleClicked(ItemColumn column);
        public event TableDoubleClicked OnTableDoubleClicked;   
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(OnTableDoubleClicked != null && listView1.SelectedItems.Count>0)
            {
                OnTableDoubleClicked(list[listView1.SelectedIndices[0]]);
            }
        }

        private void TableViewControl_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
