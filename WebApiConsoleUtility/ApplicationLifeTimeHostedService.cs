/******************************************************************
 * File: ApplicationLifeTimeHostedService.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
//using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace WebApiConsoleUtility
{
    public class ApplicationLifetimeHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<ApplicationLifetimeHostedService> _logger;

        public ApplicationLifetimeHostedService(
            IHostApplicationLifetime appLifetime,
            ILogger<ApplicationLifetimeHostedService> logger)
        {
            _appLifetime = appLifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
//            _appLifetime.
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("Hosted service OnStarted");
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping method called. Sleeping 20 seconds.");
            Thread.Sleep(20000);
            _logger.LogInformation("OnStopping method called end. Shutdown...");

        }

        private void OnStopped()
        {
            _logger.LogInformation("Hosted service OnStopped");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted service StopAsync");
            return Task.CompletedTask;
        }
    }
}
