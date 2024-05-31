/******************************************************************
 * File: TestBoolArrayComparer.cs
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

using CCFAProtocols.TIC.Instruments;
using NUnit.Framework;

namespace CCFAProtocols.Tests.TIC
{
    [TestFixture]
    public class TestBoolArrayComparer
    {
        private readonly BoolArrayEquilityComparer comparer = new();

        [Test]
        public void TestEquals()
        {
            Assert.IsTrue(comparer.Equals(null, null));
            Assert.IsTrue(comparer.Equals(new[] {true, false}, new[] {true, false}));
            Assert.IsFalse(comparer.Equals(new[] {true, false}, new[] {true, true}));
            Assert.IsFalse(comparer.Equals(new[] {true, false}, null));
            Assert.IsFalse(comparer.Equals(null, new[] {true, true}));
        }

        [Test]
        public void TestGetHashCode()
        {
            Assert.AreEqual(comparer.GetHashCode(new[] {true, false}),
                comparer.GetHashCode(new[] {true, false}));
            Assert.AreNotEqual(comparer.GetHashCode(new[] {true, true}),
                comparer.GetHashCode(new[] {true, false}));
        }
    }
}