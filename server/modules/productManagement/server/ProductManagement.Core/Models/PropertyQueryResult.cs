using ProductManagement.Core.Dto;
using System.Collections.Generic;

namespace ProductManagement.Core.Models
{
    public class PropertyQueryResult
    {
        public int Count { get; set; }

        public IEnumerable<PropertyDto> Data { get; set; } 

        public int? PageIndex { get; set; }
    }
}