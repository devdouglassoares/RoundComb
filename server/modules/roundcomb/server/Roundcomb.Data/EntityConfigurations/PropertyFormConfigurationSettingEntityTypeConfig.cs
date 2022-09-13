using Roundcomb.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Roundcomb.Data.EntityConfigurations
{
    public class PropertyFormConfigurationSettingEntityTypeConfig : EntityTypeConfiguration<PropertyFormConfigurationSetting>
    {
        public PropertyFormConfigurationSettingEntityTypeConfig()
        {
            HasKey(o => new
            {
                o.PropertyCategoryId,
                o.FormConfigurationId
            });
        }
    }
}