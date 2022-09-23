using System.IO;
using CCFAProtocols.TIC.Instruments;
using UAMP;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class MiscellaneousTransactionAttributesField
    {
        public const byte FieldNumber = 123;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var readString = reader.LVARReadString(3);
                isomessage.MiscellaneousTransactionAttributes = new UAMPMessage(readString);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.MiscellaneousTransactionAttributes is null) return;
            writer.LVARWriteString(3, isomessage.MiscellaneousTransactionAttributes.Serialize());
        }
    }
}