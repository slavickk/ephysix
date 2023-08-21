using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CamundaInterface
{
    public  class APIExecutor
    {
        public interface _ApiExecutor
        {
            Task beginSessionAsync();
            Task<_ApiFilter> ExecAsync(ExecContextItem[] commands);
            EnvelopeBodyFault getError();
            Task endSessionAsync();
        }
        public interface _ApiFilter
        {
            string[] filter(string path);
        }

        public class ExecContextItem
            {
                public class ItemParam
                {
                    public string Key; 
                    public string? Value;
                    public string? Variable;

                public ItemParam()
                {

                }
                public ItemParam(string key, string value)
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

        public async Task<bool> ExecuteApiRequest(_ApiExecutor executor, ExecContextItem[] commands, TableDefine[] tables, string baseQuery = "select '2220000000000200' PAN", string ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;")
        {
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
            if (string.IsNullOrEmpty(baseQuery))
                baseQuery = "select 1 dummy;";
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            conn.Open();
            NpgsqlConnection connBase = new NpgsqlConnection(ConnString);
            connBase.Open();
            await executor.beginSessionAsync();
            await using (var cmdCommand = new NpgsqlCommand(baseQuery, connBase))
            {
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
                            var colName = readerCom.GetName(i).ToUpper();
                            foreach(var com in commands )
                            {
                                foreach (var par in com.Params.Where(ii => ii.Variable == colName))
                                    par.Value = val.ToString();
                            }
                        }
                        var filter = await executor.ExecAsync(commands);
                        List<Item> paths = new List<Item>();
                        foreach (var tab in tables)
                        {
                            paths.AddRange(tab.Columns.Where(ii => !string.IsNullOrEmpty(ii.path) || !string.IsNullOrEmpty(ii.variable) || ii.constant != null).Select(i1 => new Item() { columnName = i1.Name, tableName = i1.TableName, currentIndex = 0, columnType = i1.Type, path = i1.path, constant=i1.constant, variable=i1.variable, values = filter.filter(i1.path)?.ToArray() }).OrderBy(i1 => i1.path));
                        }
                        foreach (var path in paths.Where(ii => ii.constant!= null))
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
                        // Dictionary<string, string> variables = new Dictionary<string, string>();
                        foreach (var table in tables.OrderBy(ii => ii.ExtIDs.Count))
                        {
                            bool exists;

                            var commandText = $"select * from {table.Table} {WhereClause(db_types, paths, table)}";// + $"where {string.Join(" and ", table.KeyColumns.Select((val, index) => val + $"=cast(@P{index} as {db_types[paths.First(ii => ii.tableName == table.Table && ii.columnName == val).columnType]})"))}";

                            await using (var cmd1 = new NpgsqlCommand(commandText, conn))
                            {
                                for (int i = 0; i < table.KeyColumns.Length; i++)
                                {
                                    var path = paths.FirstOrDefault(ii => ii.tableName == table.Table && ii.columnName == table.KeyColumns[i]);
                                    if(path == null)
                                    {
                                        throw new Exception($"Value of column {table.KeyColumns[i]} on table {table.Table} not set");
                                    }
                                    cmd1.Parameters.AddWithValue($"@P{i}"/*, db_types[path.columnType]*/, path.values[path.currentIndex]);
                                }
                                await using (var reader = await cmd1.ExecuteReaderAsync())
                                {
                                    exists = reader.HasRows;
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
                                        cmd1.Parameters.AddWithValue($"@P{index}"/*, db_types[path.columnType]*/, (object)path.values[path.currentIndex] ?? DBNull.Value);
                                        index++;
                                    }
                                    var retCol = cmd1.ExecuteNonQuery();


                                }
                            }
                            else
                            {
                                int kolVar = paths.Where(ii => ii.tableName == table.Table).Count();
                                commandText = $"UPDATE   {table.Table}  set  {string.Join(",", paths.Where(ii => ii.tableName == table.Table).Select((ii, index) => $"{ii.columnName}=CAST(@P{index} as {db_types[ii.columnType].ToString()})"))}   {WhereClause(db_types, paths, table, kolVar)}";
                                await using (var cmd1 = new NpgsqlCommand(commandText, conn))
                                {

                                    int index = 0;
                                    string out_par = "";
                                    foreach (var path in paths.Where(ii => ii.tableName == table.Table))
                                    {
                                        cmd1.Parameters.AddWithValue($"@P{index}"/*, db_types[path.columnType]*/, (object)path.values[path.currentIndex] ?? DBNull.Value);
                                        out_par += $"@P{index}={path.values[path.currentIndex]}\r\n";
                                        index++;
                                    }
                                    for (int i = 0; i < table.KeyColumns.Length; i++)
                                    {
                                        var path = paths.First(ii => ii.tableName == table.Table && ii.columnName == table.KeyColumns[i]);
                                        cmd1.Parameters.AddWithValue($"@P{i + kolVar}"/*, db_types[path.columnType]*/, (object)path.values[path.currentIndex] ?? DBNull.Value);
                                        out_par += $"@P{i + kolVar}={path.values[path.currentIndex]}\n";
                                    }

                                    var retCol = cmd1.ExecuteNonQuery();

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
                                        cmd1.Parameters.AddWithValue($"@P{i}", db_types[path.columnType], (object)path.values[path.currentIndex] ?? DBNull.Value);
                                    }
                                    await using (var reader = await cmd1.ExecuteReaderAsync())
                                    {
                                        while (await reader.ReadAsync())
                                        {
                                            for (int i = 0; i < columns.Count; i++)
                                            {
                                                var val = reader.GetInt64(i);
                                                var varName = columns[i].columnName;
                                                var columnType = table.Columns.FirstOrDefault(ii => ii.Name == columns[i].columnName)?.Type;
                                                if(columnType== null)
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

                            }
                        }
                    }
                }
            }
            await executor.endSessionAsync();

            conn.Close();
            connBase.Close();
            return true;
        }

        private static string WhereClause(Dictionary<string, NpgsqlDbType> db_types, List<Item> paths, TableDefine? table, int beforeKol = 0)
        {
            if(table.KeyColumns.Length== 0) {
                return "";
            } else
            return $"where {string.Join(" and ", table.KeyColumns.Select((val, index) => val + $"=cast(@P{beforeKol + index} as {db_types[table.Columns.First(ii => ii.Name == val).Type]})"))}";
        }
        public async static Task<_ApiFilter> ExecuteApiRequestOnly(_ApiExecutor executor, ExecContextItem[] commands, string baseQuery = "select '2220000000000200' PAN", string ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;")
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
            /*NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            conn.Open();*/
            NpgsqlConnection connBase = new NpgsqlConnection(ConnString);
            connBase.Open();
            await executor.beginSessionAsync();
            await using (var cmdCommand = new NpgsqlCommand(baseQuery, connBase))
            {
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
                                    par.Value = val.ToString();
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
            

    }
}
