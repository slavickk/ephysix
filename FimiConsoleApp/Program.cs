using CamundaInterface;
using Confluent.Kafka;
using Microsoft.OpenApi.Services;
using Namotion.Reflection;
using Npgsql;
using NpgsqlTypes;
using NUnit.Framework.Constraints;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace FimiConsoleApp
{


    internal class Program
    {
        static byte[] hashPWD(string password, string text)
        {
            var keyArray = UTF8Encoding.UTF8.GetBytes(password);
            return Encrypt3Des(text, keyArray);
        }


        private static byte[] EncryptDes(string text, byte[] keyArray)
        {
            var toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);
            DESCryptoServiceProvider tdes = new DESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format

            return resultArray;
            //            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        private static byte[] Encrypt3Des(string text, byte[] keyArray)
        {
            var toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            string hex = string.Join(" ", resultArray.Select(x => x.ToString("X2")));
            return resultArray;
            //            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public class Item
        {
            public string path;
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
        async static Task Main(string[] args)
        {
            await Send();
            // new FIMIHelper().Init();
            Dictionary<string, NpgsqlTypes.NpgsqlDbType> db_types = new Dictionary<string, NpgsqlTypes.NpgsqlDbType> {
                { "NUMBER", NpgsqlTypes.NpgsqlDbType.Numeric }, { "DECIMAL", NpgsqlTypes.NpgsqlDbType.Numeric },
                { "SMALLINT", NpgsqlTypes.NpgsqlDbType.Smallint },
                { "INT", NpgsqlTypes.NpgsqlDbType.Integer }, { "INTEGER", NpgsqlTypes.NpgsqlDbType.Integer }, { "BIGINT", NpgsqlTypes.NpgsqlDbType.Bigint }, { "BOOLEAN", NpgsqlTypes.NpgsqlDbType.Boolean },
                { "DATETIME", NpgsqlTypes.NpgsqlDbType.TimestampTZ }, { "FLOAT", NpgsqlTypes.NpgsqlDbType.Real },
                { "REAL", NpgsqlTypes.NpgsqlDbType.Real }, { "INTERVAL", NpgsqlTypes.NpgsqlDbType.Interval }, { "UNICODE", NpgsqlTypes.NpgsqlDbType.Text }, { "CLOB", NpgsqlTypes.NpgsqlDbType.Text },
                { "TEXT", NpgsqlTypes.NpgsqlDbType.Text }, { "CHAR", NpgsqlTypes.NpgsqlDbType.Varchar }, { "JSON", NpgsqlTypes.NpgsqlDbType.Varchar }, { "NCHAR", NpgsqlTypes.NpgsqlDbType.Varchar },
                { "NVARCHAR", NpgsqlTypes.NpgsqlDbType.Varchar }, { "VARCHAR", NpgsqlTypes.NpgsqlDbType.Varchar }, { "DATE", NpgsqlTypes.NpgsqlDbType.Date }, { "TIMESTAMP", NpgsqlTypes.NpgsqlDbType.Timestamp } };
            //                { "BLOB",NpgsqlTypes.NpgsqlDbType.: LargeBinary, "BYTEA": LargeBinary*/
            string content;
            using (StreamReader sr = new StreamReader("MultiTable.json"))
            {
                content = sr.ReadToEnd();
            }
            var tables = JsonSerializer.Deserialize<TableDefine[]>(content);
            APIExecutor ex = new APIExecutor();

            //??? Justify this!!! await ex.ExecuteApiRequest(new FimiXmlTransport(), new ExecContextItem[] { new ExecContextItem("GetCardInfo") { Params = new List<ExecContextItem.ItemParam> { new ExecContextItem.ItemParam() { Key = "PAN", Variable = "PAN" }, new ExecContextItem.ItemParam("RequiredData", "2047") } } }, tables);
            foreach (TableDefine table in tables)
            {
                foreach (var col in table.Columns)
                    col.TableName = table.Table;
            }
            var types = tables.SelectMany(ii => ii.Columns.Select(ii2 => ii2.Type)).Distinct().ToArray();
            using (StreamReader sr = new StreamReader(@"c:\d\Answer.xml"))
            {
                content = sr.ReadToEnd();
            }

            XmlFimi fim = new XmlFimi(content);
            List<Item> paths = new List<Item>();
            foreach (var tab in tables)
            {
                paths.AddRange(tab.Columns.Where(ii => !string.IsNullOrEmpty(ii.path)).Select(i1 => new Item() { columnName = i1.Name, tableName = i1.TableName, currentIndex = 0, columnType = i1.Type, path = i1.path, values = fim.extractMulti(i1.path).ToArray() }).OrderBy(i1 => i1.path));
            }
            var currentIndexes = paths.Select(ii => 0).ToArray();
            var ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;";
            //            var ConnString = "User ID=fp;Password=rav1234;Host=master.pgsqlanomaly01.service.dc1.consul;Port=5432;Database=fpdb;SearchPath=dm;";

            string baseQuery = "select closedate from dm.card limit 10";
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            conn.Open();
            NpgsqlConnection connBase = new NpgsqlConnection(ConnString);
            connBase.Open();
            await using (var cmd1 = new NpgsqlCommand(baseQuery, connBase))
            {
                await using (var reader = await cmd1.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object val = null;
                            if (!reader.IsDBNull(i))
                                val = reader.GetValue(i);
                            var colName = "$" + reader.GetName(i);
                            foreach (var path in paths.Where(ii => ii.path == colName))
                                path.values = new string[] { val?.ToString() };

                        }
                    }
                }
            }
            Dictionary<string, string> variables = new Dictionary<string, string>();
            foreach (var table in tables.OrderBy(ii => ii.Table))
            {
                bool exists;

                var commandText = $"select * from {table.Table} {WhereClause(db_types, paths, table)}";// + $"where {string.Join(" and ", table.KeyColumns.Select((val, index) => val + $"=cast(@P{index} as {db_types[paths.First(ii => ii.tableName == table.Table && ii.columnName == val).columnType]})"))}";

                await using (var cmd1 = new NpgsqlCommand(commandText, conn))
                {
                    for (int i = 0; i < table.KeyColumns.Length; i++)
                    {
                        var path = paths.First(ii => ii.tableName == table.Table && ii.columnName == table.KeyColumns[i]);
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
                                    var varName = "$" + columns[i].columnName;
                                    var columnType = table.Columns.First(ii => ii.Name == columns[i].columnName).Type;
                                    //                                    var path = paths.First(ii => ii.tableName == table.Table && ii.columnName == columns[i].columnName);
                                    paths.Add(new Item() { currentIndex = 0, path = varName, columnName = columns[i].columnName, columnType = columnType, tableName = columns[i].tableName, values = new string[] { val.ToString() } });
                                    /*if (variables.ContainsKey(varName))
                                        variables[varName] = val.ToString();
                                    else
                                        variables.Add(varName, val.ToString());
                                    */
                                    break;
                                }

                            }
                        }
                    }

                }
            }

            conn.Close();
            connBase.Close();
            //    file.Where()

            {
                var currentKey = "InitSession";
                FimiXmlTransport tr = new FimiXmlTransport();
                XmlFimi fimi = new XmlFimi();
                fimi.setPath("FIMI/InitSessionRq/Rq/NeedDicts", "0");
                fimi.setPath("FIMI/InitSessionRq/Rq/AllVendors", "0");
                fimi.setPath("FIMI/InitSessionRq/Rq/AvoidSession", "0");
                var ans = await tr.send(fimi, currentKey);
                //                var pwd =XmlFimi.GetChallengePassword(tr.NextChallenge/* ans.getPath("FIMI/InitSessionRp/Rp/@NextChallenge")*/);
                XmlFimi fimiLogon = new XmlFimi();
                /*              fimiLogon.setPath("FIMI/LogonRq/Rq/@Password", pwd);
                              fimiLogon.setPath("FIMI/LogonRq/Rq/@Session", ans.getPath("FIMI/InitSessionRp/Rp/Id"));*/
                var ans11 = await tr.send(fimiLogon, "Logon");
                //                pwd = XmlFimi.GetChallengePassword(tr.NextChallenge/*ans11.getPath("FIMI/LogonRp/Rp/@NextChallenge")*/);
                XmlFimi fimiRate = new XmlFimi();

                currentKey = "GetCardInfo";
                /*    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Password", pwd);
                    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Session", ans.getPath("FIMI/InitSessionRp/Rp/Id"));*/
                fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/PAN", "2220000000000200");
                fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/RequiredData", "2047");
                var ans12 = await tr.send(fimiRate, currentKey);
                XmlFimi fimiAccount = new XmlFimi();
                currentKey = "GetAcctInfo";
                /*    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Password", pwd);
                    fimiRate.setPath($"FIMI/{currentKey}Rq/Rq/@Session", ans.getPath("FIMI/InitSessionRp/Rp/Id"));*/
                fimiAccount.setPath($"FIMI/{currentKey}Rq/Rq/Account", "01021");
                var ans15 = await tr.send(fimiAccount, currentKey);
                ans12.extract("Accounts/Row/AcctNo", currentKey);
                var ans14 = await tr.send(new XmlFimi(), "Logoff");



            }



        }
        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }
        public static async Task   Send()
        {
            HttpClient client = new HttpClient();
            string request1 = @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""> <soap:Body>
<Tran xmlns=""http://schemas.tranzaxis.com/tran.wsdl"">
<tran:Request InitiatorRid=""akhramov_rtp"" LifePhase=""Single"" Kind=""ReadToken"" ProcessorInstName=""Test"" xmlns:tran=""http://schemas.tranzaxis.com/tran.xsd"" xmlns:tok=""http://schemas.tranzaxis.com/tokens-admin.xsd"">
<tran:Specific>
<tran:Admin ObjectMustExist=""true"">
<tran:Token> <tok:Card  Pan=""1234560000000009"">
<tok:ExpTime>2023-09-01T00:00:00.000</tok:ExpTime>
</tok:Card>
</tran:Token> </tran:Admin>
</tran:Specific> </tran:Request>
</Tran> </soap:Body> </soap:Envelope>
";// fimi.outerXml;
           // 00hf4k5W15    008g7b5W11
            request1 = @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""> <soap:Body>
<Tran xmlns=""http://schemas.tranzaxis.com/tran.wsdl""><tran:Request InitiatorRid=""akhramov_rtp"" LifePhase=""Single"" Kind=""GetContractInfo"" ProcessorInstName=""Test"" xmlns:tran=""http://schemas.tranzaxis.com/tran.xsd"">
<tran:Parties>
<tran:Cust ContractRid=""008g7b5W11""/>
</tran:Parties> <tran:Specific>
<tran:CustInfo Kinds=""ContractRid ContractCcy ContractAvailBalance ContractStatus ContractOwnerRid ContractOwnerTitle ContractTypeId ContractTitle ContractTypeTitle C2CContract2Rid ContractBranchId BranchInstId"" Language=""en""/> </tran:Specific>
</tran:Request></Tran> </soap:Body> </soap:Envelope>";

/*            request1 = @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""> <soap:Body>
<Tran xmlns=""http://schemas.tranzaxis.com/tran.wsdl"">
<tran:Request InitiatorRid=""akhramov_rtp"" LifePhase=""Single"" Kind=""ReadSubject"" ProcessorInstName=""Test"" xmlns:tran=""http://schemas.tranzaxis.com/tran.xsd"" xmlns:sub=""http://schemas.tranzaxis.com/subjects-admin.xsd"">
<tran:Specific>
<tran:Admin ObjectMustExist=""true"">
<tran:Subject> <sub:Person>
<sub:Rid>potentialIP</sub:Rid> </sub:Person>
</tran:Subject> </tran:Admin>
</tran:Specific> </tran:Request>
</Tran> </soap:Body> </soap:Envelope>";
*/

            //<tok:ExtRid>12345</tok:ExtRid> </tok:Card>
            //<tok:ExpTime>2030-01-01T00:00:00.000</tok:ExpTime>
            /*var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(request1);
            xmlDoc.Validate(ValidationEventHandler);
*/
            /*var path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", path + "\\input.xsd");
            XmlReader rd = XmlReader.Create(path + "\\input.xml");
            XDocument doc = XDocument.loa.Load(rd);
            doc.Validate(schema, ValidationEventHandler);
            */
            StringContent httpContent = new StringContent(request1, System.Text.Encoding.UTF8, "application/xml");
            var addr = "http://10.74.28.40:25404";
            var ans = await client.PostAsync(addr, httpContent);
            //.PostAsJsonAsync($"{camundaPath}external-task/fetchAndLock", new ItemFetchAndLock() { maxTasks = 1, usePriority = true, workerId = workerId, topics = topics.Select(ii => new ItemFetchAndLock.Topic() { lockDuration = 100000, topicName = ii }).ToList() });
            if (ans.IsSuccessStatusCode)
            {
                var ret = new XmlFimi(await ans.Content.ReadAsStringAsync());

            }
            else
            {
                var ret = new XmlFimi(await ans.Content.ReadAsStringAsync());
                //var errorContent = await ans.Content.ReadAsStringAsync();
                XmlSerializer ser = new XmlSerializer(typeof(Envelope));

                return ;
            }


        }

        private static string WhereClause(Dictionary<string, NpgsqlDbType> db_types, List<Item> paths, TableDefine? table,int beforeKol=0)
        {
            return $"where {string.Join(" and ", table.KeyColumns.Select((val, index) => val + $"=cast(@P{beforeKol+index} as {db_types[paths.First(ii => ii.tableName == table.Table && ii.columnName == val).columnType]})"))}";
        }
    }
}