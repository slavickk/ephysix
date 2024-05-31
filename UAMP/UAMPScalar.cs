/******************************************************************
 * File: UAMPScalar.cs
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

        public override string ToString() => Value;
    }
}