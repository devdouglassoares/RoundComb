using System.ComponentModel.DataAnnotations;

namespace Core.DynamicProperties.Models
{
    public class DynamicPropertySupportedEntityType
    {
        [Key, MaxLength(450)]
        public string EntityTypeFullName { get; set; }

        public string Name { get; set; }
    }
}