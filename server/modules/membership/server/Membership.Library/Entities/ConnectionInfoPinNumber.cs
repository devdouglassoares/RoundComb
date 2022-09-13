using System;
using Membership.Core.Entities.Base;

namespace Membership.Library.Entities
{
    public class ConnectionInfoPinNumber : BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public int Pin { get; set; }
        public ConnectionInfo ConnectionInfo { get; set; }
        public long UserId { get; set; }
    }
}