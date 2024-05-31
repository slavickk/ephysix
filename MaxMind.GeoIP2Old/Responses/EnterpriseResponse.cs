/******************************************************************
 * File: EnterpriseResponse.cs
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
using MaxMind.GeoIP2.Model;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by the GeoIP2 Precision:
    ///     Insights end point.
    ///     The only difference between the City and Insights response classes is
    ///     which fields in each record may be populated.
    ///     <a href="https://dev.maxmind.com/geoip/geoip2/web-services">
    ///         GeoIP2 Web
    ///         Services
    ///     </a>
    /// </summary>
    public class EnterpriseResponse : AbstractCityResponse
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public EnterpriseResponse(
            City? city = null,
            Continent? continent = null,
            Country? country = null,
            Location? location = null,
            Model.MaxMind? maxMind = null,
            Postal? postal = null,
            Country? registeredCountry = null,
            RepresentedCountry? representedCountry = null,
            IReadOnlyList<Subdivision>? subdivisions = null,
            Traits? traits = null)
            : base(
                city, continent, country, location, maxMind, postal, registeredCountry, representedCountry, subdivisions,
                traits)
        {
        }
    }
}