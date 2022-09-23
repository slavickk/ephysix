using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ExternalTransactionAttributesField
    {
        public const byte FieldNumber = 62;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ExternalTransactionAttributes = reader.LVARReadString(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.ExternalTransactionAttributes is null) return;
            writer.LVARWriteString(3, isomessage.ExternalTransactionAttributes);
        }
    }
}