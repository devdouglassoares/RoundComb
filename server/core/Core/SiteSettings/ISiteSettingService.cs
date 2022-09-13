using System;
using System.Collections.Generic;

namespace Core.SiteSettings
{
    public interface ISiteSettingService : IDependency
    {
        IEnumerable<ISiteSettingBase> GetAllAvailableSettings();
        T GetSetting<T>() where T : ISiteSettingBase;
        dynamic GetSetting(Type type);
        void SaveSetting<T>(T value);
        string GetSettingKey(Type type);
    }
}