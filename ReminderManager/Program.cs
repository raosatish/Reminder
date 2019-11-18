using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReminderAPIClient;
using ReminderManager.Interfaces;
using ReminderManager.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReminderManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Thread t = new Thread(() =>
            {
                ExecuteSTA(args);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private static void ExecuteSTA(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.AddJsonFile("appsettings.json", optional: false);
                   config.AddEnvironmentVariables();
                   if(args != null)
                   {
                       config.AddCommandLine(args);
                   }
                })
            .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<INotificationService, NotificationService>();
                    services.AddSingleton<IReminderService, MockReminderService>();
                    services.AddTransient<ReminderClient>();
                    services.Configure<ReminderConfiguration>(hostContext.Configuration.GetSection("ReminderConfiguration"));
                    services.AddHostedService<ReminderBackgroundService>();
                });
    }
}
