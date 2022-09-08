using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;

namespace ParserLibrary
{
    [Annotation("Загрузка данных в произвольную Postgres таблицу ( построчно")]
    public class PostgresSender : Sender
    {


        public override string getTemplate(string key)
        {
            return "{\"TableName\": \"\", \"field\": {\"Name\": \"\",\"Type\": \"\",\"Value\": \"\"}}";
//            return base.getTemplate(key);
        }
        public override string getExample()
        {
            return "";
//            return "{\"Define\":[]}";
        }

        public string connectionString= "User ID=postgres;Password=test;Host=localhost;Port=5432;";


        public static void Test()
        {
            var s = "select * from md_Node where nodeid=@id and s=@id and @b =@c(234)";
            char letter = '@';
            var res =
                Regex.Matches(s, letter + @"\w*(-\w+)*", RegexOptions.IgnoreCase)
                    .Cast<Match>().Select(i => i.Value).Distinct().ToArray();


            string connectionString = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var cmd1 = new NpgsqlCommand("select * from md_Node where nodeid=@id", conn);
            cmd1.Prepare();
            conn.Close();

        }
        public override TypeContent typeContent => TypeContent.internal_list;
        DateTime timeFinish;
        NpgsqlConnection conn = null;
        public async override Task<string> sendInternal(AbstrParser.UniEl root)
        {
            //            var def = root.childs.First(ii => ii.Name == "Define");
            string TableName="";
            string insert_list = "(";
            string select_list = "";// "values (";
            string create_list = "";
            int i1 = 0;
//            var connString = "User ID=postgres;Password=test;Host=localhost;Port=5432;";
            timeFinish = DateTime.Now.AddMinutes(1);
            if (conn == null)
            {
                conn= new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                Task.Run(async () =>
                {
                    while (DateTime.Now < timeFinish)
                    {
                        await Task.Delay(10000);
                    }
                    await conn.CloseAsync();
                    conn = null;
                });
            }
            NpgsqlCommand cmd=null ;
            
            foreach (var fld in root.childs)
            {

                if (fld.Name == "TableName")
                {
                    TableName = fld.Value.ToString();
                    insert_list = "(";
                    select_list = "";// "values (";
                    create_list = "";
                    i1 = 0;
                    cmd = new NpgsqlCommand();

                }
                if (fld.Name == "EndRecord")
                {
                    insert_list += ")";
                    select_list += ")";
                    cmd.CommandText = $"insert into {TableName} {insert_list}  {select_list}";

                    cmd.Connection = conn;
                    try
                    {
                        cmd.ExecuteNonQuery();

                    }
                    catch (NpgsqlException e77)
                    {
                        if (((Npgsql.PostgresException)e77).Code == "42P01")
                        {
                            try
                            {
                                var cmd1 = new NpgsqlCommand($"CREATE TABLE IF NOT EXISTS public.{TableName}(    {create_list} )", conn);
//                                cmd1.Prepare()
                                cmd1.ExecuteNonQuery();
                            }
                            catch (Exception e78)
                            {

                            }
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception e89)
                            {

                            }
                        }
                    }
                    finally
                    {

                    }

                }
                if (fld.Name == "field")
                {
                    var TypeField = fld.childs.First(ii => ii.Name == "Type").Value.ToString();
                    //  string TableName = root.childs.First(ii => ii.Name == "TableName").Value.ToString();
                    var sel_item = $"@P{i1}";
                    if (fld.childs.Count(ii => ii.Name == "Value") > 0)
                    {
                        int yy = 0;

                        var val = fld.childs.First(ii => ii.Name == "Value").Value;

                        if (!TypeField.Contains("varchar", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (val != null && val.ToString() != "")
                                sel_item = $"cast(@P{i1} as {TypeField})";
                            else
                                sel_item = null;
                        }
                        if (sel_item != null)
                        {
                            insert_list += ((insert_list.Length == 1) ? "" : ",") + fld.childs.First(ii => ii.Name == "Name").Value.ToString();
                            select_list += ((select_list.Length == 0) ? "values (" : ",") + sel_item;

                            cmd.Parameters.AddWithValue($"@P{i1}", fld.childs.First(ii => ii.Name == "Value").Value.ToString());
                        }
                    }
                    create_list += ((create_list.Length == 0) ? "" : ",") + $" {fld.childs.First(ii => ii.Name == "Name").Value} {TypeField} NULL";// id integer NOT NULL
                    i1++;
                }

            }

            return await base.sendInternal(root);
        }


    }
}
