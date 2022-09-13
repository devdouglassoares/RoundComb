using ProductManagement.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ProductManagement.Data.EntityConfigurations
{
    public class TagEntityTypeConfig : EntityTypeConfiguration<Tag>
    {
        public TagEntityTypeConfig()
        {
            HasMany(tag => tag.Properties)
                .WithMany(p => p.Tags);

            HasOptional(tag => tag.ParentTag);
        }
    }
}