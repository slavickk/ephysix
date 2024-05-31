/******************************************************************
 * File: RegisterLayoutPluginCommand.cs
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
// <copyright file="RegisterLayoutPluginCommand.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the RegisterLayoutPluginCommand type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper.Commands
{
    using GraphVizWrapper.Queries;
    
    public class RegisterLayoutPluginCommand : IRegisterLayoutPluginCommand
    {
        private readonly IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private readonly IGetStartProcessQuery _getStartProcessQuery;

        public RegisterLayoutPluginCommand(IGetProcessStartInfoQuery getProcessStartInfoQuery, IGetStartProcessQuery getStartProcessQuery)
        {
            _getStartProcessQuery = getStartProcessQuery;
            _getProcessStartInfoQuery = getProcessStartInfoQuery;
        }

        public void Invoke(string configFilePath, Enums.RenderingEngine renderingEngine)
        {
            var processStartInfo = _getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
                                                 {
                                                     FileName = configFilePath,
                                                     UseShellExecute = false,
                                                     Arguments = "-c",
                                                     CreateNoWindow = false
                                                 });

            using (_getStartProcessQuery.Invoke(processStartInfo)) { }
        }

        public void Invoke()
        {
            throw new System.NotImplementedException();
        }
    }
}
