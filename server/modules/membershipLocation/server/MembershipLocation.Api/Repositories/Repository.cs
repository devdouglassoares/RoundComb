using Core;
using Core.Database.Repositories;
using MembershipLocation.Api.DbContexts;

namespace MembershipLocation.Api.Repositories
{
    public interface IRepository: IBaseRepository, IDependency { }
    public class MembershipLocationRepository: Repository, IRepository
    {
        public MembershipLocationRepository(MembershipLocationDbContext dbContext) : base(dbContext)
        {
        }
    }
}