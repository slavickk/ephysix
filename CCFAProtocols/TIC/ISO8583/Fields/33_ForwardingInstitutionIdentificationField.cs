using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ForwardingInstitutionIdentificationField
    {
        public const byte FieldNumber = 33;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ForwardingInstitutionIdentification = reader.LVARReadString(2);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.ForwardingInstitutionIdentification is null) return;
            writer.LVARWriteString(2, isomessage.ForwardingInstitutionIdentification);
        }
    }
}