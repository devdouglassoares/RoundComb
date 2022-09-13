using Core.Database.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    public class PropertyCategory : BaseEntity
    {
	    public const string UnknownCategory = "Unknown";

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool DisplayOnMenu { get; set; }

        public bool DisplayOnHomePage { get; set; }

        public int DisplayOrder { get; set; }

		public bool EnableSimilarProperty { get; set; }

        public long? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual PropertyCategory ParentCategory { get; set; }

        public virtual ICollection<PropertyCategory> Children { get; set; }

        public virtual ICollection<PropertyDynamicPropertyCategory> DynamicProperties { get; set; }
    }
}