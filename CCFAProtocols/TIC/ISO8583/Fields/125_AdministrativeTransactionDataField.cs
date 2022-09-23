using System.IO;
using CCFAProtocols.TIC.Instruments;
using UAMP;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class AdministrativeTransactionDataField
    {
        public const byte FieldNumber = 125;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var readString = reader.LVARReadString(5);
                isomessage.AdministrativeTransactionData = new UAMPMessage(readString);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AdministrativeTransactionData is null) return;
            writer.LVARWriteString(5, isomessage.AdministrativeTransactionData.Serialize());
        }
    }
}