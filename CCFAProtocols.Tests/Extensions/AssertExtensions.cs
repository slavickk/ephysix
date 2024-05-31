/******************************************************************
 * File: AssertExtensions.cs
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

using System.IO;
using NUnit.Framework;

namespace CCFAProtocols.Tests.Extensions
{
    public static class AssertExtensions
    {
        public static void StreamEquals(Stream expect, Stream actual)
        {
            if (expect == actual) return;

            if (expect is null || actual is null)
                throw new AssertionException(expect is null ? "Expect is null" : "Actual is null");


            expect.Seek(0, SeekOrigin.Begin);
            actual.Seek(0, SeekOrigin.Begin);
            if (expect.Length != actual.Length)
                throw new AssertionException(
                    $"Stream Length are not equals. expect:{expect.Length} actual:{actual.Length}");

            for (var i = 0; i < expect.Length; i++)
            {
                var s1byte = expect.ReadByte();
                var s2byte = actual.ReadByte();
                if (s1byte != s2byte)
                    throw new AssertionException(
                        $"Streams are different. Position:{i} expect:{s1byte} actual:{s2byte}");
            }
        }
    }
}