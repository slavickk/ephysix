/******************************************************************
 * File: City.cs
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
    ///     City-level data associated with an IP address.
    /// </summary>
    /// <remarks>
    ///     Do not use any of the city names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> instead.
    /// </remarks>
    public class City : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public City()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public City(int? confidence = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            IReadOnlyDictionary<string, string>? names = null,
            IReadOnlyList<string>? locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
        }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the city
        ///     is correct. This value is only set when using the Insights
        ///     web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; internal set; }
    }
}