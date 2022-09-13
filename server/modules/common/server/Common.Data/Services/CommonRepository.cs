using Common.Core.Repositories;
using Common.Data.Context;
using Core.Database.Repositories;

namespace Common.Data.Services
{
    public class CommonRepository : Repository, IRepository
    {
        public CommonRepository(CommonContext context)
            : base(context)
        {
        }
    }
}
