using System.IO;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class PINCVVVerificationResultField
    {
        public const byte FieldNumber = 44;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.PINCVVverificationResult = new VerificationResult()
                {
                    PinResult = reader.ReadChar(),
                    CVVResult = reader.ReadChar()
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.PINCVVverificationResult is null) return;
            writer.Write(isomessage.PINCVVverificationResult.PinResult);
            writer.Write(isomessage.PINCVVverificationResult.CVVResult);
        }
    }
}