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
using Npgsql;
using ParserLibrary;

namespace TestJsonRazbor
{

    [GUI(typeof(StreamSender))]
    public partial class FormStreamSenderSetup : Form, SenderDataExchanger
    {
        public NpgsqlConnection conn { get; private set; }
        StreamSender ownerSender;
        string key;
        public FormStreamSenderSetup(StreamSender owner)
        {
            ownerSender = owner as StreamSender;
            //key = key1;
            InitializeComponent();
        }

        private void FormStreamSenderSetup_Load(object sender, EventArgs e)
        {
            comboBoxSensitive.SelectedIndex = -1;
            conn = new NpgsqlConnection(ownerSender.db_connection_string);
            conn.Open();
            key = ownerSender.streamName;

            RefreshStream();
            //            var ss=ownerSender.getTemplate("asd");

        }

        private void RefreshStream()
        {
            var st = ownerSender.getStream(key);
            setContent(st);
        }

        public class ItemTable
        {
            public string table_name;
            public long table_id;
            public override string ToString()
            {
                    return $"{table_name}";
            }
        }
        public class ItemColumn
        {
            public string col_name;
            public long col_id;
            public ItemTable table;
            public ItemColumn()
            {
            }
            public ItemColumn(long? colid,NpgsqlConnection conn)
            {
                using (var cmd = new NpgsqlCommand(@"select nc.name colname,nc.nodeid colid,nt.name tablename,nt.nodeid tableid from MD_node nc 
inner join MD_type tc on nc.typeid = tc.typeid and tc.key = 'Column'
inner join MD_arc ac on (ac.fromid = nc.nodeid  and ac.isdeleted=false)
inner join md_Node nt on ac.toid = nt.nodeid  and nt.typeid = 1 and nt.isdeleted=false
where nc.nodeid= @id and nc.isdeleted=false and nc.srcid=2", conn))
                {
                    cmd.Parameters.AddWithValue("@id", colid);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            col_name = reader.GetString(0);
                            col_id = reader.GetInt64(1);
                            table = new ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3) };
                        }
                    }
                }
            }

            public override string ToString()
            {
                    return $"{col_name}:{table.table_name}";

            }
        }

        public class FieldItem
        {
            public string Name;
            public string Type;
            public string Detail;
            public string SensitiveData;
            public ItemColumn refColumn;

        }
        List<FieldItem> fields= new List<FieldItem>();
        StreamSender.Stream toStream()
        {
            return new StreamSender.Stream() { Name = textBoxName.Text, Description = textBoxDescription.Text, fields = fields.Select(ii => new StreamSender.Stream.Field() { Name = ii.Name, Detail = ii.Detail, Type = ii.Type, linkedColumn = ii.refColumn?.col_id, SensitiveData=ii.SensitiveData }).ToList() };
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            await using (var cmd = new NpgsqlCommand(@"select nc.name colname,nc.nodeid colid,nt.name tablename,nt.nodeid tableid from MD_node nc 
inner join MD_type tc on nc.typeid = tc.typeid and tc.key = 'Column'
inner join MD_arc ac on (ac.fromid = nc.nodeid  and ac.isdeleted=false)
inner join md_Node nt on ac.toid = nt.nodeid  and nt.typeid = 1 and nt.isdeleted=false


where nc.name like '%" + textBox1.Text + "%' and nc.isdeleted=false and nc.srcid=2", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    comboBox1.Items.Add(new ItemColumn() { col_name = reader.GetString(0), col_id = reader.GetInt64(1), table = new ItemTable() { table_name = reader.GetString(2), table_id = reader.GetInt64(3) } });
                }
            }

        }


        ItemColumn linkedColumn = null;



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                linkedColumn = (ItemColumn)comboBox1.SelectedItem;
                textBoxLinkColumn.Text = linkedColumn.ToString();
            }
            else
                linkedColumn = null;
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            fields.Add(new FieldItem() { Detail = textBoxFieldDescription.Text,SensitiveData =comboBoxSensitive.SelectedValue?.ToString(), Name = textBoxFieldName.Text, Type = comboBoxFieldType.SelectedItem?.ToString(), refColumn = linkedColumn });
            RefreshListFields();
        }

        private void RefreshListFields()
        {
            listView1.Items.Clear();
            listView1.Items.AddRange(fields.Select(ii => new ListViewItem(new string[] { ii.Name, ii.Type, ii.Detail, ii.SensitiveData?.ToString(), ii.refColumn?.ToString() })).ToArray());
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count > 0)
            {
                int index = listView1.SelectedIndices[0];
                textBoxFieldDescription.Text = fields[index].Detail;
                textBoxFieldName.Text = fields[index].Name;
                comboBoxFieldType.SelectedItem = fields[index].Type;
                linkedColumn = fields[index].refColumn;
                textBoxLinkColumn.Text = (linkedColumn == null) ? "": linkedColumn.ToString();

                if (fields[index].SensitiveData==null)
                    comboBoxSensitive.SelectedIndex = -1;
                else
                    comboBoxSensitive.SelectedIndex =comboBoxSensitive.Items.IndexOf( fields[index].SensitiveData);


            }
        }

        public string getContent()
        {
            return JsonSerializer.Serialize(toStream());
            
        }

        public void setContent(string content)
        {
            StreamSender.Stream st = JsonSerializer.Deserialize<StreamSender.Stream>(content);
            setContent(st);
        }

        private void setContent(StreamSender.Stream st)
        {
            textBoxName.Text = st.Name;
            textBoxDescription.Text = st.Description;
            fields.Clear();
            fields.AddRange(st.fields.Select(ii => new FieldItem() { Name = ii.Name, Detail = ii.Detail, Type = ii.Type, SensitiveData = ii.SensitiveData, refColumn=(ii.linkedColumn==null?null:new ItemColumn(ii.linkedColumn,conn)) }));
            RefreshListFields();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ownerSender.setStream(getContent());
        }
        bool streamNameChanged = false;
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            streamNameChanged = true;
        }

        private void textBoxName_Leave(object sender, EventArgs e)
        {
            if(streamNameChanged && textBoxName.Text != key)
            {
                ownerSender.streamName =key=textBoxName.Text;
                RefreshStream();
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await using (var cmd = new NpgsqlCommand(@"select datum
from tmp_VitalyPrepare
where src = '" + textBoxName.Text + "' ", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    fields.Add(new FieldItem() {  Name = reader.GetString(0), Type="String" });
                }
            }
            RefreshListFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int index = listView1.SelectedIndices[0];
                fields[index].Detail = textBoxFieldDescription.Text;
                fields[index].SensitiveData = comboBoxSensitive.SelectedItem.ToString();
                fields[index].Name = textBoxFieldName.Text;
                fields[index].Type = comboBoxFieldType.SelectedItem?.ToString(); 
                fields[index].refColumn = linkedColumn;
                RefreshListFields();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int index = listView1.SelectedIndices[0];
                fields.RemoveAt(index);
                RefreshListFields();
            }
        }

        private void comboBoxSensitive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSensitive.SelectedIndex == 0)
                comboBoxSensitive.SelectedIndex = -1;
        }
    }
}
