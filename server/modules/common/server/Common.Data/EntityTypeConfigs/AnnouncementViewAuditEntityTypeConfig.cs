using System.Data.Entity.ModelConfiguration;
using Common.Core.Entities;

namespace Common.Data.EntityTypeConfigs
{
    public class AnnouncementViewAuditEntityTypeConfig : EntityTypeConfiguration<AnnouncementViewAudit>
    {
        public AnnouncementViewAuditEntityTypeConfig()
        {
            HasKey(x => x.Id);
        }
    }
}