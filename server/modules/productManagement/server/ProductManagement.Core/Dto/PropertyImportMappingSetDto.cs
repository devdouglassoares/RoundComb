using System;
using System.Collections.Generic;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Settings;

namespace ProductManagement.Core.Dto
{
    public class PropertyImportMappingSetDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public List<PropertyImportMappingConfig> Configuration { get; set; }
    }
}