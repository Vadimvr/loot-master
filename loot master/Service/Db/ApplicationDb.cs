// Ignore Spelling: Postgre

using loot_master.Models;
using loot_master.Service.Data;
using Microsoft.EntityFrameworkCore;

namespace loot_master.Service.Db
{
    internal abstract class ApplicationDb : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerInRaid> InRaids { get; set; }
        public DbSet<Winner> Winners { get; set; }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().Ignore(c => c.Pr);
            modelBuilder.Entity<PlayerInRaid>().Ignore(c => c.Name);
        }

    }
    internal class DbSqlite : ApplicationDb
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string name = "appSqlite.db";
            string cacheDir = FileSystem.Current.CacheDirectory;

            string path = Path.Combine(cacheDir, name);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }

    internal class PostgreSQL : ApplicationDb
    {
        public PostgreSQL()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO 
            optionsBuilder.UseNpgsql("" +
                "Host=beirn6kh5qmhxnx7jjcl-postgresql.services.clever-cloud.com;" +
                "Port=50013;" +
                "Database=beirn6kh5qmhxnx7jjcl;" +
                "Username=uyev1tdp7hbqswqfqac9;" +
                "Password=5FCD5Xsb8rpHCtxjDKYPWPozpVqiiX");

        }
    }
}
