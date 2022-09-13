using System;

namespace Membership.Core.Exceptions
{
    public class CompanyNameInUsedException : Exception
    {
        public CompanyNameInUsedException() :
            base("A Company with the same name already exists")
        {

        }

        public CompanyNameInUsedException(string msg) : base(msg)
        {

        }
    }
}