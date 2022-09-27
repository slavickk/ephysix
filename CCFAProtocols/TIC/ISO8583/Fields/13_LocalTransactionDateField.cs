using System;
using System.Globalization;
using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class LocalTransactionDateField
    {
        public const byte FieldNumber = 13;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                char[] DateChars = reader.ReadChars(4);
                if (DateChars[0] == '0' && DateChars[1] == '0' && DateChars[2] == '0' && DateChars[3] == '0')
                {
                    isomessage.LocalTransactionDate = null;
                    return;
                }

                isomessage.LocalTransactionDate =
                    DateTime.ParseExact(DateChars, "MMdd", CultureInfo.InvariantCulture);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.LocalTransactionDate is null) return;
            var date = isomessage.LocalTransactionDate?.ToString("MMdd");
            writer.Write(4, date);
        }
    }
}