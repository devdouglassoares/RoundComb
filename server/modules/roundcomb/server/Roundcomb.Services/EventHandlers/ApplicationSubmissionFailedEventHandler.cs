using Core.Events;
using Core.Logging;
using Core.SiteSettings;
using Core.Templating.Services;
using ProductManagement.Core.EmailTemplates;
using Membership.Core.Contracts;
using NotifyService.RestClient.Services;
using Roundcomb.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roundcomb.Services.EventHandlers
{
    public class ApplicationSubmissionFailedEventHandler : IConsumer<ApplicationSubmissionValidationFailed>
    {
        private readonly ISiteSettingService _siteSettingService;
        private readonly INotificationService _notificationService;
        private readonly ITemplateService _templateService;
        private readonly IUserService _userService;
        private readonly ILogger _logger = Logger.GetLogger<ApplicationSubmissionFailedEventHandler>();

        public ApplicationSubmissionFailedEventHandler(ISiteSettingService siteSettingService, INotificationService notificationService, ITemplateService templateService, IUserService userService)
        {
            _siteSettingService = siteSettingService;
            _notificationService = notificationService;
            _templateService = templateService;
            _userService = userService;
        }

        public int Order => 20;

        public void HandleEvent(ApplicationSubmissionValidationFailed eventMessage)
        {
            var requestUrl = HttpContext.Current?.Request?.Url ?? new Uri("http://wrongurl.com");

            var generalSetting = _siteSettingService.GetSetting<GeneralSiteSetting>();

            if (!generalSetting.AdminContactEmails.Any() ||
                !generalSetting.AdminContactEmails.Any(x => x.IsDefaultToSendNotification))
                return;

            var adminContactEmail = generalSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);

            var ownerId = eventMessage.Property.OwnerId;

            var model = new ApplicationSubmissionFailedNotificationToPropertyOwner
            {
                Property = eventMessage.Property,
                Recipient = _userService.GetUserPersonalInformation(ownerId.Value),
                BaseUrl = new Uri(requestUrl, "/").ToString()
            };

            var parsedTemplate = _templateService.ParseTemplate(model);

            if (parsedTemplate == null)
            {
                _logger.Error("Cannot load email template to send notification");
                return;
            }

            _notificationService.SendEmail(new List<string> { model.Recipient.Email },
                                           parsedTemplate.TemplateTitle,
                                           parsedTemplate.TemplateContent,
                                           adminContactEmail.EmailAddress,
                                           adminContactEmail.DisplayName);
        }
    }
}