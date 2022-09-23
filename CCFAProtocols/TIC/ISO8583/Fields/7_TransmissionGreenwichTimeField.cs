using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class TransmissionGreenwichTimeField
    {
        public const byte FieldNumber = 7;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                var chars = reader.ReadChars(10);
                isomessage.TransmissionGreenwichTime = new string(chars);
                // DateTime.ParseExact(chars, "MMddHHmmss", CultureInfo.InvariantCulture);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.TransmissionGreenwichTime is null) return;
            writer.Write(10, isomessage.TransmissionGreenwichTime); //?.ToString("MMddHHmmss")
        }
    }
}