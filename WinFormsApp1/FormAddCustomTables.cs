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
        private void FormAddCustomTables_Load(object sender, EventArgs e)
        {
           // using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

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
            try { 
            string table=textBoxTableName.Text;
                if (!string.IsNullOrEmpty(table))
                {
                    await using (var cmd = new NpgsqlCommand($"DROP TABLE IF EXISTS {table};", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    await using (var cmd = new NpgsqlCommand($"CREATE TABLE IF NOT EXISTS {table} ({string.Join(',', fields.Select(ii => $"{ii.Name} {ii.Type}  NULL"))})", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    await using (var cmd = new NpgsqlCommand($"insert into  {table} ({string.Join(',', fields.Select(ii => ii.Name))}) {textBoxSql.Text}", conn))
                    {
                        cmd.Parameters.AddWithValue("@body", textBoxUrlResult.Text);

                        cmd.ExecuteNonQuery();
                    }
                }
                // Save table 
                await using (var cmd = new NpgsqlCommand(@"select * from md_add_url_table(2,@id,@table_name,@url,@sql,@period)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id_table);
                    cmd.Parameters.AddWithValue("@table_name", textBoxTableName.Text);
                    cmd.Parameters.AddWithValue("@url", textBoxUrl.Text);
                    cmd.Parameters.AddWithValue("@sql", textBoxSql.Text);
                    cmd.Parameters.AddWithValue("@period", NpgsqlTypes.NpgsqlDbType.Timestamp, updatePeriod);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            id_table = reader.GetInt64(0);
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
    }
}
