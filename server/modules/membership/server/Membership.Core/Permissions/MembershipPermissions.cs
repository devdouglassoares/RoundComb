namespace Membership.Core.Permissions
{
    public class MembershipPermissions
    {
        public const string MembershipModulePermission = "Membership.Permissions";
        public const string ManagingUsers = "Membership.Permissions.ManageUsers";
        public const string ManagingUserGroups = "Membership.Permissions.ManageUserGroups";
        public const string AccessUserInformation = "Membership.Permissions.AccessUserInformation";
        public const string ApproveUserRegistration = "Membership.Permissions.ApproveUserRegistration";
        public const string RevealOwnedUserInformation = "Membership.Permissions.RevealOwnedUserInformation";
        public const string ActivateDeactivateUsers = "Membership.Permissions.ActivateDeactivateUsers";
        public const string UndoDeletionOfUser = "Membership.Permissions.UndoDeletionOfUser";
        public const string Impersonation = "Membership.Permissions.Impersonation";

        public const string MembershipModuleCustomers = "Membership.Customers";
        public const string CustomerEdit = "Membership.Customers.CustomerEdit";
        public const string CustomerView = "Membership.Customers.CustomerView";

        public const string EditUserEmail = "Membership.Management.EditUserEmail";
        public const string ChangeUserPassword = "Membership.Management.ChangeUserPassword";

        public const string MembershipModuleUsers = "Membership.Users";
        public const string UserEdit = "Membership.Users.UserEdit";
        public const string UserView = "Membership.Users.UserView";

        public const string CanEditTemplates = "TemplateServices.CanEditTemplates";

        public const string MembershipModuleCompanyAdmin = "Membership.CompanyAdmin";
        public const string PageUsers = "/admin/users";
        public const string PageRoles = "/admin/roles";
    }
}