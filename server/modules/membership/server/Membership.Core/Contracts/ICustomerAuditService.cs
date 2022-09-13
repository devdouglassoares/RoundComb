using System.Collections.Generic;
using Core;

namespace Membership.Core.Contracts
{
    public interface ICustomerAuditService : IDependency
    {
        void LogCustomerViewed(long companyId, long userId);
        List<long> GetRecentCustomers(long userId);
    }
}
