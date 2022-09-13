using Core.Logging;
using System;
using System.Net;

namespace Core.BackgroundTasks
{
    public interface IBackgroundTask : IDependency
    {
        void Execute();
    }

    public class KeepSiteAliveBackgroundTask : IBackgroundTask
    {
        public static string SiteUrl = "";

        public static DateTimeOffset? LastRun = null;

        private readonly int _defaultInterval = 5;

        private readonly ILogger _logger = Logger.GetLogger<KeepSiteAliveBackgroundTask>();

        public void Execute()
        {
            if (string.IsNullOrEmpty(SiteUrl))
                return;

            if (LastRun.HasValue && LastRun.Value > DateTimeOffset.Now.AddMinutes(-_defaultInterval))
            {
                return;
            }

            var wr = new WebClient();

            try
            {
                var rootSite = new Uri(new Uri(SiteUrl), "/");

                try
                {
                    wr.DownloadString(rootSite);
                }
                catch (Exception)
                {

                }

                try
                {
                    wr.DownloadString(SiteUrl);
                }
                catch (Exception)
                {
                }
                LastRun = DateTimeOffset.Now;
            }
            catch (Exception exception)
            {
            }

        }
    }
}