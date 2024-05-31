/******************************************************************
 * File: UAMPMessageTest.cs
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
using System.Text.Json;
using NUnit.Framework;

namespace UAMP.Tests
{
    [TestFixture]
    internal class UAMPMessageTest
    {
        [TestCaseSource(typeof(TestSource), "FileSource")]
        public void Parse(string uampmessage)
        {
            var uampMessage = new UAMPMessage(uampmessage);
            var json_serialize = JsonSerializer.Serialize(uampMessage);
            var message = JsonSerializer.Deserialize<UAMPMessage>(json_serialize);
            Assert.AreEqual(uampmessage, message.Serialize());
        }

        [Test]
        public void ParseString()
        {
            var s = "CBA=12312|CC=0|IC=643|P2=123456789|TC=5|TCE=36|TIPA=45645|TL=2|TLC=ru";
            var uampMessage = new UAMPMessage(s);
            Assert.AreEqual(s, uampMessage.Serialize());

            var s1 = "CBA:12312|CC:0|IC:643|P2:123456789|TC:5|TCE:36|TIPA:45645|TL:2|TLC:ru";
            Assert.Catch<ArgumentException>(() => new UAMPMessage(s1));
        }
    }
}