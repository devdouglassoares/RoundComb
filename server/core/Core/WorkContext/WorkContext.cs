using Core.SiteSettings;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Concurrent;

namespace Core.WorkContext
{
    public class WorkContext : ISelfRegisterDependency
    {
        readonly ConcurrentDictionary<string, object> _stateResolvers = new ConcurrentDictionary<string, object>();

        public WorkContext()
        {
            var siteSettingService = ServiceLocator.Current.GetInstance<ISiteSettingService>();

            var generalSiteSetting = siteSettingService.GetSetting<GeneralSiteSetting>();
            GeneralSiteSetting = generalSiteSetting;
        }

        public GeneralSiteSetting GeneralSiteSetting { get; private set; }

        public T GetState<T>(string name)
        {
            object result;
            if (_stateResolvers.TryGetValue(name, out result))
            {
                return (T)result;
            }

            return default(T);
        }

        public object GetState(string name)
        {
            object result;
            return _stateResolvers.TryGetValue(name, out result) ? result : null;
        }


        public void SetState<T>(string name, T value)
        {
            _stateResolvers[name] = value;
        }
    }
}