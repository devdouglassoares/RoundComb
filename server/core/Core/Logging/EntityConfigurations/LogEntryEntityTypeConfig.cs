using Core.Logging.Models;
using System.Data.Entity.ModelConfiguration;

namespace Core.Logging.EntityConfigurations
{
    public class LogEntryEntityTypeConfig : EntityTypeConfiguration<LogEntry>
    {
        public LogEntryEntityTypeConfig()
        {
            HasKey(logEntry => logEntry.Id);
        }
    }
}
