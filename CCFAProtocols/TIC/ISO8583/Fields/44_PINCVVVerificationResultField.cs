/******************************************************************
 * File: 44_PINCVVVerificationResultField.cs
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

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class PINCVVVerificationResultField
    {
        public const byte FieldNumber = 44;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.PINCVVverificationResult = new VerificationResult()
                {
                    PinResult = reader.ReadChar(),
                    CVVResult = reader.ReadChar()
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.PINCVVverificationResult is null) return;
            writer.Write(isomessage.PINCVVverificationResult.PinResult);
            writer.Write(isomessage.PINCVVverificationResult.CVVResult);
        }
    }
}