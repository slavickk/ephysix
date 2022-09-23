using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class SystemTraceAuditNumberField
    {
        public const byte FieldNumber = 11;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.SytemTraceAuditNumber = reader.ReadUint(6);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.SytemTraceAuditNumber is null) return;
            writer.Write(6, isomessage.SytemTraceAuditNumber);
        }
    }
}