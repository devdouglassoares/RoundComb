using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomForm.Core.Entities
{
    public class FormDynamicPropertyConfig
    {
        [Key]
        public long FormConfigurationId { get; set; }

        [Key]
        public long DynamicPropertyId { get; set; }

        [ForeignKey("FormConfigurationId")]
        public virtual FormConfiguration FormConfiguration { get; set; }

        public int DisplayOrder { get; set; }
    }
}