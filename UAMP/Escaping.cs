using System.Globalization;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     Escapes special characters by adding a character [SP]
    ///     <example>[PS]->[SP][PS}</example>
    /// </summary>
    public static class FirstTypeEscaping
    {
        private static readonly string SpecialChars =
            $"{(char) Symbols.IS}{(char) Symbols.FS}{(char) Symbols.PS}{(char) Symbols.MS}{(char) Symbols.NI}{(char) Symbols.SP}";

        public static string Escape(string uampmessage)
        {
            return Regex.Replace(uampmessage, $"([{SpecialChars}])", $"{(char) Symbols.SP}$1");
        }

        public static string Unescape(string uampmessage)
        {
            return Regex.Replace(uampmessage, $"{(char) Symbols.SP}" + @"(\W)", "$1");
        }
    }

    /// <summary>
    ///     Escapes special characters by adding [SP] and replacing the special character with a hexadecimal code
    ///     <example>[PS]->[SP]10</example>
    /// </summary>
    public static class SecondTypeEscaping
    {
        private static readonly string SpecialChars =
            $"{(char) Symbols.IS}{(char) Symbols.FS}{(char) Symbols.PS}{(char) Symbols.MS}{(char) Symbols.NI}{(char) Symbols.SP}";

        public static string Escape(string uampmessage)
        {
            return Regex.Replace(uampmessage, $"[{SpecialChars}]",
                match => (char) Symbols.SP + ((byte) match.Value[0]).ToString("X2"));
        }

        public static string Unescape(string uampmessage)
        {
            return Regex.Replace(uampmessage, (char) Symbols.SP + @"(\w{2})",
                match => "" + (char) byte.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier));
        }
    }
}