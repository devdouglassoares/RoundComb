using Core.Events;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using ProductManagement.Core.SignalR;
using ProductManagement.Core.SignalRHubClientInterfaces;
using ProductManagement.Services;
using System;

namespace ProductManagement.Api.Hubs
{
    public class ChatHub : AuthenticationEnabledHub<IContactMessageClientService>
    {
        private readonly IPropertyContactMessageService _contactMessageService;
        private readonly IEventPublisher _eventPublisher;

        public ChatHub(IPropertyContactMessageService contactMessageService, IEventPublisher eventPublisher)
        {
            _contactMessageService = contactMessageService;
            _eventPublisher = eventPublisher;
        }

        public void MarkMessageAsViewed(long messageId)
        {
            _contactMessageService.MarkMessageAsViewed(messageId);

            var messageDto = _contactMessageService.GetDto(messageId);
            messageDto.IsMyMessage = false;
            this.BroadcastToClient(messageDto.DestinationUserId.ToString(), o => o.OnMessageReceived(messageDto));
        }

        public long SubmitMessage(PropertyContactMessageDto model)
        {
            if (model.PropertyId == null && model.ReplyToMessageId == null)
                throw new InvalidOperationException("Please either select property to leave contact message, or specify message to reply");

            model.OwnerId = GetUserIdInLong(Context.Request);
            PropertyContactMessage message = null;

            if (model.PropertyId != null)
            {
                message = _contactMessageService.SubmitContactMessageToProperty(model.PropertyId.Value, model);
            }
            else if (model.ReplyToMessageId != null)
            {
                message = _contactMessageService.ReplyToMessage(model.ReplyToMessageId.Value, model);
            }


            if (message == null)
                throw new InvalidOperationException("Message cannot be sent");

            var broadcastedToDestination = SendMessageToClients(message);

            //if (!broadcastedToDestination && model.ReplyToMessageId != null)
            _eventPublisher.Publish(new PropertyContactMessageReplied { PropertyContactMessage = message });

            return message.Id;
        }

        private bool SendMessageToClients(PropertyContactMessage message)
        {
            var messageDto = _contactMessageService.ToDto(message);

            messageDto.IsMyMessage = true;
            this.BroadcastToClient(messageDto.OwnerId.ToString(), o => o.OnMessageReceived(messageDto));

            messageDto.IsMyMessage = false;
            return this.BroadcastToClient(messageDto.DestinationUserId.ToString(), o => o.OnMessageReceived(messageDto));

        }
    }
}