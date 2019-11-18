using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReminderManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReminderManager.Services
{
    public class ReminderBackgroundService : BackgroundService
    {
        ILogger<ReminderBackgroundService> _logger;
        IReminderService _reminderService;
        public ReminderBackgroundService(ILogger<ReminderBackgroundService> logger, IReminderService reminderService)
        {
            _logger = logger;
            _reminderService = reminderService;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            base.StartAsync(cancellationToken);
            _reminderService.Startup();
            return Task.CompletedTask;
        }


        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _reminderService.Shutdown();
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _reminderService.RunScheduledCheck(stoppingToken);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
