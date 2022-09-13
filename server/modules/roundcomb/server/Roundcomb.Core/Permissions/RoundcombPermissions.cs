using Membership.Core.Contracts;
using Membership.Core.Models;
using System.Collections.Generic;

namespace Roundcomb.Core.Permissions
{
    public class RoundcombPermissions
    {
        public const string CanReceiveApplications = "ProductManagement.Permissions.CanReceiveApplications";
    }

    public class RoundcombPermissionsProvider : IPermissionProvider
    {
        public const string PropertyManagementModuleName = "ProductManagement";

        public static PermissionRegistrationModel CanReceiveTenantAppications =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Receive Buyers' Applications]",
                RoundcombPermissions.CanReceiveApplications);

        public IEnumerable<PermissionRegistrationModel> Register()
        {
            return new[]
                   {
                       CanReceiveTenantAppications
                   };
        }
    }
}
