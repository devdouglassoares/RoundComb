using Core;
using Core.Database;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Services
{
	public interface IPropertyContactMessageService : IBaseService<PropertyContactMessage, PropertyContactMessageDto>,
                                                     IDependency
    {
        void MarkMessageAsViewed(long messageId);

        void MarkMessageAsReplied(long messageId);

        PropertyContactMessage SubmitContactMessageToProperty(long propertyId, PropertyContactMessageDto model);

	    PropertyContactMessage ContactMessageToUser(long userId, PropertyContactMessageDto model);


		PropertyContactMessage ReplyToMessage(long replyingMessageId, PropertyContactMessageDto model);
    }
}