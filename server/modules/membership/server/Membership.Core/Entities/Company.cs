using Core.DynamicProperties.Models;
using Membership.Core.Entities.Base;
using Membership.Core.Entities.Enums;
using Membership.Core.Extentions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Membership.Core.Entities
{
    public class Company : BaseEntity, IHasDynamicProperty
    {
        public string Domain { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string LogoUrl { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public virtual User MainContactUser { get; set; }

        public virtual User Owner { get; set; }

        //        public virtual Computer Computer { get; set; }

        public List<User> Users { get; set; }

        public string CurrentTheme { get; set; }

        public int? ClientsLimit { get; set; }


        public DateTimeOffset? ModifiedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<CompanySetting> Settings { get; set; }

        public Tms_Account TmsAccount { get; set; }

        [Obsolete("Not maintained any more, will be removed soon")]
        public virtual ICollection<CustomerContact> CustomerContacts { get; set; }

        public virtual ICollection<CustomerSite> CustomerSites { get; set; }

        public DateTime? StatusLastUpdated { get; set; }

        [NotMapped]
        public CompanyStatus? Status { get; set; }

        [Column("Status")]
        public string StatusString
        {
            get { return Status.HasValue ? Status.ToString() : null; }
            set { this.Status = string.IsNullOrEmpty(value) ? (CompanyStatus?)null : value.ToEnum<CompanyStatus>(); }
        }

        public DateTime? StatusValidDate { get; set; }

        public long? MasterCompanyId { get; set; }

        public virtual Company MasterCompany { get; set; }

        public virtual ICollection<Company> ClientCompanies { get; set; }

        public bool IsMasterCompany => MasterCompany == null;
    }
}
