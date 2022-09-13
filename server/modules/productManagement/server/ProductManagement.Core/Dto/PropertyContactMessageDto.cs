using System;

namespace ProductManagement.Core.Dto
{
	public class PropertyContactMessageDto
    {
        public long Id { get; set; }

        public long OwnerId { get; set; }

        public string SenderName { get; set; }

        public long? DestinationUserId { get; set; }

        public Guid ThreadGuid { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public long? PropertyId { get; set; }

        public virtual PropertyDto Property { get; set; }

        public long? ReplyToMessageId { get; set; }

        public virtual PropertyContactMessageDto ReplyToMessage { get; set; }

        public DateTimeOffset? SentDate { get; set; }

        public bool IsViewed { get; set; }

        public DateTimeOffset? ViewedDate { get; set; }

        public DateTimeOffset? RepliedDate { get; set; }

        public bool IsMyMessage { get; set; }

		public string ParticipantName { get; set; }
    }
}