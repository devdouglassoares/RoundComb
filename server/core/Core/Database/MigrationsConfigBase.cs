using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Core.Database
{
    public class MigrationsConfigBase<TDbContext> : DbMigrationsConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        public MigrationsConfigBase()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = typeof(TDbContext).FullName;
            MigrationsNamespace = typeof(TDbContext).Assembly.GetName().Name + ".Migrations";

            var migrationDirectory =
                MigrationsNamespace.Replace(typeof(TDbContext).Assembly.GetName().Name, "")
                                   .Trim('.')
                                   .Replace('.', '\\');
            MigrationsDirectory = migrationDirectory;
            MigrationsAssembly = typeof(TDbContext).Assembly;
        }
    }
}