using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Core.SiteSettings
{
    public class BaseSiteSettingController<TSetting> : ApiController where TSetting : ISiteSettingBase
    {
        protected readonly ISiteSettingService SiteSettingService;

        public BaseSiteSettingController(ISiteSettingService siteSettingService)
        {
            SiteSettingService = siteSettingService;
        }
        
        [HttpGet, Route("")]
        public virtual HttpResponseMessage Get()
        {
            var setting = GetSetting();

            return Request.CreateResponse(HttpStatusCode.OK, setting);
        }

        [HttpPost, Route("")]
        public virtual HttpResponseMessage Save(TSetting modelSetting)
        {
            SaveSetting(modelSetting);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        protected virtual void SaveSetting(TSetting value)
        {
            SiteSettingService.SaveSetting(value);
        }

        protected virtual TSetting GetSetting()
        {
            return SiteSettingService.GetSetting<TSetting>();
        }
    }
}