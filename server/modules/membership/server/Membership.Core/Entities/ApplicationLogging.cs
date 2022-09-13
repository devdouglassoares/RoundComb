using System;
using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class ApplicationLogging : BaseEntity
    {
        public DateTime? TimeStamp { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
    }
}