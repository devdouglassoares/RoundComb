using Core.Events;
using Core.Logging;
using Core.SiteSettings;
using Core.Templating.Services;
using ProductManagement.Core.EmailTemplates;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using Membership.Core.Contracts;
using NotifyService.RestClient.Services;
using Roundcomb.Core.Entities;
using Roundcomb.Core.Services;
using Roundcomb.Services.EmailTemplates;
using Roundcomb.Services.Events;
using System.Linq;

namespace Roundcomb.Services.EventHandlers
{
    public class PropertyApplicationEventsHandler : IConsumer<PropertyApplicationSubmitted>,
                                                   IConsumer<PropertyApplicationApproved>,
                                                   IConsumer<PropertyApplicationUserAccepted>,
                                                   IConsumer<PropertyApplicationUserDeclined>,
                                                   IConsumer<PropertyApplicationRejected>

    {
        private const string DateShortFormat = "MM/dd/yyyy";
        private readonly ILogger _logger = Logger.GetLogger<PropertyApplicationEventsHandler>();
        public int Order => 10;

        private readonly IRepository _repository;
        private readonly IUserService _userService;
        private readonly ITemplateService _templateService;

        private readonly INotificationService _notificationService;
        private readonly IPropertyCustomerConsumingMappingService _propertyCustomerConsumingMappingService;
        private readonly IPropertyService _propertyService;
        private readonly ISiteSettingService _siteSettingService;

        public PropertyApplicationEventsHandler(IRepository repository, IUserService userService,
                                               ITemplateService templateService,
                                               INotificationService notificationService,
                                               IPropertyCustomerConsumingMappingService propertyCustomerConsumingMappingService,
                                               IPropertyService propertyService, ISiteSettingService siteSettingService)
        {
            _repository = repository;
            _userService = userService;
            _templateService = templateService;
            _notificationService = notificationService;
            _propertyCustomerConsumingMappingService = propertyCustomerConsumingMappingService;
            _propertyService = propertyService;
            _siteSettingService = siteSettingService;
        }

        public void HandleEvent(PropertyApplicationSubmitted eventMessage)
        {
            SendApplicationSubmittedNotificationToUser(eventMessage.ApplicationForm);
        }

        public void HandleEvent(PropertyApplicationApproved eventMessage)
        {
            var property = _repository.GetById<Property>(eventMessage.ApplicationForm.PropertyId);

            if (property == null)
                return;

            SendApplicationApprovedNotificationToUser(eventMessage.ApplicationForm);
        }

        public void HandleEvent(PropertyApplicationUserAccepted eventMessage)
        {
            var property = _repository.GetById<Property>(eventMessage.ApplicationForm.PropertyId);

            if (property == null)
                return;

            property.Status = PropertyStatus.NotAvailableOrTaken;

            _repository.Update(property);

            _repository.SaveChanges();

            if (property.PropertySellType != null)
                _propertyCustomerConsumingMappingService.CreateConsumingMappingForProperty(eventMessage.ApplicationForm,
                                                                                         property.PropertySellType.Value);

            SendApplicationAcceptedNotificationToOwner(eventMessage.ApplicationForm, property);
        }

        public void HandleEvent(PropertyApplicationUserDeclined eventMessage)
        {
            var property = _repository.GetById<Property>(eventMessage.ApplicationForm.PropertyId);

            if (property == null)
                return;

            SendApplicationDeclinedNotificationToOwner(eventMessage.ApplicationForm, property);
        }

        public void HandleEvent(PropertyApplicationRejected eventMessage)
        {
            SendApplicationRejectedNotificationToUser(eventMessage.ApplicationForm);
        }

        private void SendApplicationSubmittedNotificationToUser(PropertyApplicationFormInstance applicationForm)
        {
            var property = _propertyService.GetEntity(applicationForm.PropertyId);

            var propertyOwnerUser = _userService.GetUser(property.OwnerId.Value);

            if (propertyOwnerUser == null)
            {
                _logger.Error("User could not be found, cannot send out notification");
                return;
            }

            var applicationUser = _userService.GetUser(applicationForm.UserId);

            if (applicationUser == null)
            {
                _logger.Error("User could not be found, cannot send out notification");
                return;
            }

            var model = new PropertyApplicationFormSubmittedTemplate
            {
                PropertyName = property.Name,
                SubmittedDate = applicationForm.CreatedDate?.ToString(DateShortFormat),
                Comment = applicationForm.RejectReason,
                PropertyOwnerName = propertyOwnerUser.FirstName + ' ' + propertyOwnerUser.LastName,
                ApplicantName = applicationUser.FirstName + ' ' + applicationUser.LastName
            };

            var template = _templateService.ParseTemplate(model);

            SendEmailToUser(propertyOwnerUser.Email, template.TemplateTitle, template.TemplateContent);
        }

        private void SendApplicationRejectedNotificationToUser(PropertyApplicationFormInstance applicationForm)
        {
            var property = _propertyService.GetEntity(applicationForm.PropertyId);
            var user = _userService.GetUser(applicationForm.UserId);

            if (user == null)
            {
                _logger.Error("User could not be found, cannot send out notification");
                return;
            }

            var model = new PropertyApplicationFormRejectedTemplate
            {
                PropertyName = property.Name,
                RejectedDate = applicationForm.RejectedDate?.ToString(DateShortFormat),
                Comment = applicationForm.RejectReason,
                UserName = user.FirstName + ' ' + user.LastName
            };
            var template = _templateService.ParseTemplate(model);

            SendEmailToUser(user.Email, template.TemplateTitle, template.TemplateContent);
        }

        private void SendApplicationApprovedNotificationToUser(PropertyApplicationFormInstance applicationForm)
        {
            var property = _propertyService.GetEntity(applicationForm.PropertyId);
            var user = _userService.GetUser(applicationForm.UserId);

            if (user == null)
            {
                _logger.Error("User could not be found, cannot send out notification");
                return;
            }

            var model = new PropertyApplicationFormApprovedTemplate
            {
                PropertyName = property.Name,
                ApproveDate = applicationForm.ApprovedDate?.ToString(DateShortFormat),
                UserName = user.FirstName + ' ' + user.LastName
            };
            var template = _templateService.ParseTemplate(model);

            SendEmailToUser(user.Email, template.TemplateTitle, template.TemplateContent);
        }

        private void SendApplicationAcceptedNotificationToOwner(PropertyApplicationFormInstance applicationForm, Property property)
        {
            var user = _userService.GetUser(property.OwnerId.Value);
            var client = _userService.GetUser(applicationForm.UserId);

            if (user == null || client == null)
            {
                _logger.Error("User could not be found, cannot send out notification");
                return;
            }

            var model = new PropertyApplicationUserAcceptedNotificationTemplate
            {
                PropertyName = property.Name,
                AcceptedDate = applicationForm.UserAcceptedDate?.ToString(DateShortFormat),
                ClientName = client.FirstName + ' ' + client.LastName,
                OwnerName = user.FirstName + ' ' + user.LastName,
            };
            var template = _templateService.ParseTemplate(model);

            SendEmailToUser(user.Email, template.TemplateTitle, template.TemplateContent);
        }

        private void SendApplicationDeclinedNotificationToOwner(PropertyApplicationFormInstance applicationForm, Property property)
        {
            var user = _userService.GetUser(property.OwnerId.Value);
            var client = _userService.GetUser(applicationForm.UserId);

            if (user == null || client == null)
            {
                _logger.Error("User could not be found, cannot send out notification");
                return;
            }

            var model = new PropertyApplicationUserDeclinedNotificationTemplate
            {
                PropertyName = property.Name,
                DeclinedDate = applicationForm.UserDeclinedDate?.ToString(DateShortFormat),
                ClientName = client.FirstName + ' ' + client.LastName,
                OwnerName = user.FirstName + ' ' + user.LastName,
                DeclineReason = applicationForm.DeclinedReason
            };
            var template = _templateService.ParseTemplate(model);

            SendEmailToUser(user.Email, template.TemplateTitle, template.TemplateContent);
        }

        private void SendEmailToUser(string email, string subject, string content)
        {
            var generalSetting = _siteSettingService.GetSetting<GeneralSiteSetting>();
            var adminEmail = generalSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);
            if (adminEmail == null)
            {
                return;
            }
            _notificationService.SendEmail(new[] { email }, subject, content, adminEmail.EmailAddress,
                                           adminEmail.DisplayName);
        }
    }
}