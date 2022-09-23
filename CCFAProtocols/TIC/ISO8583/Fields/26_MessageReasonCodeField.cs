using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class MessageReasonCodeField
    {
        public const byte FieldNumber = 26;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.MessageReasonCode = reader.ReadUshort(4);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.MessageReasonCode is null) return;
            writer.Write(4, isomessage.MessageReasonCode);
        }
    }
}