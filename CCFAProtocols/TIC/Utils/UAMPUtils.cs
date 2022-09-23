namespace CCFAProtocols.TIC.Instruments
{
    public enum UAMPSymbols
    {
        /// <summary>
        ///     Message Seporator. Use between "key"="val"
        /// </summary>
        MS = 0x0a,

        /// <summary>
        ///     Parameter separator
        /// </summary>
        PS = 0x10,

        /// <summary>
        ///     Item Separator. Use in array
        /// </summary>
        IS = 0x1d,

        /// <summary>
        ///     Field Separator. Use in structs
        /// </summary>
        FS = 0x1c,

        /// <summary>
        ///     Null indicator
        /// </summary>
        NI = 0x07,

        /// <summary>
        ///     Escaping char.
        /// </summary>
        SP = 0x13,

        /// <summary>
        ///     "=" in ASCII encoding.
        /// </summary>
        Eq = 0x3d
    }

    public static class UAMPUtils
    {
        /// <summary>
        ///     Check is value null.
        /// </summary>
        /// <param name="s">string to check</param>
        /// <returns>null if value is <see cref="UAMPSymbols.NI" />. Else return s.</returns>
        public static string? IsNull(string s)
        {
            return s.Length == 1 && s[0] == (char) UAMPSymbols.NI ? null : s;
        }
    }
}