using Core.UI.DataTablesExtensions;

namespace Membership.Api.Models
{
    public class CustomersDataTableRequest : DefaultDataTablesRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsShowDeleted { get; set; }
    }
}