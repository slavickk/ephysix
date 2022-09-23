namespace CCFAProtocols.TIC.ISO8583
{
    public class MultiCurrencyData
    {
        public uint FromAccountExhangeRate; //106.5
        public ulong OriginalAmount; //106.4
        public ushort OriginalCurrency; //106.2
        public ulong TOAccountAmount; //106.3
        public ushort TOAccountCurrency; //106.1
        public uint ToAccountExchangeRate; //106.6
    }
}