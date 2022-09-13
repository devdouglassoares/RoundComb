using System;

namespace Membership.Core.Exceptions
{
    public class UserProfilePropertyNotFoundException : Exception
    {
        public UserProfilePropertyNotFoundException() : base("The requested UserProfileProperty could not be found")
        {

        }
    }
}