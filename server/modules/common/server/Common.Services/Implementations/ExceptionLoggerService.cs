using Common.Core.Entities;
using Common.Core.Repositories;
using Common.Services.Interfaces;
using Core.Database;
using Core.Logging;
using Core.ObjectMapping;
using Core.SiteSettings;
using NotifyService.RestClient.Services;
using System;
using System.Configuration;
using System.Linq;

namespace Common.Services.Implementations
{
    public class ExceptionLoggerService : BaseService<ExceptionLogger, ExceptionLoggerDto>, IExceptionLoggerService
    {
        private readonly bool _emailNotificationEnabled;
        private readonly string[] _targetEmails;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly INotificationService _notificationService;
        private readonly ISiteSettingService _siteSettingService;
        private readonly ILogger _logger = global::Core.Logging.Logger.GetLogger<ExceptionLoggerService>();

        public ExceptionLoggerService(IMappingService mappingService, IRepository repository, INotificationService notificationService, ISiteSettingService siteSettingService) : base(mappingService, repository)
        {
            _notificationService = notificationService;
            _siteSettingService = siteSettingService;
            _emailNotificationEnabled =
                ConfigurationManager.AppSettings["EnableEmailNotificationWhenExceptionOccurs"] != null &&
                ConfigurationManager.AppSettings["EnableEmailNotificationWhenExceptionOccurs"] == "true";

            _targetEmails =
                (ConfigurationManager.AppSettings["RecipientEmails"] ?? "").Split(new[] { ",", ";" },
                                                                                  StringSplitOptions.RemoveEmptyEntries);
            _senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            _senderName = ConfigurationManager.AppSettings["SenderName"];
        }

        public void LogExceptions(ExceptionLoggerDto[] exceptions)
        {
            var generalSettings = _siteSettingService.GetSetting<GeneralSiteSetting>();

            var emailHtmlStr = "";

            foreach (var exceptionLoggerDto in exceptions)
            {
                emailHtmlStr += $"<h4>Error {exceptionLoggerDto.ExceptionType} on {exceptionLoggerDto.DateTime}: {exceptionLoggerDto.Message}</h4>" +
                                $"<p>{exceptionLoggerDto.Data} </p>" +
                                $"<hr />";
            }

            bool notified = false;
            try
            {
                _notificationService.SendEmail(_targetEmails.ToList(),
                                               $"[{generalSettings?.SiteName ?? "Roundcomb"}] Error catched on {exceptions.LastOrDefault()?.DateTime}", emailHtmlStr,
                                               _senderEmail, _senderName);
                notified = true;
            }
            catch (Exception exception)
            {
                notified = false;
                _logger.Error("Cannot send notification", exception);
            }

            var notifiedDate = DateTimeOffset.Now;

            foreach (var x in exceptions)
            {
                if (notified)
                {
                    x.Notified = notifiedDate;
                    x.NotifiedTo = string.Join("; ", _targetEmails);
                }
                Create(x);
            }
        }
    }
}