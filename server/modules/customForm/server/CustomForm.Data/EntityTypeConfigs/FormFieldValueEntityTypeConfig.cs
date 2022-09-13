using System.Data.Entity.ModelConfiguration;
using CustomForm.Core.Entities;

namespace CustomForm.Data.EntityTypeConfigs
{
    public class FormFieldValueEntityTypeConfig : EntityTypeConfiguration<FormFieldValue>
    {
        public FormFieldValueEntityTypeConfig()
        {
            HasKey(o => new
                        {
                            o.FieldId,
                            o.FormInstanceId
                        });
        }
    }
}