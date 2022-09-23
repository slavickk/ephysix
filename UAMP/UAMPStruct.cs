using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     UAMP struct. Contains <see cref="UAMPScalar" />
    /// </summary>
    public record UAMPStruct : UAMPValue
    {
        // private static readonly string StructSeparator = $"(?<!{(char) Symbols.SP}){(char) Symbols.FS}";
        private static readonly string StructSeparator = $"(?<!{(char) Symbols.SP}){(char) Symbols.FS}";
        public override UAMPType Type => UAMPType.Struct;
        public UAMPValue?[] Value { get; set; }

        public UAMPValue? this[int i]
        {
            get => Value[i];
            set => Value[i] = value;
        }

        /// <summary>
        ///     Instance UAMPStruct
        /// </summary>
        /// <param name="values">list of fields</param>
        public UAMPStruct(params UAMPValue?[] values)
        {
            Value = values;
        }

        public UAMPStruct()
        {
            Value = Array.Empty<UAMPScalar?>();
        }

        /// <summary>
        ///     Deserialize UAMPStruct message from string
        /// </summary>
        /// <remarks> String must contain <c>FS</c> </remarks>
        /// <param name="uampvalue">string contains uamp struct</param>
        public UAMPStruct(string? uampvalue)
        {
            Value = Regex.Split(uampvalue, StructSeparator).Select(s => ParseValue(s)).ToArray();
        }

        public override string Serialize()
        {
            return string.Join((char) Symbols.FS,
                Value.Select(uampValue => uampValue is null ? $"{(char) Symbols.NI}" : SerializeValue(uampValue))
                    .ToArray());
        }

        public virtual bool Equals(UAMPStruct? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Value.SequenceEqual(other.Value);
        }
    }
}