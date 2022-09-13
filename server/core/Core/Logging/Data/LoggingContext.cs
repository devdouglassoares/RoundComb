using Core.Database;
using Core.Database.Conventions;
using Core.Logging.EntityConfigurations;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Core.Logging.Data
{
    public class LoggingContextDbMigrationConfig : MigrationsConfigBase<LoggingContext>
    {
        public LoggingContextDbMigrationConfig()
        {
            MigrationsNamespace = typeof(LoggingContext).Namespace + ".Migrations";

            var migrationDirectory =
                MigrationsNamespace.Replace(typeof(LoggingContext).Assembly.GetName().Name, "")
                                   .Trim('.')
                                   .Replace('.', '\\');
            MigrationsDirectory = migrationDirectory;
        }
    }

    public class LoggingContext : DbContext, ISelfRegisterDependency
    {
        public LoggingContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null &&
                ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                System.Data.Entity.Database.SetInitializer(
                    new MigrateDatabaseToLatestVersion<LoggingContext, LoggingContextDbMigrationConfig>());
            else
                System.Data.Entity.Database.SetInitializer<LoggingContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            // register all entities in context
            modelBuilder.Configurations.Add(new LogEntryEntityTypeConfig());
        }
    }
}