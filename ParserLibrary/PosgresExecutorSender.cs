using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;

namespace ParserLibrary
{
    [Annotation("Выполнение произвольного  Postgres запроса с возвратом результатов выполнения")]
    public class PostgresExecutorSender : Sender
    {


        public override string getTemplate(string key)
        {
            return "{" + String.Join(",", getSQLVariables(statement).Select(ii => $"\"{ii}\":\"\"")) + "}";
            //            return base.getTemplate(key);
        }
        public override string getExample()
        {
            return "";
            //            return "{\"Define\":[]}";
        }

        public string connectionString = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;SearchPath=md;";
        public string statement = "select * from md_node where nodeid=@id";


        private IEnumerable<string> getSQLVariables(string statement)
        {
            //            var s = "select * from md_Node where nodeid=@id and s=@id and @b =@c(234)";
            char letter = '@';
            return
                Regex.Matches(statement, letter + @"\w*(-\w+)*", RegexOptions.IgnoreCase)
                    .Cast<Match>().Select(i => i.Value).Distinct();
        }

        public override TypeContent typeContent => TypeContent.internal_list;
        DateTime timeFinish;
        NpgsqlConnection conn = null;
        public async override Task<string> sendInternal(AbstrParser.UniEl root)
        {
            //            var def = root.childs.First(ii => ii.Name == "Define");
            string TableName = "";
            string insert_list = "(";
            string select_list = "";// "values (";
            string create_list = "";
            int i1 = 0;
            //            var connString = "User ID=postgres;Password=test;Host=localhost;Port=5432;";
            timeFinish = DateTime.Now.AddMinutes(1);
            if (conn == null)
            {
                conn = new NpgsqlConnection(connectionString);
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
            NpgsqlCommand cmd = null;
            cmd = new NpgsqlCommand(statement, conn);

            foreach (var fld in root.childs)
            {
                if (fld.childs.Count > 0)
                {
                    cmd.Parameters.AddWithValue(fld.Name, NpgsqlTypes.NpgsqlDbType.Jsonb, fld.toJSON());
                } else
                    cmd.Parameters.AddWithValue(fld.Name, fld.Value);

            }
            AbstrParser.UniEl rootOut = new AbstrParser.UniEl() { Name = "Root" };
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var leaf = new AbstrParser.UniEl(rootOut) { Name = reader.GetName(i), Value = reader.GetValue(i) };
                    }
                }
            }
            return await send(rootOut.toJSON());
        }

        //            return await base.sendInternal(root);
    }



}

