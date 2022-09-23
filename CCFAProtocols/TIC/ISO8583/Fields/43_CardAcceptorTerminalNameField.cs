using System.IO;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class CardAcceptorTerminalNameField
    {
        public const byte FieldNumber = 43;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                Terminal.TerminalInfo terminalInfo = new();
                terminalInfo.DeserializeInfo(reader);
                isomessage.CardAcceptorTerminalInfo = terminalInfo;
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.CardAcceptorTerminal?.Info is null) return;
            isomessage.CardAcceptorTerminal.Info.SerializeInfo(writer);
        }
    }
}