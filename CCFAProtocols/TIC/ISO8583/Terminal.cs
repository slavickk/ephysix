using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583
{
    public class Terminal
    {
        public string? ID; //41
        public TerminalInfo? Info; //43

        public override string ToString()
        {
            string str = $"Terminal:\n\t\tID:{ID}";
            if (Info is not null)
                foreach (var field in typeof(TerminalInfo).GetFields())
                    str += $"\n\t\t{field.Name}: {field.GetValue(Info)}";
            return str;
        }

        public class TerminalInfo
        {
            public string Address; //43.5
            public string Branch; //43.6
            public string City; //43.2
            public TerminalClass? Class; //43.8
            public ushort CountryCode; //43.4
            public ushort CountyCode; //43.13

            /// <summary>
            ///     Terminal Buisness data YYYYMMDD
            /// </summary>
            /// <example>20210228</example>
            public string Date; //43.9

            public string FiName; //43.11
            public string Owner; //43.1
            public string PSName; //43.10
            public string Region; //43.7
            public string RetailerName; //43.12
            public ushort StateCode; //43.3
            public short TimeOffset; //43.15
            public uint ZipCode; //43.14

            public void DeserializeInfo(BinaryReader reader)
            {
                Owner = reader.ReadString(30);
                City = reader.ReadString(30);
                StateCode = reader.ReadUshort(3);
                CountryCode = reader.ReadUshort(3);
                Address = reader.ReadString(30);
                Branch = reader.ReadString(30);
                Region = reader.ReadString(30);
                Class = (TerminalClass) reader.ReadByte(3);
                Date = reader.ReadString(8);
                PSName = reader.ReadString(10);
                FiName = reader.ReadString(4);
                RetailerName = reader.ReadString(25);
                CountyCode = reader.ReadUshort(3);
                ZipCode = reader.ReadUint(9);
                TimeOffset = reader.ReadShort(4);
            }

            public void SerializeInfo(BinaryWriter writer)
            {
                writer.Write(30, Owner ?? "");
                writer.Write(30, City ?? "");
                writer.Write(3, StateCode);
                writer.Write(3, CountryCode);
                writer.Write(30, Address ?? "");
                writer.Write(30, Branch ?? "");
                writer.Write(30, Region ?? "");
                writer.Write(3, (byte) (Class ?? 0));
                writer.Write(8, Date ?? "");
                writer.Write(10, PSName ?? "");
                writer.Write(4, FiName ?? "");
                writer.Write(25, RetailerName ?? "");
                writer.Write(3, CountyCode);
                writer.Write(9, ZipCode);
                writer.Write(4, TimeOffset);
            }
        }
    }

    public enum TerminalClass : byte
    {
        ATM = 1,
        POS = 2,
        CRT = 3,
        TELEBANK = 4
    }
}