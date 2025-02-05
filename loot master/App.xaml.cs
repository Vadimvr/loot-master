// Ignore Spelling: App

using loot_master.Service;
using loot_master.Service.Alert;
using loot_master.ViewModels;
using loot_master.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace loot_master
{
    public partial class App : Application
    {
        public App() => InitializeComponent();
        private static IHost? __Host;
        public static IHost Host => __Host ??= Microsoft.Extensions.Hosting.Host
          .CreateDefaultBuilder(Environment.GetCommandLineArgs())
          .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json", true, true))
          .ConfigureServices((host, services) => services
                .AddServices()
              .AddViewModels())
          .Build();

        public static IServiceProvider Services => Host.Services;
        public static IAlertService Alert = default!;
        private async void OnStartup(object? sender, EventArgs e)
        {
            var host = Host;
            var alert = Services.GetService<IAlertService>();
            if (alert != null)
            {
                Alert = alert;
            }
            await host.StartAsync();
        }

        private async void Destroying(object? sender, EventArgs e)
        {
            if (Host != null)
            {
                using (Host) { await Host.StopAsync(); };
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new MainWindow();
            window.Created += OnStartup;
            window.Destroying += Destroying;
            return window;
        }
    }
}