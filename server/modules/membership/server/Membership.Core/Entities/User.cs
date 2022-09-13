using Core.Database.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Membership.Core.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Roles = new Collection<Role>();
            UserAccessRights = new Collection<UserAccessRight>();
            Contacts = new Collection<Contact>();
            ExternalLogins = new Collection<UserExternalLogin>();

            FollowedByUsers = new List<User>();
            FollowedUsers = new List<User>();
        }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Comment { get; set; }

        public bool IsApproved { get; set; }

        public bool IsVirtual { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        public DateTime LastPasswordFailureDate { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime LastLockoutDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string ConfirmationToken { get; set; }

        public bool IsLockedOut { get; set; }

        public DateTime? LastPasswordChangeDate { get; set; }

        public string PasswordVerificationToken { get; set; }

        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public bool IsActive { get; set; }

        public long? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<UserAccessRight> UserAccessRights { get; set; }

        public virtual ICollection<User> FollowedUsers { get; set; }

        public virtual ICollection<User> FollowedByUsers { get; set; }

        public string HomePhoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public virtual User ImpersonatedAs { get; set; }

        public string ExternalKey { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }


        public long? ClientCompanyId { get; set; }

        [Obsolete("We may not need it any more")]
        public virtual Company ClientCompany { get; set; }


        public virtual IList<UserExternalLogin> ExternalLogins { get; set; }

        public string Devices { get; set; }

        public DateTimeOffset? RegistrationDate { get; set; }
    }
}