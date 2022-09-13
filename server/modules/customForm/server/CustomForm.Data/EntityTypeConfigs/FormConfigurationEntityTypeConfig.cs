using CustomForm.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CustomForm.Data.EntityTypeConfigs
{
    public class FormConfigurationEntityTypeConfig : EntityTypeConfiguration<FormConfiguration>
    {
        public FormConfigurationEntityTypeConfig()
        {
            HasKey(o => o.Id);
        }
    }

    public class FormDynamicPropertyConfigEntityTypeConfig : EntityTypeConfiguration<FormDynamicPropertyConfig>
    {
        public FormDynamicPropertyConfigEntityTypeConfig()
        {
            HasKey(x => new
                        {
                            x.FormConfigurationId,
                            x.DynamicPropertyId
                        });
        }
    }
}