using Core;
using Core.Database.Repositories;

namespace ProductManagement.Core.Repositories
{
    public interface IRepository : IBaseRepository, IDependency
    {
    }   
}