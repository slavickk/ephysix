using System.IO;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class SecurityRelatedControlInformationField
    {
        public const byte FieldNumber = 53;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.SecurityRelatedControlInformation = reader.ReadBytes(48);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.SecurityRelatedControlInformation is null) return;
            writer.Write(isomessage.SecurityRelatedControlInformation);
        }
    }
}