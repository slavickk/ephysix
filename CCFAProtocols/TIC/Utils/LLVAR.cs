/******************************************************************
 * File: LLVAR.cs
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
using System.Text;

namespace CCFAProtocols.TIC.Instruments
{
    public static class LVAR
    {
        private static readonly Encoding encoding = Encoding.GetEncoding(866);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="charsLength">Count of length chars <example>LLVAR-2 LLLVAR-3 LLLLLVAR-5</example>></param>
        /// <returns></returns>
        private static byte[] Deserialize(BinaryReader reader, byte charsLength)
        {
            char[] readChars = reader.ReadChars(charsLength);
            var length = int.Parse(readChars);
            return reader.ReadBytes(length);
        }

        private static void Serialize(BinaryWriter writer, byte charsLength, byte[] chars)
        {
            writer.Write(chars.Length.ToString().PadLeft(charsLength, '0').ToCharArray());
            writer.Write(chars);
        }

        /// <summary>
        /// Read LVAR String
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="CharsLength">Count of length chars <example>LLVAR-2 LLLVAR-3 LLLLLVAR-5</example>></param>
        /// <returns></returns>
        public static string? LVARReadString(this BinaryReader reader, byte length)
        {
            byte[] value = Deserialize(reader, length);
            if (value.Length == 0) return null;

            return encoding.GetString(value);
        }

        public static void LVARWriteString(this BinaryWriter writer, byte length, string? obj)
        {
            Serialize(writer, length, encoding.GetBytes(obj));
        }

        public static byte[] LVARReadBytes(this BinaryReader reader, byte length)
        {
            return Deserialize(reader, length);
        }

        public static void LVARWriteBytes(this BinaryWriter writer, byte length, byte[]? obj)
        {
            Serialize(writer, length, obj);
        }

        public static ulong LVARReadUlong(this BinaryReader reader, byte length)
        {
            byte[] value = Deserialize(reader, length);
            return ulong.Parse(encoding.GetString(value));
        }

        public static void LVARWriteUlong(this BinaryWriter writer, byte length, ulong? obj)
        {
            Serialize(writer, length, encoding.GetBytes(obj.ToString()));
        }

        public static uint LVARReadUInt(this BinaryReader reader, byte length)
        {
            byte[] value = Deserialize(reader, length);
            return uint.Parse(encoding.GetString(value));
        }

        public static void LVARWriteUInt(this BinaryWriter writer, byte length, uint? obj)
        {
            Serialize(writer, length, encoding.GetBytes(obj.ToString()));
        }
    }
}