using System.Data.Entity.ModelConfiguration.Conventions;

namespace Core.Database.Conventions
{
    public class CustomTablePrefixConvention : Convention
    {
        public CustomTablePrefixConvention(string tablePrefix)
        {
            Types().Configure(configuration => configuration.ToTable(tablePrefix + "_" + configuration.ClrType.Name));
        }
    }
}