using System.IO;
using CCFAProtocols.TIC.Instruments;
using UAMP;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class MisscellaneousTransactionAttributes2Field
    {
        public const byte FieldNumber = 124;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var readString = reader.LVARReadString(5);
                isomessage.MiscellaneousTransactionAttributes2 = new UAMPMessage(readString);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.MiscellaneousTransactionAttributes2 is null) return;
            writer.LVARWriteString(5, isomessage.MiscellaneousTransactionAttributes2.Serialize());
        }
    }
}