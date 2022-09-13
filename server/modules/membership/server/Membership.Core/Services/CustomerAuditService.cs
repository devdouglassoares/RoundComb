using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Entities;

namespace Membership.Core.Services
{
    public class CustomerAuditService : ICustomerAuditService
    {
        private readonly ICoreRepository repository;
        private readonly ISmallCache cache;
        private readonly IBasicCompanyService basicCompanyService;

        public CustomerAuditService(ICoreRepository repository, ISmallCache cache, IBasicCompanyService basicCompanyService)
        {
            this.repository = repository;
            this.cache = cache;
            this.basicCompanyService = basicCompanyService;
        }

        public void LogCustomerViewed(long companyId, long userId)
        {
            var audit =
                this.repository.GetAll<CustomerViewAudit>()
                    .FirstOrDefault(x => x.UserId == userId && x.CompanyId == companyId);

            if (audit == null)
            {
                audit = new CustomerViewAudit
                {
                    UserId = userId,
                    CompanyId = companyId,
                    Date = DateTime.UtcNow
                };

                this.repository.Insert(audit);
            }
            else
            {
                audit.Date = DateTime.UtcNow;
                this.repository.Update(audit);
            }

            this.repository.SaveChanges();

            var recentIds = this.GetRecentCustomers(userId);
            recentIds.Reverse();

            var data = this.basicCompanyService.GetAll().Where(x => recentIds.Contains(x.Id)).ToList();

            foreach (var id in recentIds)
            {
                var item = data.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    data.Remove(item);
                    data.Insert(0, item);
                }
            }

            cache.Put("RECENT_CUSTOMERS", data);
        }

        public List<long> GetRecentCustomers(long userId)
        {
            return this.repository.GetAll<CustomerViewAudit>()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Date)
                .Select(x => x.CompanyId)
                .Take(6)
                .ToList();
        }
    }
}
