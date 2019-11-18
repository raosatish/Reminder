using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReminderAPIClient;
using ReminderApp.Data.Model;
using ReminderManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace ReminderManager.Services
{
    public class MockReminderService : IReminderService
    {
        ILogger<MockReminderService> _logger;
        INotificationService _notificationService;
        ReminderClient _client;
        IOptions<ReminderConfiguration> _options;

        List<Reminder> _activeReminders;

        public MockReminderService(IOptions<ReminderConfiguration> options, ILogger<MockReminderService> logger, INotificationService notificationService, ReminderClient client)
        {
            _logger = logger;
            _notificationService = notificationService;
            _client = client;
            _options = options;
        }

        private void GetAllActiveReminders(bool force = false)
        {
            if(_activeReminders == null || force)
            {
                _activeReminders = _client.GetActiveReminders();
            }
        }

        public int GetReminderCount()
        {
            GetAllActiveReminders();
            return _activeReminders?.Count ?? 0;
        }

        public void RunScheduledCheck(CancellationToken stoppingToken)
        {
            _logger.LogError("Checking Reminder API Service for reminders");
        }

        public void Shutdown()
        {
            _notificationService.Shutdown("Shutting down Reminder Service");
        }

        public void Startup()
        {
            _client.SetURL(_options.Value.API);
            _notificationService.PrepareNotificationArea(new Icon(@"Icons\Google-Noto-Emoji-Travel-Places-42476-stadium.ico"), new List<string> { "Test Option 1" }, new List<Action<string>> { DelegateMethod });
            _notificationService.ShowNotificationMessage($"You have {GetReminderCount()} reminder(s).");
        }

        private void DelegateMethod(string obj)
        {

        }
    }
}
