public enum Symbols : byte
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