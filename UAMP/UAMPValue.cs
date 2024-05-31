/******************************************************************
 * File: UAMPValue.cs
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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace UAMP
{
    /// <summary>
    ///     Base class of UAMP values
    /// </summary>
    [JsonConverter(typeof(UAMPValueJsonConverter))]
    public abstract record UAMPValue
    {
        /// <summary>
        ///     Type of UAMP object
        /// </summary>
        /// <seealso cref="UAMPType" />
        public abstract UAMPType Type { get; }

        public virtual bool Equals(UAMPValue? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type;
        }


        /// <summary>
        ///     Parse value of uamp parameter
        /// </summary>
        /// <param name="uampvalue"></param>
        /// <returns></returns>
        public static UAMPValue? ParseValue(string uampvalue)
        {
            if (Regex.IsMatch(uampvalue, $"(?<!{(char)Symbols.SP}){(char)Symbols.IS}"))
                return new UAMPArray(uampvalue);

            if (Regex.IsMatch(uampvalue, $"(?<!{(char)Symbols.SP}){(char)Symbols.FS}"))
                return new UAMPStruct(uampvalue);

            if (uampvalue.Contains($"{(char)Symbols.SP}0a"))
                return new UAMPPackage(SecondTypeEscaping.Escape(uampvalue));

            if (Regex.IsMatch(uampvalue, @"=\b")) //{(char) Symbols.SP}({(char) Symbols.PS}|10)
            {
                string _uampvalue;
                if (Regex.IsMatch(uampvalue,
                        $"[{(char)Symbols.IS}{(char)Symbols.FS}{(char)Symbols.PS}{(char)Symbols.MS}{(char)Symbols.NI}]"))
                    _uampvalue = FirstTypeEscaping.Unescape(uampvalue);
                else
                    _uampvalue = SecondTypeEscaping.Unescape(uampvalue);
                return new UAMPMessage(_uampvalue);
            }


            if (uampvalue.Length == 1 && uampvalue[0] == (char)Symbols.NI) return null;

            return new UAMPScalar(uampvalue);
        }

        public static implicit operator UAMPValue(string val)
        {
            return new UAMPScalar(val);
        }


        /// <summary>
        ///     Serialize type in UAMP format
        /// </summary>
        /// <returns>string in UAMP format</returns>
        public abstract string Serialize();

        protected string SerializeValue(UAMPValue? value)
        {
            if (value is null) return "" + (char)Symbols.NI;

            string s = value.Serialize();
            switch (value.Type)
            {
                case UAMPType.UAMPPackage:
                    if ((value as UAMPPackage)!.Value.Count > 1)
                        return SecondTypeEscaping.Escape(s);
                    else
                        return FirstTypeEscaping.Escape(s);
                case UAMPType.UAMPMessage:
                    return FirstTypeEscaping.Escape(s);
                default:
                    return s;
            }
        }
    }

    public class UAMPValueJsonConverter : JsonConverter<UAMPValue>
    {
        public override UAMPValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);
            return jsonDocument.RootElement.ParseAsUAMP();
        }

        public override void Write(Utf8JsonWriter writer, UAMPValue value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType());
        }
    }

    public static class UAMPValueExtensions
    {
        public static UAMPValue? ParseAsUAMP(this JsonElement document)
        {
            if (document.ValueKind == JsonValueKind.Null) return null;

            var t = (UAMPType)document.GetProperty("Type").GetInt16();
            switch (t)
            {
                case UAMPType.Scalar:
                    return new UAMPScalar(document.GetProperty("Value").GetString());
                case UAMPType.Struct:
                    return new UAMPStruct(document.GetProperty("Value").EnumerateArray()
                        .Select(element => element.ParseAsUAMP()).ToArray());
                case UAMPType.Array:
                    return JsonSerializer.Deserialize<UAMPArray>(document.GetRawText());
                case UAMPType.UAMPMessage:
                    return JsonSerializer.Deserialize<UAMPMessage>(document.GetRawText());
                case UAMPType.UAMPPackage:
                    return JsonSerializer.Deserialize<UAMPPackage>(document.GetRawText());
                default:
                    throw new JsonException();
            }
        }
    }
}