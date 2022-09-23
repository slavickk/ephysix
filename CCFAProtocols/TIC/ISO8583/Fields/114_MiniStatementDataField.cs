using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class MiniStatementDataField
    {
        public const byte FieldNumber = 114;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.MiniStatementData = reader.LVARReadString(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.MiniStatementData is null) return;
            writer.LVARWriteString(3, isomessage.MiniStatementData);
        }
    }
}