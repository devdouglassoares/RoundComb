using Core.Exceptions;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Core.WebApi.ActionFilters;
using ProductManagement.Api.Hubs;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using ProductManagement.Core.SignalR;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Contracts.AuthAttributes;
using Membership.Core.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ProductManagement.Api.Controllers
{
	[ErrorResponseHandler(typeof(BaseNotFoundException<>), HttpStatusCode.NotFound)]
	[RoutePrefix("api/contactMessage")]
	public class PropertyContactMessageController : ApiController
	{
		private readonly IPropertyContactMessageService _contactMessageService;
		private readonly IDataTableService _dataTableService;
		private readonly IMembership _membership;
		private readonly IUserService _userService;

		public PropertyContactMessageController(IPropertyContactMessageService contactMessageService,
											   IMembership membership,
											   IDataTableService dataTableService,
											   IUserService userService)
		{
			_contactMessageService = contactMessageService;
			_membership = membership;
			_dataTableService = dataTableService;
			_userService = userService;
		}

		[HttpPost, Route("~/api/properties/{propertyId:long}/contactMessages")]
		[RequireAuthTokenApi]
		public HttpResponseMessage GetMessagesForProperty(long propertyId,
														 [ModelBinder(typeof(DataTableModelBinderProvider))] DefaultDataTablesRequest requestModel)
		{
			var contactMessages = _contactMessageService.Fetch(x => x.PropertyId == propertyId && x.ReplyToMessage == null)
														.OrderBy(x => x.SentDate);

			var dto = _contactMessageService.ToQueryableDtos(contactMessages);

			var response = _dataTableService.GetResponse(dto, requestModel);
			return Request.CreateResponse(HttpStatusCode.OK, response);
		}

		[HttpGet, Route("myConversation")]
		[RequireAuthTokenApi]
		public HttpResponseMessage GetMessagesSentToCurrentUser()
		{
			var contactMessages = _contactMessageService.Fetch(
															   x =>
															   x.DestinationUserId == _membership.UserId ||
															   x.OwnerId == _membership.UserId)
														.GroupBy(x => x.ThreadGuid)
														.Select(x => x.OrderByDescending(y => y.SentDate).FirstOrDefault())
														.Select(x => new PropertyContactMessageDto
														{
															OwnerId = x.OwnerId,
															DestinationUserId = x.DestinationUserId,
															ThreadGuid = x.ThreadGuid,
															Id = x.Id,
															IsViewed = x.IsViewed,
															Message = x.Message,
															SentDate = x.SentDate,
															ViewedDate = x.ViewedDate,
															ReplyToMessageId = x.ReplyToMessageId,
															Title = x.Title,
															IsMyMessage = x.OwnerId == _membership.UserId
														})
														.ToArray();

			foreach (var message in contactMessages)
			{
				try
				{
					var userInfo = _userService.GetUserPersonalInformation(message.OwnerId);
					message.SenderName = $"{userInfo.FirstName} {userInfo.LastName}";

					long? participantId = message.IsMyMessage ? message.DestinationUserId : message.OwnerId;

					if (!participantId.HasValue)
						continue;

					var participantInfo = _userService.GetUserPersonalInformation(participantId.Value);
					message.ParticipantName = $"{participantInfo.FirstName} {participantInfo.LastName}";
				}
				catch (BaseNotFoundException<User> exception)
				{
					// no need to fulfill data.
				}
			}

			return Request.CreateResponse(HttpStatusCode.OK, contactMessages);
		}

		[HttpGet, Route("getThread/{messageId:long}")]
		[RequireAuthTokenApi]
		public HttpResponseMessage GetMessageThreadFromMessage(long messageId)
		{
			var message = _contactMessageService.GetEntity(messageId);
			if (message == null ||
				(message.DestinationUserId != _membership.UserId && message.OwnerId != _membership.UserId))
				throw new BaseNotFoundException<PropertyContactMessage>();

			var messages = _contactMessageService.Fetch(x => x.ThreadGuid == message.ThreadGuid);

			var propertyContactMessageDtos = _contactMessageService.ToDtos(messages)
																  .OrderBy(x => x.SentDate)
																  .ToArray();

			return Request.CreateResponse(HttpStatusCode.OK, propertyContactMessageDtos);
		}

		[HttpGet, Route("getThread/{threadId}")]
		[RequireAuthTokenApi]
		public HttpResponseMessage GetMessageThread(string threadId)
		{
			var threadGuid = Guid.Parse(threadId);
			
			var messages = _contactMessageService.Fetch(x => x.ThreadGuid ==threadGuid);

			var propertyContactMessageDtos = _contactMessageService.ToDtos(messages)
																  .OrderBy(x => x.SentDate)
																  .ToArray();

			return Request.CreateResponse(HttpStatusCode.OK, propertyContactMessageDtos);
		}

		[HttpPost, Route("")]
		[RequireAuthTokenApi]
		public HttpResponseMessage SubmitMessage(PropertyContactMessageDto model)
		{
			/*if (model.ProductId == null && model.ReplyToMessageId == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Please either select product to leave contact message, or specify message to reply");*/

			model.OwnerId = _membership.UserId;
			PropertyContactMessage message = null;

			if (model.PropertyId != null)
			{
				message = _contactMessageService.SubmitContactMessageToProperty(model.PropertyId.Value, model);
			}
			else if (model.ReplyToMessageId != null)
			{
				message = _contactMessageService.ReplyToMessage(model.ReplyToMessageId.Value, model);
			}
			else if (model.DestinationUserId != null)
			{
				message = _contactMessageService.ContactMessageToUser(model.DestinationUserId.Value, model);
			}

			if (message == null)
				return Request.CreateResponse(HttpStatusCode.InternalServerError, "Message cannot be sent");

			SendMessageToClient(message);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		private void SendMessageToClient(PropertyContactMessage message)
		{
			var messageDto = _contactMessageService.ToDto(message);
			var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
			hubContext.BroadcastToClient(messageDto.DestinationUserId.ToString(), o => o.OnMessageReceived(messageDto));
		}
	}
}