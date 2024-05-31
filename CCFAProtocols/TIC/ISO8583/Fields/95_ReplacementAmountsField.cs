/******************************************************************
 * File: 95_ReplacementAmountsField.cs
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
using CCFAProtocols.TIC.Instruments;
using CCFAProtocols.TIC.ISO8583;

namespace CCFAProtocols.TIC.Fields
{
    public static class ReplacementAmountsField
    {
        public const byte FieldNumber = 95;

        public static void Deserialize(BinaryReader reader, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ReplacementAmounts = new ReplacementAmounts()
                {
                    ReplacementAmount = reader.ReadUlong(12),
                    ReplacementOriginalAmount = reader.ReadUlong(12)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.ReplacementAmounts is null) return;
            writer.Write(12, isomessage.ReplacementAmounts.ReplacementAmount);
            writer.Write(12, isomessage.ReplacementAmounts.ReplacementOriginalAmount);
        }
    }
}