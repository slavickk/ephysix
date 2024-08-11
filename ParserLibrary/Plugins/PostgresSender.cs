/******************************************************************
 * File: PostgresSender.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using PluginBase;
using UniElLib;

namespace Plugins;

[Annotation("Загрузка данных в произвольную Postgres таблицу ( построчно")]
public class IPostgresSender : ISender
{
    public ISenderHost host { get; set; }
    
    public Task<string> send(string JsonBody, ContextItem context)
    {
        // The original PostgresSender didn't implement this method.
        // Let's make it throw an exception, so we can see if it's ever called.
        throw new System.NotImplementedException();
    }

    public string getTemplate(string key)
    {
        return "{\"TableName\": \"\",\"Key\": \"\", \"field\": {\"Name\": \"\",\"Type\": \"\",\"Value\": \"\"}}";
//            return base.getTemplate(key);
    }

    public void setTemplate(string key, string body)
    {
    }

    public void Init()
    {
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
    public TypeContent typeContent => TypeContent.internal_list;
    DateTime timeFinish;
    NpgsqlConnection conn = null;
    public async Task<string> send(AbstrParser.UniEl root, ContextItem context )
    {
        //            var def = root.childs.First(ii => ii.Name == "Define");
        string TableName="";
        string insert_list = "(";
        string select_list = "";// "values (";
        string update_list = "";
        string where_list = "";
        string key = "";

        string create_list = "";
        int i1 = 0;
        DateTime timeStamp = DateTime.Now;  
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
                insert_list = "(impTimeStamp";
                select_list = "values (@P0";// "values (";
                create_list = "impTimeStamp timestamp without time zone";
                update_list = " SET impTimeStamp=  @P0";
                where_list = "";

                i1 = 1;
                cmd = new NpgsqlCommand();
                cmd.Parameters.AddWithValue($"@P0", timeStamp);

            }
            if (fld.Name == "EndRecord")
            {
                insert_list += ")";
                select_list += ")";
                var inserted = $"insert into {TableName} {insert_list}  {select_list}";
                var updated = $"update {TableName}   {update_list}   {where_list}";

              //  cmd.CommandText = $"DO\r\n$$\r\ndeclare \r\n selected integer;\r\nbegin\r\n  {updated};\r\n  GET DIAGNOSDummyProtocol1S selected = ROW_COUNT;\r\n  if(selected =0) Then\r\n        {inserted};\r\n  END IF;\r\n  ----select selected;\r\nend  \r\n$$ LANGUAGE plpgsql;\r\n";
            //    cmd.CommandText = $"insert into {TableName} {insert_list}  {select_list}";
                 cmd.CommandText=updated;
                cmd.Connection = conn;
                try
                {
                    ExecQu(cmd, inserted);
                }
                catch (NpgsqlException e77)
                {
                    if (((Npgsql.PostgresException)e77).SqlState == "42P01")
                    {
                        try
                        {
                            var cmd1 = new NpgsqlCommand($"CREATE TABLE IF NOT EXISTS fp.{TableName}(    {create_list} )", conn);
//                                cmd1.Prepare()
                            cmd1.ExecuteNonQuery();
                        }
                        catch (Exception e78)
                        {

                        }
                        try
                        {
                            ExecQu(cmd, inserted);
                          //  cmd.ExecuteNonQuery();
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
            if (fld.Name == "Key")
            {
                key=fld.Value.ToString();
                where_list = key+"=";
            }
                if (fld.Name == "field")
            {
                var TypeField = fld.childs.First(ii => ii.Name == "Type").Value.ToString();
                //  string TableName = root.childs.First(ii => ii.Name == "TableName").Value.ToString();
                var sel_item = $"@P{i1}";
                if (fld.childs.Count(ii => ii.Name == "Value") > 0)
                {
                    int yy = 0;


                    string nameField=fld.childs.First(ii => ii.Name == "Name").Value.ToString();

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

                        string name = fld.childs.First(ii => ii.Name == "Name").Value.ToString();
                        insert_list += ((insert_list.Length == 1) ? "" : ",") + name;
                        select_list += ((select_list.Length == 0) ? "values (" : ",") + sel_item;
                        update_list += ((update_list.Length == 0) ? " SET " : ",") + name+$"={sel_item}";
                        if (name == key)
                            where_list = $" WHERE {key}={sel_item}";
                        cmd.Parameters.AddWithValue($"@P{i1}", fld.childs.First(ii => ii.Name == "Value").Value.ToString());
                    }
                }
                create_list += ((create_list.Length == 0) ? "" : ",") + $" {fld.childs.First(ii => ii.Name == "Name").Value} {TypeField} NULL";// id integer NOT NULL
                i1++;
            }

        }

        // The original code called the base sendInternal() method, which returns an empty string. 
        return string.Empty;
    }

    private static void ExecQu(NpgsqlCommand cmd, string inserted)
    {
        int returns = cmd.ExecuteNonQuery();
        if (returns == 0)
        {
            cmd.CommandText = inserted;
            cmd.ExecuteNonQuery();
        }
    }
}
