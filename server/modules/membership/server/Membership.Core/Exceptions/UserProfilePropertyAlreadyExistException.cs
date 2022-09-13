using System;

namespace Membership.Core.Exceptions
{
    public class UserProfilePropertyAlreadyExistException : Exception
    {
        public UserProfilePropertyAlreadyExistException() : base("UserProfile properties with specified name already exist. Consider adding role to the existing profile instead of creating new one")
        {

        }

        public UserProfilePropertyAlreadyExistException(string message) : base(message)
        {

        }
    }
}