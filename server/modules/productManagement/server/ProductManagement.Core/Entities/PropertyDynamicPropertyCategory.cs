using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.DynamicProperties.Models;

namespace ProductManagement.Core.Entities
{
    public class PropertyDynamicPropertyCategory
    {
        [Key]
        public long CategoryId { get; set; }

        [Key]
        public long DynamicPropertyId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual PropertyCategory Category { get; set; }
        
        public int DisplayOrder { get; set; }
    }
}