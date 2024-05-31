/******************************************************************
 * File: PermissionRequiredException.cs
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

using System;
using System.Runtime.Serialization;

#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    /// This class represents an authentication error.
    /// </summary>
    [Serializable]
    public class PermissionRequiredException : GeoIP2Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PermissionRequiredException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">The underlying exception that caused this one.</param>
        public PermissionRequiredException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PermissionRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}