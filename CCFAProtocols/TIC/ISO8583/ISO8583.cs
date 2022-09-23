#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using CCFAProtocols.TIC.ISO8583.Fields;
using UAMP;

namespace CCFAProtocols.TIC.ISO8583
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class ISO8583
    {
        /// <summary>
        ///     Field:105 Account Balance Data
        /// </summary>
        private AccountBalanceData? _AccountBalanceData;

        /// <summary>
        ///     Field:111 AccountIdentification2
        /// </summary>
        private UAMPValue? _AccountIdentification2;

        /// <summary>
        ///     Field:102 AccountIdentificationFROM
        /// </summary>
        private string? _AccountIdentificationFROM;

        /// <summary>
        ///     Field:103 AccountIdentificationTO
        /// </summary>
        private string? _AccountIdentificationTO;

        /// <summary>
        ///     Field:28 FeeAmount
        /// </summary>
        private AcquirerFeeAmount? _AcquirerFeeAmount;

        /// <summary>
        ///     Field:19 AcquiringInstitutionCountryCode
        /// </summary>
        private ushort? _AcquiringInstitutionCountryCode;

        /// <summary>
        ///     Field:32 AcquiringInstitutionIdentification
        /// </summary>
        private string? _AcquiringInstitutionIdentification;

        /// <summary>
        ///     Field:127 AdditionalInformation
        /// </summary>
        private string? _AdditionalInformation;

        /// <summary>
        ///     Field:121 AdditionalPOSData
        /// </summary>
        private POSData? _AdditionalPOSData;

        /// <summary>
        ///     Field:54 AdjustmentAmount
        /// </summary>
        private ulong? _AdjustmentAmount;

        /// <summary>
        ///     Field:125 AdministrativeTransactionData
        /// </summary>
        private UAMPMessage? _AdministrativeTransactionData;

        /// <summary>
        ///     Field:38 AuthorizationIdentificationResponse
        /// </summary>
        private string? _AuthorizationIdentificationResponse;

        /// <summary>
        ///     Field:116 BillingData
        /// </summary>
        private UAMPMessage? _BillingData;


        /// <summary>
        ///     Field:6 CardholderBillingAmount
        /// </summary>
        private ulong? _CardholderBillingAmount;

        /// <summary>
        ///     Field:51 CardholderBillingCurrencyCode
        /// </summary>
        private ushort? _CardholderBillingCurrencyCode;

        /// <summary>
        ///     Field:61 CardIssuerData
        /// </summary>
        private CardIssuer? _CardIssuerData;

        /// <summary>
        ///     Field:10 CoversionRate
        /// </summary>
        private uint? _CoversionRate;

        /// <summary>
        ///     Field:62 ExternalTransactionAttributes
        /// </summary>
        private string? _ExternalTransactionAttributes;

        /// <summary>
        ///     Field:107 FinalRRN
        /// </summary>
        private string? _FinalRRN;

        /// <summary>
        ///     Field:33 ForwardingInstitutionIdentification
        /// </summary>
        private string? _ForwardingInstitutionIdentification;

        /// <summary>
        ///     Field:104 HostNetIdentification
        /// </summary>
        private string? _HostNetIdentification;

        /// <summary>
        ///     Field:55 ICCSystemRelatedData
        /// </summary>
        private byte[]? _ICCSystemRelatedData;

        /// <summary>
        ///     Field:64 MAC
        /// </summary>
        private string? _MAC;

        /// <summary>
        ///     Field:23 MBR
        /// </summary>
        private ushort? _MBR;

        /// <summary>
        ///     Field:26 MessageReasonCode
        /// </summary>
        private ushort? _MessageReasonCode;

        /// <summary>
        ///     Field:114 MiniStatementData
        /// </summary>
        private string? _MiniStatementData;

        /// <summary>
        ///     Field:123 MiscellaneousTransactionAttributes
        /// </summary>
        private UAMPMessage? _MiscellaneousTransactionAttributes;

        /// <summary>
        ///     Field:124 MiscellaneousTransactionAttributes2
        /// </summary>
        private UAMPMessage? _MiscellaneousTransactionAttributes2;

        /// <summary>
        ///     Field:109 MultiAccountData
        /// </summary>
        private MultiAccountData[]? _MultiAccountData;

        /// <summary>
        ///     Field:106 MultiCurrencyData
        /// </summary>
        private MultiCurrencyData? _MultiCurrencyData;

        /// <summary>
        ///     Field:70 NetworkManagementInformationCode
        /// </summary>
        private NetworkManagementInformationCodes? _NetworkManagementInformationCode;

        /// <summary>
        ///     Field:63 NewPIN
        /// </summary>
        private string? _NewPIN;

        /// <summary>
        ///     Field:110 NumericMessage
        /// </summary>
        private uint? _NumericMessage;

        /// <summary>
        ///     Field:2 PAN
        /// </summary>
        private string? _PAN;

        /// <summary>
        ///     Field:52 PIN
        /// </summary>
        private string? _PIN;

        /// <summary>
        ///     Field:44 PINCVVverificationResult
        /// </summary>
        private VerificationResult? _PINCVVverificationResult;

        /// <summary>
        ///     Field:25 POSConditionCode
        /// </summary>
        private POSConditionCodes? _POSConditionCode;

        /// <summary>
        ///     Field:22 POSEntryMode
        /// </summary>
        private POSEntryMode? _POSEntryMode;

        /// <summary>
        ///     Field:126 PredAuthorizationParameters
        /// </summary>
        private PredAuthorizationParameters? _PredAuthorizationParameters;

        /// <summary>
        ///     Field:3 ProcessingCode
        /// </summary>
        private ProcessingCode? _processingCode;

        /// <summary>
        ///     Field:100 ReceivingInstitutionIdentificationCOde
        /// </summary>
        private string? _ReceivingInstitutionIdentificationCode;

        /// <summary>
        ///     Field:48 ReferenceToOtherTransaction
        /// </summary>
        private TransactionReference? _ReferenceToOtherTransaction;

        /// <summary>
        ///     Field:108 RegionalListingData
        /// </summary>
        private string? _RegionalListingData;

        /// <summary>
        ///     Field:95 ReplacementAmounts
        /// </summary>
        private ReplacementAmounts? _ReplacementAmounts;

        /// <summary>
        ///     Field:39 ResponseCode
        /// </summary>
        private uint? _ResponseCode;

        /// <summary>
        ///     Field:128 SecondaryMac
        /// </summary>
        private string? _SecondaryMac;

        /// <summary>
        ///     Field:122 SecureData3D
        /// </summary>
        private string? _SecureData3D;

        /// <summary>
        ///     Field:53 SecurityRelatedControlInformation
        /// </summary>
        private byte[]? _SecurityRelatedControlInformation;

        /// <summary>
        ///     Field:18 SIC
        /// </summary>
        private ushort? _SIC;

        /// <summary>
        ///     Field:115 StatementData
        /// </summary>
        private UAMPMessage? _StatementData;

        /// <summary>
        ///     Field:11 SytemTraceAuditNumber
        /// </summary>
        private uint? _SytemTraceAuditNumber;

        /// <summary>
        ///     Field:45 Track1
        /// </summary>
        private string? _Track1;

        /// <summary>
        ///     Field:35 Track2
        /// </summary>
        private string? _Track2;

        /// <summary>
        ///     Field:4 TransactionAmount
        /// </summary>
        private ulong? _TransactionAmount;

        /// <summary>
        ///     Field:49 TransactionCurrencyCode
        /// </summary>
        private ushort? _TransactionCurrencyCode;

        /// <summary>
        ///     Field:37 TransactionRetrievalReferenceNumber
        /// </summary>
        private string? _TransactionRetrievalReferenceNumber;


        /// <summary>
        ///     Field:7 TransmissionGreenwichTime
        /// </summary>
        private string? _TransmissionGreenwichTime;

        /// <summary>
        ///     Field:0 PrimaryBitMap
        /// </summary>
        [JsonIgnore] public bool[] PrimaryBitMap =
            new bool[64];

        /// <summary>
        ///     Field:1 SecondaryBitMap
        /// </summary>
        [JsonIgnore] public bool[]? SecondaryBitMap;


        /// <inheritdoc cref="_AccountBalanceData" />
        public AccountBalanceData? AccountBalanceData
        {
            get => _AccountBalanceData;
            set
            {
                _AccountBalanceData = value;
                SetBitMap(105, value is not null);
            }
        }

        /// <inheritdoc cref="_AccountIdentification2" />
        public UAMPValue? AccountIdentification2
        {
            get => _AccountIdentification2;
            set
            {
                _AccountIdentification2 = value;
                SetBitMap(111, value is not null);
            }
        }

        /// <inheritdoc cref="_AccountIdentificationFROM" />
        public string? AccountIdentificationFROM
        {
            get => _AccountIdentificationFROM;
            set
            {
                _AccountIdentificationFROM = value;
                SetBitMap(102, value is not null);
            }
        }

        /// <inheritdoc cref="_AccountIdentificationTO" />
        public string? AccountIdentificationTO
        {
            get => _AccountIdentificationTO;
            set
            {
                _AccountIdentificationTO = value;
                SetBitMap(103, value is not null);
            }
        }

        /// <inheritdoc cref="_AcquiringInstitutionCountryCode" />
        public ushort? AcquiringInstitutionCountryCode
        {
            get => _AcquiringInstitutionCountryCode;
            set
            {
                _AcquiringInstitutionCountryCode = value;
                SetBitMap(19, value is not null);
            }
        }

        /// <inheritdoc cref="_AcquiringInstitutionIdentification" />
        public string? AcquiringInstitutionIdentification
        {
            get => _AcquiringInstitutionIdentification;
            set
            {
                _AcquiringInstitutionIdentification = value;
                SetBitMap(32, value is not null);
            }
        }

        /// <inheritdoc cref="_AdditionalInformation" />
        public string? AdditionalInformation
        {
            get => _AdditionalInformation;
            set
            {
                _AdditionalInformation = value;
                SetBitMap(127, value is not null);
            }
        }

        /// <inheritdoc cref="_AdditionalPOSData" />
        public POSData? AdditionalPOSData
        {
            get => _AdditionalPOSData;
            set
            {
                _AdditionalPOSData = value;
                SetBitMap(121, value is not null);
            }
        }

        /// <inheritdoc cref="_AdjustmentAmount" />
        public ulong? AdjustmentAmount
        {
            get => _AdjustmentAmount;
            set
            {
                _AdjustmentAmount = value;
                SetBitMap(54, value is not null);
            }
        }

        /// <inheritdoc cref="_AdministrativeTransactionData" />
        public UAMPMessage? AdministrativeTransactionData
        {
            get => _AdministrativeTransactionData;
            set
            {
                _AdministrativeTransactionData = value;
                SetBitMap(125, value is not null);
            }
        }

        /// <inheritdoc cref="_AuthorizationIdentificationResponse" />
        public string? AuthorizationIdentificationResponse
        {
            get => _AuthorizationIdentificationResponse;
            set
            {
                _AuthorizationIdentificationResponse = value;
                SetBitMap(38, value is not null);
            }
        }

        /// <inheritdoc cref="_BillingData" />
        public UAMPMessage? BillingData
        {
            get => _BillingData;
            set
            {
                _BillingData = value;
                SetBitMap(116, value is not null);
            }
        }

        /// <inheritdoc cref="_CardholderBillingAmount" />
        public ulong? CardholderBillingAmount
        {
            get => _CardholderBillingAmount;
            set
            {
                _CardholderBillingAmount = value;
                SetBitMap(6, value is not null);
            }
        }

        /// <inheritdoc cref="_CardholderBillingCurrencyCode" />
        public ushort? CardholderBillingCurrencyCode
        {
            get => _CardholderBillingCurrencyCode;
            set
            {
                _CardholderBillingCurrencyCode = value;
                SetBitMap(51, value is not null);
            }
        }

        /// <inheritdoc cref="_CardIssuerData" />
        public CardIssuer? CardIssuerData
        {
            get => _CardIssuerData;
            set
            {
                _CardIssuerData = value;
                SetBitMap(61, value is not null);
            }
        }

        /// <inheritdoc cref="_CoversionRate" />
        public uint? CoversionRate
        {
            get => _CoversionRate;
            set
            {
                _CoversionRate = value;
                SetBitMap(10, value is not null);
            }
        }

        /// <inheritdoc cref="_ExternalTransactionAttributes" />
        public string? ExternalTransactionAttributes
        {
            get => _ExternalTransactionAttributes;
            set
            {
                _ExternalTransactionAttributes = value;
                SetBitMap(62, value is not null);
            }
        }

        /// <inheritdoc cref="_AcquirerFeeAmount" />
        public AcquirerFeeAmount? AcquirerFeeAmount
        {
            get => _AcquirerFeeAmount;
            set
            {
                _AcquirerFeeAmount = value;
                SetBitMap(28, value is not null);
            }
        }

        /// <inheritdoc cref="_FinalRRN" />
        public string? FinalRRN
        {
            get => _FinalRRN;
            set
            {
                _FinalRRN = value;
                SetBitMap(107, value is not null);
            }
        }

        /// <inheritdoc cref="_ForwardingInstitutionIdentification" />
        public string? ForwardingInstitutionIdentification
        {
            get => _ForwardingInstitutionIdentification;
            set
            {
                _ForwardingInstitutionIdentification = value;
                SetBitMap(33, value is not null);
            }
        }

        /// <inheritdoc cref="_HostNetIdentification" />
        public string? HostNetIdentification
        {
            get => _HostNetIdentification;
            set
            {
                _HostNetIdentification = value;
                SetBitMap(104, value is not null);
            }
        }

        /// <inheritdoc cref="_ICCSystemRelatedData" />
        public byte[]? ICCSystemRelatedData
        {
            get => _ICCSystemRelatedData;
            set
            {
                _ICCSystemRelatedData = value;
                SetBitMap(55, value is not null);
            }
        }

        /// <inheritdoc cref="_MAC" />
        public string? MAC
        {
            get => _MAC;
            set
            {
                _MAC = value;
                SetBitMap(64, value is not null);
            }
        }

        /// <inheritdoc cref="_MBR" />
        public ushort? MBR
        {
            get => _MBR;
            set
            {
                _MBR = value;
                SetBitMap(23, value is not null);
            }
        }

        /// <inheritdoc cref="_MessageReasonCode" />
        public ushort? MessageReasonCode
        {
            get => _MessageReasonCode;
            set
            {
                _MessageReasonCode = value;
                SetBitMap(26, value is not null);
            }
        }

        /// <inheritdoc cref="_MiniStatementData" />
        public string? MiniStatementData
        {
            get => _MiniStatementData;
            set
            {
                _MiniStatementData = value;
                SetBitMap(114, value is not null);
            }
        }

        /// <inheritdoc cref="_MiscellaneousTransactionAttributes" />
        public UAMPMessage? MiscellaneousTransactionAttributes
        {
            get => _MiscellaneousTransactionAttributes;
            set
            {
                _MiscellaneousTransactionAttributes = value;
                SetBitMap(123, value is not null);
            }
        }

        /// <inheritdoc cref="_MiscellaneousTransactionAttributes2" />
        public UAMPMessage? MiscellaneousTransactionAttributes2
        {
            get => _MiscellaneousTransactionAttributes2;
            set
            {
                _MiscellaneousTransactionAttributes2 = value;
                SetBitMap(124, value is not null);
            }
        }

        /// <inheritdoc cref="_MultiAccountData" />
        public MultiAccountData[]? MultiAccountData
        {
            get => _MultiAccountData;
            set
            {
                _MultiAccountData = value;
                SetBitMap(109, value is not null);
            }
        }

        /// <inheritdoc cref="_MultiCurrencyData" />
        public MultiCurrencyData? MultiCurrencyData
        {
            get => _MultiCurrencyData;
            set
            {
                _MultiCurrencyData = value;
                SetBitMap(106, value is not null);
            }
        }

        /// <inheritdoc cref="_NetworkManagementInformationCode" />
        public NetworkManagementInformationCodes? NetworkManagementInformationCode
        {
            get => _NetworkManagementInformationCode;
            set
            {
                _NetworkManagementInformationCode = value;
                SetBitMap(70, value is not null);
            }
        }

        /// <inheritdoc cref="_NewPIN" />
        public string? NewPIN
        {
            get => _NewPIN;
            set
            {
                _NewPIN = value;
                SetBitMap(63, value is not null);
            }
        }

        /// <inheritdoc cref="_NumericMessage" />
        public uint? NumericMessage
        {
            get => _NumericMessage;
            set
            {
                _NumericMessage = value;
                SetBitMap(110, value is not null);
            }
        }

        /// <inheritdoc cref="_PAN" />
        public string? PAN
        {
            get => _PAN;
            set
            {
                _PAN = value;
                SetBitMap(2, value is not null);
            }
        }

        /// <inheritdoc cref="_PIN" />
        public string? PIN
        {
            get => _PIN;
            set
            {
                _PIN = value;
                SetBitMap(52, value is not null);
            }
        }

        /// <inheritdoc cref="_PINCVVverificationResult" />
        public VerificationResult? PINCVVverificationResult
        {
            get => _PINCVVverificationResult;
            set
            {
                _PINCVVverificationResult = value;
                SetBitMap(44, value is not null);
            }
        }

        /// <inheritdoc cref="_POSConditionCode" />
        public POSConditionCodes? POSConditionCode
        {
            get => _POSConditionCode;
            set
            {
                _POSConditionCode = value;
                SetBitMap(25, value is not null);
            }
        }

        /// <inheritdoc cref="_POSEntryMode" />
        public POSEntryMode? POSEntryMode
        {
            get => _POSEntryMode;
            set
            {
                _POSEntryMode = value;
                SetBitMap(22, value is not null);
            }
        }

        /// <inheritdoc cref="_PredAuthorizationParameters" />
        public PredAuthorizationParameters? PredAuthorizationParameters
        {
            get => _PredAuthorizationParameters;
            set
            {
                _PredAuthorizationParameters = value;
                SetBitMap(126, value is not null);
            }
        }

        /// <inheritdoc cref="_processingCode" />
        public ProcessingCode? ProcessingCode
        {
            get => _processingCode;
            set
            {
                _processingCode = value;
                SetBitMap(3, value is not null);
            }
        }

        /// <inheritdoc cref="_ReceivingInstitutionIdentificationCode" />
        public string? ReceivingInstitutionIdentificationCode
        {
            get => _ReceivingInstitutionIdentificationCode;
            set
            {
                _ReceivingInstitutionIdentificationCode = value;
                SetBitMap(100, value is not null);
            }
        }

        /// <inheritdoc cref="_ReferenceToOtherTransaction" />
        public TransactionReference? ReferenceToOtherTransaction
        {
            get => _ReferenceToOtherTransaction;
            set
            {
                _ReferenceToOtherTransaction = value;
                SetBitMap(48, value is not null);
            }
        }

        /// <inheritdoc cref="_RegionalListingData" />
        public string? RegionalListingData
        {
            get => _RegionalListingData;
            set
            {
                _RegionalListingData = value;
                SetBitMap(108, value is not null);
            }
        }

        /// <inheritdoc cref="_ReplacementAmounts" />
        public ReplacementAmounts? ReplacementAmounts
        {
            get => _ReplacementAmounts;
            set
            {
                _ReplacementAmounts = value;
                SetBitMap(95, value is not null);
            }
        }

        /// <inheritdoc cref="_ResponseCode" />
        public uint? ResponseCode
        {
            get => _ResponseCode;
            set
            {
                _ResponseCode = value;
                SetBitMap(39, value is not null);
            }
        }

        /// <inheritdoc cref="_SecondaryMac" />
        public string? SecondaryMac
        {
            get => _SecondaryMac;
            set
            {
                _SecondaryMac = value;
                SetBitMap(128, value is not null);
            }
        }

        /// <inheritdoc cref="_SecureData3D" />
        public string? SecureData3D
        {
            get => _SecureData3D;
            set
            {
                _SecureData3D = value;
                SetBitMap(122, value is not null);
            }
        }

        /// <inheritdoc cref="_SecurityRelatedControlInformation" />
        public byte[]? SecurityRelatedControlInformation
        {
            get => _SecurityRelatedControlInformation;
            set
            {
                if (value != null && value.Length > 48)
                    throw new ArgumentException(
                        $"SecurityRelatedControlInformation >48 bytes, current size {value.Length}");
                _SecurityRelatedControlInformation = value;
                SetBitMap(53, value is not null);
            }
        }

        /// <inheritdoc cref="_SIC" />
        public ushort? SIC
        {
            get => _SIC;
            set
            {
                _SIC = value;
                SetBitMap(18, value is not null);
            }
        }

        /// <inheritdoc cref="_StatementData" />
        public UAMPMessage? StatementData
        {
            get => _StatementData;
            set
            {
                _StatementData = value;
                SetBitMap(115, value is not null);
            }
        }

        /// <inheritdoc cref="_SytemTraceAuditNumber" />
        public uint? SytemTraceAuditNumber
        {
            get => _SytemTraceAuditNumber;
            set
            {
                _SytemTraceAuditNumber = value;
                SetBitMap(11, value is not null);
            }
        }

        /// <inheritdoc cref="_Track1" />
        public string? Track1
        {
            get => _Track1;
            set
            {
                _Track1 = value;
                SetBitMap(45, value is not null);
            }
        }

        /// <inheritdoc cref="_Track2" />
        public string? Track2
        {
            get => _Track2;
            set
            {
                _Track2 = value;
                SetBitMap(35, value is not null);
            }
        }

        /// <inheritdoc cref="_TransactionAmount" />
        public ulong? TransactionAmount
        {
            get => _TransactionAmount;
            set
            {
                _TransactionAmount = value;
                SetBitMap(4, value is not null);
            }
        }

        /// <inheritdoc cref="_TransactionCurrencyCode" />
        public ushort? TransactionCurrencyCode
        {
            get => _TransactionCurrencyCode;
            set
            {
                _TransactionCurrencyCode = value;
                SetBitMap(49, value is not null);
            }
        }

        /// <inheritdoc cref="_TransactionRetrievalReferenceNumber" />
        public string? TransactionRetrievalReferenceNumber
        {
            get => _TransactionRetrievalReferenceNumber;
            set
            {
                _TransactionRetrievalReferenceNumber = value;
                SetBitMap(37, value is not null);
            }
        }

        /// <inheritdoc cref="_TransmissionGreenwichTime" />
        public string? TransmissionGreenwichTime
        {
            get => _TransmissionGreenwichTime;
            set
            {
                _TransmissionGreenwichTime = value;
                SetBitMap(7, value is not null);
            }
        }

        public bool CheckBitExist(int fieldNumber)
        {
            if (fieldNumber < 65)
            {
                return PrimaryBitMap[fieldNumber - 1];
            }
            else
            {
                return SecondaryBitMap != null && SecondaryBitMap[fieldNumber - 65];
            }
        }

        private void SetBitMap(int position, bool value)
        {
            if (position < 65)
            {
                PrimaryBitMap[position - 1] = value;
            }
            else
            {
                if (SecondaryBitMap is null)
                {
                    SecondaryBitMap ??= new bool[64];
                    PrimaryBitMap[0] = true;
                }

                SecondaryBitMap[position - 65] = value;
                if (SecondaryBitMap.All(b => !b)) PrimaryBitMap[0] = false;
            }
        }

        public override string ToString()
        {
            var str = "ISO8583:\n";
            var t = typeof(ISO8583);
            var fields = t.GetProperties();
            str += $"\tPrimaryBitMap: {new string(PrimaryBitMap.Select(b => b ? '1' : '0').ToArray())}\n";
            str += $"\tSecondaryBitMap: {new string(SecondaryBitMap?.Select(b => b ? '1' : '0').ToArray())}\n";
            foreach (var field in fields)
                str += $"\t{field.Name}: {field.GetValue(this)?.ToString() ?? "null"}\n";

            // var val =field.GetValue(this) ;

            // cstr += val switch
            // {
            //     IEnumerable<bool> => new string((val as bool[]).Select(b => b ? '1' : '0').ToArray()),
            //     null => "null",
            //     _ => val.ToString(),
            // };
            // str += cstr + '\n';

            return str;
        }

        #region CardAcceptorTerminal

        /// <summary>
        ///     Field:41,43 CardAcceptorTerminal
        /// </summary>
        private Terminal? _CardAcceptorTerminal;

        /// <inheritdoc cref="_CardAcceptorTerminal" />
        public Terminal? CardAcceptorTerminal
        {
            get => _CardAcceptorTerminal;
            set
            {
                _CardAcceptorTerminal = value;
                SetBitMap(41, value?.ID is not null);
                SetBitMap(43, value?.Info is not null);
            }
        }

        [JsonIgnore]
        public string? CardAcceptorTerminalID
        {
            get => _CardAcceptorTerminal?.ID;
            set
            {
                _CardAcceptorTerminal ??= new Terminal();
                _CardAcceptorTerminal.ID = value;
                SetBitMap(41, value is not null);
            }
        }

        [JsonIgnore]
        public Terminal.TerminalInfo? CardAcceptorTerminalInfo
        {
            get => _CardAcceptorTerminal?.Info;
            set
            {
                _CardAcceptorTerminal ??= new Terminal();
                _CardAcceptorTerminal.Info = value;
                SetBitMap(43, value is not null);
            }
        }

        #endregion

        #region LocalTransactionDateTime

        //TODO: разделить. в случае Отсутствия одного из полей
        private DateTime? _localTransactionDateTime;


        /// <summary>
        ///     Field:12 LocalTransactionTime
        /// </summary>
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan? LocalTransactionTime

        {
            get => _localTransactionDateTime?.TimeOfDay;
            set
            {
                if (_localTransactionDateTime is null) _localTransactionDateTime = DateTime.Now;

                _localTransactionDateTime = _localTransactionDateTime?.Date + value;
                SetBitMap(12, value is not null);
            }
        }

        /// <summary>
        ///     Field:13 LocalTransactionDate
        /// </summary>
        public DateTime? LocalTransactionDate
        {
            get => _localTransactionDateTime?.Date;
            set
            {
                if (_localTransactionDateTime is null) _localTransactionDateTime = DateTime.Now;

                _localTransactionDateTime = value?.Date + _localTransactionDateTime?.TimeOfDay;
                SetBitMap(13, value is not null);
            }
        }

        #endregion
    }
}