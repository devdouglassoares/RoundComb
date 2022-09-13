using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Core.Database.Conventions
{
    public class ManyToManyCustomTablePrefixConvention : IStoreModelConvention<EntitySet>
    {
        private readonly string tablePrefix;

        public ManyToManyCustomTablePrefixConvention(string tablePrefix)
        {
            this.tablePrefix = tablePrefix;
        }

        public void Apply(EntitySet set, DbModel model)
        {
            var properties = set.ElementType.Properties;
            if (properties.Count == 2)
            {
                var relationEnds = new List<string>();
                int i = 0;
                foreach (var metadataProperty in properties)
                {
                    if (metadataProperty.Name.EndsWith("Id"))
                    {
                        var name = metadataProperty.Name;
                        relationEnds.Add(name.Substring(0, name.Length - 2));
                        i++;
                    }
                }
                if (relationEnds.Count == 2)
                {
                    set.Table = tablePrefix + "_" + relationEnds.ElementAt(0) + relationEnds.ElementAt(1);
                }
            }
        }
    }
}