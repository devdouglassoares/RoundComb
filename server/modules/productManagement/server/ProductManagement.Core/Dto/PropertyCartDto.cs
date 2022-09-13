using Membership.Core.Dto;
using System;
using System.Collections.Generic;

namespace ProductManagement.Core.Dto
{
    public class PropertyCartDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long? DelegateToUserId { get; set; }

        public UserPersonalInformation DelegateToUser { get; set; }

        public bool CheckedOut { get; set; }

        public DateTimeOffset? CheckedOutDate { get; set; }

        public bool Closed { get; set; }

        public List<PropertyCartItemDto> Items { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }
    }
}