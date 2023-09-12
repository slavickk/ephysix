using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CamundaInterface.CamundaExecutor;
using System.Text.RegularExpressions;

namespace CamundaInterface
{
    public  class APIExecutor
    {
        public interface _ApiExecutor
        {
            public class ItemCommand
            {
                public override string ToString()
                {
                    return Name;
                }
                public string Name;
                public class Parameter
                {
                    public string name;
                    public bool isDemand = false;
                    public List<string> alternatives = new List<string>();
                }
                public List<Parameter> parameters = new List<Parameter>();
                public class OutputItems
                {
                    public string path;
                    public OutputItems parent = null;
                    public override string ToString()
                    {
                        return path;
                    }

                }
                public List<OutputItems> outputItems = new List<OutputItems>();
            }

            Task beginSessionAsync();
            Task<_ApiFilter> ExecAsync(ExecContextItem[] commands);
            public class ErrorItem
            {
                public string content;
                public string error;
            }
            ErrorItem getError();
            Task endSessionAsync();
            List<ItemCommand> getDefine();


        }
        public interface _ApiFilter
        {
            string[] filter(string path);
        }

        public class ExecContextItem
            {
                public class ItemParam
                {
                    public string Key { get; set; } 
                    public object? Value { get; set; }
                public string? Variable { get; set; }

                public ItemParam()
                {

                }
                public ItemParam(string key, object value)
                {
                    Key = key;
                    Value = value;
                }

                }
            public ExecContextItem()
            {

            }
            public ExecContextItem(string command)
            {
                Command = command;
            }
                public string Command { get; set; }
                public List<ItemParam> Params { get; set; }
            }

        public class Item
        {
            public string? path;
            public string? variable;
            public string? constant;
            public int currentIndex;
            public string columnName;
            public string columnType;
            public string tableName;
            public string[] values;
        }

        public class ItemIIDS
        {
            public string columnName;
            public string tableName;
        }
        Dictionary<string, NpgsqlTypes.NpgsqlDbType> db_types = new Dictionary<string, NpgsqlTypes.NpgsqlDbType> {
                { "NUMBER", NpgsqlTypes.NpgsqlDbType.Numeric }, { "DECIMAL", NpgsqlTypes.NpgsqlDbType.Numeric },
                { "SMALLINT", NpgsqlTypes.NpgsqlDbType.Smallint },
                { "INT", NpgsqlTypes.NpgsqlDbType.Integer }, { "INTEGER", NpgsqlTypes.NpgsqlDbType.Integer }, { "BIGINT", NpgsqlTypes.NpgsqlDbType.Bigint }, { "BOOLEAN", NpgsqlTypes.NpgsqlDbType.Boolean },
                { "DATETIME", NpgsqlTypes.NpgsqlDbType.TimestampTZ }, { "FLOAT", NpgsqlTypes.NpgsqlDbType.Real },
                { "REAL", NpgsqlTypes.NpgsqlDbType.Real }, { "INTERVAL", NpgsqlTypes.NpgsqlDbType.Interval }, { "UNICODE", NpgsqlTypes.NpgsqlDbType.Text }, { "CLOB", NpgsqlTypes.NpgsqlDbType.Text },
                { "TEXT", NpgsqlTypes.NpgsqlDbType.Text }, { "CHAR", NpgsqlTypes.NpgsqlDbType.Varchar }, { "JSON", NpgsqlTypes.NpgsqlDbType.Varchar }, { "NCHAR", NpgsqlTypes.NpgsqlDbType.Varchar },
                { "NVARCHAR", NpgsqlTypes.NpgsqlDbType.Varchar }, { "VARCHAR", NpgsqlTypes.NpgsqlDbType.Varchar }, { "DATE", NpgsqlTypes.NpgsqlDbType.Date }, { "TIMESTAMP", NpgsqlTypes.NpgsqlDbType.Timestamp } };

