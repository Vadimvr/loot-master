using loot_master.Models;
using loot_master.Service.Data;
using Microsoft.EntityFrameworkCore;

namespace loot_master.Service.Db
{
    internal class ApplicationDb : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerInRaid> InRaids { get; set; }
        public DbSet<Winner> Winners { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        
        {
            string name = "appSqlite.db";
            string cacheDir = FileSystem.Current.CacheDirectory;

            string path = Path.Combine(cacheDir, name);
            //     optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite($"Data Source={path}");

        }
    }
}
