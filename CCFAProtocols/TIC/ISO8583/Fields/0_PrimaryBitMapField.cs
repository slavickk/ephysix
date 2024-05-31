/******************************************************************
 * File: 0_PrimaryBitMapField.cs
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
using System.Linq;
using CCFAProtocols.TIC.Instruments;
using Serilog;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class PrimaryBitMapField
    {
        public const byte FieldNumber = 0;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            char[] bitMapChars = reader.ReadChars(16);
            var bitmap = bitMapChars.SelectMany(c => HexUtils.HexToBoolMap[c]).ToArray();
            isomessage.PrimaryBitMap = bitmap;
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            var res = new char[16];

            for (var i = 0; i < isomessage.PrimaryBitMap.LongCount(); i += 4)
                res[i / 4] = HexUtils.BoolsToHexMap[isomessage.PrimaryBitMap[i..(i + 4)]];
            writer.Write(res);
        }
    }
}