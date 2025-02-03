namespace loot_master.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public GuildViewModel GuildViewModel => App.Services.GetRequiredService<GuildViewModel>();
        public RaidViewModel RaidViewModel => App.Services.GetRequiredService<RaidViewModel>();
    }
    internal static class ViewModelsRegistrar
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<GuildViewModel>()
            .AddSingleton<RaidViewModel>()
           .AddSingleton<MainWindowViewModel>()
        ;
    }
}
