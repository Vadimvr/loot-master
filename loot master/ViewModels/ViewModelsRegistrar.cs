namespace loot_master.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public GuildViewModel GuildViewModel => App.Services.GetRequiredService<GuildViewModel>();
        public RaidViewModel RaidViewModel => App.Services.GetRequiredService<RaidViewModel>();
        public LogWinnerViewModel LogWinnerViewModel => App.Services.GetRequiredService<LogWinnerViewModel>();
    }
    internal static class ViewModelsRegistrar
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<GuildViewModel>()
            .AddSingleton<RaidViewModel>()
            .AddSingleton<LogWinnerViewModel>()
            .AddSingleton<MainWindowViewModel>()
        ;
    }
}
