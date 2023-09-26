using Npgsql;
using ParserLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CamundaInterface.SendToRefDataLoader;

namespace CamundaInterface
{
    public class url_crowler
    {
        public static async Task<ExportItem> execGet(HttpClient client, string connectionStringBase = "User ID=postgres;Password=test;Host=localhost;Port=5432;", string connectionStringAdmin = "User ID=fp;Password=rav1234;Host=192.168.75.220;Port=5432;Database=fpdb;"
            ,string TableName=""
         ,string URL = "People", string SQL = "TEST"
     , int UpdateTimeout = 1)
        {
            ExportItem retValue = new ExportItem();
            NpgsqlConnection conn=null, connAdm=null;
            try
            {
                //            client.BaseAddress=new Uri(baseAddr);
                //            var client = new HttpClient() { BaseAddress = new Uri(baseAddr) };
                var separator = ",";


                conn = new NpgsqlConnection(connectionStringBase);
                conn.Open();

                // Check Hash
                connAdm = conn;
                if (connectionStringBase != connectionStringAdmin)
                {
                    connAdm = new NpgsqlConnection(connectionStringAdmin);
                    connAdm.Open();
                }

                await using (var cmd = new NpgsqlCommand(@"select* from md_check_update_time(@table_name, @interval)", connAdm))
                {
                    cmd.Parameters.AddWithValue("@table_name", TableName.ToLower());
                    cmd.Parameters.AddWithValue("@interval", NpgsqlTypes.NpgsqlDbType.Bigint, UpdateTimeout);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (!reader.GetBoolean(0))
                            {

                                return FinishStep(retValue, conn, connAdm);

                            }
                        }
                    }
                }
                string body= await client.GetStringAsync( URL);

                await using (var cmd1 = new NpgsqlCommand($"delete from {TableName};", conn))
                {
                    cmd1.ExecuteNonQuery();
                }

                // CatchBlock()

//                List<ItemType> fields = new List<ItemType>();

                await using (var cmd = new NpgsqlCommand($"insert into  {TableName}  {SQL}", conn))
                {
                    cmd.Parameters.AddWithValue("@body",body);
                    retValue.all++;

                    cmd.ExecuteNonQuery();
                }


                return FinishStep(retValue, conn, connAdm);
            }
            catch
            {
                retValue.errors++;
                FinishStep(retValue, conn, connAdm);
                throw;
            }
        }

        private static ExportItem FinishStep(ExportItem retValue, NpgsqlConnection conn, NpgsqlConnection connAdm)
        {
            if(conn != null)
            conn.Close();
            if (connAdm != conn && connAdm != null)
                connAdm.Close();

            return retValue;
        }
    }
}
