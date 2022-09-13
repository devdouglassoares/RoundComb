using ProductManagement.Core.Entities;
using System.Collections.Generic;

namespace ProductManagement.Core.Dto
{
    public class ParentTagDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<TagDto> ChildTags { get; set; }

        public static ParentTagDto Map(Tag tag, List<TagDto> childTags)
        {
            var dto = new ParentTagDto();
            dto.Id = tag.Id;
            dto.Name = tag.Name;
            dto.ChildTags = childTags;

            return dto;
        }
    }
}
