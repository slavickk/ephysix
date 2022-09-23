using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class POSEntryModeField
    {
        public const byte FieldNumber = 22;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.POSEntryMode = new POSEntryMode()
                {
                    EntryMethod = (EntryMethod) reader.ReadByte(2),
                    PinMethod = (PinMethod) reader.ReadByte(1)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.POSEntryMode is null) return;
            writer.Write(2, (byte) isomessage.POSEntryMode.EntryMethod);
            writer.Write(1, (byte) isomessage.POSEntryMode.PinMethod);
        }
    }
}