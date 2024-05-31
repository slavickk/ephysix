/******************************************************************
 * File: UAMPType.cs
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

namespace UAMP
{
    /// <summary>
    ///     Types of UAMP values
    /// </summary>
    public enum UAMPType
    {
        /// <summary>
        ///     <seealso cref="UAMPScalar" />
        /// </summary>
        Scalar,

        /// <summary>
        ///     <seealso cref="UAMPStruct" />
        /// </summary>
        Struct,


        /// <summary>
        ///     <seealso cref="UAMPArray" />
        /// </summary>
        Array,

        /// <summary>
        ///     <seealso cref="UAMPMessage" />
        /// </summary>
        UAMPMessage,

        /// <summary>
        ///     <seealso cref="UAMP" />
        /// </summary>
        UAMPPackage
    }
}