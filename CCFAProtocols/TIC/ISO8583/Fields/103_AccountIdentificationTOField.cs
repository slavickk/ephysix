using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class AccountIdentificationTOField
    {
        public const byte FieldNumber = 103;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AccountIdentificationTO = reader.LVARReadString(2);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AccountIdentificationTO is null) return;
            writer.LVARWriteString(2, isomessage.AccountIdentificationTO);
        }
    }
}