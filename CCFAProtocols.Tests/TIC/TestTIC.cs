/******************************************************************
 * File: TestTIC.cs
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

using System;
using System.Collections;
using System.IO;
using System.Text;
using CCFAProtocols.Tests.Extensions;
using CCFAProtocols.TIC;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using Serilog;

namespace CCFAProtocols.Tests.TIC
{
    [TestFixture]
    public class TICTest
    {
        [OneTimeSetUp]
        public void SetEncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("module", "TEST")
                .CreateLogger();
        }

        [Test]
        [TestCaseSource(nameof(FileTestCases), Category = "File")]
        [TestCaseSource(nameof(DBTestCases), Category = "DB")]
        [Parallelizable]
        public void MessageSerializationTest(Stream original)
        {
            using var reader = new BinaryReader(original, Encoding.GetEncoding(866));

            var ticMessage = TICMessage.Deserialize(reader);
            Console.WriteLine(ticMessage);


            MemoryStream current = new();

            using var tStream = new MemoryStream();
            using var twriter = new BinaryWriter(tStream, Encoding.GetEncoding(866), true);

            ticMessage.Serialize(twriter);

            tStream.WriteTo(current);
            AssertExtensions.StreamEquals(original, current);
        }

        [Test]
        [TestCaseSource(nameof(FileTestCases), Category = "File")]
        [TestCaseSource(nameof(DBTestCases), Category = "DB")]
        [Parallelizable]
        public void MessageJsonSerializationTest(Stream original)
        {
            using var reader = new BinaryReader(original, Encoding.GetEncoding(866));
            var ticMessage = TICMessage.DeserializeToJSON(reader);
            MemoryStream current = new();
            using var tStream = new MemoryStream();
            using var twriter = new BinaryWriter(tStream, Encoding.GetEncoding(866), true);
            TICMessage.SerializeFromJson(twriter, ticMessage);
            tStream.WriteTo(current);
            AssertExtensions.StreamEquals(original, current);
        }

        [Test]
        [TestCaseSource(nameof(FileTestCases), Category = "File")]
        [TestCaseSource(nameof(DBTestCases), Category = "DB")]
        [Parallelizable]
        public void CheckBitMaps(Stream original)
        {
            using var reader = new BinaryReader(original, Encoding.GetEncoding(866));
            var ticMessage = TICMessage.Deserialize(reader);

            var primaryBitMap = ticMessage.Fields.PrimaryBitMap;
            var secondaryBitMap = ticMessage.Fields.SecondaryBitMap;
            var jsonTicMessage = ticMessage.ToJSON();
            var resTicMessage = TICMessage.FromJSON(jsonTicMessage);
            Assert.AreEqual(primaryBitMap, resTicMessage.Fields.PrimaryBitMap, "PrimaryBitMap");
            Assert.AreEqual(secondaryBitMap, resTicMessage.Fields.SecondaryBitMap, "SecondaryBitMap");
        }

        public static IEnumerable DBTestCases()
        {
            var connection = new SqliteConnection("Filename=TestData/TEST.sqlite");
            connection.Open();
            var cmd = new SqliteCommand("Select id,MESS from TIC", connection);
            var reader = cmd.ExecuteReader();
            foreach (var res in reader)
                yield return new TestCaseData(reader.GetStream(1)).SetProperty("source", "db")
                    .SetName($@"ID:{reader["ID"]}");

            connection.Close();
        }

        public static IEnumerable FileTestCases()
        {
            foreach (var s in new[] { "100", "200", "400", "echo", "TIC_124" })
                using (var file = File.Open($"TestData/test{s}.tic", FileMode.Open))
                {
                    file.ReadByte();
                    file.ReadByte();
                    MemoryStream stream = new();
                    file.CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    yield return new TestCaseData(stream).SetProperty("source", "file").SetName(s);
                }
        }
    }
}