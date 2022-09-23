using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class HostNetIdentificationField
    {
        public const byte FieldNumber = 104;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.HostNetIdentification = reader.ReadString(4);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.HostNetIdentification is null) return;
            writer.Write(4, isomessage.HostNetIdentification);
        }
    }
}