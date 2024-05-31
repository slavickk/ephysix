﻿/******************************************************************
 * File: Country.cs
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
    ///     Contains data for the country record associated with an IP address.
    ///     Do not use any of the country names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> or <see cred="IsoCode" />
    ///     instead.
    /// </summary>
    public class Country : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Country()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Country(
            int? confidence = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            [Parameter("is_in_european_union")] bool isInEuropeanUnion = false,
            [Parameter("iso_code")] string? isoCode = null,
            IReadOnlyDictionary<string, string>? names = null,
            IReadOnlyList<string>? locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
            IsoCode = isoCode;
            IsInEuropeanUnion = isInEuropeanUnion;
        }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the country
        ///     is correct. This value is only set when using the Insights
        ///     web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        ///     This is true if the country is a member state of the
        ///     European Union. This is available from  all location
        ///     services and databases.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_in_european_union")]
        public bool IsInEuropeanUnion { get; internal set; }

        /// <summary>
        ///     The
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/ISO_3166-1">
        ///         two-character ISO
        ///         3166-1 alpha code
        ///     </a>
        ///     for the country.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("iso_code")]
        public string? IsoCode { get; internal set; }
    }
}