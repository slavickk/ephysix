using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     Dictionary of UAMP parameters.
    /// </summary>
    public record UAMPMessage : UAMPValue
    {
        private static readonly string ParameterSeparator = $"(?<!{(char) Symbols.SP}){(char) Symbols.PS}";
        public override UAMPType Type => UAMPType.UAMPMessage;
        public Dictionary<string, UAMPValue?> Value { get; set; }

        public UAMPValue? this[string key]
        {
            get => Value[key];
            set => Value[key] = value;
        }

        public UAMPMessage()
        {
            Value = new Dictionary<string, UAMPValue?>();
        }

        public UAMPMessage(params KeyValuePair<string, UAMPValue?>[] values)
        {
            Value = new Dictionary<string, UAMPValue?>(values);
        }


        /// <summary>
        ///     Parse UAMP Message from string
        /// </summary>
        /// <param name="uampmessage"></param>
        public UAMPMessage(string uampmessage)
        {
            Value = new Dictionary<string, UAMPValue?>();
            string[] parameters = Regex.Split(uampmessage, ParameterSeparator);
            foreach (var parameter in parameters)
            {
                var keyval = parameter.Split((char) Symbols.Eq, 2);
                if (keyval.Length < 2) throw new ArgumentException("Message not contain '='", "uampmessage");

                Value[keyval[0]] = ParseValue(keyval[1]);
            }
        }


        /// <summary>
        ///     Serialize message as UAMP.
        /// </summary>
        /// <returns>string in uamp format</returns>
        public override string Serialize()
        {
            return string.Join((char) Symbols.PS,
                Value.Select(pair => pair.Key + (char) Symbols.Eq + SerializeValue(pair.Value)));
        }


        public void Add(string key, UAMPValue? value)
        {
            Value[key] = value;
        }
    }
}