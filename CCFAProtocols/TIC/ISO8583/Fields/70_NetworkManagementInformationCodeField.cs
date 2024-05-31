/******************************************************************
 * File: 70_NetworkManagementInformationCodeField.cs
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
    public static class NetworkManagementInformationCodeField
    {
        public const byte FieldNumber = 70;

        public static void Deserialize(BinaryReader reader, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.NetworkManagementInformationCode = (NetworkManagementInformationCodes)reader.ReadUshort(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.NetworkManagementInformationCode is null) return;
            writer.Write(3, (ushort)isomessage.NetworkManagementInformationCode);
        }
    }
}