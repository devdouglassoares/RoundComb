using System;
using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Dto
{
    public class PropertyHistoryDto
    {
        public long Id { get; set; }

        public long PropertyId { get; set; }

        public virtual PropertyDto Property { get; set; }

        public long? UserId { get; set; }

        public string UserName { get; set; }

        public PropertyHistoryType PropertyHistoryType { get; set; }

        public string Comment { get; set; }

        public DateTimeOffset? RecordDate { get; set; }
    }

    public class PropertyUserHistoryDto
    {
        public long? UserId { get; set; }

        public string UserName { get; set; }

        public PropertyHistoryType PropertyHistoryType { get; set; }

        public int Count { get; set; }

        public DateTimeOffset? FirstRecorded { get; set; }

        public DateTimeOffset? LastRecorded { get; set; }
    }
}