using Core.UI.DataTablesExtensions;

namespace Membership.Api.Models
{
    public class UsersDataTableRequest : DefaultDataTablesRequest
    {
        public long? CompanyId { get; set; }
        public string Role { get; set; }
        public long? ClientCompanyId { get; set; }
    }
}