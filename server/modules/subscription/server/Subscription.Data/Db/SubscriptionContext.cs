using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Core;
using Core.Database;
using Core.Database.Conventions;

namespace Subscription.Data.Db
{
    public class SubscriptionContextMigrationConfig: MigrationsConfigBase<SubscriptionContext> { }

    public class SubscriptionContext: DbContext, ISelfRegisterDependency
    {
        //public const string SubscriptionTablePrefix = "EXPENSE";

        public SubscriptionContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null && ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<SubscriptionContext, SubscriptionContextMigrationConfig>());
            else
                Database.SetInitializer<SubscriptionContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            //modelBuilder.Conventions.Add(new CustomTablePrefixConvention(SubscriptionTablePrefix));
            //modelBuilder.Conventions.Add(new ManyToManyCustomTablePrefixConvention(SubscriptionTablePrefix));

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Configurations.AddFromAssembly(typeof (SubscriptionContext).Assembly);
        }
    }
}