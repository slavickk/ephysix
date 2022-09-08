using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public class TestPostgres
    {
        public static async Task test()
        {
            var connString = "User ID=postgres;Password=test;Host=localhost;Port=5432;";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            var command = "CREATE TABLE IF NOT EXISTS public.T2(    id integer NOT NULL )";
            await using (var cmd = new NpgsqlCommand(command, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            // Insert some data
            await using (var cmd = new NpgsqlCommand("INSERT INTO data1 (some_field) VALUES (@p)", conn))
            {
                cmd.Parameters.AddWithValue("p", "Hello world");
                await cmd.ExecuteNonQueryAsync();
            }

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                    Console.WriteLine(reader.GetString(0));
            }
        }
    }
}
