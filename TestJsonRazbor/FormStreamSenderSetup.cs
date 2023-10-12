using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using ParserLibrary;
using UniElLib;

namespace TestJsonRazbor
{

    [GUI(typeof(StreamSender))]
    public partial class FormStreamSenderSetup : Form, SenderDataExchanger
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        

        public class JsonStream
        {
            public class Field
            {
                public string Name { get; set; }
                public string Type { get; set; }
                public string Detail { get; set; }
            }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Field> fields { get; set; }
        }


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

            comboBoxSensitive.Items.Clear();
            foreach (var item in Assembly.GetAssembly(typeof(AliasProducer)).GetTypes().Where(t => t.IsAssignableTo(typeof(AliasProducer)) && !t.IsAbstract))
            {
                var item1 = item.CustomAttributes.First(ii => ii.AttributeType == typeof(SensitiveAttribute));
                comboBoxSensitive.Items.Add(item1.ConstructorArguments[0].Value);
            }

/*            foreach (var item in Assembly.GetAssembly(typeof(AliasProducer)).GetTypes().Where(t => t.IsAssignableTo(typeof(AliasProducer)) && !t.IsAbstract))
                comboBoxSensitive.Items.Add(item);*/
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
            public bool Calculated;
            public ItemColumn refColumn;

        }
        List<FieldItem> fields= new List<FieldItem>();
        StreamSender.Stream toStream()
        {
            var ff = fields.Select(ii => new StreamSender.Stream.Field() { Name = ii.Name, Detail = ii.Detail, Type = ii.Type, linkedColumn = ii.refColumn?.col_id, SensitiveData = ii.SensitiveData, Calculated=(ii.Calculated?ii.Calculated:null) }).ToList();
            var ff1=ff.ToDictionary(p => p.Name);
            return new StreamSender.Stream() { Name = textBoxName.Text, Description = textBoxDescription.Text, fieldsDict =ff1 };
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
            fields.Add(new FieldItem() { Calculated=checkBoxCalculated.Checked, Detail = textBoxFieldDescription.Text,SensitiveData =comboBoxSensitive.SelectedValue?.ToString(), Name = textBoxFieldName.Text, Type = comboBoxFieldType.SelectedItem?.ToString(), refColumn = linkedColumn });
            RefreshListFields();
        }

        private void RefreshListFields()
        {
            listView1.Items.Clear();
            listView1.Items.AddRange(fields.Select(ii => new ListViewItem(new string[] { ii.Name, ii.Type, ii.Detail, ii.SensitiveData?.ToString(), ii.refColumn?.ToString(), (ii.Calculated?"Y":"") })).ToArray());
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count > 0)
            {
                int index = listView1.SelectedIndices[0];
                textBoxFieldDescription.Text = fields[index].Detail;
                checkBoxCalculated.Checked = fields[index].Calculated;
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
            fields.AddRange(st.fieldsDict.Select(ii => new FieldItem() { Name = ii.Value.Name, Detail = ii.Value.Detail, Type = ii.Value.Type, Calculated=(bool)((ii.Value.Calculated== null)?false: ii.Value.Calculated), SensitiveData = ii.Value?.SensitiveData, refColumn=(ii.Value.linkedColumn==null?null:new ItemColumn(ii.Value.linkedColumn,conn)) }));
            RefreshListFields();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                StreamSender.ToConsul(toStream());

                ownerSender.setStream(getContent());
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                fields[index].Calculated=checkBoxCalculated.Checked;
                fields[index].SensitiveData = comboBoxSensitive.SelectedItem?.ToString();
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

        private void button6_Click(object sender, EventArgs e)
        {
            var fieldsSt=textBoxFromString.Text.Split(",");
            foreach(var fld  in fieldsSt)
            {
                if(fields.Count(ii=>ii.Name== fld)==0)
                {
                    fields.Add(new FieldItem() { Name = fld, Type = "String" });
                }
            }
            RefreshListFields() ;   
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormStreamFromJson frm = new FormStreamFromJson();
            if(frm.ShowDialog() == DialogResult.OK)
            {
                textBoxName.Text = frm.retValue.Name;
                textBoxDescription.Text = frm.retValue.Description;
                fields.Clear();
                foreach( var fld in frm.retValue.fields)
                {
                    fields.Add(new FieldItem() { Name = fld.Name, Detail = fld.Detail, Type = fld.Type });
                }
                RefreshListFields();                
            }
        }
    }
}
