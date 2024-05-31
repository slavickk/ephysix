/******************************************************************
 * File: BinaryWriterExtensions.cs
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
using System.IO;
using System.Linq;

namespace CCFAProtocols.TIC.Instruments
{
    internal static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, int length, object obj)
        {
            string s = obj switch
            {
                string sobj => sobj.PadRight(length),
                ushort or uint or ulong or uint or byte => obj.ToString().PadLeft(length, '0'),
                long lobj => lobj.ToString(lobj < 0 ? $"D{length - 1}" : $"D{length}"),
                short sobj => sobj.ToString(sobj < 0 ? $"D{length - 1}" : $"D{length}"),
                null => "null",
                _ => throw new ArgumentException($"Type is not valid:{obj.GetType()}", nameof(obj))
            };
            writer.Write(s.ToArray());
        }
    }
}