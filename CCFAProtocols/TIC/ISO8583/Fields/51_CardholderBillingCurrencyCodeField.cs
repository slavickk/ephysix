using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class CardholderBillingCurrencyCodeField
    {
        public const byte FieldNumber = 51;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.CardholderBillingCurrencyCode = reader.ReadUshort(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.CardholderBillingCurrencyCode is null) return;
            writer.Write(3, isomessage.CardholderBillingCurrencyCode);
        }
    }
}