using Core;
using Membership.Core.Data;
using Membership.Library.Data.Context;
using CoreRepo = Core.Database.Repositories.Repository;

namespace Membership.Library.Data
{
    [SuppressDependency(typeof(CoreRepository))]
    public class Repository : CoreRepo, IRepository, ICoreRepository
    {
        public Repository(MembershipContext context) : base(context)
        {
        }

        public void Dispose()
        {
            try
            {
                SaveChanges();
            }
            catch
            {

            }
        }
    }
}