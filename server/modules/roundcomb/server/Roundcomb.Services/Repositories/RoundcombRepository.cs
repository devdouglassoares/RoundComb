using Core.Database.Repositories;
using Roundcomb.Core.Repositories;
using Roundcomb.Data.DbContexts;

namespace Roundcomb.Services.Repositories
{
    public class RoundcombRepository : Repository, IRoundcombRepository
    {
        public RoundcombRepository(RoundcombContext dbContext) : base(dbContext)
        {
        }
    }
}