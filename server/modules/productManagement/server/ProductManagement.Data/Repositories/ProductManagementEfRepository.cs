using Core.Database.Repositories;
using ProductManagement.Core.Repositories;
using ProductManagement.Data.Context;

namespace ProductManagement.Data.Repositories
{
    public class ProductManagementEfRepository : Repository, IRepository
    {
        public ProductManagementEfRepository(PropertyManagementContext dbContext) : base(dbContext)
        {
        }
    }
}