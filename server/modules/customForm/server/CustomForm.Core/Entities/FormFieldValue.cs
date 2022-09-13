using Core.DynamicProperties.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomForm.Core.Entities
{
    public class FormFieldValue
    {
        [Key]
        public long FieldId { get; set; }

        public virtual FormField Field { get; set; }

        [Key]
        public long FormInstanceId { get; set; }

        public virtual FormInstance FormInstance { get; set; }

        [Column("CheckboxValues")]
        public string CheckboxValueString { get; set; }

        [NotMapped]
        public string[] CheckboxValues
        {
            get
            {
                if (string.IsNullOrEmpty(CheckboxValueString))
                    return new string[] { };

                return JsonConvert.DeserializeObject<string[]>(CheckboxValueString);
            }
            set { CheckboxValueString = JsonConvert.SerializeObject(value); }
        }

        public string StringValue { get; set; }

        public int? IntValue { get; set; }

        public decimal? DecimalValue { get; set; }

        public DateTimeOffset? DateTimeValue { get; set; }

        public bool? BooleanValue { get; set; }

        [Column("UploadedPaths")]
        public string UploadedPathsString { get; set; }

        [NotMapped]
        public UploadedFilePaths[] UploadedPaths
        {
            get
            {
                if (string.IsNullOrEmpty(UploadedPathsString))
                    return new UploadedFilePaths[] { };

                return JsonConvert.DeserializeObject<UploadedFilePaths[]>(UploadedPathsString);
            }
            set { UploadedPathsString = JsonConvert.SerializeObject(value); }
        }
    }
}