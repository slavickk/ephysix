using System.IO;
using CCFAProtocols.TIC.Instruments;
using UAMP;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class BillingDataField
    {
        public const byte FieldNumber = 116;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var readString = reader.LVARReadString(5);
                isomessage.BillingData = new UAMPMessage(readString);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.BillingData is null) return;
            string s = isomessage.BillingData.Serialize();
            writer.LVARWriteString(5, s);
        }
    }
}