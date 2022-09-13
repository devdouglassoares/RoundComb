using System;

namespace Membership.Core.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException() : base("The requested Role could not be found")
        {

        }
    }
}