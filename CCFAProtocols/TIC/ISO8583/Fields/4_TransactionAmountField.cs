using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class TransactionAmountField
    {
        public const byte FieldNumber = 4;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.TransactionAmount = reader.ReadUlong(12);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.TransactionAmount is null) return;
            writer.Write(12, isomessage.TransactionAmount);
        }
    }
}