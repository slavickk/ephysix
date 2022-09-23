namespace UAMP
{
    /// <summary>
    ///     Types of UAMP values
    /// </summary>
    public enum UAMPType
    {
        /// <summary>
        ///     <seealso cref="UAMPScalar" />
        /// </summary>
        Scalar,

        /// <summary>
        ///     <seealso cref="UAMPStruct" />
        /// </summary>
        Struct,


        /// <summary>
        ///     <seealso cref="UAMPArray" />
        /// </summary>
        Array,

        /// <summary>
        ///     <seealso cref="UAMPMessage" />
        /// </summary>
        UAMPMessage,

        /// <summary>
        ///     <seealso cref="UAMP" />
        /// </summary>
        UAMPPackage
    }
}