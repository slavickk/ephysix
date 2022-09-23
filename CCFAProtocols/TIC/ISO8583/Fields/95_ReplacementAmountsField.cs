using System.IO;
using CCFAProtocols.TIC.Instruments;
using CCFAProtocols.TIC.ISO8583;

namespace CCFAProtocols.TIC.Fields
{
    public static class ReplacementAmountsField
    {
        public const byte FieldNumber = 95;

        public static void Deserialize(BinaryReader reader, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ReplacementAmounts = new ReplacementAmounts()
                {
                    ReplacementAmount = reader.ReadUlong(12),
                    ReplacementOriginalAmount = reader.ReadUlong(12)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.ReplacementAmounts is null) return;
            writer.Write(12, isomessage.ReplacementAmounts.ReplacementAmount);
            writer.Write(12, isomessage.ReplacementAmounts.ReplacementOriginalAmount);
        }
    }
}