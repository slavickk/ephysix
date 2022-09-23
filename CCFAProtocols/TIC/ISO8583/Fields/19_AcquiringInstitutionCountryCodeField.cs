using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class AcquiringInstitutionCountryCodeField
    {
        public const byte FieldNumber = 19;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AcquiringInstitutionCountryCode = reader.ReadUshort(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AcquiringInstitutionCountryCode is null) return;
            writer.Write(3, isomessage.AcquiringInstitutionCountryCode);
        }
    }
}