using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class RegionalListingDataField
    {
        public const byte FieldNumber = 108;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.RegionalListingData = reader.LVARReadString(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.RegionalListingData is null) return;
            writer.LVARWriteString(3, isomessage.RegionalListingData);
        }
    }
}