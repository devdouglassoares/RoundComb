using Core;
using Core.Database;
using Core.Database.Conventions;
using LocationService.Library.Entities;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LocationService.Library.Data
{
    public class LocationServiceDbMigrationConfig : MigrationsConfigBase<LocationServiceDbContext> { }

    public class LocationServiceDbContext : DbContext, ISelfRegisterDependency
    {
        public LocationServiceDbContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "locationServiceConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null &&
                ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                Database.SetInitializer(
                                        new MigrateDatabaseToLatestVersion
                                            <LocationServiceDbContext, LocationServiceDbMigrationConfig>());
            else
                Database.SetInitializer<LocationServiceDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<Location>().HasKey(x => x.Id);
        }
    }
}