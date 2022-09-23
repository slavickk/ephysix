using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.Fields
{
    public static class ResponseCodeField
    {
        public const byte FieldNumber = 39;

        public static void Deserialize(BinaryReader reader, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ResponseCode = reader.ReadUint(5);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.ResponseCode is null) return;
            writer.Write(5, isomessage.ResponseCode);
        }
    }
}