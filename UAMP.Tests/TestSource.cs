/******************************************************************
 * File: TestSource.cs
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

using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace UAMP.Tests
{
    internal static class TestSource
    {
        public static IEnumerable<TestCaseData> FileSource()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var encoding = Encoding.GetEncoding(866);
            foreach (var filename in new[]
                     {
                         "UAMP1.bin", "UAMP2.bin", "SUBUAMP.bin", "TIC124.bin", "TIC_124.bin", "TIC_124_duplicate.bin",
                         "recHis.bin"
                     }) // "UAMP3.bin", - not correct uamp

            {
                byte[] bytearray;
                string uampmessage;
                using (var file = File.Open($"TestData/{filename}", FileMode.Open))
                {
                    bytearray = new byte[file.Length];
                    var read = file.Read(bytearray);
                    Assert.AreEqual(bytearray.Length, read);
                    uampmessage = encoding.GetString(bytearray);
                }

                yield return new TestCaseData(uampmessage).SetName(filename[..^4]);
            }
        }
    }
}