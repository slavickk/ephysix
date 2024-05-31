﻿/******************************************************************
 * File: AnonymousIPResponse.cs
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
    ///     This class represents the GeoIP2 Anonymous IP response.
    /// </summary>
    public class AnonymousIPResponse : AbstractResponse
    {
        /// <summary>
        /// Construct AnonymousIPResponse model
        /// </summary>
        public AnonymousIPResponse()
        {
        }

        /// <summary>
        /// Construct AnonymousIPResponse model
        /// </summary>
        /// <param name="isAnonymous"></param>
        /// <param name="isAnonymousVpn"></param>
        /// <param name="isHostingProvider"></param>
        /// <param name="isPublicProxy"></param>
        /// <param name="isResidentialProxy"></param>
        /// <param name="isTorExitNode"></param>
        /// <param name="ipAddress"></param>
        /// <param name="network"></param>
        [Constructor]
        public AnonymousIPResponse(
            [Parameter("is_anonymous")] bool isAnonymous,
            [Parameter("is_anonymous_vpn")] bool isAnonymousVpn,
            [Parameter("is_hosting_provider")] bool isHostingProvider,
            [Parameter("is_public_proxy")] bool isPublicProxy,
            [Parameter("is_residential_proxy")] bool isResidentialProxy,
            [Parameter("is_tor_exit_node")] bool isTorExitNode,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null
        )
        {
            IsAnonymous = isAnonymous;
            IsAnonymousVpn = isAnonymousVpn;
            IsHostingProvider = isHostingProvider;
            IsPublicProxy = isPublicProxy;
            IsResidentialProxy = isResidentialProxy;
            IsTorExitNode = isTorExitNode;
            IPAddress = ipAddress;
            Network = network;
        }

        /// <summary>
        ///     Returns true if the IP address belongs to any sort of anonymous network.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_anonymous")]
        public bool IsAnonymous { get; internal set; }

        /// <summary>
        ///     Returns true if the IP address is registered to an anonymous
        ///     VPN provider.
        /// </summary>
        /// <remarks>
        ///     If a VPN provider does not register subnets under names
        ///     associated with them, we will likely only flag their IP ranges
        ///     using the IsHostingProvider property.
        /// </remarks>
        [JsonInclude]
        [JsonPropertyName("is_anonymous_vpn")]
        public bool IsAnonymousVpn { get; internal set; }

        /// <summary>
        ///     Returns true if the IP address belongs to a hosting or
        ///     VPN provider (see description of IsAnonymousVpn property).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_hosting_provider")]
        public bool IsHostingProvider { get; internal set; }

        /// <summary>
        ///     Returns true if the IP address belongs to a public proxy.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_public_proxy")]
        public bool IsPublicProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP address is on a suspected anonymizing
        ///     network and belongs to a residential ISP.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_residential_proxy")]
        public bool IsResidentialProxy { get; internal set; }

        /// <summary>
        ///     Returns true if IP is a Tor exit node.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_tor_exit_node")]
        public bool IsTorExitNode { get; internal set; }

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
