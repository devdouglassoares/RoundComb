using System;

namespace Membership.Core.Exceptions
{
    public class UserProfileNotFoundException : Exception
    {
        public UserProfileNotFoundException() : base("Requested User Profile could not be found")
        {

        }

        public UserProfileNotFoundException(string message) : base(message)
        {

        }
    }
}