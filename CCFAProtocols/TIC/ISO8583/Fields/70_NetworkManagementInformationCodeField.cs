using System.IO;
using CCFAProtocols.TIC.Instruments;
using CCFAProtocols.TIC.ISO8583;

namespace CCFAProtocols.TIC.Fields
{
    public static class NetworkManagementInformationCodeField
    {
        public const byte FieldNumber = 70;

        public static void Deserialize(BinaryReader reader, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.NetworkManagementInformationCode = (NetworkManagementInformationCodes)reader.ReadUshort(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583.ISO8583 isomessage)
        {
            if (isomessage.NetworkManagementInformationCode is null) return;
            writer.Write(3, (ushort)isomessage.NetworkManagementInformationCode);
        }
    }
}