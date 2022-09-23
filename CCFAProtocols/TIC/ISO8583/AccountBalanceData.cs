namespace CCFAProtocols.TIC.ISO8583
{
    public class AccountBalanceData
    {
        public long AvailableBalance; //105.2
        public bool BalanceCurrency; //105.3 1=true 0=false
        public long LedgerBalance; //105.1
    }
}