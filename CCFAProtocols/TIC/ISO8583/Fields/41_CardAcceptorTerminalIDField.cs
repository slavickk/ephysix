using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class CardAcceptorTerminalIDField
    {
        public const byte FieldNumber = 41;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.CardAcceptorTerminalID = reader.LVARReadString(2);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.CardAcceptorTerminal?.ID is null) return;
            writer.LVARWriteString(2, isomessage.CardAcceptorTerminal.ID);
        }
    }
}