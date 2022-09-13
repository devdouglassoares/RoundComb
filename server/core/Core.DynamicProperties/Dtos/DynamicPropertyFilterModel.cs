using Core.DynamicProperties.Models;
using System;
using System.Linq;

namespace Core.DynamicProperties.Dtos
{
    public class DynamicPropertyFilterModel
    {
        public long PropertyId { get; set; }

        public PropertyType PropertyType { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string[] CheckBoxesValues { get; set; }

        public DateTimeOffset? MinDateValue { get; set; }

        public DateTimeOffset? MaxDateValue { get; set; }

        public int? MinNumberValue { get; set; }

        public int? MaxNumberValue { get; set; }

        public decimal? MinCurrencyValue { get; set; }

        public decimal? MaxCurrencyValue { get; set; }

        public bool? BoleanFilterValue { get; set; }

        public string StringFilterValue { get; set; }

        public bool HasFilter
        {
            get
            {
                return (CheckBoxesValues != null && CheckBoxesValues.Any()) ||
                  MinDateValue.HasValue ||
                  MaxDateValue.HasValue ||
                  MinNumberValue.HasValue ||
                  MaxNumberValue.HasValue ||
                  MinCurrencyValue.HasValue ||
                  MaxCurrencyValue.HasValue ||
                  BoleanFilterValue.HasValue ||
                  !string.IsNullOrEmpty(StringFilterValue);
            }
        }
    }
}