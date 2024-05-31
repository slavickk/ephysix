/******************************************************************
 * File: MaxMind.cs
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
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data related to your MaxMind account.
    /// </summary>
    public class MaxMind
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public MaxMind()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public MaxMind([Parameter("queries_remaining")] int queriesRemaining)
        {
            QueriesRemaining = queriesRemaining;
        }

        /// <summary>
        ///     The number of remaining queries in your account for the web
        ///     service end point. This will be null when using a local
        ///     database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("queries_remaining")]
        public int? QueriesRemaining { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"MaxMind [ QueriesRemaining={QueriesRemaining} ]";
        }
    }
}