        public class ReturnItem
        {
            public int All=0;
            public string OperUUID;
            public int Errors=0;
        }
        async Task ReportError(string connectionString,string open_uuid,string camunda_process_id,string detail_operation,string detail_error, Dictionary<string, ExternalTaskAnswer.Variables> variables)
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            var command = @"INSERT INTO dm.etl_error(
	 open_uuid, camunda_process_id, detail_operation, detail_error, camunda_context)
	VALUES (@open_uuid, @camunda_process_id, @detail_operation, @detail_error, @camunda_context);";
            using (var cmdCommand = new NpgsqlCommand(command, conn))
            {
                cmdCommand.Parameters.AddWithValue("@open_uuid", open_uuid);
                cmdCommand.Parameters.AddWithValue("@camunda_process_id", (object)camunda_process_id?? DBNull.Value);
                cmdCommand.Parameters.AddWithValue("@detail_operation", (object)detail_operation ?? DBNull.Value);
                cmdCommand.Parameters.AddWithValue("@detail_error", (object)detail_error ?? DBNull.Value);
                cmdCommand.Parameters.AddWithValue("@camunda_context", NpgsqlDbType.Jsonb, (object)JsonSerializer.Serialize<Dictionary<string, ExternalTaskAnswer.Variables>>(variables) ?? DBNull.Value);
                cmdCommand.ExecuteNonQuery();
            }


