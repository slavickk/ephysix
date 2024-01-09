using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     UAMP array. May contain <see cref="UAMPStruct" /> or <see cref="UAMPScalar" />
    /// </summary>
    public record UAMPArray : UAMPValue
    {
        private static readonly string ArraySeparator = $"(?<!{(char)Symbols.SP}){(char)Symbols.IS}";

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

        public virtual bool Equals(UAMPArray? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Value.SequenceEqual(other.Value);
        }


        public override string Serialize()
        {
            return string.Join((char)Symbols.IS,
                Value.Select(uampValue => uampValue is null ? $"{(char)Symbols.NI}" : SerializeValue(uampValue)));
        }

        protected override bool PrintMembers(StringBuilder builder)
        {
            builder.AppendLine();
            for (var i = 0; i < Value.Length; i++)
            {
                builder.AppendLine($"\t\t[{i}]: {Value[i]}");
            }

            return true;
        }
    }
}