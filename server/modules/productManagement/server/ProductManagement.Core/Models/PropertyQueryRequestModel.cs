using Core.DynamicProperties.Dtos;
using ProductManagement.Core.Entities;
using System.Collections.Generic;

namespace ProductManagement.Core.Models
{
    public class PropertyQueryRequestModel
    {
        public int? PageSize { get; set; }

        public int? PageIndex { get; set; }

        public string Keyword { get; set; }

        public long? CategoryId { get; set; }

        public long[] LocationIds { get; set; }

        public string[] TagNames { get; set; }

        public bool SearchByLocations { get; set; }

        public string CustomQueries { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public PropertySellType? PropertySellType { get; set; }

        public Dictionary<long, DynamicPropertyFilterModel> DynamicPropertyFilters { get; set; }
    }
}