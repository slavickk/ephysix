/******************************************************************
 * File: Postal.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARDummyProtocol1ULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

#region

using MaxMind.Db;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the postal record associated with an IP address.
    /// </summary>
    public class Postal
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Postal()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Postal(string? code = null, int? confidence = null)
        {
            Code = code;
            Confidence = confidence;
        }

        /// <summary>
        ///     The postal code of the location. Postal codes are not available
        ///     for all countries. In some countries, this will only contain part
        ///     of the postal code.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("code")]
        public string? Code { get; internal set; }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the
        ///     postal code is correct. This value is only set when using the
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Code: {Code}, Confidence: {Confidence}";
        }
    }
}