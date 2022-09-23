namespace CCFAProtocols.TIC.ISO8583
{
    public enum AccountType : byte
    {
        Unknown = 0,
        Checking = 1,
        Savings = 11,
        Credit = 31,
        Bonus = 91
    }
}