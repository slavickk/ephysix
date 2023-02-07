using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Npgsql;
using static NpgsqlTypes.NpgsqlTsQuery;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Linq.Expressions;

namespace WinFormsApp1
{
    public partial class FormAddCustomTables : Form
    {
        NpgsqlConnection conn;
        NpgsqlConnection connExt;
        public FormAddCustomTables(Npgsql.NpgsqlConnection con)
        {
            conn = con;
 
            InitializeComponent();
        }

        private async void buttonExecUrl_Click(object sender, EventArgs e)
        {

            textBoxUrlResult.Text = await client.GetStringAsync(
     textBoxUrl.Text);
   


        }
        HttpClient client=new HttpClient();

        public class TableItem
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
        private async void FormAddCustomTables_Load(object sender, EventArgs e)
        {
            connExt= new NpgsqlConnection();
            var src = await GenerateStatement.getSrcInfo(5, conn);
          //  src.
            connExt.ConnectionString = src.connectionString;
            connExt.Open();
           // using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            
            await using (var cmd = new NpgsqlCommand(@"select n1.NodeID, n1.Name
    from md_node n1, md_node_attr_val nav2
    where n1.typeid = md_get_type('Table')
      and n1.NodeID = nav2.NodeID
      and nav2.AttrID = 107"
, conn))
            {
                /*cmd.Parameters.AddWithValue("@id", id_table);
                cmd.Parameters.AddWithValue("@table_name", textBoxTableName.Text);
                cmd.Parameters.AddWithValue("@url", textBoxUrl.Text);
                cmd.Parameters.AddWithValue("@sql", textBoxSql.Text);
                cmd.Parameters.AddWithValue("@period", NpgsqlTypes.NpgsqlDbType.Interval, updatePeriod);*/
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        TableItem item = new TableItem() { Id = reader.GetInt64(0), Name = reader.GetString(1) };
                        comboBox1.Items.Add(item);
                        //id_table = reader.GetInt64(0);
                    }
                }
            }
           
        }

        private async void buttonExecSql_Click(object sender, EventArgs e)
        {
            try
            {
                await using (var cmd = new NpgsqlCommand(textBoxSql.Text, conn))
                {
                    /*                        listViewSelectedField.Items.Clear();
                                            foreach (var field in selectedFields)
                                                listViewSelectedField.Items.Add(new ListViewItem(new String[] { field.col_name, field.alias, field.table.table_name, field.table.alias }));*/
                    cmd.Parameters.AddWithValue("@body", textBoxUrlResult.Text);
                    fields.Clear();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            ItemType item = new ItemType();
                            item.Name = reader.GetName(i);
                            item.Type = reader.GetDataTypeName(i);
                            fields.Add(item);
                        }

                    }
                }
                refreshFields();
            }
            catch(Exception e66) {
                MessageBox.Show(e66.ToString());
            }
        }
        List<ItemType> fields= new List<ItemType>();
        public class ItemType
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Description { get; set; } 
        }


        public void refreshFields()
        {
            int currentIndex = -1;
            if(listView1.SelectedIndices.Count > 0)
            {
                currentIndex = listView1.SelectedIndices[0];
            }
            listView1.Items.Clear();
            foreach(var item in fields)
            {
                listView1.Items.Add( new ListViewItem(new string[] { item.Name, item.Description, item.Type }));
            }
            if(currentIndex>=0 && currentIndex < listView1.Items.Count)
            {
                listView1.SelectedIndices.Clear();
                 listView1.SelectedIndices.Add(currentIndex);
 //                listView1.SelectedItems
            }
        }

        long id_table = -1;
        TimeSpan updatePeriod =new TimeSpan(24,0,0);
        private async void ButtonSave_Click(object sender, EventArgs e)
        {
 //           id_table = 2056845;
            try
            { 
            string table=textBoxTableName.Text;
                if (!string.IsNullOrEmpty(table))
                {
                    await using (var cmd = new NpgsqlCommand($"DROP TABLE IF EXISTS {table};", connExt))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    await using (var cmd = new NpgsqlCommand($"CREATE TABLE IF NOT EXISTS {table} ({string.Join(',', fields.Select(ii => $"{ii.Name} {ii.Type}  NULL"))})", connExt))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    await using (var cmd = new NpgsqlCommand($"GRANT ALL ON TABLE  {table}  TO md", connExt))
                    {
                        cmd.ExecuteNonQuery();
                    }

//                    GRANT ALL ON TABLE md.md_arc TO md;
                    await using (var cmd = new NpgsqlCommand($"insert into  {table} ({string.Join(',', fields.Select(ii => ii.Name))}) {textBoxSql.Text}", connExt))
                    {
                        cmd.Parameters.AddWithValue("@body", textBoxUrlResult.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                
                await using (var cmd = new NpgsqlCommand(@"select* from md_check_update_time(@table_name, @interval)", conn))
                {
                    cmd.Parameters.AddWithValue("@table_name", textBoxTableName.Text.ToLower());
                    cmd.Parameters.AddWithValue("@interval",NpgsqlTypes.NpgsqlDbType.Bigint, updatePeriod.TotalSeconds);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bool updated  = reader.GetBoolean(0);
                        }
                    }
                }

                // Save table 
                await using (var cmd = new NpgsqlCommand(@"select * from md_add_url_table(5,@id,@table_name,@url,@sql,@example,@period)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id_table);
                    cmd.Parameters.AddWithValue("@table_name", textBoxTableName.Text.ToLower());
                    cmd.Parameters.AddWithValue("@url", textBoxUrl.Text);
                    cmd.Parameters.AddWithValue("@sql", textBoxSql.Text);
                    cmd.Parameters.AddWithValue("@example", textBoxUrlResult.Text);
                    cmd.Parameters.AddWithValue("@period",NpgsqlTypes.NpgsqlDbType.Bigint,  updatePeriod.TotalSeconds);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            id_table = reader.GetInt64(0);
                        }
                    }
                }
                foreach (var field in fields)
                {
                    await using (var cmd = new NpgsqlCommand(@"select * from md_add_url_table_column(5,@id,@field_name,@field_type)", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id_table);
                        cmd.Parameters.AddWithValue("@field_name", field.Name.ToLower());
                        cmd.Parameters.AddWithValue("@field_type", field.Type switch
                        {
                            "text" => "VARCHAR",
                            "VARCHAR" => "VARCHAR",
                            "double precision" => "FLOAT",
                            "FLOAT" => "FLOAT",
                            _ => "VARCHAR"
                        });
                        await using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                              //  id_table = reader.GetInt64(0);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }


//            throw new NotImplementedException();
        }

        private void buttonSaveField_Click(object sender, EventArgs e)
        {
            int currentIndex = -1;
            if (listView1.SelectedIndices.Count > 0)
            {
                currentIndex = listView1.SelectedIndices[0];
            }
            if(currentIndex >=0)
            {
                var field = fields[currentIndex];
                if (field != null)
                {
                    field.Name=textBoxFieldName.Text;
                    field.Description = textBox1.Text;
                    field.Type = textBox2.Text;
                }
                refreshFields();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentIndex = -1;
            if (listView1.SelectedIndices.Count > 0)
            {
                currentIndex = listView1.SelectedIndices[0];
            }
            if(currentIndex>=0)
            {
                var field = fields[currentIndex];
                if (field != null)
                {
                    textBoxFieldName.Text = field.Name;
                    textBox1.Text=field.Description;
                    textBox2.Text=field.Type;
                }
            }


        }

        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            await using (var cmd = new NpgsqlCommand(@"select n1.NodeID, n1.Name, n2.Name, a2.key,nav_url.val,nav_url_sql.val,nav_per.val
    from md_node n1, md_arc a1, md_node n2, md_node_attr_val nav2, md_attr a2
	, md_node_attr_val nav_url
	, md_node_attr_val nav_url_sql
	, md_node_attr_val nav_per
    where n1.typeid=md_get_type('Table') and a1.toid=n1.nodeid and a1.fromid=n2.nodeid and a1.typeid=md_get_type('Column2Table')
      and n2.typeid=md_get_type('Column')
      and n2.NodeID=nav2.NodeID
      and n1.NodeID=nav_url.NodeID
	  and nav_url.attrID=107
      and n1.NodeID=nav_url_sql.NodeID
	  and nav_url_sql.attrID=108
      and n1.NodeID=nav_per.NodeID
	  and nav_per.attrID=109
	  
      and nav2.AttrID=a2.AttrID
 and a2.attrid>=5 and a2.attrid<=17
 and n2.isdeleted=false
  and n1.NodeId=@id
"
  , conn))
            {
                cmd.Parameters.AddWithValue("@id", (comboBox1.SelectedItem as TableItem).Id);
                /*cmd.Parameters.AddWithValue("@table_name", textBoxTableName.Text);
                cmd.Parameters.AddWithValue("@url", textBoxUrl.Text);
                cmd.Parameters.AddWithValue("@sql", textBoxSql.Text);
                cmd.Parameters.AddWithValue("@period", NpgsqlTypes.NpgsqlDbType.Interval, updatePeriod);*/
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    fields.Clear();
                    while (await reader.ReadAsync())
                    {
                        this.id_table=reader.GetInt64(0);
                        textBoxTableName.Text=reader.GetString(1);
                        fields.Add(new ItemType() { Name = reader.GetString(2), Type = reader.GetString(3) });
                        textBoxUrl.Text=reader.GetString(4);
                        textBoxSql.Text=reader.GetString(5);

                       this.updatePeriod = new TimeSpan(0,0,Convert.ToInt32(reader.GetString(6)));

                    }
                    refreshFields();    
                }
            }

        }
    }
}
