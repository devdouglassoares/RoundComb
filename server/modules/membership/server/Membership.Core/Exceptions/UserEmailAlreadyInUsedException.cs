using System;

namespace Membership.Core.Exceptions
{
    public class UserEmailAlreadyInUsedException : Exception
    {
        public UserEmailAlreadyInUsedException() :
            base(MembershipConstant.C_ERROR_EMAIL_ALREADY_EXIST)
        {

        }

        public UserEmailAlreadyInUsedException(string msg) : base(msg)
        {

        }
    }
    public class UserPhoneNumberAlreadyInUsedException : Exception
    {
        public UserPhoneNumberAlreadyInUsedException() :
            base(MembershipConstant.C_ERROR_PHONE_NUMBER_ALREADY_EXIST)
        {

        }

        public UserPhoneNumberAlreadyInUsedException(string msg) : base(msg)
        {

        }
    }
}