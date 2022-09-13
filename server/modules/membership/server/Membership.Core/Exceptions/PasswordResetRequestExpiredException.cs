using System;

namespace Membership.Core.Exceptions
{
    public class PasswordResetRequestExpiredException : Exception
    {
        public PasswordResetRequestExpiredException() : base("The reset password request has expired. Please request another reset password.")
        {

        }

        public PasswordResetRequestExpiredException(string message) : base(message)
        {

        }
    }
}