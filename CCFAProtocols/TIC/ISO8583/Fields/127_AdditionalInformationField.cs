using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class AdditionalInformationField
    {
        public const byte FieldNumber = 127;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AdditionalInformation = reader.LVARReadString(5);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AdditionalInformation is null) return;
            writer.LVARWriteString(5, isomessage.AdditionalInformation);
        }
    }
}