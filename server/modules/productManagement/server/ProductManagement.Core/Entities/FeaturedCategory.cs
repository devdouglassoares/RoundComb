using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    public class FeaturedCategory
    {
        public long Id { get; set; }

        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual PropertyCategory Category { get; set; }

        public int DisplayOrder { get; set; }
    }
}