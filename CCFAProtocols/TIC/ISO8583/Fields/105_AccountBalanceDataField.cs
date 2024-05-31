/******************************************************************
 * File: 105_AccountBalanceDataField.cs
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
    public static class AccountBalanceDataField
    {
        public const byte FieldNumber = 105;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AccountBalanceData = new AccountBalanceData()
                {
                    LedgerBalance = reader.ReadLong(12),
                    AvailableBalance = reader.ReadLong(12),
                    BalanceCurrency = reader.ReadChar() == '1'
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AccountBalanceData is null) return;
            writer.Write(12, isomessage.AccountBalanceData.LedgerBalance);
            writer.Write(12, isomessage.AccountBalanceData.AvailableBalance);
            writer.Write(isomessage.AccountBalanceData.BalanceCurrency ? '1' : '0');
        }
    }
}