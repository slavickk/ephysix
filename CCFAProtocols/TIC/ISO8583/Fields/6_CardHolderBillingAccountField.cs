using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class CardHolderBillingAccountField
    {
        public const byte FieldNumber = 6;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.CardholderBillingAmount = reader.ReadUlong(12);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.CardholderBillingAmount is null) return;
            writer.Write(12, isomessage.CardholderBillingAmount);
        }
    }
}