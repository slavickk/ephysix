using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class NewPinField
    {
        public const byte FieldNumber = 63;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.NewPIN = reader.ReadString(16);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.NewPIN is null) return;
            writer.Write(16, isomessage.NewPIN);
        }
    }
}