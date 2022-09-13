using Core.Database.Entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.DynamicProperties.Models
{
    public class DynamicPropertyValue : BaseEntity
    {
        public long PropertyId { get; set; }

        /// <summary>
        /// Gets the identifier of the linked entity
        /// </summary>
        public long ExternalEntityId { get; set; }

        public string TargetEntityType { get; set; }

        [ForeignKey("PropertyId")]
        public virtual DynamicProperty Property { get; set; }

        public string StringValue { get; set; }

        public int? IntValue { get; set; }

        public decimal? DecimalValue { get; set; }

        public DateTimeOffset? DateTimeValue { get; set; }

        public bool? BooleanValue { get; set; }

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