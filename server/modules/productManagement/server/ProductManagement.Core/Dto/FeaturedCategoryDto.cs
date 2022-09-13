using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Dto
{
    public class FeaturedCategoryDto
    {
        public long Id { get; set; }

        public long CategoryId { get; set; }
        
        public virtual PropertyCategoryDto Category { get; set; }

        public int DisplayOrder { get; set; }
    }
}