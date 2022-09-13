using System;
using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class CustomerViewAudit : BaseEntity
    {
        public long CompanyId { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
