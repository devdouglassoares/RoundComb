using ProductManagement.Core.Entities;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Dto
{
    public class TagDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long ParentId { get; set; }

        public int PropertyCounts { get; set; }

        public List<TagDto> ChildrenTags { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public static TagDto Map(Tag tag)
        {
            var dto = new TagDto();
            dto.Id = tag.Id;
            dto.Name = tag.Name;
            dto.ParentId = tag.ParentTag != null ? tag.ParentTag.Id : 0;

            return dto;
        }
    }
}
