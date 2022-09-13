using Common.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Common.Data.EntityTypeConfigs
{
    public class ExceptionLoggerEntityTypeConfig : EntityTypeConfiguration<ExceptionLogger>
    {
        public ExceptionLoggerEntityTypeConfig()
        {
            HasKey(x => x.Id);
        }
    }
}