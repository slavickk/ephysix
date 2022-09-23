namespace CCFAProtocols.TIC.ISO8583
{
    public enum NetworkManagementInformationCodes : ushort
    {
        LogOn = 1,
        LogOff = 2,
        CutoverOffline = 3,
        InquiryMode = 4,
        PinWorkingKeyChange = 101,
        EchoTest = 301
    }
}