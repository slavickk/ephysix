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
            _logger.LogInformation("Hosted service OnStopping");
            Thread.Sleep(20000);
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
