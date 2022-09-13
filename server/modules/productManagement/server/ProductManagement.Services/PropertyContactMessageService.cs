using Core.Database;
using Core.Events;
using Core.Exceptions;
using Core.ObjectMapping;
using Core.SiteSettings;
using Core.Templating.Models;
using Core.Templating.Services;
using ProductManagement.Core.Dto;
using ProductManagement.Core.EmailTemplates;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Entities;
//using NotifyService.RestClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace ProductManagement.Services
{
	public class PropertyContactMessageReplied
    {
        public PropertyContactMessage PropertyContactMessage { get; set; }
    }


    public class PropertyContactMessageService : BaseService<PropertyContactMessage, PropertyContactMessageDto>,
                                                IConsumer<PropertyContactMessageReplied>,
                                                IPropertyContactMessageService
    {
        private readonly IPropertyService _propertyService;
        private readonly IMembership _membership;
        private readonly IUserService _userService;
        //private readonly INotificationService _notificationService;
        private readonly ITemplateService _templateService;
        private readonly ISiteSettingService _siteSettingService;

        public PropertyContactMessageService(IMappingService mappingService,
                                            IRepository repository,
                                            IPropertyService propertyService,
                                            IMembership membership,
                                            IUserService userService,
                                           // INotificationService notificationService,
                                            ITemplateService templateService,
                                            ISiteSettingService siteSettingService)
            : base(mappingService, repository)
        {
            _propertyService = propertyService;
            _membership = membership;
            _userService = userService;
           // _notificationService = notificationService;
            _templateService = templateService;
            _siteSettingService = siteSettingService;
        }

        public override IQueryable<PropertyContactMessage> Fetch(Expression<Func<PropertyContactMessage, bool>> expression)
        {
            var inactiveUsers = _userService.QueryUsers()
                                            .Where(user => !user.IsActive || !user.IsApproved || user.IsDeleted)
                                            .Select(x => x.Id)
                                            .ToArray();

            return base.Fetch(x => !inactiveUsers.Contains(x.OwnerId))
                       .Where(expression);
        }

        public override PropertyContactMessage Create(PropertyContactMessageDto model)
        {
            var message = base.Create(model);

            return message;
        }

        public void MarkMessageAsViewed(long messageId)
        {
            Repository.Update<PropertyContactMessage>(x => x.Id == messageId, message => new PropertyContactMessage
            {
                IsViewed = true,
                ViewedDate = DateTimeOffset.Now
            });
            Repository.SaveChanges();
        }

        public override PropertyContactMessageDto ToDto(PropertyContactMessage entity)
        {
            var propertyContactMessageDto = base.ToDto(entity);
            propertyContactMessageDto.ThreadGuid = entity.ThreadGuid;

            try
            {
                var userInfo = _userService.GetUserPersonalInformation(entity.OwnerId);
                propertyContactMessageDto.SenderName = $"{userInfo.FirstName} {userInfo.LastName}";
            }
            catch (BaseNotFoundException<User> exception)
            {
                return null;
            }

            try
            {
                if (propertyContactMessageDto.OwnerId == _membership.UserId)
                    propertyContactMessageDto.IsMyMessage = true;
            }
            catch
            {
            }

            return propertyContactMessageDto;
        }

        public override IEnumerable<PropertyContactMessageDto> ToDtos(IEnumerable<PropertyContactMessage> entities, bool wireup = true)
        {
            var result = base.ToDtos(entities);

            var propertyContactMessageDtos = result as PropertyContactMessageDto[] ?? result.ToArray();

            foreach (var message in propertyContactMessageDtos)
            {
                try
                {
                    var userInfo = _userService.GetUserPersonalInformation(message.OwnerId);
                    message.SenderName = $"{userInfo.FirstName} {userInfo.LastName}";
                }
                catch (BaseNotFoundException<User> exception)
                {

                }

                if (message.OwnerId == _membership.UserId)
                    message.IsMyMessage = true;
            }

            return propertyContactMessageDtos;
        }

        public void MarkMessageAsReplied(long messageId)
        {
            Repository.Update<PropertyContactMessage>(x => x.Id == messageId,
                                                     message => new PropertyContactMessage { RepliedDate = DateTimeOffset.Now });
            Repository.SaveChanges();
        }

        public PropertyContactMessage SubmitContactMessageToProperty(long propertyId, PropertyContactMessageDto model)
        {
            var propertyDto = _propertyService.GetDto(propertyId);
            if (propertyDto == null)
                throw new BaseNotFoundException<Property>();

            model.PropertyId = propertyId;
            model.DestinationUserId = propertyDto.OwnerId;
            model.ThreadGuid = Guid.NewGuid();

            var propertyContactMessage = Create(model);

            SendContactMessageNotificationToOwner(propertyContactMessage);
            return propertyContactMessage;
        }

		public PropertyContactMessage ContactMessageToUser(long userId, PropertyContactMessageDto model)
		{
			var existingThread = Repository.First<PropertyContactMessage>(
				x => x.DestinationUserId == userId && x.OwnerId == model.OwnerId ||
				     x.DestinationUserId == model.OwnerId && x.OwnerId == userId);

			model.DestinationUserId = userId;
			model.ThreadGuid = existingThread?.ThreadGuid ?? Guid.NewGuid();

			var propertyContactMessage = Create(model);

			SendContactMessageNotificationToOwner(propertyContactMessage);
			return propertyContactMessage;
		}

        public PropertyContactMessage ReplyToMessage(long replyingMessageId, PropertyContactMessageDto model)
        {
            var messageToReply = GetEntity(replyingMessageId);
            if (messageToReply == null)
                throw new BaseNotFoundException<PropertyContactMessage>();

            messageToReply.RepliedDate = DateTimeOffset.Now;
            Repository.Update(messageToReply);

            model.ReplyToMessageId = replyingMessageId;
            model.DestinationUserId = messageToReply.OwnerId == model.OwnerId
                                          ? messageToReply.DestinationUserId
                                          : messageToReply.OwnerId;

            model.ThreadGuid = messageToReply.ThreadGuid;
            model.PropertyId = messageToReply.PropertyId;

            var propertyContactMessage = Create(model);
            return propertyContactMessage;
        }

        public override PropertyContactMessage PrepareForInserting(PropertyContactMessage entity,
                                                                  PropertyContactMessageDto model)
        {
            entity = base.PrepareForInserting(entity, model);
            entity.ThreadGuid = model.ThreadGuid;
            entity.SentDate = DateTimeOffset.Now;

            return entity;
        }

        private void SendContactMessageNotificationToOwner(PropertyContactMessage propertyContactMessage)
        {
            var ownerUser = _userService.GetUserPersonalInformation(propertyContactMessage.DestinationUserId.Value);
            var email = ownerUser.Email;

			BaseTemplateModel notification = null;
			if (propertyContactMessage.PropertyId.HasValue)
			{
				notification = new PropertyContactMessageNotification
				               {
					               ThreadGuid = propertyContactMessage.ThreadGuid.ToString(),
					               SenderName = _membership.Name,
					               OwnerName = $"{ownerUser.FirstName} {ownerUser.LastName}",
					               SentDate = propertyContactMessage.SentDate,
					               PropertyName = propertyContactMessage.Property?.Name ?? "",
					               PropertyId = propertyContactMessage.PropertyId.Value,
					               Message = propertyContactMessage.Message
				               };
			}
			else
			{
				notification = new DirectContactMessageNotification
				               {
					               ThreadGuid = propertyContactMessage.ThreadGuid.ToString(),
					               SenderName = _membership.Name,
					               OwnerName = $"{ownerUser.FirstName} {ownerUser.LastName}",
					               SentDate = propertyContactMessage.SentDate,
					               Message = propertyContactMessage.Message
				               };
			}

			var template = propertyContactMessage.PropertyId.HasValue
				               ? _templateService.LoadTemplate<PropertyContactMessageNotification>()
				               : _templateService.LoadTemplate<DirectContactMessageNotification>();

            var emailBody = _templateService.ParseTemplate(template.TemplateContent, notification);
            var emailSubject = _templateService.ParseTemplate(template.TemplateTitle, notification);

            var emailBodyDecoded = string.IsNullOrWhiteSpace(emailBody) ? string.Empty : WebUtility.HtmlDecode(emailBody);
            SendEmail(email, emailSubject, emailBodyDecoded);
        }

        private void SendReplyMessageNotification(PropertyContactMessage propertyContactMessage)
        {
            var ownerUser = _userService.GetUserPersonalInformation(propertyContactMessage.DestinationUserId.Value);
            var email = ownerUser.Email;
            var contactMessageNotification = new ContactMessageRepliedNotification
            {
				ThreadGuid = propertyContactMessage.ThreadGuid.ToString(),
                SenderName = _membership.Name,
                RecipientName = ownerUser.FirstName + " " + ownerUser.LastName,
                SentDate = propertyContactMessage.SentDate,
                Message = propertyContactMessage.Message
            };

            var template = _templateService.LoadTemplate<ContactMessageRepliedNotification>();

            var emailBody = _templateService.ParseTemplate(template.TemplateContent, contactMessageNotification);
            var emailSubject = _templateService.ParseTemplate(template.TemplateTitle, contactMessageNotification);

            var emailBodyDecoded = string.IsNullOrWhiteSpace(emailBody) ? string.Empty : WebUtility.HtmlDecode(emailBody);
            SendEmail(email, emailSubject, emailBodyDecoded);
        }

        private void SendEmail(string email, string emailSubject, string emailBody)
        {
            var generalSetting = _siteSettingService.GetSetting<GeneralSiteSetting>();
            var adminEmail = generalSetting.AdminContactEmails.FirstOrDefault(x => x.IsDefaultToSendNotification);
            if (adminEmail == null)
            {
                return;
            }

           // _notificationService.SendEmail(new[] { email }, emailSubject, emailBody, adminEmail.EmailAddress, adminEmail.DisplayName);
        }

        public int Order => 10;

        public void HandleEvent(PropertyContactMessageReplied eventMessage)
        {
            SendReplyMessageNotification(eventMessage.PropertyContactMessage);
        }
    }
}