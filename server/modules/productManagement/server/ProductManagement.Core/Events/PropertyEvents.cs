using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using System.Collections.Generic;

namespace ProductManagement.Core.Events
{
    public class PropertyImportingEvent
    {
        public List<PropertyDto> PropertiesToImport { get; set; }

        public long ImportingByUserId { get; set; }
    }

    public class PropertyImportedEvent
    {
        public Property Property { get; set; }

        public PropertyDto PropertyDto { get; set; }
    }

    public class PropertyCreated
    {
        public Property Property { get; set; }

        public PropertyDto PropertyDto { get; set; }
    }

    public class PropertyUpdated
    {
        public Property Property { get; set; }

        public PropertyDto PropertyDto { get; set; }
    }

    public class PropertyDeleted
    {
        public Property Property { get; set; }
    }

    public class PropertyDtoWired
    {
        public PropertyDto PropertyDto { get; set; }
    }
}