using System;
using Membership.Core.Entities.Base;
using Newtonsoft.Json;

namespace Membership.Core.Entities
{
    public class SiteSetting: BaseEntity
    {
        public string SettingKey { get; set; }

        public string Value { get; set; }

        public DateTimeOffset? LastUpdated { get; set; }
         
        public string LastUpdatedBy { get; set; }

        public T GetSettingValueAs<T>() where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(Value);
            }
            catch
            {
                return default(T);
            }
        }

        public dynamic GetSettingValueAs(Type type)
        {
            try
            {
                var settingValue = JsonConvert.DeserializeObject(Value, type);

                return settingValue;
            }
            catch
            {
                return Activator.CreateInstance(type);
            }
        }

        public void UpdateSettingValue<T>(T value)
        {
            if (GetSettingKey(typeof(T)) != SettingKey)
                throw new Exception(String.Format(@"Cannot save setting to a different type.
\n\nSetting type: {0}\n\n
Value Type: {1}", SettingKey, GetSettingKey(typeof(T))));


            try
            {
                Value = JsonConvert.SerializeObject(value);
            }
            catch
            {
                throw new Exception("Cannot serialize setting.");
            }
        }

        internal static string GetSettingKey(Type settingType)
        {
            return settingType.FullName;
        }
    }
}
