using System.Collections.Concurrent;

namespace Core.DynamicProperties.Dtos
{
    public class DynamicPropertyValuesModel : ConcurrentDictionary<string, DynamicPropertyValueDto>
    {
        public long ExternalEntityId { get; set; }
    }
}