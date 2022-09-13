using System.Collections.Generic;
using System.Linq;
using Membership.Core.Entities;
using Membership.Core.Entities.Enums;
using Membership.Library.Dto;

namespace Membership.Library.Extentions
{
    public static class CompanyExtentions
    {
        public static string GetSetting(this Company company, CompanySettings settingType)
        {
            var setting = company.Settings.FirstOrDefault(x => x.Type == settingType);

            if (setting != null && !string.IsNullOrEmpty(setting.Value))
            {
                return setting.Value;
            }

            return string.Empty;
        }

        public static void SetSetting(this Company company, CompanySettings settingType, string value)
        {
            var setting = company.Settings.FirstOrDefault(x => x.Type == settingType);

            if (setting == null)
            {
                setting = new CompanySetting
                {
                    Company = company,
                    Type = settingType
                };

                company.Settings.Add(setting);
            }

            setting.Value = value;
        }
        
        public static bool HasValidJiraAccount(this Company company)
        {
            return
                !string.IsNullOrEmpty(company.GetSetting(CompanySettings.JiraUrl)) &&
                !string.IsNullOrEmpty(company.GetSetting(CompanySettings.JiraProjectKey)) &&
                !string.IsNullOrEmpty(company.GetSetting(CompanySettings.JiraLogin)) &&
                !string.IsNullOrEmpty(company.GetSetting(CompanySettings.JiraPassword));
        }

        public static IEnumerable<CompanySettingsGroupDto> GetSettingsGroupsByType(this Company company, CompanySettings settingType)
        {
            var settingsWithNotEmptySubsettings = company.Settings
                .Where(x => x.Type == settingType)
                .Where(setting => setting.SubSettings != null && setting.SubSettings.Any())
                .ToList();

            if (settingsWithNotEmptySubsettings.Any())
            {
                return settingsWithNotEmptySubsettings.Select(x => new CompanySettingsGroupDto
                {
                    Id = x.Id,
                    Name = x.Value,
                    Settings = x.SubSettings.ToDictionary(key => key.Type, value => value.Value)
                });
            }

            return null;
        }
        public static Dictionary<CompanySettings, string> GetSettingsGroupById(this Company company, long groupId)
        {
            var settingsGroup = company.Settings.FirstOrDefault(x => x.Id == groupId);

            if (settingsGroup != null && settingsGroup.SubSettings != null && settingsGroup.SubSettings.Any())
            {
                return settingsGroup.SubSettings.ToDictionary(x => x.Type, y => y.Value);
            }

            return null;
        }

        public static void AddSettingsGroup(this Company company, CompanySettings settingType, string name, Dictionary<CompanySettings, string> group)
        {
            company.Settings.Add(new CompanySetting
            {
                Company = company,
                Type = settingType,
                Value = name,
                SubSettings = group.Select(x => new CompanySetting { Type = x.Key, Value = x.Value }).ToList()
            });
        }
    }
}
