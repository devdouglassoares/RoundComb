using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Core.Extensions;

namespace ProductManagement.Core.Dto
{
    public class PropertyCategoryDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool DisplayOnMenu { get; set; }

        public bool DisplayOnHomePage { get; set; }

        public int DisplayOrder { get; set; }

	    public bool EnableSimilarProperty { get; set; }

		public long? ParentCategoryId { get; set; }

        [JsonIgnore]
        public PropertyCategoryDto ParentCategory { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }
		
	    [DictionaryIgnore]
		public List<PropertyCategoryDto> Children { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}