using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Core;
using Core.Database;
using Core.Database.Conventions;

namespace EntityReviews.DbContexts
{
    public class EntityReviewMigrationConfig : MigrationsConfigBase<EntityReviewContext> { }

    public class EntityReviewContext: DbContext, ISelfRegisterDependency
    {
        public const string EntityRatingTablePrefix = "EntityReview";
        public EntityReviewContext()
            : base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
        {
            if (ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] != null && ConfigurationManager.AppSettings["__AutoMigrationEnabled__"] == "true")
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<EntityReviewContext, EntityReviewMigrationConfig>());
            else
                Database.SetInitializer<EntityReviewContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // setup conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Add<ForeignKeyConvention>();

            modelBuilder.Conventions.Add(new CustomTablePrefixConvention(EntityRatingTablePrefix));
            modelBuilder.Conventions.Add(new ManyToManyCustomTablePrefixConvention(EntityRatingTablePrefix));

            // manual behavior overrides
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Configurations.AddFromAssembly(typeof (EntityReviewContext).Assembly);
        }
    }
}