/******************************************************************
 * File: UAMPPackageTests.cs
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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NUnit.Framework;
using UAMP;

namespace UAMP.Tests
{
    [TestFixture]
    public class UAMPTests
    {

        [TestCaseSource("FileTestCases")]
        public void Deserialize(string message,string filename)
        {
            UAMPPackage uampValue = new UAMPPackage(message);
            Assert.AreEqual(message,uampValue.Serialize());
        }
        public static  IEnumerable FileTestCases()
        {
            List<string> paths = new() { "UPDATE.acq","UPDATE.iss" };
            foreach (string path in paths)
            {
                using (StreamReader reader=new StreamReader(Path.Combine("TestData",path)))
                {
                    yield return new TestCaseData(reader.ReadToEnd(),path).SetProperty("source", "file")
                        .SetName($@"File:{path}");
                }
                
                
            }
        }
    }
    
}