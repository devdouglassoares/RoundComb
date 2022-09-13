using Core;
using Core.Database.Repositories;

namespace LocationService.Library.Data
{
    public interface IRepository : IBaseRepository, IDependency
    {
    }

    public class Repository : Core.Database.Repositories.Repository, IRepository
    {
        public Repository(LocationServiceDbContext dbContext) : base(dbContext)
        {
        }
    }
}