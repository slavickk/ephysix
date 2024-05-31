/******************************************************************
 * File: BinaryReaderExtensions.cs
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
using System.Globalization;
using System.IO;

namespace CCFAProtocols.TIC.Instruments
{
    public static class BinaryReaderExtensions
    {
        public static string ReadString(this BinaryReader reader, int length)
        {
            return new string(reader.ReadChars(length)).Trim();
        }

        public static ushort ReadUshort(this BinaryReader reader, int length)
        {
            char[] chars = reader.ReadChars(length);
            try
            {
                return ushort.Parse(chars);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Cannot cast to Ushort: [{string.Join(',', chars)}]", e);
            }
        }

        public static ulong ReadUlong(this BinaryReader reader, int length)
        {
            char[] chars = reader.ReadChars(length);
            try
            {
                return ulong.Parse(chars);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Cannot cast to Ulong: [{string.Join(',', chars)}]", e);
            }
        }

        public static long ReadLong(this BinaryReader reader, int length)
        {
            char[] chars = reader.ReadChars(length);
            try
            {
                return long.Parse(chars);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Cannot cast to Long: [{string.Join(',', chars)}]", e);
            }
        }

        public static uint ReadUint(this BinaryReader reader, int length)
        {
            char[] chars = reader.ReadChars(length);
            try
            {
                return uint.Parse(chars);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Cannot cast to Uint: [{string.Join(',', chars)}]", e);
            }
        }

        public static byte ReadByte(this BinaryReader reader, int length)
        {
            char[] chars = reader.ReadChars(length);

            try
            {
                return byte.Parse(chars);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Cannot cast to Byte: [{string.Join(',', chars)}]", e);
            }
        }

        public static DateTime ReadDateTime(this BinaryReader reader, string format)
        {
            char[] chars = reader.ReadChars(format.Length);
            try
            {
                return DateTime.ParseExact(chars, format,
                    CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Cannot cast to DateTime: [{string.Join(',', chars)}]", e);
            }
        }

        public static short ReadShort(this BinaryReader reader, int length)
        {
            char[] chars = reader.ReadChars(length);
            try
            {
                return short.Parse(chars);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Cannot cast to short: [{string.Join(',', chars)}]", e);
            }
        }
    }
}