using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class FinalRRNField
    {
        public const byte FieldNumber = 107;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.FinalRRN = reader.ReadString(12);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.FinalRRN is null) return;
            writer.Write(12, isomessage.FinalRRN);
        }
    }
}