/******************************************************************
 * File: IGeoIP2Provider.cs
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

using MaxMind.GeoIP2.Responses;
using System.Net;

#endregion

namespace MaxMind.GeoIP2
{
    /// <summary>
    ///     This class provides the interface implemented by both <see cref="DatabaseReader" />
    ///     and <see cref="WebServiceClient" />.
    /// </summary>
    public interface IGeoIP2Provider
    {
        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        CountryResponse Country(string ipAddress);

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        CountryResponse Country(IPAddress ipAddress);

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        CityResponse City(string ipAddress);

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        CityResponse City(IPAddress ipAddress);
    }
}
