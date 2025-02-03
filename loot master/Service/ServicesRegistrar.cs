using loot_master.Service.Data;

namespace loot_master.Service
{
    internal static class ServicesRegistrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
          .AddSingleton<IDataService, DataService>()
        ;
    }
}
