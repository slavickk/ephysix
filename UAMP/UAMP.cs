using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     Type contains list of messages
    /// </summary>
    public record UAMPPackage : UAMPValue
    {
        private static readonly string MessageSeparator = $"(?<!{(char) Symbols.SP}){(char) Symbols.MS}";

        /// <summary>
        /// List of messages
        /// </summary>
        /// <remarks> In serialized view, messages separated by <c>MS</c></remarks>
        public List<UAMPMessage> Value { get; set; } = new();

        public UAMPMessage this[int index]
        {
            get => Value[index];
            set => Value[index] = value;
        }

        public UAMPPackage()
        {
        }

        /// <summary>
        /// Deserialize UAMP message from string
        /// </summary>
        /// <remarks> String must contain <c>PS</c> </remarks>
        /// <param name="uampmessage">string contains uamp message</param>
        public UAMPPackage(string uampmessage)
        {
            foreach (var message in Regex.Split(uampmessage, MessageSeparator))
            {
                if (message==String.Empty)
                {
                     continue;
                }
                Value.Add(new UAMPMessage(message));
            }
        }

        public override UAMPType Type => UAMPType.UAMPPackage;


        public override string Serialize()
        {
            return string.Join((char) Symbols.MS, Value.Select(item => item.Serialize()));
        }


        public override string ToString()
        {
            return '[' + string.Join(',', Value) + ']';
        }

        public virtual bool Equals(UAMPPackage? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Value.Equals(other.Value);
        }
    }
}