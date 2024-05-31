/******************************************************************
 * File: DmnParserException.cs
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

using System;

namespace net.adamec.lib.common.dmn.engine.parser
{
    /// <inheritdoc />
    /// <summary>
    /// Exception thrown while parsing the DMN Model
    /// </summary>
    public class DmnParserException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates <see cref="T:net.adamec.lib.common.dmn.engine.parser.DmnParserException" /> with given <paramref name="message" /> and optional <paramref name="innerException" />
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Optional inner exception</param>
        public DmnParserException(string message, Exception innerException=null) : base(message, innerException)
        {
        }
    }
}
