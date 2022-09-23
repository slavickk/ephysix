using System.IO;
using CCFAProtocols.TIC.Instruments;
using UAMP;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class AccountIdentification2Field
    {
        public const byte FieldNumber = 111;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var readString = reader.LVARReadString(2);
                isomessage.AccountIdentification2 = UAMPValue.ParseValue(readString);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AccountIdentification2 is null) return;
            writer.LVARWriteString(2, isomessage.AccountIdentification2.Serialize());
        }
    }
}