using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ReminderManager.Interfaces
{
    public interface INotificationService
    {
        void PrepareNotificationArea(Icon notificationIcon, List<string> contextMenu, List<Action<string>> contextMenuHandlers);
        void ShowNotificationMessage(string message);
        void Shutdown(string shutdownMessage);
    }
}
