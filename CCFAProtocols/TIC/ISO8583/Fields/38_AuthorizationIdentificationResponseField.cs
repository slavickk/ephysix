using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.Fields
{
    public static class AuthorizationIdentificationResponseField
    {
        public const byte FieldNumber = 38;

        public static void Deserialize(BinaryReader reader, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AuthorizationIdentificationResponse = reader.ReadString(6);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.AuthorizationIdentificationResponse is null) return;
            writer.Write(6, isomessage.AuthorizationIdentificationResponse);
        }
    }
}