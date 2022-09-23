using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class TransactionRetrievalReferenceNumberField
    {
        public const byte FieldNumber = 37;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.TransactionRetrievalReferenceNumber = reader.ReadString(12);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.TransactionRetrievalReferenceNumber is null) return;
            writer.Write(12, isomessage.TransactionRetrievalReferenceNumber);
        }
    }
}