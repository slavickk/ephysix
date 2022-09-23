using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ConversionRateField
    {
        public const byte FieldNumber = 10;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.CoversionRate = reader.ReadUint(8);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.CoversionRate is null) return;
            writer.Write(8, isomessage.CoversionRate);
        }
    }
}