using System.IO;
using CCFAProtocols.TIC.Instruments;
using UAMP;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class StatementDataField
    {
        public const byte FieldNumber = 115;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                string readString = reader.PLVARReadString(5);
                isomessage.StatementData = new UAMPMessage(readString);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.StatementData is null) return;
            writer.PLVARWriteString(5, isomessage.StatementData.Serialize());
        }
    }
}