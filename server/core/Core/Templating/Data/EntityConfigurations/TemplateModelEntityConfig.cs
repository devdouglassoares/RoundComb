using System.Data.Entity.ModelConfiguration;
using Core.Templating.Data.Entities;

namespace Core.Templating.Data.EntityConfigurations
{
    public class TemplateModelEntityConfig: EntityTypeConfiguration<TemplateModel>
    {
        public TemplateModelEntityConfig()
        {
            HasKey(templateModel => templateModel.Id);
        }
    }
}