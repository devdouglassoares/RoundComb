using CustomForm.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CustomForm.Data.EntityTypeConfigs
{
    public class FormFieldFormConfigurationEntityTypeConfig : EntityTypeConfiguration<FormFieldFormConfiguration>
    {
        public FormFieldFormConfigurationEntityTypeConfig()
        {
            HasKey(o => new
            {
                o.FormFieldId,
                o.FormConfigurationId
            });
        }
    }
}