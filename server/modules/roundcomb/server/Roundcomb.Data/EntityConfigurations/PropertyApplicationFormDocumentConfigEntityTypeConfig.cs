using System.Data.Entity.ModelConfiguration;
using Roundcomb.Core.Entities;

namespace Roundcomb.Data.EntityConfigurations
{
	public class PropertyApplicationFormDocumentConfigEntityTypeConfig : EntityTypeConfiguration<PropertyApplicationFormDocumentConfig>
	{
		public PropertyApplicationFormDocumentConfigEntityTypeConfig()
		{
			HasKey(o => new
			            {
				            o.PropertyId,
				            o.Id
			            });
		}
	}
}