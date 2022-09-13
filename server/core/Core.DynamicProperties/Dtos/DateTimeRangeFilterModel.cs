using System;

namespace Core.DynamicProperties.Dtos
{
    public class DateTimeRangeFilterModel
    {
        public string PropertyName { get; set; }

        public DateTimeOffset? MinValue { get; set; }

        public DateTimeOffset? MaxValue { get; set; }
    }
}