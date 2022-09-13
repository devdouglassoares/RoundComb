using System;
using System.Collections.Generic;
using System.Linq;
using DTWrapper.Core;
using Membership.Api.Controllers.Base;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Entities;
using Membership.Library.Contracts;

namespace Membership.Api.Controllers
{
    [RequireAuthTokenApi]
    public class CustomersContactsController : DTApiController<CustomerContact>
    {
        private readonly ICustomersService customersService;

        public CustomersContactsController(ICustomersService customersService)
        {
            this.customersService = customersService;
        }

        #region Data tables adapter

        protected override IQueryable<CustomerContact> GetFilteredEntities()
        {
            return GetFilteredEntities(null);
        }

        protected override IQueryable<CustomerContact> GetFilteredEntities(dynamic customFilter)
        {
            var query = this.customersService.QueryCustomerContacts();

            var customFilterDictionary = customFilter as Dictionary<string, object>;

            if (customFilterDictionary != null)
            {
                if (customFilterDictionary.ContainsKey("phone") && customFilterDictionary["phone"] != null)
                {
                    var phone = customFilterDictionary["phone"].ToString();
                    if (!string.IsNullOrEmpty(phone))
                    {
                        phone = phone.Replace("-", String.Empty);
                        query = query.Where(x => x.PhoneNumber.Replace("-", String.Empty).Contains(phone));
                    }
                }
                if (customFilterDictionary.ContainsKey("email") && customFilterDictionary["email"] != null)
                {
                    var email = customFilterDictionary["email"].ToString();
                    if (!string.IsNullOrEmpty(email))
                    {
                        query = query.Where(x => x.Email.Contains(email));
                    }
                }
            }

            return query;
        }

        protected override void TableCustomize(DataTablesConfig<CustomerContact> dtCfg)
        {
            dtCfg.Set.Properties(x => x.Company.Id,
                x => x.Company.Name,
                x => x.FirstName,
                x => x.LastName,
                x => x.Email,
                x => x.PhoneNumber);
        }

        #endregion Data tables adapter


    }
}                                                        