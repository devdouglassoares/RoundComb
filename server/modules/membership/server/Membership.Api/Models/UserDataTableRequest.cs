using Core.UI.DataTablesExtensions;

namespace Membership.Api.Models
{
    public class UserDataTableRequest : DefaultDataTablesRequest
    {
        public string Role { get; set; }

        public long? ClientCompanyId { get; set; }

        public bool ShowDeletedOnly { get; set; }

        public bool ShowApprovalPendingOnly { get; set; }
        public bool ShowVirtualUsers { get; set; }

        public long? CompanyId { get; set; }
    }
}