/******************************************************************
 * File: TestPostgres.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

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
