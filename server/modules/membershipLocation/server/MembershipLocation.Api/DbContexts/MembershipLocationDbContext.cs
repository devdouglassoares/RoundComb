using Core;
using Core.Database;
using Core.Database.Conventions;
using MembershipLocation.Api.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MembershipLocation.Api.DbContexts
{
    public class MembershipLocationDbContextMigrationConfig: MigrationsConfigBase<MembershipLocationDbContext> { }

    public class MembershipLocationDbContext: DbContext, ISelfRegisterDependency
    {
        public MembershipLocationDbContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null && ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<MembershipLocationDbContext, MembershipLocationDbContextMigrationConfig>());
            else
                Database.SetInitializer<MembershipLocationDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<UserLocation>()
                        .HasKey(x => new
                                     {
                                         x.UserId,
                                         x.LocationId
                                     })
                        .HasOptional(x => x.LocationType);

            modelBuilder.Entity<LocationType>()
                        .HasKey(x => x.Id);
        }
    }
}