                conn.Close();
        }


        public async Task<ReturnItem> ExecuteApiRequest(_ApiExecutor executor, ExecContextItem[] commands, TableDefine[] tables, string baseQuery = "select '2220000000000200' PAN", string ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;",Dictionary<string, ExternalTaskAnswer.Variables> variables=null,string processId=null)
        {
            ReturnItem retValue = new ReturnItem();
            //                { "BLOB",NpgsqlTypes.NpgsqlDbType.: LargeBinary, "BYTEA": LargeBinary*/
            foreach (TableDefine table in tables)
            {
                foreach (var col in table.Columns)
                {
                    col.TableName = table.Table;
                    col.variable?.ToUpper();
                }
            }
            var types = tables.SelectMany(ii => ii.Columns.Select(ii2 => ii2.Type)).Distinct().ToArray();

            //            XmlFimi fim = new XmlFimi(content);
            //  var currentIndexes = paths.Select(ii => 0).ToArray();
            //   var ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;";
            //            var ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;SearchPath=dm;";

            //     string baseQuery = "select closedate from dm.card limit 10";
            if (variables != null)
            {




                try
                {
                    foreach (var var in variables)
                    {
                        foreach (var com in commands)
                        {
                            foreach (var par in com.Params.Where(ii => ii.Variable == var.Key))
                            {
                                if (var.Value.type == "Json")
                                    par.Value = JsonDocument.Parse(var.Value.value?.ToString());
                                else
                                    if(var.Value.value?.GetType()==typeof(DateTime))
                                      par.Value=((DateTime)var.Value.value).ToString("o");
                                    else
                                    par.Value = var.Value.value?.ToString() ?? "";
                            }
                        }
                    }
                } catch
                {
                    throw;
                }
            }
            var guid= Guid.NewGuid().ToString();
            retValue.OperUUID = guid;
            if (string.IsNullOrEmpty(baseQuery))
                baseQuery = "select 1 dummy;";
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            conn.Open();
            NpgsqlConnection connBase = new NpgsqlConnection(ConnString);
            connBase.Open();
            await executor.beginSessionAsync();
            await using (var cmdCommand = new NpgsqlCommand(baseQuery, connBase))
            {
                foreach (var patt in baseQuery.getVariablesForPattern())
                {
                    cmdCommand.Parameters.AddWithValue(patt, variables[patt.Substring(1)].value);
                }
                await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                {
                    while (await readerCom.ReadAsync())
                    {
                        // Initialize commands variables
                        for (int i = 0; i < readerCom.FieldCount; i++)
                        {
                            object val = null;
                            if (!readerCom.IsDBNull(i))
                                val = readerCom.GetValue(i);
                            var colName = readerCom.GetName(i)/*.ToUpper()*/;
                            foreach (var com in commands)
                            {
                                foreach (var par in com.Params.Where(ii => ii.Variable == colName))
                                {
                                    if (val != null)
                                        par.Value = val.ToString();
                                }
                            }
                        }
                        retValue.All++;
                        var filter = await executor.ExecAsync(commands);
                        if (filter == null)
                        {
                            var lastError = executor.getError();
                            await ReportError(ConnString, guid, processId, lastError.content, lastError.error, variables);
                            retValue.Errors++;

                        }
                        else
                        {
                            List<Item> paths = new List<Item>();
                            foreach (var tab in tables)
                            {
                                paths.AddRange(tab.Columns.Where(ii => ii.uid || !string.IsNullOrEmpty(ii.path) || !string.IsNullOrEmpty(ii.variable) || !string.IsNullOrEmpty(ii.constant)).Select(i1 => new Item() { columnName = i1.Name, tableName = i1.TableName, currentIndex = 0, columnType = i1.Type, path = i1.path, constant = i1.constant, variable = i1.variable, values = (i1.uid) ? (new string[1] { guid }) : filter.filter(i1.path)?.ToArray() }).OrderBy(i1 => i1.path));
                            }
                            foreach (var path in paths.Where(ii => !string.IsNullOrEmpty(ii.constant)))
                                path.values = new string[] { path.constant };

                            for (int i = 0; i < readerCom.FieldCount; i++)
                            {
                                object val = null;
                                if (!readerCom.IsDBNull(i))
                                    val = readerCom.GetValue(i);
                                var colName = readerCom.GetName(i);
                                foreach (var path in paths.Where(ii => ii.variable == colName))
                                    path.values = new string[] { val?.ToString() };
                            }
                            foreach (var var in variables)
                            {
                                foreach (var path in paths.Where(ii => ii.variable == var.Key))
                                    path.values = new string[] { var.Value.value.ToString() ?? "" };
                                /*                            foreach (var com in commands)
                                                            {
                                                                foreach (var par in com.Params.Where(ii => ii.Variable == var.Key))
                                                                    par.Value = var.Value.value.ToString() ?? "";
                                                            }*/
                            }

                            // Dictionary<string, string> variables = new Dictionary<string, string>();
                            foreach (var table in tables.OrderBy(ii => ii.ExtIDs.Count))
                            {
                                var maxIndex = paths.Where(ii => ii.tableName == table.Table).Max(ii => ii.values?.Length);
                                for (int currentIndex = 0; currentIndex < maxIndex; currentIndex++)
                                {
                                    bool exists = false;

                                    var commandText = $"select * from {table.Table} {WhereClause(db_types, paths, table)}";// + $"where {string.Join(" and ", table.KeyColumns.Select((val, index) => val + $"=cast(@P{index} as {db_types[paths.First(ii => ii.tableName == table.Table && ii.columnName == val).columnType]})"))}";
                                    await using (var cmd1 = new NpgsqlCommand(commandText, conn))
                                    {
                                        for (int i = 0; i < table.KeyColumns.Length; i++)
                                        {
                                            var path = paths.FirstOrDefault(ii => ii.tableName == table.Table && ii.columnName == table.KeyColumns[i]);
                                            if (path == null)
                                            {
                                                throw new Exception($"Value of column {table.KeyColumns[i]} on table {table.Table} not set");
                                            }
                                            cmd1.Parameters.AddWithValue($"@P{i}"/*, db_types[path.columnType]*/, path.values[calcCurrentIndex(currentIndex, path)]);
                                        }
                                        try
                                        {
                                            await using (var reader = await cmd1.ExecuteReaderAsync())
                                            {
                                                exists = reader.HasRows;
                                            }
                                        }
                                        catch (Exception ex)
                                        {

//                                            GenerateSQLError(cmd1, ex);
                                            await ReportError(ConnString, guid, processId, "select", DetailError(cmd1, ex), variables);
                                            goto nextCycle;
                                        }



                                    }
                                    if (!exists)
                                    {
                                        commandText = $"insert into  {table.Table}  ({string.Join(",", paths.Where(ii => ii.tableName == table.Table).Select(ii => ii.columnName))}) values ({string.Join(",", paths.Where(ii => ii.tableName == table.Table).Select((ii, index) => $"cast(@P{index} as {db_types[ii.columnType]})"))}) ";

                                        await using (var cmd1 = new NpgsqlCommand(commandText, conn))
                                        {

                                            int index = 0;
                                            foreach (var path in paths.Where(ii => ii.tableName == table.Table))
                                            {
                                                cmd1.Parameters.AddWithValue($"@P{index}"/*, db_types[path.columnType]*/, (object)path.values[calcCurrentIndex(currentIndex, path)] ?? DBNull.Value);
                                                index++;
                                            }
                                            try
                                            {
                                                var retCol = cmd1.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
  //                                              GenerateSQLError(cmd1, ex);
                                                await ReportError(ConnString, guid, processId, "insert", DetailError(cmd1, ex), variables);
                                                goto nextCycle;
                                            }


                                        }
                                    }
                                    else
                                    {
                                        /*await using (var cmd5 = new NpgsqlCommand(@"SELECT r.rolname, rs.setconfig
        FROM   pg_db_role_setting rs
        LEFT   JOIN pg_roles      r ON r.oid = rs.setrole
        WHERE  r.rolname ='dm'
        ", conn))
                                        {
                                            await using (var reader = await cmd5.ExecuteReaderAsync())
                                            {
                                                while (await reader.ReadAsync())
                                                {

                                                }
                                            }
                                        }*/

                                        int kolVar = paths.Where(ii => ii.tableName == table.Table).Count();
                                        commandText = $"UPDATE   {table.Table}  set  {string.Join(",", paths.Where(ii => ii.tableName == table.Table).Select((ii, index) => $"{ii.columnName}=CAST(@P{index} as {db_types[ii.columnType].ToString()})"))}   {WhereClause(db_types, paths, table, kolVar)}";
                                        await using (var cmd1 = new NpgsqlCommand(commandText, conn))
                                        {

                                            int index = 0;
                                            string out_par = "";
                                            foreach (var path in paths.Where(ii => ii.tableName == table.Table))
                                            {
                                                cmd1.Parameters.AddWithValue($"@P{index}"/*, db_types[path.columnType]*/, (object)path.values[path.currentIndex] ?? DBNull.Value);
                                                out_par += $"@P{index}={path.values[calcCurrentIndex(currentIndex, path)]}\r\n";
                                                index++;
                                            }
                                            for (int i = 0; i < table.KeyColumns.Length; i++)
                                            {
                                                var path = paths.First(ii => ii.tableName == table.Table && ii.columnName == table.KeyColumns[i]);
                                                cmd1.Parameters.AddWithValue($"@P{i + kolVar}"/*, db_types[path.columnType]*/, (object)path.values[path.currentIndex] ?? DBNull.Value);
                                                out_par += $"@P{i + kolVar}={path.values[calcCurrentIndex(currentIndex, path)]}\n";
                                            }
                                            try
                                            {
                                                var retCol = cmd1.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
//                                                GenerateSQLError(cmd1, ex);
                                                await ReportError(ConnString, guid, processId, "update", DetailError(cmd1, ex), variables);
                                                goto nextCycle;

                                            }

                                        }

                                    }
                                    //                var columns = tables.SelectMany(ii1 => ii1.ExtIDs).Where(ii => ii.Table == table.Table).Select(ii => new ItemIIDS() { columnName= ii.Column }).Distinct().ToList();
                                    List<ItemIIDS> columns = new List<ItemIIDS>();
                                    foreach (var tab in tables.Where(ii => ii.ExtIDs.Count(i1 => i1.Table == table.Table) > 0))
                                        columns.AddRange(tab.ExtIDs.Where(ii => ii.Table == table.Table).Select(ii => new ItemIIDS() { tableName = tab.Table, columnName = ii.Column }).Distinct().ToList());
                                    if (columns.Count > 0)
                                    {
                                        commandText = $"select {string.Join(',', columns.Select(ii => ii.columnName))} from {table.Table}  {WhereClause(db_types, paths, table)}";

                                        await using (var cmd1 = new NpgsqlCommand(commandText, conn))
                                        {
                                            for (int i = 0; i < table.KeyColumns.Length; i++)
                                            {
                                                var path = paths.First(ii => ii.tableName == table.Table && ii.columnName == table.KeyColumns[i]);
                                                cmd1.Parameters.AddWithValue($"@P{i}", db_types[path.columnType], (object)path.values[calcCurrentIndex(currentIndex, path)] ?? DBNull.Value);
                                            }
                                            try
                                            {
                                                await using (var reader = await cmd1.ExecuteReaderAsync())
                                                {
                                                    while (await reader.ReadAsync())
                                                    {
                                                        for (int i = 0; i < columns.Count; i++)
                                                        {
                                                            var val = reader.GetInt64(i);
                                                            var varName = columns[i].columnName;
                                                            var columnType = table.Columns.FirstOrDefault(ii => ii.Name == columns[i].columnName)?.Type;
                                                            if (columnType == null)
                                                            {
                                                                throw new Exception($"Column {columns[i].columnName} not present in {table.Table} definition , but present in external keys");
                                                            }

                                                            //                                    var path = paths.First(ii => ii.tableName == table.Table && ii.columnName == columns[i].columnName);
                                                            paths.Add(new Item() { currentIndex = 0, variable = varName, columnName = columns[i].columnName, columnType = columnType, tableName = columns[i].tableName, values = new string[] { val.ToString() } });
                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
  //                                              GenerateSQLError(cmd1, ex);
                                                await ReportError(ConnString, guid, processId, "select", DetailError(cmd1, ex), variables);
                                                goto nextCycle;
                                            }

                                        }
                                    }
                                nextCycle:
                                    ;
                                }
                            }
                        }
                    }
                }
            }
            await executor.endSessionAsync();

            conn.Close();
            connBase.Close();
            return retValue;
        }

        private static int calcCurrentIndex(int currentIndex, Item? path)
        {
            return ((currentIndex < path.values.Length) ? currentIndex : path.values.Length - 1);
        }

        private static string WhereClause(Dictionary<string, NpgsqlDbType> db_types, List<Item> paths, TableDefine? table, int beforeKol = 0)
        {
            if(table.KeyColumns.Length== 0) {
                return "";
            } else
            return $"where {string.Join(" and ", table.KeyColumns.Select((val, index) => val + $"=cast(@P{beforeKol + index} as {db_types[table.Columns.First(ii => ii.Name == val).Type]})"))}";
        }
        public async static Task<_ApiFilter> ExecuteApiRequestOnly(_ApiExecutor executor, ExecContextItem[] commands, string baseQuery = "select '2220000000000200' PAN", Dictionary<string, object> variables=null, string ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;")
        {
            _ApiFilter retValue = null;
            //                { "BLOB",NpgsqlTypes.NpgsqlDbType.: LargeBinary, "BYTEA": LargeBinary*/

            //            XmlFimi fim = new XmlFimi(content);
            //  var currentIndexes = paths.Select(ii => 0).ToArray();
            //   var ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;";
            //            var ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;SearchPath=dm;";

            //     string baseQuery = "select closedate from dm.card limit 10";
            if (string.IsNullOrEmpty(baseQuery))
                baseQuery = "select 1 dummy;";

            foreach (var var in variables)
            {
                foreach (var com in commands)
                {
                    foreach (var par in com.Params.Where(ii => ii.Variable == var.Key))
                    {
                        if (var.Value?.GetType() == typeof(JsonDocument))
                            par.Value = (JsonDocument)var.Value;
                        else
                            par.Value = var.Value?.ToString() ?? "";
                    }
                }
            }
            /*NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            conn.Open();*/
            NpgsqlConnection connBase = new NpgsqlConnection(ConnString);
            connBase.Open();
            await executor.beginSessionAsync();
            await using (var cmdCommand = new NpgsqlCommand(baseQuery, connBase))
            {
                foreach (var patt in baseQuery.getVariablesForPattern())
                {
                    cmdCommand.Parameters.AddWithValue(patt, variables[patt.Substring(1)]);
                }

                await using (var readerCom = await cmdCommand.ExecuteReaderAsync())
                {
                    while (await readerCom.ReadAsync())
                    {
                        // Initialize commands variables
                        for (int i = 0; i < readerCom.FieldCount; i++)
                        {
                            object val = null;
                            if (!readerCom.IsDBNull(i))
                                val = readerCom.GetValue(i);
                            var colName = readerCom.GetName(i)/*.ToUpper()*/;
                            foreach (var com in commands)
                            {
                                foreach (var par in com.Params.Where(ii => ii.Variable == colName))
                                {
                                    if(val != null)
                                    par.Value = val.ToString();
                                }
                            }
                        }
                        retValue = await executor.ExecAsync(commands);
                    }
                }
            }
                    await executor.endSessionAsync();

                    //            conn.Close();
                    connBase.Close();
                    return retValue;
                }
            
            static void GenerateSQLError(NpgsqlCommand command,Exception e)
        {
            throw new Exception(DetailError(command, e));
        }

        private static string DetailError(NpgsqlCommand command, Exception e)
        {
            return $"Error happend in statement:\n{command.CommandText} \nconn:{command.Connection.ConnectionString} \npar:\n{string.Join("\n", command.Parameters.Select(ii => ii.ParameterName + "=" + ii.Value))} \nerr:\n{e.Message}";
        }
    }

    public static class HelperReg
    {
        public static IEnumerable<string> getVariablesForPattern(this string str, string pat = "@\\b\\w+\\b")
        {
            List<string> allVars = new List<string>();
            /*            var str = " @a1=0 and @a3 = 6 and @a1<8";
                        var pat = "@\\b\\w+\\b";*/
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            Match m = r.Match(str);
            //   int matchCount = 0;
            while (m.Success)
            {
                var var1 = m.Groups[0].Captures[0].Value;
                if (!allVars.Contains(var1))
                {
                    //  Console.WriteLine("Match" + (++matchCount));
                    allVars.Add(var1);
                    yield return var1;
                }

                /*                int i = 0;
                                {
                                    Group g = m.Groups[i];
                                    Console.WriteLine("Group" + i + "='" + g + "'");

                                    var  cc = g.Captures[0].Value;
                                }*/
                m = m.NextMatch();
            }
        }

    }


}
