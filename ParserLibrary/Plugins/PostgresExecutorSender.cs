/******************************************************************
 * File: PostgresExecutorSender.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using ParserLibrary;
using PluginBase;
using UniElLib;

namespace Plugins;

public class PostgresExecutorSender : ISender
{
    public ISenderHost host { get; set; }

    public Task<string> send(string JsonBody, ContextItem context)
    {
        // The original PostgresExecutorSender didn't implement this method.
        // Let's make it throw an exception, so we can see if it's ever called.
        throw new NotImplementedException();
    }

    public string getTemplate(string key)
    {
        return "{" + String.Join(",", getSQLVariables(statement).Select(ii => $"\"{ii}\":\"\"")) + "}";
        //            return base.getTemplate(key);
    }

    public void setTemplate(string key, string body)
    {
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

    public TypeContent typeContent => TypeContent.internal_list;
    public void Init()
    {
    }

    DateTime timeFinish;
    NpgsqlConnection conn = null;
    public class ItemVar
    {
        public string name;
        public List<AbstrParser.UniEl> list;
    }
    List<ItemVar> sqlVariables = null;

    public async Task<string> send(AbstrParser.UniEl root, ContextItem context)
    {
        //            var def = root.childs.First(ii => ii.Name == "Define");
//            if (sqlVariables == null)
            sqlVariables = getSQLVariables(statement).Select(ii=>new ItemVar() { name = ii }).ToList();
        string TableName = "";
        string insert_list = "(";
        string select_list = "";// "values (";
        string create_list = "";
        int i1 = 0;
        //            var connString = "User ID=postgres;Password=test;Host=localhost;Port=5432;";
        try
        {
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
            foreach (var it in sqlVariables)
            {
                it.list = root.childs.Where(ii => ii.Name == it.name).ToList();
                if (it.list.Count > 0)
                {
                    if (it.list.Count > 1 || (it.list.First().childs.Count > 0))
                        cmd.Parameters.AddWithValue(it.name, NpgsqlTypes.NpgsqlDbType.Jsonb,$"[{String.Join(",",it.list.Select(ii=>ii.toJSON()))}]");
                    else
                        cmd.Parameters.AddWithValue(it.name, it.list[0].Value);

                }

            }
/*                foreach (var fld in root.childs)
            {
                if (fld.childs.Count > 0)
                {
                    cmd.Parameters.AddWithValue(fld.Name, NpgsqlTypes.NpgsqlDbType.Jsonb, fld.toJSON());
                }
                else
                    cmd.Parameters.AddWithValue(fld.Name, fld.Value);

            }*/
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
            return rootOut.toJSON();
        } 
        catch(Exception e77)
        {
            Logger.log("PostgresError:{err}", Serilog.Events.LogEventLevel.Error, e77);
            return "";
        }
    }

    //            return await base.sendInternal(root);
}
