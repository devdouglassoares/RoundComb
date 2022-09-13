using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Core.Database;
using Core.Database.Conventions;
using Core.Templating.Data.EntityConfigurations;

namespace Core.Templating.Data.Contexts
{
    public class TemplatingContextDbMigrationConfig : MigrationsConfigBase<TemplatingContext>
    {
    }

    public class TemplatingContext : DbContext, ISelfRegisterDependency
    {
        public const string TemplateServiceDbTablePrefix = "TEMPLATE";

        public TemplatingContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null &&
                ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                System.Data.Entity.Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<TemplatingContext, TemplatingContextDbMigrationConfig>());
            else
                System.Data.Entity.Database.SetInitializer<TemplatingContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            modelBuilder.Conventions.Add(new CustomTablePrefixConvention(TemplateServiceDbTablePrefix));
            modelBuilder.Conventions.Add(new ManyToManyCustomTablePrefixConvention(TemplateServiceDbTablePrefix));

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            // register all entities in context
            modelBuilder.Configurations.Add(new TemplateModelEntityConfig());
        }
    }
}