using Core;
using Core.Database.Conventions;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Configuration = Common.Data.Migrations.Configuration;

namespace Common.Data.Context
{
    public class CommonContext : DbContext, ISelfRegisterDependency
    {
        public CommonContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CommonContext, Configuration>());
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null && ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<CommonContext, Configuration>());
            else
                Database.SetInitializer<CommonContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            // register all entities in context
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }
}
