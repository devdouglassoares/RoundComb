using Core.Database.Entities;
using System;

namespace Common.Core.Entities
{
    public class AnnouncementViewAudit : BaseEntity
    {
        public string AnnouncementKey { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
