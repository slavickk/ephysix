using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class NumericMessageField
    {
        public const byte FieldNumber = 110;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.NumericMessage = reader.LVARReadUInt(1);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.NumericMessage is null) return;
            writer.LVARWriteUInt(1, isomessage.NumericMessage);
        }
    }
}