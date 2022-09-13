using Core.Database.Repositories;
using Membership.Core.Data.Context;

namespace Membership.Core.Data
{
    public class CoreRepository : Repository, ICoreRepository
    {
        public CoreRepository(MembershipCoreContext context) : base(context)
        {
        }

        public void Dispose()
        {
            SaveChanges();
        }
    }
}