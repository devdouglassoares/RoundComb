using System.Linq;
using Membership.Core.Entities;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Dto;

namespace Membership.Library.Services
{
    public class CompanySettingsService : ICompanySettingsService
    {
        private readonly IRepository repository;

        public CompanySettingsService(IRepository repository)
        {
            this.repository = repository;
        }

        public void SaveSettingsGroup(Company company, CompanySettingsGroupDto settingsGroup)
        {
            var settingParent = company.Settings.FirstOrDefault(x => x.Id == settingsGroup.Id);

            if (settingParent != null)
            {
                foreach (var settingModel in settingsGroup.Settings)
                {
                    var setting = settingParent.SubSettings.FirstOrDefault(x => x.Type == settingModel.Key);
                    if (setting != null)
                    {
                        setting.Value = settingModel.Value;
                    }
                    else
                    {
                        settingParent.SubSettings.Add(new CompanySetting
                        {
                            Company = company,
                            //Parent = settingParent,
                            Type = settingModel.Key,
                            Value = settingModel.Value
                        });
                    }
                }
            }

            this.repository.Update(company);
            this.repository.SaveChanges();
        }
    }
}
