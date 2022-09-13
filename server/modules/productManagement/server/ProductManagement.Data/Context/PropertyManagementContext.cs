using Core;
using Core.Database.Conventions;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Configuration = ProductManagement.Data.Migrations.Configuration;

namespace ProductManagement.Data.Context
{
    public class PropertyManagementContext : DbContext, ISelfRegisterDependency
    {
        public const string PropertyManagementTablePrefix = "PROD";

        public PropertyManagementContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null && ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<PropertyManagementContext, Configuration>());
            else
                Database.SetInitializer<PropertyManagementContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            modelBuilder.Conventions.Add(new CustomTablePrefixConvention(PropertyManagementTablePrefix));
            modelBuilder.Conventions.Add(new ManyToManyCustomTablePrefixConvention(PropertyManagementTablePrefix));

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            // register all entities in context

            modelBuilder.Configurations.AddFromAssembly(typeof(PropertyManagementContext).Assembly);
        }
    }
}
