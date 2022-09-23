using System.IO;
using CCFAProtocols.TIC.Fields;
using CCFAProtocols.TIC.ISO8583.Fields;

namespace CCFAProtocols.TIC.ISO8583
{
    public partial class ISO8583
    {
        public void Serialize(BinaryWriter writer)
        {
            PrimaryBitMapField.Serialize(writer, this);
            SecondaryBitMapField.Serialize(writer, this);
            PANField.Serialize(writer, this);
            ProcessingCodeField.Serialize(writer, this);
            TransactionAmountField.Serialize(writer, this);
            CardHolderBillingAccountField.Serialize(writer, this);
            TransmissionGreenwichTimeField.Serialize(writer, this);
            ConversionRateField.Serialize(writer, this);
            SystemTraceAuditNumberField.Serialize(writer, this);
            LocalTransactionTimeField.Serialize(writer, this);
            LocalTransactionDateField.Serialize(writer, this);
            SICField.Serialize(writer, this);
            AcquiringInstitutionCountryCodeField.Serialize(writer, this);
            POSEntryModeField.Serialize(writer, this);
            MBRField.Serialize(writer, this);
            POSConditionCodeField.Serialize(writer, this);
            MessageReasonCodeField.Serialize(writer, this);
            FeeAmountField.Serialize(writer, this);
            AcquiringInstitutionIdentificationField.Serialize(writer, this);
            ForwardingInstitutionIdentificationField.Serialize(writer, this);
            Track2Field.Serialize(writer, this);
            TransactionRetrievalReferenceNumberField.Serialize(writer, this);
            AuthorizationIdentificationResponseField.Serialize(writer, this);
            ResponseCodeField.Serialize(writer, this);
            CardAcceptorTerminalIDField.Serialize(writer, this);
            CardAcceptorTerminalNameField.Serialize(writer, this);
            PINCVVVerificationResultField.Serialize(writer, this);
            Track1Field.Serialize(writer, this);
            ReferenceToOtherTransactionField.Serialize(writer, this);
            TransactionCurrencyCodeField.Serialize(writer, this);
            CardholderBillingCurrencyCodeField.Serialize(writer, this);
            PINField.Serialize(writer, this);
            SecurityRelatedControlInformationField.Serialize(writer, this);
            AdjustmentAmountField.Serialize(writer, this);
            ICCSystemRelatedDataField.Serialize(writer, this);
            CardIssuerDataField.Serialize(writer, this);
            ExternalTransactionAttributesField.Serialize(writer, this);
            NewPinField.Serialize(writer, this);
            MACField.Serialize(writer, this);
            NetworkManagementInformationCodeField.Serialize(writer, this);
            ReplacementAmountsField.Serialize(writer, this);
            ReceivingInstitutionIdentificationCodeField.Serialize(writer, this);
            AccountIdentificationFromField.Serialize(writer, this);
            AccountIdentificationTOField.Serialize(writer, this);
            HostNetIdentificationField.Serialize(writer, this);
            AccountBalanceDataField.Serialize(writer, this);
            MultiCurrencyDataField.Serialize(writer, this);
            FinalRRNField.Serialize(writer, this);
            RegionalListingDataField.Serialize(writer, this);
            MultiAccountDataField.Serialize(writer, this);
            NumericMessageField.Serialize(writer, this);
            AccountIdentification2Field.Serialize(writer, this);
            MiniStatementDataField.Serialize(writer, this);
            StatementDataField.Serialize(writer, this);
            BillingDataField.Serialize(writer, this);
            AdditionalPosDataField.Serialize(writer, this);
            SecureData3DField.Serialize(writer, this);
            MiscellaneousTransactionAttributesField.Serialize(writer, this);
            MisscellaneousTransactionAttributes2Field.Serialize(writer, this);
            AdministrativeTransactionDataField.Serialize(writer, this);
            PredauthorizationParametersField.Serialize(writer, this);
            AdditionalInformationField.Serialize(writer, this);
            SecondaryMacField.Serialize(writer, this);
        }
    }
}