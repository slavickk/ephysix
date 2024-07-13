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
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Npgsql;
using Testcontainers.PostgreSql;

namespace ParserLibrary.Tests
{
    [TestFixture]
    public class TestPostgresTest
    {
        private PostgreSqlContainer _postgresContainer;
        
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithDatabase("my_database")
                .Build();
            
            await _postgresContainer.StartAsync();
        }
        
        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await _postgresContainer.DisposeAsync();
        }
            
        [Test]
        public async Task Begin()
        {
            var connString = _postgresContainer.GetConnectionString();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            const string command = "CREATE TABLE IF NOT EXISTS public.data1(id integer NOT NULL, some_field varchar(20))";
            await using (var cmd = new NpgsqlCommand(command, conn))
                await cmd.ExecuteNonQueryAsync();

            // Insert some data
            await using (var cmd = new NpgsqlCommand("INSERT INTO data1 (id, some_field) VALUES (@id, @p)", conn))
            {
                cmd.Parameters.AddWithValue("id", 123);
                cmd.Parameters.AddWithValue("p", "Hello world");
                await cmd.ExecuteNonQueryAsync();
            }

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("SELECT some_field FROM data1", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                    Console.WriteLine(reader.GetString(0));
            }
        }
    }
}
