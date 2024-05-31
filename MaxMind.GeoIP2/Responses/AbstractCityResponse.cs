﻿/******************************************************************
 * File: AbstractCityResponse.cs
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

using MaxMind.GeoIP2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract class that city-level response.
    /// </summary>
    public abstract class AbstractCityResponse : AbstractCountryResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractCityResponse" /> class.
        /// </summary>
        protected AbstractCityResponse()
        {
            City = new City();
            Location = new Location();
            Postal = new Postal();
            Subdivisions = new List<Subdivision>().AsReadOnly();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractCityResponse" /> class.
        /// </summary>
        protected AbstractCityResponse(
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
            : base(continent, country, maxMind, registeredCountry, representedCountry, traits)
        {
            City = city ?? new City();
            Location = location ?? new Location();
            Postal = postal ?? new Postal();
            Subdivisions = subdivisions ?? new List<Subdivision>().AsReadOnly();
        }

        /// <summary>
        ///     Gets the city for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("city")]
        public City City { get; internal set; }

        /// <summary>
        ///     Gets the location for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("location")]
        public Location Location { get; internal set; }

        /// <summary>
        ///     Gets the postal object for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("postal")]
        public Postal Postal { get; internal set; }

        /// <summary>
        ///     An <see cref="System.Collections.Generic.List{T}" /> of <see cref="Subdivision" /> objects representing
        ///     the country subdivisions for the requested IP address. The number
        ///     and type of subdivisions varies by country, but a subdivision is
        ///     typically a state, province, county, etc. Subdivisions are
        ///     ordered from most general (largest) to most specific (smallest).
        ///     If the response did not contain any subdivisions, this method
        ///     returns an empty array.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("subdivisions")]
        public IReadOnlyList<Subdivision> Subdivisions { get; internal set; }

        /// <summary>
        ///     An object representing the most specific subdivision returned. If
        ///     the response did not contain any subdivisions, this method
        ///     returns an empty <see cref="Subdivision" /> object.
        /// </summary>
        [JsonIgnore]
        public Subdivision MostSpecificSubdivision
        {
            get
            {
                if (Subdivisions == null || Subdivisions.Count == 0)
                    return new Subdivision();

                return Subdivisions[Subdivisions.Count - 1];
            }
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetType().Name + " ["
                   + (City != null ? "City=" + City + ", " : "")
                   + (Location != null ? "Location=" + Location + ", " : "")
                   + (Postal != null ? "Postal=" + Postal + ", " : "")
                   +
                   (Subdivisions != null
                       ? "Subdivisions={" + string.Join(",", Subdivisions.Select(s => s.ToString()).ToArray()) + "}, "
                       : "")
                   + (Continent != null ? "Continent=" + Continent + ", " : "")
                   + (Country != null ? "Country=" + Country + ", " : "")
                   + (RegisteredCountry != null ? "RegisteredCountry=" + RegisteredCountry + ", " : "")
                   + (RepresentedCountry != null ? "RepresentedCountry=" + RepresentedCountry + ", " : "")
                   + (Traits != null ? "Traits=" + Traits : "")
                   + "]";
        }

        /// <summary>
        ///     Sets the locales on all the NamedEntity properties.
        /// </summary>
        /// <param name="locales">The locales specified by the user.</param>
        protected internal override void SetLocales(IReadOnlyList<string> locales)
        {
            locales = locales.ToList();
            base.SetLocales(locales);

            if (City != null)
                City.Locales = locales;

            if (Subdivisions == null || Subdivisions.Count <= 0) return;
            foreach (var subdivision in Subdivisions)
                subdivision.Locales = locales;
        }
    }
}
