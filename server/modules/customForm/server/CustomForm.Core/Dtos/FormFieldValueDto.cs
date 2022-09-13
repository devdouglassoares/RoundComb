using Core.DynamicProperties.Models;
using System;

namespace CustomForm.Core.Dtos
{
    public class FormFieldValueDto
    {
        public PropertyType PropertyType { get; set; }

        public long FieldId { get; set; }

        public string[] CheckboxValues { get; set; }

        public UploadedFilePaths[] UploadedPaths { get; set; }

        public string StringValue { get; set; }

        public int? IntValue { get; set; }

        public decimal? DecimalValue { get; set; }

        public DateTimeOffset? DateTimeValue { get; set; }

        public bool? BooleanValue { get; set; }

        public long FormInstanceId { get; set; }
    }
}