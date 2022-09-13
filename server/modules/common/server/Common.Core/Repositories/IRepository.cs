using Core;
using Core.Database.Repositories;

namespace Common.Core.Repositories
{
    public interface IRepository : IBaseRepository, IDependency
    {
    }
}
