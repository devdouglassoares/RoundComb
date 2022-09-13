using Core.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.DynamicProperties.Models
{
    public class DynamicProperty : BaseEntity
    {
        public string DisplayName { get; set; }

        public string PropertyName { get; set; }

        public bool Searchable { get; set; }

        public bool IsRequired { get; set; }

        public bool RangeSearchable { get; set; }

        public string UploadEndpointUrl { get; set; }

        public bool MultipleUpload { get; set; }

        [NotMapped]
        public PropertyType PropertyType { get; set; }

        [Column("PropertyType")]
        public virtual string PropertyTypeString
        {
            get
            {
                return PropertyType.ToString();
            }
            set
            {
                PropertyType newValue;
                if (Enum.TryParse(value, out newValue))
                {
                    PropertyType = newValue;
                }
            }
        }

        public bool AvailableToAllEntities { get; set; }

        public List<string> AvailableOptions
        {
            get
            {
                return string.IsNullOrEmpty(AvailableOptionsString)
                    ? new List<string>()
                    : AvailableOptionsString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            set { AvailableOptionsString = string.Join(";", value); }
        }

        [Column("AvailableOptions")]
        public string AvailableOptionsString { get; set; }

        public virtual ICollection<DynamicPropertyEntityTypeMapping> DynamicPropertyEntityTypeMappings { get; set; }

        public DynamicProperty()
        {
            DynamicPropertyEntityTypeMappings = new List<DynamicPropertyEntityTypeMapping>();
        }
    }
}