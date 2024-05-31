/******************************************************************
 * File: ProcessStartInfoWrapper.cs
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

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessStartInfoWrapper.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the ProcessStartInfoWrapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper
{
    public class ProcessStartInfoWrapper : IProcessStartInfoWrapper
    {
        public string FileName { get; set; }

        public string Arguments { get; set; }

        public bool RedirectStandardInput { get; set; }

        public bool RedirectStandardOutput { get; set; }

        public bool RedirectStandardError { get; set; }

        public bool UseShellExecute { get; set; }

        public bool CreateNoWindow { get; set; }
    }
}
