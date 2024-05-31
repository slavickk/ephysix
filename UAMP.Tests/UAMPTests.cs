/******************************************************************
 * File: UAMPTests.cs
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
using NUnit.Framework;

namespace UAMP.Tests
{
    [TestFixture]
    internal class TestUAMP : UAMPBaseTypeTests
    {
        [TestCaseSource(typeof(TestSource), "FileSource", Category = "files")]
        public void ParseFromFiles(string uampmessage)
        {
            var messages = new UAMPMessage(uampmessage);
            Console.WriteLine(messages);
            var serialize = messages.Serialize();
            Assert.AreEqual(uampmessage, serialize);
        }

        [Test]
        public void ParseExample()
        {
            var messages = new UAMPMessage("RCC=643\u0010TPH=3400000\u0010CCC=1\u0010CAT=0");
        }


        public override void Parse()
        {
            throw new NotImplementedException();
        }

        public override void NI()
        {
            throw new NotImplementedException();
        }

        public override void Json()
        {
            throw new NotImplementedException();
        }

        public override void BuildObject()
        {
            throw new NotImplementedException();
        }
    }
}