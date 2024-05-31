﻿/******************************************************************
 * File: RepresentedCountry.cs
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
    ///     Contains data for the represented country associated with an IP address.
    ///     This class contains the country-level data associated with an IP address for
    ///     the IP's represented country. The represented country is the country
    ///     represented by something like a military base.
    ///     Do not use any of the country names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> or <see cred="IsoCode" />
    ///     instead.
    /// </summary>
    public class RepresentedCountry : Country
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public RepresentedCountry()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public RepresentedCountry(
            string? type = null,
            int? confidence = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            [Parameter("is_in_european_union")] bool isInEuropeanUnion = false,
            [Parameter("iso_code")] string? isoCode = null,
            IReadOnlyDictionary<string, string>? names = null,
            IReadOnlyList<string>? locales = null)
            : base(confidence, geoNameId, isInEuropeanUnion, isoCode, names, locales)
        {
            Type = type;
        }

        /// <summary>
        ///     A string indicating the type of entity that is representing the
        ///     country. Currently we only return <c>military</c> but this could
        ///     expand to include other types in the future.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("type")]
        public string? Type { get; internal set; }
    }
}