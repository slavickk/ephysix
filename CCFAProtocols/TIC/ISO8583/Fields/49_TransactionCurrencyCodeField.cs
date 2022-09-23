using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class TransactionCurrencyCodeField
    {
        public const byte FieldNumber = 49;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.TransactionCurrencyCode = reader.ReadUshort(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.TransactionCurrencyCode is null) return;
            writer.Write(3, isomessage.TransactionCurrencyCode);
        }
    }
}