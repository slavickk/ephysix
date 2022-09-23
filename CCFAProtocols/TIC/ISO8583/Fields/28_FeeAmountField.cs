using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class FeeAmountField
    {
        public const byte FieldNumber = 28;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AcquirerFeeAmount = new AcquirerFeeAmount
                {
                    IsWithdraw = (reader.ReadChar() == 'D'),
                    Amount = reader.ReadUint(8)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AcquirerFeeAmount is null) return;
            writer.Write(isomessage.AcquirerFeeAmount._isWithdraw);
            writer.Write(8, isomessage.AcquirerFeeAmount.Amount);
        }
    }
}