using System.ComponentModel.DataAnnotations;

namespace Core.DynamicProperties.Models
{
    public class DynamicPropertyEntityTypeMapping
    {
        [Key]
        public long DynamicPropertyId { get; set; }

        public virtual DynamicProperty DynamicProperty { get; set; }

        [Key, MaxLength(440)]
        public string TargetEntityType { get; set; }
    }
}