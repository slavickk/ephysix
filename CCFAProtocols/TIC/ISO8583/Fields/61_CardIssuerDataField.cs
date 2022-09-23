using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class CardIssuerDataField
    {
        public const byte FieldNumber = 61;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.CardIssuerData = new CardIssuer()
                {
                    AuthFIName = reader.ReadString(4),
                    AuthPSName = reader.ReadString(10)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.CardIssuerData is null) return;
            writer.Write(4, isomessage.CardIssuerData.AuthFIName);
            writer.Write(10, isomessage.CardIssuerData.AuthPSName);
        }
    }
}