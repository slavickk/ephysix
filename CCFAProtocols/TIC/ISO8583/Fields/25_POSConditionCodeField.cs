using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class POSConditionCodeField
    {
        public const byte FieldNumber = 25;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.POSConditionCode = (POSConditionCodes) reader.ReadUshort(3);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.POSConditionCode is null) return;
            writer.Write(3, (ushort) isomessage.POSConditionCode);
        }
    }
}