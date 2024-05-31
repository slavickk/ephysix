/******************************************************************
 * File: Continent.cs
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
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the continent record associated with an IP address.
    ///     Do not use any of the continent names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> or <see cred="Code" />
    ///     instead.
    /// </summary>
    public class Continent : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Continent()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Continent(
            string? code = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            IReadOnlyDictionary<string, string>? names = null,
            IReadOnlyList<string>? locales = null)
            : base(geoNameId, names, locales)
        {
            Code = code;
        }

        /// <summary>
        ///     A two character continent code like "NA" (North America) or "OC"
        ///     (Oceania).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("code")]
        public string? Code { get; internal set; }
    }
}