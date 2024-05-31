/******************************************************************
 * File: Subdivision.cs
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

#region

using MaxMind.Db;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the subdivisions associated with an IP address.
    ///     Do not use any of the subdivision names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> or <see cred="IsoCode" />
    ///     instead.
    /// </summary>
    public class Subdivision : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Subdivision()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Subdivision(
            int? confidence = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            [Parameter("iso_code")] string? isoCode = null,
            IReadOnlyDictionary<string, string>? names = null,
            IReadOnlyList<string>? locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
            IsoCode = isoCode;
        }

        /// <summary>
        ///     This is a value from 0-100 indicating MaxMind's confidence that
        ///     the subdivision is correct. This value is only set when using the
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        ///     This is a string up to three characters long contain the
        ///     subdivision portion of the
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/ISO_3166-2 ISO 3166-2">
        ///         code
        ///     </a>
        ///     .
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("iso_code")]
        public string? IsoCode { get; internal set; }
    }
}