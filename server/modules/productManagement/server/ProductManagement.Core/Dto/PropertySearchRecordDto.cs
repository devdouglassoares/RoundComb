using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Dto
{
    public class PropertySearchRecordDto
    {
        public long Id { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public Dictionary<string, object> SearchQuery { get; set; }
    }
}