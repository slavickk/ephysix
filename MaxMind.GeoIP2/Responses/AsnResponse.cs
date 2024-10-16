﻿/******************************************************************
 * File: AsnResponse.cs
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

using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoLite2 ASN response.
    /// </summary>
    public class AsnResponse : AbstractResponse
    {
        /// <summary>
        ///     Construct an IspResponse model.
        /// </summary>
        public AsnResponse() : this(null, null, null)
        {
        }

        /// <summary>
        ///     Construct an IspResponse model.
        /// </summary>
        [Constructor]
        public AsnResponse(
            [Parameter("autonomous_system_number")] long? autonomousSystemNumber,
            [Parameter("autonomous_system_organization")] string? autonomousSystemOrganization,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null
        )
        {
            AutonomousSystemNumber = autonomousSystemNumber;
            AutonomousSystemOrganization = autonomousSystemOrganization;
            IPAddress = ipAddress;
            Network = network;
        }

        /// <summary>
        ///     The
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("autonomous_system_number")]
        public long? AutonomousSystemNumber { get; internal set; }

        /// <summary>
        ///     The organization associated with the registered
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     for the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("autonomous_system_organization")]
        public string? AutonomousSystemOrganization { get; internal set; }

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