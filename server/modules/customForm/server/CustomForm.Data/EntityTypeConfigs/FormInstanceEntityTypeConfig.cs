using System.Data.Entity.ModelConfiguration;
using CustomForm.Core.Entities;

namespace CustomForm.Data.EntityTypeConfigs
{
    public class FormInstanceEntityTypeConfig : EntityTypeConfiguration<FormInstance>
    {
        public FormInstanceEntityTypeConfig()
        {
            HasKey(o => o.Id);
        }
    }
}