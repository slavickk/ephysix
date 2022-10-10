namespace CCFAProtocols.TIC.ISO8583
{
    public class POSEntryMode
    {
        public EntryMethod EntryMethod;
        public PinMethod PinMethod;
    }

    public enum PinMethod : byte
    {
        Unknown = 0,
        CanAcceptPin = 1,
        CannotAcceptPIN = 2,
        TerminalPINpadDown = 8
    }

    public enum EntryMethod : byte
    {
        Unknown = 0,
        ManyalKey = 1,
        MagneticStripeReadCVVnotReliable = 2,
        ConsumerPresentedQR = 3,
        OpticalChacterReader = 4,
        ICCCVVRelivle = 5,
        ContactlessEMV = 7,
        eCommerceChip = 9,
        MagneticStripeReadCVVReliable = 90,
        ContactlessMagneticStripeData = 91,
        ICCCVVUnreliable = 95
    }
}