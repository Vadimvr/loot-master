using loot_master.Service.Alert;
using loot_master.Service.Data;
using loot_master.Service.Db;

namespace loot_master.Service
{
    internal static class ServicesRegistrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddSingleton<IDataService, DataService>()
            .AddSingleton<IAlertService, AlertService>()
            //.AddSingleton<ApplicationDb, DbSqlite>()
            .AddSingleton<ApplicationDb, PostgreSQL>()

            ;
    }
}
