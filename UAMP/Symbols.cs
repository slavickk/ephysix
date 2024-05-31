/******************************************************************
 * File: Symbols.cs
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

public enum Symbols : byte
{
    /// <summary>
    ///     Message Seporator. Use between "key"="val"
    /// </summary>
    MS = 0x0a,

    /// <summary>
    ///     Parameter separator
    /// </summary>
    PS = 0x10,

    /// <summary>
    ///     Item Separator. Use in array
    /// </summary>
    IS = 0x1d,

    /// <summary>
    ///     Field Separator. Use in structs
    /// </summary>
    FS = 0x1c,

    /// <summary>
    ///     Null indicator
    /// </summary>
    NI = 0x07,

    /// <summary>
    ///     Escaping char.
    /// </summary>
    SP = 0x13,

    /// <summary>
    ///     "=" in ASCII encoding.
    /// </summary>
    Eq = 0x3d
}