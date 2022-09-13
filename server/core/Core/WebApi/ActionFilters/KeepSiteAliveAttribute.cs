using Core.BackgroundTasks;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Core.WebApi.ActionFilters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class KeepSiteAliveAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (string.IsNullOrEmpty(KeepSiteAliveBackgroundTask.SiteUrl))
            {
                var siteUrl = new Uri(actionContext.Request.RequestUri, actionContext.Request.GetRequestContext().VirtualPathRoot).ToString();
                KeepSiteAliveBackgroundTask.SiteUrl = siteUrl;
            }
            base.OnActionExecuting(actionContext);
        }
    }
}