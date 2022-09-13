using System.Data.Entity;
using Core.Database.Repositories;
using Core.Templating.Data.Contexts;

namespace Core.Templating.Data.Repositories
{
    public interface ITemplatingRepository : IBaseRepository, IDependency { }

    public class TemplatingRepository : Repository, ITemplatingRepository
    {
        public TemplatingRepository(TemplatingContext dbContext) : base(dbContext)
        {
        }
    }
}