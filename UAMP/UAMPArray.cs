using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     UAMP array. May contain <see cref="UAMPStruct" /> or <see cref="UAMPScalar" />
    /// </summary>
    public record UAMPArray : UAMPValue
    {
        private static readonly string ArraySeparator = $"(?<!{(char) Symbols.SP}){(char) Symbols.IS}";

        /// <summary>
        ///     Values of UAMPArray. May contain <see cref="UAMPStruct" /> or <see cref="UAMPScalar" />
        /// </summary>
        public UAMPValue?[] Value { get; set; }

        public override UAMPType Type => UAMPType.Array;

        public UAMPValue? this[int index]
        {
            get => Value[index];
            set => Value[index] = value;
        }

        /// <summary>
        ///     Instance of UAMPArray
        /// </summary>
        /// <param name="values">list of uampvalues</param>
        public UAMPArray(params UAMPValue?[] values)
        {
            Value = values;
        }

        public UAMPArray()
        {
            Value = Array.Empty<UAMPValue?>();
        }

        /// <summary>
        ///     Deserialize UAMPArr message from string
        /// </summary>
        /// <remarks> String must contain <c>IS</c> </remarks>
        /// <param name="uampvalue">string contains uamp array</param>
        public UAMPArray(string uampvalue)
        {
            Value = Regex.Split(uampvalue, ArraySeparator).Select(s => ParseValue(s)).ToArray();
        }


        public override string Serialize()
        {
            return string.Join((char) Symbols.IS,
                Value.Select(uampValue => uampValue is null ? $"{(char) Symbols.NI}" : SerializeValue(uampValue)));
        }

        public virtual bool Equals(UAMPArray? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Value.SequenceEqual(other.Value);
        }
    }
}