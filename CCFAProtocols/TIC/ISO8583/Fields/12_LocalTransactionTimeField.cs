using System;
using System.Globalization;
using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class LocalTransactionTimeField
    {
        public const byte FieldNumber = 12;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                char[] timechars = reader.ReadChars(6);
                isomessage.LocalTransactionTime =
                    TimeSpan.ParseExact(timechars, "hhmmss", CultureInfo.InvariantCulture);
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.LocalTransactionTime is null) return;
            var time = isomessage.LocalTransactionTime?.ToString(@"hhmmss", CultureInfo.InvariantCulture);
            writer.Write(6, time);
        }
    }
}