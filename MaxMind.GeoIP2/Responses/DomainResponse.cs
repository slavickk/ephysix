/******************************************************************
 * File: DomainResponse.cs
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

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Domain response.
    /// </summary>
    public class DomainResponse : AbstractResponse
    {
        /// <summary>
        /// Construct a DomainResponse model object.
        /// </summary>
        public DomainResponse() : this(null, null)
        {
        }

        /// <summary>
        /// Construct a DomainResponse model object.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="ipAddress"></param>
        /// <param name="network"></param>
        [Constructor]
        public DomainResponse(
            string? domain,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null
        )
        {
            Domain = domain;
            IPAddress = ipAddress;
            Network = network;
        }

        /// <summary>
        ///     The second level domain associated with the IP address. This will
        ///     be something like "example.com" or "example.co.uk", not
        ///     "foo.example.com".
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("domain")]
        public string? Domain { get; internal set; }

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
