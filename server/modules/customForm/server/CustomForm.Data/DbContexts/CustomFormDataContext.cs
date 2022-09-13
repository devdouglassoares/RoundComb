using Core;
using Core.Database;
using Core.Database.Conventions;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CustomForm.Data.DbContexts
{

    public class CustomFormDataContext : DbContext, ISelfRegisterDependency
    {
        public CustomFormDataContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null && ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<CustomFormDataContext, CustomFormDataContextMigrationConfig>());
            else
                Database.SetInitializer<CustomFormDataContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();
            
            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Configurations.AddFromAssembly(typeof (CustomFormDataContext).Assembly);
        }
    }

    public class CustomFormDataContextMigrationConfig: MigrationsConfigBase<CustomFormDataContext>
    {
    }
}
