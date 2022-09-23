using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ICCSystemRelatedDataField
    {
        public const byte FieldNumber = 55;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ICCSystemRelatedData = reader.LVARReadBytes(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.ICCSystemRelatedData is null) return;
            writer.LVARWriteBytes(3, isomessage.ICCSystemRelatedData);
        }
    }
}