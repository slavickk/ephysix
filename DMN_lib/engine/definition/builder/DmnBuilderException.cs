/******************************************************************
 * File: DmnBuilderException.cs
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

using System;

namespace net.adamec.lib.common.dmn.engine.engine.definition.builder
{
    /// <inheritdoc />
    /// <summary>
    /// Exception thrown while building the DMN Definition
    /// </summary>
    public class DmnBuilderException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates <see cref="T:net.adamec.lib.common.dmn.engine.definition.builder.DmnBuilderException" /> with given <paramref name="message" /> and optional <paramref name="innerException" />
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Optional inner exception</param>
        public DmnBuilderException(string message, Exception innerException=null) : base(message, innerException)
        {
        }
    }
}
