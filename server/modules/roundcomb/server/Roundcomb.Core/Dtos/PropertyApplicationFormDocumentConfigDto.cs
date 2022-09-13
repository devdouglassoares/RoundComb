using Roundcomb.Core.Base;

namespace Roundcomb.Core.Dtos
{
	public class PropertyApplicationFormDocumentConfigDto : PropertyApplicationFormDocumentConfigBase
	{
		public string[] FileNames { get; set; }

		public string[] FileUrls { get; set; }
	}
}