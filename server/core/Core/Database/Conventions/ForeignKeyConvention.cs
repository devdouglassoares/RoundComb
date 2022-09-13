using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Core.Database.Conventions
{
    public class ForeignKeyConvention : IStoreModelConvention<AssociationType>
    {
        public void Apply(AssociationType association, DbModel model)
        {
            if (association.IsForeignKey)
            {
                foreach (EdmProperty property in association.Constraint.FromProperties)
                {
                    property.Name = property.Name.Replace("_", string.Empty);
                }

                foreach (EdmProperty property in association.Constraint.ToProperties)
                {
                    property.Name = property.Name.Replace("_", string.Empty);
                }
            }
        }
    }
}