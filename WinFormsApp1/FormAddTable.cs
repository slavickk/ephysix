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
    public partial class FormAddTable : Form
    {
        Int64 fromId, toId;
        NpgsqlConnection conn;
        public FormAddTable(Int64 fromId1,Int64 toId1, NpgsqlConnection conn1)
        {
            fromId = fromId1;
            toId = toId1;
            conn = conn1;
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

     //   string[] returnNodesNames = new string[] { "Table", "ForeignKey" };

        public class ItemReturn
        {
            public string itemName;
            public Int64 itemId;
            public string[] additionalInfo;
        }
        List<ItemReturn> itemsReturn = new List<ItemReturn>();

        public IEnumerable<ItemReturn> returnedItems
        {
            get
            {
                return itemsReturn.Skip(1);
            }
        }

        List<List<string[]>> allPaths = null;
        private async void listViewPaths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPaths.SelectedIndices.Count > 0)
            {

                button2.Enabled = true;
                itemsReturn.Clear();
                itemsReturn.Add(new ItemReturn() { itemName = "Table", itemId = fromId });
                itemsReturn.Add(new ItemReturn() { itemName = "Table", itemId = toId });
                int index = listViewPaths.SelectedIndices[0];
                var path = allPaths[index];
                foreach (var pp in allPaths[index].Where(ii => ii[0] == "ForeignKey" || ii[0] == "VForeignKey"))
                {
                    var ppLast = path[path.IndexOf(pp) - 1];
                    var ppNext = path[path.IndexOf(pp) + 1];
                    var lastId = await GetNodeName1(Convert.ToInt64(ppLast[1]));
                    var nextId = await GetNodeName1(Convert.ToInt64(ppNext[1]));

                    if (itemsReturn.Count(ii => ii.itemId == lastId) == 0)
                        itemsReturn.Add(new ItemReturn() { itemId = lastId, itemName = "Table" });
                    //                    list.Add($"{ppLast[2]}:{ppNext[2]}");
                    itemsReturn.Add(new ItemReturn() { itemId = Convert.ToInt64(pp[1]), itemName = pp[0], additionalInfo= new string[3] { ppLast[3], ppNext[3], pp[1] } });
                    if (itemsReturn.Count(ii => ii.itemId == nextId) == 0)
                        itemsReturn.Add(new ItemReturn() { itemId = nextId, itemName = "Table" });
                }

            }
        }

        private async Task<Int64> GetNodeName1(Int64 id)
        {
            var command = "select toid from md_Arc where fromid=@id and typeid=9";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        return reader.GetInt64(0);
                    }
                }
            }
            return -1;
        }

        private async void FormAddTable_Load(object sender, EventArgs e)
        {
            textBox1.Text=  await GetNodeName(fromId);
            textBox2.Text = await GetNodeName(toId);
        }

        private async Task<string> GetNodeName(Int64 id)
        {
            var command = "select name from md_Node where nodeid=@id";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@id",id);
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

        private void button3_Click(object sender, EventArgs e)
        {
            FormConstructFK form = new FormConstructFK(toId, fromId, conn);
            if(form.ShowDialog() == DialogResult.OK)
            {
                itemsReturn.Clear();
                itemsReturn.Add(new ItemReturn() { itemName = "Table", itemId = fromId });
                itemsReturn.Add(new ItemReturn() { itemName = "Table", itemId = toId });
                itemsReturn.Add(new ItemReturn() { itemId = form.IDRelation, itemName = "VForeignKey", additionalInfo = new string[3] { textBox1.Text, textBox2.Text, form.nameRelation } });

            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            //            var command = "select pathlen,ccfa_nodeinfo(p.n1),ccfa_nodeinfo(p.n2),ccfa_nodeinfo(p.n3),ccfa_nodeinfo(p.n4),ccfa_nodeinfo(p.n5),ccfa_nodeinfo(p.n6),ccfa_nodeinfo(p.n7),ccfa_nodeinfo(p.n8),ccfa_nodeinfo(p.n9),ccfa_nodeinfo(p.n10) from ccfa_MD_allpaths_4(@FROMID,@TOID,@DEPTH,@EXCLUDE) p order by 1";
            var command = @"select pathlen
,case when(ccfa_nodeinfo(p.n2) like 'Column%') then '' else COALESCE(CAST(p.n2 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n3) like 'Column%') then '' else COALESCE(CAST(p.n3 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n4) like 'Column%') then '' else COALESCE(CAST(p.n4 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n5) like 'Column%') then '' else COALESCE(CAST(p.n5 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n6) like 'Column%') then '' else COALESCE(CAST(p.n6 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n7) like 'Column%') then '' else COALESCE(CAST(p.n7 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n8) like 'Column%') then '' else COALESCE(CAST(p.n8 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n9) like 'Column%') then '' else COALESCE(CAST(p.n9 as varchar(30)), '') end
|| case when(ccfa_nodeinfo(p.n10) like 'Column%') then '' else COALESCE(CAST(p.n10 as varchar(30)), '') end
,ccfa_nodeinfo(p.n1),ccfa_nodeinfo(p.n2),ccfa_nodeinfo(p.n3),ccfa_nodeinfo(p.n4),ccfa_nodeinfo(p.n5),ccfa_nodeinfo(p.n6),ccfa_nodeinfo(p.n7),ccfa_nodeinfo(p.n8),ccfa_nodeinfo(p.n9),ccfa_nodeinfo(p.n10)
from ccfa_MD_allpaths_5(@FROMID,@TOID,@DEPTH,@EXCLUDE, @EXCLUDE_TYPES) p order by 1,2";

            int i1 = 0;
            allPaths = new List<List<string[]>>();
            string oldKey = "";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@FROMID", fromId);
                cmd.Parameters.AddWithValue("@TOID",toId);
                cmd.Parameters.AddWithValue("@DEPTH",(int)numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@EXCLUDE", "");
                cmd.Parameters.AddWithValue("@EXCLUDE_TYPES", "4,3,5,7,102,103,104,105");
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var Key = reader.GetString(1);
                        if (Key != oldKey)
                        {
                            allPaths.Add(new List<string[]>());
                            for (int i = 2; i < reader.FieldCount; i++)
                            {
                                if (!reader.IsDBNull(i))
                                {
                                    var st = reader.GetString(i);
                                    var arr = st.Split(':');
                                    allPaths.Last().Add(arr);
                                }
                            }
                        }
                        oldKey = Key;
                        i1++;
                    }
                }
            }
            if (allPaths.Count == 0)
                return;

//            int maxCol = 0;
            int maxCol=allPaths.Max(ii => ii.Count(i1 => i1[0] == "ForeignKey" || i1[0] == "VForeignKey"));
            maxCol = maxCol * 2 + 1;
            listViewPaths.Items.Clear();
            listViewPaths.Columns.Clear();
            for(int i=0; i <maxCol;i++)
            {
                listViewPaths.Columns.Add(new ColumnHeader() { Name = "Table", Width=160, Text="Table" });
                if(i <maxCol-1)
                    listViewPaths.Columns.Add(new ColumnHeader() { Name = "Rel", Width = 260, Text = "Rel" });
            }
            foreach(var path in allPaths)
            {
                List<string> list = new List<string>();
                bool first = true;
                foreach(var pp in path.Where(ii => ii[0] == "ForeignKey" || ii[0] == "VForeignKey"))
                {
                    int index = path.IndexOf(pp);
                    if (index < path.Count-1)
                    {
                        var ppLast = path[path.IndexOf(pp) - 1];
                        var ppNext = path[path.IndexOf(pp) + 1];
                        if (first)
                            list.Add($"{ppLast[3]}");
                        //                    list.Add($"{ppLast[2]}:{ppNext[2]}");
                        string firstColumns = "", secondColumns = "";
                        bool fromLeftToRight = true;
                        for (int i = 2; i < pp.Length; i++)
                        {
                            bool secondPart = i - 2 >= (pp.Length - 2) / 2;
                            if (i % 2 == 0)
                            {
                                if (secondPart)
                                    secondColumns += (((secondColumns != "") ? "," : "") + pp[i]);
                                else
                                    firstColumns += (((firstColumns != "") ? "," : "") + pp[i]);

                            }
                            else
                            {
                                if (secondPart)
                                {
                                    if (pp[i] == ppLast[1])
                                        fromLeftToRight = false;
                                }
                                else
                                {
                                    if (pp[i] == ppNext[1])
                                        fromLeftToRight = false;

                                }

                            }

                        }
                        if (fromLeftToRight)
                            list.Add(firstColumns + "->" + secondColumns);
                        else
                            list.Add(secondColumns + "<-" + firstColumns);
                        if (list.Last() == "pan:mbr")
                        {
                            int yy = 0;
                        }
                        list.Add($"{ppNext[3]}");
                        /*                    if (pp != path.Last())
                                            {
                                                var tt = path[path.IndexOf(pp) + 1];
                                                list.Add("<->");
                                            }*/
                        first = false;
                    }
                }
                listViewPaths.Items.Add(new ListViewItem(list.ToArray()));
            }
        }
    }
}
