using System.Collections.Generic;
using Core;
using Membership.Core.Models;

namespace Membership.Core.Contracts
{
    public interface IPermissionProvider : IDependency
    {
        IEnumerable<PermissionRegistrationModel> Register();
    }

    public class DefaultPermissionProvider : IPermissionProvider
    {
        public IEnumerable<PermissionRegistrationModel> Register()
        {
            return new PermissionRegistrationModel[]
                   {
                   };
        }
    }
}