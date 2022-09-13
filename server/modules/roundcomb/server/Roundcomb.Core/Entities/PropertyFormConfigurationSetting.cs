using ProductManagement.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Roundcomb.Core.Entities
{
    public class PropertyFormConfigurationSetting
    {
        [Key]
        public long PropertyCategoryId { get; set; }

        [Key]
        public long FormConfigurationId { get; set; }

        public virtual PropertyCategory PropertyCategory { get; set; }
    }
}