using Core;
using Core.Database;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Library.Dto;
using Membership.Library.Dto.Customer;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Library.Contracts
{
    public interface ICustomersService : IBaseService<Company, CompanyDto>, IDependency
    {
        IQueryable<CompanyAutocompleteDto> GetAllAutocomplete();

        IQueryable<Company> GetAllCustomers();

        Company CreateCustomer(CompanyDto model);

        void Revert(long id);

        object GetCustomerDetails(long companyId);

        /*ConnectionInfoDto GetConnectionInfo(int companyId);
        void SaveConnectionInfo(ConnectionInfoDto dto);*/


        object GetDocuments(long companyId);

        List<VendorDto> GetCustomerVendors(long companyId);
        void SaveCustomerVendor(VendorDto dto);
        void DeleteCustomerVendor(VendorDto dto);

        IQueryable<CustomerContact> QueryCustomerContacts();

        List<UserBaseModel> GetContacts(long companyId);

        void SaveContact(UserBaseModel contact);

        void DeleteContact(long contactId);

        List<SiteDto> GetSites(long companyId);
        void SaveSite(SiteDto dto);
        void DeleteSite(SiteDto dto);

        List<PartnershipDto> GetPartnerships(long companyId);
        void SavePartnership(PartnershipDto dto);
        void DeletePartnership(PartnershipDto dto);

        List<LookupDto> GetPartners();
        void SavePartner(PartnershipDto dto);

        List<LookupDto> GetVendors();
        void SaveVendor(LookupDto dto);

        List<LookupDto> GetVendorTypes();
        void SaveVendorType(LookupDto dto);

        List<LookupDto> GetProjectDescriptions();
        void SaveProjectDescription(LookupDto dto);
    }
}
