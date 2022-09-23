using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ReceivingInstitutionIdentificationCodeField
    {
        //TODO: Прилетают пустые поля
        public const byte FieldNumber = 100;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ReceivingInstitutionIdentificationCode = reader.LVARReadString(2);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.ReceivingInstitutionIdentificationCode is null) return;
            writer.LVARWriteString(2, isomessage.ReceivingInstitutionIdentificationCode);
        }
    }
}