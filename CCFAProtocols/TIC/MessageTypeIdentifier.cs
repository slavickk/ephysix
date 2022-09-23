using System;
using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC
{
    public enum EnumMessageTypeIdentifier : short
    {
        AuthorizationRequest = 100,
        AuthorizationResponse = 110,

        /// <remarks>Balance Request (TCI-from-Host)</remarks>
        PreauthorizationRequest = 103,

        /// <remarks>Balance Response (TCI-from-BS)</remarks>
        PreauthorizationResponse = 113,
        PostauthorizationRequest = 104,
        PostauthorizationResponse = 114,
        FinancialTransactionRequest = 200,
        FinanciatlPreauthorizationRequsest = 203,
        FinancialPostauthorizationRequest = 204,
        FinancialTransactionRequestResponse = 210,
        FinancialPreauthorizationResponse = 213,
        FinancialPostauthorizationResponse = 214,
        AuthorizationAdvice = 120,
        AuthorizationAdviceResponse = 130,

        /// <remarks>Representment,Adjustment Advice</remarks>
        FinancialTransactionAdvice = 220,

        ///<remarks>Representment Response,Adjustment Advice Response</remarks>
        FinancialTransactionAdviceResponse = 230,
        BatchUploadAdvice = 320,
        BatchUploadAdviceResponse = 330,
        AcquirerReversalRequest = 400,
        AcquirerRequestRequestResponse = 410,
        AcquirerReversalAdvice = 420,
        AcquirerReversalAdviceResponse = 430,

        ///<remarks>Change Token Status request, PIN Verification File Update Request, Chargback Request</remarks>
        StoplistRequest = 422,

        ///<remarks>Change Token Status response, Pin Verification File Update Request Chargback Response</remarks>
        StoplistResponse = 432,
        AdministrativeRequest = 600,
        AdministrativeResponse = 610,
        AdministrativeAdbvice = 620,
        AdministrativeAdviceResponse = 630,
        NetworkManagementRequest = 800,
        NetworkManagementRequestResponse = 810
    }

    /// <summary>
    /// TIC Message Type
    /// </summary>
    /// <remarks>4 decimal digits</remarks>
    public class MessageTypeIdentifier
    {
        private char _isReject; // true=9 false=0
        public EnumMessageTypeIdentifier TypeIdentifier;

        public bool IsReject
        {
            get => _isReject == '9';
            set => _isReject = value ? '9' : '0';
        }


        public void Serialize(BinaryWriter writer)
        {
            writer.Write(_isReject);
            writer.Write(3, (short) TypeIdentifier);
        }

        public static MessageTypeIdentifier Deserialize(BinaryReader reader)
        {
            return new MessageTypeIdentifier()
            {
                _isReject = reader.ReadChar(),
                TypeIdentifier = (EnumMessageTypeIdentifier) reader.ReadUint(3)
            };
        }

        public override string ToString()
        {
            return
                $"MIT:\n\t isReject:{_isReject} {Enum.GetName(TypeIdentifier)} ({(short) TypeIdentifier})";
        }
    }
}