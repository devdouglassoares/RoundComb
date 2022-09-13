using Membership.Core.Dto;
using Membership.Core.Entities;

namespace Membership.Core.Events
{
    public class OnUserRegisteredEvent
    {
        public User User { get; set; }

        public UserRegistrationModel RegistrationModel { get; set; }
    }
}