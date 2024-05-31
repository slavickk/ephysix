/******************************************************************
 * File: 106_MultiCurrencyDataField.cs
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
    public static class MultiCurrencyDataField
    {
        public const byte FieldNumber = 106;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.MultiCurrencyData = new MultiCurrencyData()
                {
                    TOAccountCurrency = reader.ReadUshort(3),
                    OriginalCurrency = reader.ReadUshort(3),
                    TOAccountAmount = reader.ReadUlong(12),
                    OriginalAmount = reader.ReadUlong(12),
                    FromAccountExhangeRate = reader.ReadUint(8),
                    ToAccountExchangeRate = reader.ReadUint(8)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.MultiCurrencyData is null) return;
            writer.Write(3, isomessage.MultiCurrencyData.TOAccountCurrency);
            writer.Write(3, isomessage.MultiCurrencyData.OriginalCurrency);
            writer.Write(12, isomessage.MultiCurrencyData.TOAccountAmount);
            writer.Write(12, isomessage.MultiCurrencyData.OriginalAmount);
            writer.Write(8, isomessage.MultiCurrencyData.FromAccountExhangeRate);
            writer.Write(8, isomessage.MultiCurrencyData.ToAccountExchangeRate);
        }
    }
}