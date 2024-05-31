/******************************************************************
 * File: GetProcessStartInfoQuery.cs
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
// <copyright file="GetProcessStartInfoQuery.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the GetProcessStartInfoQuery type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;

namespace GraphVizWrapper.Queries
{
    public class GetProcessStartInfoQuery : IGetProcessStartInfoQuery
    {
        public System.Diagnostics.ProcessStartInfo Invoke(IProcessStartInfoWrapper startInfoWrapper)
        {
            return new System.Diagnostics.ProcessStartInfo
                       {
                           WorkingDirectory = Path.GetDirectoryName(startInfoWrapper.FileName) ?? "",
                           FileName = '"' + startInfoWrapper.FileName + '"',
                           Arguments = startInfoWrapper.Arguments,
                           RedirectStandardInput = startInfoWrapper.RedirectStandardInput,
                           RedirectStandardOutput = startInfoWrapper.RedirectStandardOutput,
                           RedirectStandardError = startInfoWrapper.RedirectStandardError,
                           UseShellExecute = startInfoWrapper.UseShellExecute,
                           CreateNoWindow = startInfoWrapper.CreateNoWindow
                       };
        }
    }
}
