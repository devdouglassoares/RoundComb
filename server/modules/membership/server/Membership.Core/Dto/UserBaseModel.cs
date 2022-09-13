using System;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Core.Dto
{
    public class UserBaseModel
    {

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string CompanyName { get; set; }

        public string Ssn { get; set; }
        public string Password { get; set; }
        public string CellPhoneNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public bool IsNeedInvite { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVirtual { get; set; }

        public bool IsMainContactUser { get; set; }

        public bool IsOwner { get; set; }
        public long? BizOwnerId { get; set; }
        public List<string> Roles { get; set; }

        public string RolesString
        {
            get
            {
                return (this.Roles != null && Roles.Any())
                    ? string.Join(", ", this.Roles.Select(x => x.Replace("Biz ", string.Empty)))
                    : string.Empty;
            }
        }

        public string BizCompanyName { get; set; }

        public string ExternalKey { get; set; }
        public string Address { get; set; }
        public long? OtherAccessEntityId { get; set; }

        public List<string> Emails { get; set; }

        public List<string> PhoneNumbers { get; set; }

        public bool IsExternalUser
        {
            get
            {
                // External user:
                // has key, not MERGED#, not TBD_BY_MBAC
                if (!string.IsNullOrEmpty(this.ExternalKey) && !this.ExternalKey.Contains("MERGED#") && !this.ExternalKey.Contains("TBD_BY_MBAC"))
                {
                    return true;
                }

                return false;
            }
        }

        public long? ClientCompanyId { get; set; }

        public long? CompanyId { get; set; }

        public CompanyShortDto Company { get; set; }

        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public virtual DateTimeOffset? CreatedDate { get; set; }

        public virtual string FormattedCreatedDate
        {
            get
            {
                if (CreatedDate != null)
                {
                    return CreatedDate.Value.ToString("G");
                }
                return string.Empty;
            }
        }
    }
}
