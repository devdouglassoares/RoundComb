using Core.Database;
using Core.Database.Conventions;
using Core.DynamicProperties.EntityConfigMapping;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using static System.Data.Entity.Database;

namespace Core.DynamicProperties.Db
{
    public class DynamicPropertyMigrationConfig : MigrationsConfigBase<DynamicPropertyContext> { }

    public class DynamicPropertyContext : DbContext, ISelfRegisterDependency
    {
        public const string DynamicPropertyTablePrefix = "DYNAMIC_PROP";


        public DynamicPropertyContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null && ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                SetInitializer(new MigrateDatabaseToLatestVersion<DynamicPropertyContext, DynamicPropertyMigrationConfig>());
            else
                SetInitializer<DynamicPropertyContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            modelBuilder.Conventions.Add(new CustomTablePrefixConvention(DynamicPropertyTablePrefix));
            modelBuilder.Conventions.Add(new ManyToManyCustomTablePrefixConvention(DynamicPropertyTablePrefix));

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Configurations.Add(new DynamicPropertyEntityTypeConfiguration());
            modelBuilder.Configurations.Add(new DynamicPropertySupportedEntityTypesEntityTypeConfig());
            modelBuilder.Configurations.Add(new DynamicPropertyEntityTypeMappingEntityTypeConfig());
            modelBuilder.Configurations.Add(new DynamicPropertyValueEntityTypeConfiguration());
        }
    }
}