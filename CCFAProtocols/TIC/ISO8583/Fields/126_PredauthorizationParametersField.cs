/******************************************************************
 * File: 126_PredauthorizationParametersField.cs
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
    public static class PredauthorizationParametersField
    {
        public const byte FieldNumber = 126;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.PredAuthorizationParameters = new PredAuthorizationParameters()
                {
                    OriginalTransactionInvoiceNumber = reader.ReadString(16),
                    OriginalSeqNumber = reader.ReadString(9),
                    PredAuthorizationHold = reader.ReadString(9)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.PredAuthorizationParameters is null) return;
            writer.Write(16, isomessage.PredAuthorizationParameters.OriginalTransactionInvoiceNumber);
            writer.Write(9, isomessage.PredAuthorizationParameters.OriginalSeqNumber);
            writer.Write(9, isomessage.PredAuthorizationParameters.PredAuthorizationHold);
        }
    }

    public class PredAuthorizationParameters
    {
        public string OriginalSeqNumber;
        public string OriginalTransactionInvoiceNumber;
        public string PredAuthorizationHold;

        public override string ToString()
        {
            return
                $"OriginalSeqNumber:{OriginalSeqNumber},OriginalTransactionInvoiceNumber:{OriginalTransactionInvoiceNumber},PredAuthorizationHold:{PredAuthorizationHold}";
        }
    }
}