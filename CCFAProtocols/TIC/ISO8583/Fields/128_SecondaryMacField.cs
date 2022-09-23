using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class SecondaryMacField
    {
        public const byte FieldNumber = 128;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.SecondaryMac = reader.ReadString(16);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.SecondaryMac is null) return;
            writer.Write(16, isomessage.SecondaryMac);
        }
    }
}