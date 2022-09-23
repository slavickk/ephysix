using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class SICField
    {
        public const byte FieldNumber = 18;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.SIC = ushort.Parse(reader.ReadChars(4));
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.SIC is null) return;
            writer.Write(4, isomessage.SIC);
        }
    }
}