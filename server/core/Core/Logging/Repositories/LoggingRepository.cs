using Core.Database.Repositories;
using Core.Logging.Data;

namespace Core.Logging.Repositories
{
    public interface ILoggingRepository : IBaseRepository, IDependency { }

    public class LoggingRepository : Repository, ILoggingRepository
    {
        public LoggingRepository(LoggingContext dbContext) : base(dbContext)
        {
        }
    }
}