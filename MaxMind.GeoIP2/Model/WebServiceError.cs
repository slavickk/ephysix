/******************************************************************
 * File: WebServiceError.cs
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

using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data about an error that occurred while calling the web service
    /// </summary>
    internal class WebServiceError
    {
        /// <summary>
        ///     Gets or sets the error.
        /// </summary>
        /// <value>
        ///     The error message returned by the service.
        /// </value>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>
        ///     The error code returned by the service.
        /// </value>
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}