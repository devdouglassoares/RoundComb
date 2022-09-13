using Core;
using Membership.Core.Entities;
using Membership.Library.Dto;

namespace Membership.Library.Contracts
{
    public interface ICompanySettingsService : IDependency
    {
        void SaveSettingsGroup(Company company, CompanySettingsGroupDto settingsGroup);
    }
}
