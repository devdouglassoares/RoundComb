using System.Data.Entity.ModelConfiguration;
using CustomForm.Core.Entities;

namespace CustomForm.Data.EntityTypeConfigs
{
    public class FormFieldEntityTypeConfig : EntityTypeConfiguration<FormField>
    {
        public FormFieldEntityTypeConfig()
        {
            HasKey(o => o.Id);
        }
    }
}