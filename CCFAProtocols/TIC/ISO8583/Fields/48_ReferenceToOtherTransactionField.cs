/******************************************************************
 * File: 48_ReferenceToOtherTransactionField.cs
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

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ReferenceToOtherTransactionField
    {
        public const byte FieldNumber = 48;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ReferenceToOtherTransaction = new TransactionReference()
                {
                    RRN = reader.ReadString(12),
                    PAN = reader.ReadString(19)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.ReferenceToOtherTransaction is null) return;
            writer.Write(12, isomessage.ReferenceToOtherTransaction.RRN);
            writer.Write(19, isomessage.ReferenceToOtherTransaction.PAN);
        }
    }
}