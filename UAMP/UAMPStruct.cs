/******************************************************************
 * File: UAMPStruct.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     UAMP struct. Contains <see cref="UAMPScalar" />
    /// </summary>
    public record UAMPStruct : UAMPValue
    {
        // private static readonly string StructSeparator = $"(?<!{(char) Symbols.SP}){(char) Symbols.FS}";
        private static readonly string StructSeparator = $"(?<!{(char)Symbols.SP}){(char)Symbols.FS}";

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

        public override UAMPType Type => UAMPType.Struct;
        public UAMPValue?[] Value { get; set; }

        public UAMPValue? this[int i]
        {
            get => Value[i];
            set => Value[i] = value;
        }

        public virtual bool Equals(UAMPStruct? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Value.SequenceEqual(other.Value);
        }

        public override string Serialize()
        {
            return string.Join((char)Symbols.FS,
                Value.Select(uampValue => uampValue is null ? $"{(char)Symbols.NI}" : SerializeValue(uampValue))
                    .ToArray());
        }

        protected override bool PrintMembers(StringBuilder builder)
        {
            // builder.Append("{");
            builder.AppendJoin(" ; ", Value.Select(value => value?.ToString()));
            // builder.Append("}");
            return true;
        }
    }
}