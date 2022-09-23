using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class AccountBalanceDataField
    {
        public const byte FieldNumber = 105;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.AccountBalanceData = new AccountBalanceData()
                {
                    LedgerBalance = reader.ReadLong(12),
                    AvailableBalance = reader.ReadLong(12),
                    BalanceCurrency = reader.ReadChar() == '1'
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.AccountBalanceData is null) return;
            writer.Write(12, isomessage.AccountBalanceData.LedgerBalance);
            writer.Write(12, isomessage.AccountBalanceData.AvailableBalance);
            writer.Write(isomessage.AccountBalanceData.BalanceCurrency ? '1' : '0');
        }
    }
}