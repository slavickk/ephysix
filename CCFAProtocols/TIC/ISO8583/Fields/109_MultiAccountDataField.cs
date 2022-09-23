using System.ComponentModel.DataAnnotations;
using System.IO;
using CCFAProtocols.TIC.Instruments;

namespace CCFAProtocols.TIC.ISO8583.Fields
{
    public static class MultiAccountDataField
    {
        public const byte FieldNumber = 109;

        public static void Deserialize(BinaryReader reader, ISO8583 isomessage)
        {
            if (isomessage.CheckBitExist(FieldNumber))
            {
                string s = reader.LVARReadString(3);
                for (var i = 0; i < s.Length / 59; i += 59)
                    isomessage.MultiAccountData[i] = new MultiAccountData
                    {
                        Type = byte.Parse(s[0].ToString()),
                        Account = s[1..31].Trim(),
                        Title = s[31..57].Trim(),
                        Currency = ushort.Parse(s[57..60])
                    };
            }
        }

        public static void Serialize(BinaryWriter writer, ISO8583 isomessage)
        {
            if (isomessage.MultiAccountData is null) return;
            string s = string.Empty;
            foreach (var multiAccountData in isomessage.MultiAccountData)
                s +=
                    $"{multiAccountData.Type}{multiAccountData.Account.PadRight(30)}{multiAccountData.Title.PadRight(25)}{multiAccountData.Currency}";
            writer.LVARWriteString(3, s);
        }
    }

    public struct MultiAccountData
    {
        [Range(1, 2)] public byte Type;
        [MaxLength(30)] public string Account;
        [MaxLength(25)] public string Title;
        [Range(0, 999)] public ushort Currency;
    }
}