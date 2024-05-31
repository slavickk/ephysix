/******************************************************************
 * File: AbstractResponse.cs
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

using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract class that represents a generic response.
    /// </summary>
    public abstract class AbstractResponse
    {
        /// <summary>
        ///     This is simplify the database API. Also, we may need to use the locales in the future.
        /// </summary>
        /// <param name="locales"></param>
        protected internal virtual void SetLocales(IReadOnlyList<string> locales)
        {
        }
    }
}