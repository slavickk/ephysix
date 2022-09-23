using System;

namespace CCFAProtocols.TIC.ISO8583
{
    public class ProcessingCode
    {
        public AccountType FromAccountType;
        public AccountType ToAccountType;
        public ushort TransactionCode;

        public override string ToString()
        {
            return
                $"FromAccountType: {Enum.GetName<AccountType>(FromAccountType)}({(byte)FromAccountType}),ToAccountType: {Enum.GetName<AccountType>(ToAccountType)}({(byte)ToAccountType}),TransactionCode:{TransactionCode}";
        }
    }
}