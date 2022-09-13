using Core.SiteSettings;
using Membership.Core.Data;
using Membership.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Membership.Core.SiteSettings
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly IEnumerable<ISiteSettingBase> _settingModelBases;
        private readonly ICoreRepository _siteSettingRepository;

        private readonly IDictionary<Type, SiteSetting> _cachedSettings = new ConcurrentDictionary<Type, SiteSetting>();

        public SiteSettingService(ICoreRepository siteSettingRepository,
                                  IEnumerable<ISiteSettingBase> settingModelBases)
        {
            _siteSettingRepository = siteSettingRepository;
            _settingModelBases = settingModelBases;
        }

        public IEnumerable<ISiteSettingBase> GetAllAvailableSettings()
        {
            return _settingModelBases;
        }

        public T GetSetting<T>() where T : ISiteSettingBase
        {
            return GetSetting(typeof(T));
        }

        public dynamic GetSetting(Type type)
        {
            if (!typeof(ISiteSettingBase).IsAssignableFrom(type))
                throw new ArgumentException(
                    $"The setting model must be inherited from '{typeof(ISiteSettingBase).FullName}' interface");

            if (_cachedSettings.ContainsKey(type))
            {
                return _cachedSettings[type].GetSettingValueAs(type);
            }

            var settingKey = SiteSetting.GetSettingKey(type);
            var setting = _siteSettingRepository.First<SiteSetting>(x => x.SettingKey == settingKey);

            if (setting != null)
            {
                lock (_cachedSettings)
                {
                    if (_cachedSettings.ContainsKey(type))
                        _cachedSettings.Remove(type);

                    _cachedSettings.Add(type, setting);
                }
            }

            return setting == null
                ? Activator.CreateInstance(type)
                : setting.GetSettingValueAs(type);
        }

        public void SaveSetting<T>(T value)
        {
            if (!typeof(ISiteSettingBase).IsAssignableFrom(typeof(T)))
                throw new ArgumentException(
                    $"The setting model must be inherited from '{typeof(ISiteSettingBase).FullName}' interface");

            if (_cachedSettings.ContainsKey(typeof(T)))
            {
                _cachedSettings.Remove(typeof(T));
            }

            var settingKey = SiteSetting.GetSettingKey(typeof(T));
            var setting = _siteSettingRepository.First<SiteSetting>(x => x.SettingKey == settingKey);

            if (setting == null)
            {
                setting = new SiteSetting
                {
                    SettingKey = settingKey
                };
                setting.UpdateSettingValue(value);
                setting.LastUpdated = DateTimeOffset.Now;
                _siteSettingRepository.Insert(setting);
            }
            else
            {
                setting.UpdateSettingValue(value);
                setting.LastUpdated = DateTimeOffset.Now;
                _siteSettingRepository.Update(setting);
            }
            _siteSettingRepository.SaveChanges();
        }

        public string GetSettingKey(Type type)
        {
            return SiteSetting.GetSettingKey(type);
        }
    }
}