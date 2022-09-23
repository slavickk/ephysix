using System.IO;
using CCFAProtocols.TIC.Fields;
using CCFAProtocols.TIC.ISO8583.Fields;

namespace CCFAProtocols.TIC.ISO8583
{
    public partial class ISO8583
    {
        public static ISO8583 Deserialize(BinaryReader reader)
        {
            var IsoMessage = new ISO8583();
            PrimaryBitMapField.Deserialize(reader, IsoMessage);
            SecondaryBitMapField.Deserialize(reader, IsoMessage);
            PANField.Deserialize(reader, IsoMessage);
            ProcessingCodeField.Deserialize(reader, IsoMessage);
            TransactionAmountField.Deserialize(reader, IsoMessage);
            CardHolderBillingAccountField.Deserialize(reader, IsoMessage);
            TransmissionGreenwichTimeField.Deserialize(reader, IsoMessage);
            ConversionRateField.Deserialize(reader, IsoMessage);
            SystemTraceAuditNumberField.Deserialize(reader, IsoMessage);
            LocalTransactionTimeField.Deserialize(reader, IsoMessage);
            LocalTransactionDateField.Deserialize(reader, IsoMessage);
            SICField.Deserialize(reader, IsoMessage);
            AcquiringInstitutionCountryCodeField.Deserialize(reader, IsoMessage);
            POSEntryModeField.Deserialize(reader, IsoMessage);
            MBRField.Deserialize(reader, IsoMessage);
            POSConditionCodeField.Deserialize(reader, IsoMessage);
            MessageReasonCodeField.Deserialize(reader, IsoMessage);
            FeeAmountField.Deserialize(reader, IsoMessage);
            AcquiringInstitutionIdentificationField.Deserialize(reader, IsoMessage);
            ForwardingInstitutionIdentificationField.Deserialize(reader, IsoMessage);
            Track2Field.Deserialize(reader, IsoMessage);
            TransactionRetrievalReferenceNumberField.Deserialize(reader, IsoMessage);
            AuthorizationIdentificationResponseField.Deserialize(reader, IsoMessage);
            ResponseCodeField.Deserialize(reader, IsoMessage);
            CardAcceptorTerminalIDField.Deserialize(reader, IsoMessage);
            CardAcceptorTerminalNameField.Deserialize(reader, IsoMessage);
            PINCVVVerificationResultField.Deserialize(reader, IsoMessage);
            Track1Field.Deserialize(reader, IsoMessage);
            ReferenceToOtherTransactionField.Deserialize(reader, IsoMessage);
            TransactionCurrencyCodeField.Deserialize(reader, IsoMessage);
            CardholderBillingCurrencyCodeField.Deserialize(reader, IsoMessage);
            PINField.Deserialize(reader, IsoMessage);
            SecurityRelatedControlInformationField.Deserialize(reader, IsoMessage);
            AdjustmentAmountField.Deserialize(reader, IsoMessage);
            ICCSystemRelatedDataField.Deserialize(reader, IsoMessage);
            CardIssuerDataField.Deserialize(reader, IsoMessage);
            ExternalTransactionAttributesField.Deserialize(reader, IsoMessage);
            NewPinField.Deserialize(reader, IsoMessage);
            MACField.Deserialize(reader, IsoMessage);
            NetworkManagementInformationCodeField.Deserialize(reader, IsoMessage);
            ReplacementAmountsField.Deserialize(reader, IsoMessage);
            ReceivingInstitutionIdentificationCodeField.Deserialize(reader, IsoMessage);
            AccountIdentificationFromField.Deserialize(reader, IsoMessage);
            AccountIdentificationTOField.Deserialize(reader, IsoMessage);
            HostNetIdentificationField.Deserialize(reader, IsoMessage);
            AccountBalanceDataField.Deserialize(reader, IsoMessage);
            MultiCurrencyDataField.Deserialize(reader, IsoMessage);
            FinalRRNField.Deserialize(reader, IsoMessage);
            RegionalListingDataField.Deserialize(reader, IsoMessage);
            MultiAccountDataField.Deserialize(reader, IsoMessage);
            NumericMessageField.Deserialize(reader, IsoMessage);
            AccountIdentification2Field.Deserialize(reader, IsoMessage);
            MiniStatementDataField.Deserialize(reader, IsoMessage);
            StatementDataField.Deserialize(reader, IsoMessage);
            BillingDataField.Deserialize(reader, IsoMessage);
            AdditionalPosDataField.Deserialize(reader, IsoMessage);
            SecureData3DField.Deserialize(reader, IsoMessage);
            MiscellaneousTransactionAttributesField.Deserialize(reader, IsoMessage);
            MisscellaneousTransactionAttributes2Field.Deserialize(reader, IsoMessage);
            AdministrativeTransactionDataField.Deserialize(reader, IsoMessage);
            PredauthorizationParametersField.Deserialize(reader, IsoMessage);
            AdditionalInformationField.Deserialize(reader, IsoMessage);
            SecondaryMacField.Deserialize(reader, IsoMessage);
            return IsoMessage;
        }
    }
}