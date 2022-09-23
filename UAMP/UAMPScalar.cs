namespace UAMP
{
    /// <summary>
    ///     String value of uamp
    /// </summary>
    public record UAMPScalar : UAMPValue
    {
        public UAMPScalar(string? uampvalue)
        {
            Value = uampvalue;
        }

        public string Value { get; set; }

        public override UAMPType Type => UAMPType.Scalar;

        public override string Serialize()
        {
            return Value;
        }

        public static implicit operator UAMPScalar(string val)
        {
            return new(val);
        }

        public static implicit operator string(UAMPScalar val)
        {
            return val.Value;
        }
    }
}