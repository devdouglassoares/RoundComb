using Core;
using Core.Database;
using Core.Database.Conventions;
using ProductManagement.Core.Entities;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Roundcomb.Data.DbContexts
{
	public class RoundcombDbContextMigrationConfig : MigrationsConfigBase<RoundcombContext>
	{

	}

	public class RoundcombContext : DbContext, ISelfRegisterDependency
	{
		public const string PropertyManagementTablePrefix = "PROD";

		public RoundcombContext()
			: base(ConfigurationManager.AppSettings["DefaultConnectionString"] ?? "portalConnectionString")
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<RoundcombContext, RoundcombDbContextMigrationConfig>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// setup conventions
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Conventions.Add<ForeignKeyConvention>();

			modelBuilder.Conventions.Add(new CustomTablePrefixConvention(PropertyManagementTablePrefix));
			modelBuilder.Conventions.Add(new ManyToManyCustomTablePrefixConvention(PropertyManagementTablePrefix));


			modelBuilder.Configurations.AddFromAssembly(typeof(RoundcombContext).Assembly);

			modelBuilder.Entity<Property>().ToTable("PROD_Property");

			modelBuilder.Entity<PropertyCategory>().ToTable("PROD_PropertyCategory");

			modelBuilder.Entity<PropertyDynamicPropertyCategory>().ToTable("PROD_PropertyDynamicPropertyCategory")
						.HasKey(x => new
						{
							x.CategoryId,
							x.DynamicPropertyId
						})
						.HasRequired(x => x.Category)
						.WithMany(y => y.DynamicProperties)
						.HasForeignKey(x => x.CategoryId);
		}
	}
}