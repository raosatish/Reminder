using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ReminderManager
{
    internal class Worker : IHostedService
    {
        ILogger<Worker> _logger;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogError("Started Worker");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogError("Clossed Worker");
            return Task.CompletedTask;
        }
    }

    internal class Worker1 : IHostedService
    {
        ILogger<Worker1> _logger;
        public Worker1(ILogger<Worker1> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogError("Started Worker1");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogError("Closed Worker1");
            return Task.CompletedTask;
        }
    }
}