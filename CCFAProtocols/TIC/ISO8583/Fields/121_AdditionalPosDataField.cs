/******************************************************************
 * File: 121_AdditionalPosDataField.cs
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
    public static class AdditionalPosDataField
    {
        public const byte FieldNumber = 121;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AdditionalPOSData = new POSData()
                {
                    TransactionCategory = reader.ReadByte(2),
                    DraftCapture = reader.ReadByte(1),
                    CVV2 = reader.ReadString(3),
                    Clerk = reader.ReadString(16),
                    InvoiceNumber = reader.ReadString(16),
                    PosBatchAndShiftData = reader.ReadString(9)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AdditionalPOSData is null) return;
            writer.Write(2, isomessage.AdditionalPOSData.TransactionCategory);
            writer.Write(1, isomessage.AdditionalPOSData.DraftCapture);
            writer.Write(3, isomessage.AdditionalPOSData.CVV2);
            writer.Write(16, isomessage.AdditionalPOSData.Clerk);
            writer.Write(16, isomessage.AdditionalPOSData.InvoiceNumber);
            writer.Write(9, isomessage.AdditionalPOSData.PosBatchAndShiftData);
        }
    }

    public class POSData
    {
        public string Clerk;
        public string CVV2;
        public byte DraftCapture;
        public string InvoiceNumber;
        public string PosBatchAndShiftData;
        public byte TransactionCategory;
    }
}