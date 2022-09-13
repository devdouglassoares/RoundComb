using Core.DynamicProperties.Models;
using System;

namespace Core.DynamicProperties.Dtos
{
    public class DynamicPropertyValueDto
    {
        public DynamicPropertyModel Property { get; set; }

        public string StringValue { get; set; }

        public int? IntValue { get; set; }

        public decimal? DecimalValue { get; set; }

        public DateTimeOffset? DateTimeValue { get; set; }

        public bool? BooleanValue { get; set; }

        public string[] CheckboxValues { get; set; }

        public UploadedFilePaths[] UploadedPaths { get; set; }

        public object RawValue
        {
            get
            {
                if (Property == null || Property.PropertyType == PropertyType.Text || Property.PropertyType == PropertyType.LongText)
                {
                    return StringValue;
                }

                switch (Property.PropertyType)
                {
                    case PropertyType.Boolean:
                        return BooleanValue;

                    case PropertyType.DatePicker:
                        return DateTimeValue;

                    case PropertyType.Number:
                        return IntValue;

                    case PropertyType.Currency:
                        return DecimalValue;

                    case PropertyType.CheckBoxes:
                        return CheckboxValues;

                    default:
                        return StringValue;
                }
            }
        }
    }
}