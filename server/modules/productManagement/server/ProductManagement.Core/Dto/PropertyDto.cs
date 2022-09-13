using Core.DynamicProperties.Dtos;
using Core.Extensions;
using ProductManagement.Core.Base;
using ProductManagement.Core.Entities;
using System.Collections.Generic;

namespace ProductManagement.Core.Dto
{
    public class PropertyDto : PropertyBaseModel
    {
        public string NameFormat { get; set; }

        public PropertyCategoryDto Category { get; set; }

		[DictionaryIgnore]
        public IEnumerable<PropertyImageDto> Images { get; set; }

	    [DictionaryIgnore]
		public string[] Tags { get; set; }

        public DynamicPropertyValuesModel ExtendedProperties { get; set; }

        public string OwnerName { get; set; }

        public int Variants { get; set; }

        public int Similars { get; set; }

        [DictionaryIgnore]
        public IEnumerable<PropertyDto> PropertyVariants { get; set; }

        public dynamic ExtensionProperties { get; set; }

        public PropertyDto()
        {
            Status = PropertyStatus.Draft;
        }

        public PropertyDto(Property property)
        {
            property.CopyTo(this);
        }
    }

    public class PropertyWithSettingDto : PropertyDto
    {
        //public ProductSettingDto ProductSetting { get; set; }
    }

    public class FeaturedPropertyDto : PropertyDto
    {
        public int DisplayOrder { get; set; }
    }
}
