/******************************************************************
 * File: UAMPScalerTests.cs
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
    [TestFixture(TestOf = typeof(UAMPScalar))]
    internal class UampScalerTests : UAMPBaseTypeTests
    {
        [Test]
        [Ignore("NotUsed")]
        public override void Parse()
        {
            throw new NotImplementedException();
        }

        [Test]
        [Ignore("NotUsed")]
        public override void NI()
        {
            UAMPScalar s = new(null);
            Assert.AreEqual(UAMPType.Scalar, s.Type);
            Assert.IsNull(s.Value);

            UAMPScalar s1 = $"{(char) Symbols.NI}";
            Assert.AreEqual(UAMPType.Scalar, s1.Type);
            Assert.IsNull(s1.Value);

            Assert.AreEqual($"{(char) Symbols.NI}", s.Serialize());
        }

        [Test]
        public override void Json()
        {
            UAMPScalar s = "scalar_value";
            var json = JsonSerializer.Serialize(s);
            var document = JsonDocument.Parse(json);
            document.RootElement.GetProperty("Type");
            document.RootElement.GetProperty("Value");
            Assert.AreEqual(UAMPType.Scalar, (UAMPType) document.RootElement.GetProperty("Type").GetByte());
            Assert.AreEqual("scalar_value", document.RootElement.GetProperty("Value").GetString());
        }

        [Test]
        public override void BuildObject()
        {
            // Check Type
            UAMPScalar uampScalar = "scaler_value";
            Assert.AreEqual(UAMPType.Scalar, uampScalar.Type);
            Assert.AreEqual("scaler_value", uampScalar.Value);
            // Check Implicity
            string s = uampScalar;
            Assert.AreEqual("scaler_value", s);
        }
    }
}