using Core.Database.Entities;

namespace Roundcomb.Core.Base
{
	public abstract class PropertyApplicationFormDocumentConfigBase : BaseEntity
	{
		public string FileName { get; set; }

		public string FileUrl { get; set; }

		public long PropertyId { get; set; }

		public bool IsExternalSite { get; set; }

		public string ResultUrl { get; set; }

		public int DownloadTime { get; set; }
	}
}