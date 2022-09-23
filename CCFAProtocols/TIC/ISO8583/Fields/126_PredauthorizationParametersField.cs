using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class PredauthorizationParametersField
    {
        public const byte FieldNumber = 126;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                isomessage.PredAuthorizationParameters = new PredAuthorizationParameters()
                {
                    OriginalTransactionInvoiceNumber = reader.ReadString(16),
                    OriginalSeqNumber = reader.ReadString(9),
                    PredAuthorizationHold = reader.ReadString(9)
                };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.PredAuthorizationParameters is null) return;
            writer.Write(16, isomessage.PredAuthorizationParameters.OriginalTransactionInvoiceNumber);
            writer.Write(9, isomessage.PredAuthorizationParameters.OriginalSeqNumber);
            writer.Write(9, isomessage.PredAuthorizationParameters.PredAuthorizationHold);
        }
    }

    public class PredAuthorizationParameters
    {
        public string OriginalSeqNumber;
        public string OriginalTransactionInvoiceNumber;
        public string PredAuthorizationHold;

        public override string ToString()
        {
            return
                $"OriginalSeqNumber:{OriginalSeqNumber},OriginalTransactionInvoiceNumber:{OriginalTransactionInvoiceNumber},PredAuthorizationHold:{PredAuthorizationHold}";
        }
    }
}