using System;

namespace Membership.Core.Exceptions
{
    public class PropertyNotAssignedToRoleException : InvalidOperationException
    {
        public PropertyNotAssignedToRoleException(string propertyName, string name) : base("The property '" + propertyName + "' is not assigned to role named '" + name + "'")
        {
        }
    }
}