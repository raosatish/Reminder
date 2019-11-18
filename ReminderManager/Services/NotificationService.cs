using Microsoft.Extensions.Logging;
using ReminderManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using NotificationAreaService;
using System.Windows.Interop;

namespace ReminderManager.Services
{
    public class NotificationService : INotificationService
    {
        ILogger<NotificationService> _logger;
        NotificationAreaManager _manager;
        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }
        public void PrepareNotificationArea(string notificationIconPath, List<string> contextMenu, List<Action<string>> contextMenuHandlers)
        {
            PrepareNotificationArea(new Icon(notificationIconPath), contextMenu, contextMenuHandlers);
        }

        public void PrepareNotificationArea(Icon notificationIcon, List<string> contextMenu, List<Action<string>> contextMenuHandlers)
        {
            _manager = new NotificationAreaManager();
            _manager.Start(notificationIcon.Handle);
            _manager.ShowBalloon("Reminder App", "Welcome to Reminder App", 1000);
            _logger.LogError("Notification Shown");
        }

        public void ShowNotificationMessage(string message)
        {
            _manager?.ShowBalloon("Reminder", message, 10000);
            _logger.LogError($"Notification: {message}");
        }

        public void Shutdown(string message)
        {
            _logger.LogError(message);
        }

        public void DelegateMethod(string context)
        {

        }
    }
}
