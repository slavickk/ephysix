/******************************************************************
 * File: 7_TransmissionGreenwichTimeField.cs
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
    public static class TransmissionGreenwichTimeField
    {
        public const byte FieldNumber = 7;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var chars = reader.ReadChars(10);
                isomessage.TransmissionGreenwichTime = new string(chars);
                // DateTime.ParseExact(chars, "MMddHHmmss", CultureInfo.InvariantCulture);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.TransmissionGreenwichTime is null) return;
            writer.Write(10, isomessage.TransmissionGreenwichTime); //?.ToString("MMddHHmmss")
        }
    }
}