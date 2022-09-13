using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    public class PropertyContactMessage
    {
        [Key]
        public long Id { get; set; }

        public long OwnerId { get; set; }

        public long? DestinationUserId { get; set; }

        public Guid ThreadGuid { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string Message { get; set; }

        public long? PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }
        
        public long? ReplyToMessageId { get; set; }

        [ForeignKey("ReplyToMessageId")]
        public virtual PropertyContactMessage ReplyToMessage { get; set; }

        public DateTimeOffset? SentDate { get; set; }

        public bool IsViewed { get; set; }

        public DateTimeOffset? ViewedDate { get; set; }

        public DateTimeOffset? RepliedDate { get; set; }
    }
}