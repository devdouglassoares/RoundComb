using Core;
using Core.Database;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Library.Dto;
using System;

namespace Membership.Library.Contracts
{
    public interface ICompanyService : IBaseService<Company, CompanyDto>, IDependency
    {
        void AssignSubDomain(string subdomain);

        void InsertOrUpdateCompany(string key, string name);
        void InsertOrUpdateCompany(CompanyDto dto);

        //IQueryable<CompanyDto> GetAll();
        //CompanyDto Get(long id);
        void UpdateCompanyStatus(long id, CompanyStatus status, DateTime? statusValidDate);

        void CreateCompanyForUser(CompanyDto companyModel, long userId);
    }

    public class CompanyAutocompleteDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Alias { get; set; }
        public long Id { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }

        public UserPersonalInformation MainContactUser { get; set; }

        public DateTime? StatusLastUpdated { get; set; }
        public DateTime? StatusValidDate { get; set; }
    }
}
