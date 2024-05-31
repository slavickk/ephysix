/******************************************************************
 * File: TICHeader.cs
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

namespace CCFAProtocols.TIC
{
    /// <summary>
    /// TIC HEADER
    /// </summary>
    /// <remarks>"A4M"|ProtocolVersion(N2)|RejectStatus(N3)</remarks>
    public class TICHeader
    {
        // public string Indicator;
        public byte ProtocolVersion;
        public ushort RejectStatus;

        public static TICHeader Deserialize(BinaryReader reader)
        {
            return new TICHeader()
            {
                ProtocolVersion = reader.ReadByte(2),
                RejectStatus = reader.ReadUshort(3)
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(2, ProtocolVersion);
            writer.Write(3, RejectStatus);
        }

        public override string ToString()
        {
            return $"HEADER:\n\t ProtocolVersion {ProtocolVersion}, RejectStatus {RejectStatus}";
        }
    }
}