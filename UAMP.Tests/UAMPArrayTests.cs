/******************************************************************
 * File: UAMPArrayTests.cs
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

using System.Linq;
using System.Text.Json;
using NUnit.Framework;

namespace UAMP.Tests
{
    [TestFixture(TestOf = typeof(UAMPArray))]
    internal class UAMPArrayTests : UAMPBaseTypeTests
    {
        [Test]
        public override void Parse()
        {
            var serializedArrRec = string.Join((char) Symbols.IS,
                new UAMPStruct("struct11", "struct12", "struct13").Serialize(),
                new UAMPStruct("struct21", "struct22", "struct23").Serialize(),
                new UAMPStruct("struct31", "struct32", "struct33").Serialize());
            var uarRec = new UAMPArray(serializedArrRec);
            Assert.AreEqual(UAMPType.Array, uarRec.Type);
            Assert.IsTrue(uarRec.Value.All(value => value?.Type == UAMPType.Struct));

            var serializedArrStr = string.Join((char) Symbols.IS, "item1", "item2", "item3");
            var uarrStr = new UAMPArray(serializedArrStr);
            Assert.AreEqual(UAMPType.Array, uarrStr.Type);
            Assert.IsTrue(uarrStr.Value.All(value => value?.Type == UAMPType.Scalar));
        }

        [Test]
        public override void NI()
        {
            var serializedArrRec = string.Join((char) Symbols.IS,
                new UAMPStruct("struct11", "struct12", "struct13").Serialize(),
                $"{(char) Symbols.NI}",
                new UAMPStruct("struct31", "struct32", "struct33").Serialize());
            var uarRec = new UAMPArray(serializedArrRec);
            Assert.AreEqual(UAMPType.Array, uarRec.Type);
            Assert.IsNull(uarRec.Value[1]);
            Assert.AreEqual(UAMPType.Struct, uarRec.Value[0].Type);
            Assert.AreEqual(UAMPType.Struct, uarRec.Value[2].Type);
            Assert.AreEqual((char) Symbols.NI, uarRec.Serialize()[27]);
        }

        [Test]
        public override void Json()
        {
            var uarRec = new UAMPArray(new UAMPStruct("struct_11", "struct_12", "struct_13"),
                null,
                new UAMPStruct("struct_31", "struct_32", "struct_33"));
            var serialize = JsonSerializer.Serialize(uarRec);
            var array = JsonSerializer.Deserialize<UAMPArray>(serialize);
            Assert.AreEqual(uarRec, array);
        }

        [Test]
        public override void BuildObject()
        {
            var uarRec = new UAMPArray(new UAMPStruct("struct_11", "struct_12", "struct_13"),
                new UAMPStruct("struct_21", "struct_22", "struct_23"),
                new UAMPStruct("struct_31", "struct_32", "struct_33"));
            Assert.AreEqual(UAMPType.Array, uarRec.Type);
            Assert.IsTrue(uarRec.Value.All(value => value?.Type == UAMPType.Struct));

            var uarrStr = new UAMPArray("item1", "item2", "item3");
            Assert.AreEqual(UAMPType.Array, uarrStr.Type);
            Assert.IsTrue(uarrStr.Value.All(value => value?.Type == UAMPType.Scalar));
        }
    }
}