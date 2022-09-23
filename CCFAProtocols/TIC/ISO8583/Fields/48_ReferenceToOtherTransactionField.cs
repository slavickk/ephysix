using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ReferenceToOtherTransactionField
    {
        public const byte FieldNumber = 48;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ReferenceToOtherTransaction = new TransactionReference()
                {
                    RRN = reader.ReadString(12),
                    PAN = reader.ReadString(19)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.ReferenceToOtherTransaction is null) return;
            writer.Write(12, isomessage.ReferenceToOtherTransaction.RRN);
            writer.Write(19, isomessage.ReferenceToOtherTransaction.PAN);
        }
    }
}