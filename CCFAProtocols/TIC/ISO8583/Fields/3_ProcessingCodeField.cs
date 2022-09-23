using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class ProcessingCodeField
    {
        public const byte FieldNumber = 3;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.ProcessingCode = new ProcessingCode()
                {
                    TransactionCode = reader.ReadUshort(3),
                    FromAccountType = (AccountType) reader.ReadByte(2),
                    ToAccountType = (AccountType) reader.ReadByte(2)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.ProcessingCode is null) return;
            writer.Write(3, isomessage.ProcessingCode.TransactionCode);
            writer.Write(2, (byte) isomessage.ProcessingCode.FromAccountType);
            writer.Write(2, (byte) isomessage.ProcessingCode.ToAccountType);
        }
    }
}