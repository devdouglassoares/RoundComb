using System.Data.Entity;
using Core.Database.Repositories;
using Core.DynamicProperties.Db;

namespace Core.DynamicProperties.Repositories
{
    public interface IDynamicPropertyRepository: IBaseRepository, IDependency
    {
         
    }

    public class DynamicPropertyRepository : Repository, IDynamicPropertyRepository
    {
        public DynamicPropertyRepository(DynamicPropertyContext dbContext) : base(dbContext)
        {
        }
    }
}