using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ReminderManager.Interfaces
{
    public interface IReminderService
    {
        int GetReminderCount();
        void Startup();
        void Shutdown();
        void RunScheduledCheck(CancellationToken stoppingToken);
    }
}
