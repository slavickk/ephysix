/******************************************************************
 * File: IProcessStartInfoWrapper.cs
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

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessStartInfoWrapper.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the IProcessStartInfoWrapper interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper
{
    public interface IProcessStartInfoWrapper
    {
        string FileName { get; set; }
        string Arguments { get; set; }

        bool RedirectStandardInput { get; set; }
        bool RedirectStandardOutput { get; set; }
        bool RedirectStandardError { get; set; }
        bool UseShellExecute { get; set; }
        bool CreateNoWindow { get; set; }
    }
}
