using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamundaInterface
{
    public  class CSVExecutor
    {

        public static void Test()
        {
            string connectionString = "User ID=fp;Password=rav1234;Host=master.pgfp01.service.dev-fp.consul;Port=5432;Database=fpdb;SearchPath=fp;";
            NpgsqlConnection conn= new NpgsqlConnection(connectionString);
            conn.Open();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {

                DetectDelimiter = true,
                WhiteSpaceChars = new[] { ' ', '\t' }, // Note \t, otherwise it won't be trimmed.
                TrimOptions = TrimOptions.Trim//.InsideQuotes
            };
            string tableName = "tmp_asd234556";
            //            TrimOptions.
            //          configuration..UseNewObjectForNullReferenceMembers
            //   var conf =new CsvConfiguration()
            using (var reader = new StreamReader(@"C:\D\latencies.csv"))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Read();
                csv.ReadHeader();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"drop table {tableName}", conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand($"create table {tableName} ({string.Join(",", csv.HeaderRecord.Select(ii=>ii+" varchar"))})", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand($"alter table {tableName}\r\n             owner to fp ", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                
                int i = 0;
                while (csv.Read())
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand($"insert into {tableName} ({string.Join(",", csv.HeaderRecord)}) values({string.Join(",",csv.HeaderRecord.Select((ii,index)=> "'"+csv.GetField(index)+"'"))})", conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("executed " + (++i));
                    }

                }
                //                var records = csv.GetRecords<Foo>();
            }

            conn.Close();
        }
    }
}
