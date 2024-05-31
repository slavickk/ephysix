/******************************************************************
 * File: ConnectionTypeResponse.cs
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

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Connection-Type response.
    /// </summary>
    public class ConnectionTypeResponse : AbstractResponse
    {
        /// <summary>
        ///     Construct ConnectionTypeResponse model
        /// </summary>
        public ConnectionTypeResponse()
        {
        }

        /// <summary>
        ///     Construct ConnectionTypeResponse model
        /// </summary>
        [Constructor]
        public ConnectionTypeResponse(
            [Parameter("connection_type")] string? connectionType,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null
        )
        {
            ConnectionType = connectionType;
            IPAddress = ipAddress;
            Network = network;
        }

        /// <summary>
        ///     The connection type of the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("connection_type")]
        public string? ConnectionType { get; internal set; }

        /// <summary>
        ///     The IP address that the data in the model is for. If you
        ///     performed a "me" lookup against the web service, this will be the
        ///     externally routable IP address for the system the code is running
        ///     on. If the system is behind a NAT, this may differ from the IP
        ///     address locally assigned to it.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ip_address")]
        public string? IPAddress { get; internal set; }

        /// <summary>
        ///     The network associated with the record. In particular, this is
        ///     the largest network where all of the fields besides
        ///     <c>IPAddress</c> have the same value.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network")]
        public Network? Network { get; internal set; }
    }
}