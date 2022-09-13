using System;
using Membership.Core.Entities.Base;

namespace Membership.Core.Entities
{
    public class AccessLog : BaseEntity
    {
        public int AccessLogId { get; set; }
        public int? UserId { get; set; }
        public int? PersonId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int? FeatureID { get; set; }
        public string FeatureName { get; set; }
        public string UrlAccessed { get; set; }
        public string LOGON_USER { get; set; }
        public string AUTH_USER { get; set; }
        public string LOCAL_ADDR { get; set; }
        public string REMOTE_ADDR { get; set; }
        public string REMOTE_HOST { get; set; }
        public string WebType { get; set; }
        public DateTime AccessedDateTime { get; set; }
    }
}