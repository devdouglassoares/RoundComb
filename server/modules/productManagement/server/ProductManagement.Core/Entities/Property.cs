using Core.DynamicProperties.Models;
using ProductManagement.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    public class Property : PropertyBaseModel, IHasDynamicProperty
    {
        public Property()
        {
            Images = new List<PropertyImage>();
            Tags = new HashSet<Tag>();
        }

        public virtual ICollection<PropertyImage> Images { get; set; }

        public virtual Property ParentProperty { get; set; }

        public virtual ICollection<Property> PropertyVariants { get; set; }

        public virtual PropertyCategory Category { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        [Column("Status")]
        public string StatusString
        {
            get { return Status.ToString(); }
            set { Status = (PropertyStatus)Enum.Parse(typeof(PropertyStatus), value, true); }
        }

        [Column("PropertySellType")]
        public string PropertySellTypeString
        {
            get
            {
                if (PropertySellType == null) return "";
                return PropertySellType.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    PropertySellType = null;
                    return;
                }
                PropertySellType = (PropertySellType)Enum.Parse(typeof(PropertySellType), value, true);
            }
        }
    }
}
