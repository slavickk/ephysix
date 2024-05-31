/******************************************************************
 * File: GeoIP2Exception.cs
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

using System;
using System.Runtime.Serialization;
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This class represents a generic GeoIP2 error. All other exceptions thrown by
    ///     the GeoIP2 API subclass this exception
    /// </summary>
    [Serializable]
    public class GeoIP2Exception : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GeoIP2Exception" /> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public GeoIP2Exception(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GeoIP2Exception" /> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public GeoIP2Exception(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected GeoIP2Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
