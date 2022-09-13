using Core.UI.DataTablesExtensions;
using ProductManagement.Core.Entities;

namespace ProductManagement.Api.Models
{
    public class PropertyBackendFilterQueryRequest : DefaultDataTablesRequest
    {
        public long? CategoryId { get; set; }

        public string QueryString { get; set; }

        public PropertySellType? PropertySellType { get; set; }

        public PropertyStatus? PropertyStatus { get; set; }

        public bool IsShowDeleted { get; set; }
    }
}