using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class SecureData3DField
    {
        public const byte FieldNumber = 122;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.SecureData3D = reader.ReadString(48);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.SecureData3D is null) return;
            writer.Write(48, isomessage.SecureData3D);
        }
    }
}