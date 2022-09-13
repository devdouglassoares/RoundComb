using Membership.Core.Contracts;
using Membership.Core.Models;
using Membership.Core.Permissions;
using System.Collections.Generic;

namespace Membership.Library.PermissionProviders
{
    public class MembershipPermissionsProvider : IPermissionProvider
    {
        public IEnumerable<PermissionRegistrationModel> Register()
        {
            return new[]
                   {
                       // Users

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "Access User's Contact Information",
                                                           MembershipPermissions.AccessUserInformation),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "Reveal own's information to other users",
                                                           MembershipPermissions.RevealOwnedUserInformation),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "Manage users",
                                                           MembershipPermissions.ManagingUsers),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "Manage user groups",
                                                           MembershipPermissions.ManagingUserGroups),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "Modify user's email",
                                                           MembershipPermissions.EditUserEmail),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "Ability to change underneath users' passwords",
                                                           MembershipPermissions.ChangeUserPassword),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "Approve User's registration",
                                                           MembershipPermissions.ApproveUserRegistration),
                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleUsers,
                                                           "[Entity-Users][Featue-User View]",
                                                           MembershipPermissions.UserView),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleUsers,
                                                           "[Entity-Users][Featue-User Edit]",
                                                           MembershipPermissions.UserEdit),

                       // Customers

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleCustomers,
                                                           "[Entity-Customers][Featue-Customer View]",
                                                           MembershipPermissions.CustomerView),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleCustomers,
                                                           "[Entity-Customers][Featue-Customer Edit]",
                                                           MembershipPermissions.CustomerEdit),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCustomers,
                                                        "[Entity-Customer.Contacts][Featue-Contacts View]",
                                                        "Membership.Customers.Contacts.ContactsView"),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleCustomers,
                                                           "[Entity-Customer.Contacts][Featue-Contacts Edit]",
                                                           "Membership.Customers.Contacts.ContactsEdit"),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCustomers,
                                                        "[Entity-Customer.Sites][Featue-Sites View]",
                                                        "Membership.Customers.Sites.SitesView"),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleCustomers,
                                                           "[Entity-Customer.Sites][Featue-Sites Edit]",
                                                           "Membership.Customers.Sites.SitesEdit"),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCustomers,
                                                        "[Entity-Customer.Documents][Featue-Documents View]",
                                                        "Membership.Customers.Documents.DocumentsView"),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleCustomers,
                                                           "[Entity-Customer.Documents][Featue-Documents Edit]",
                                                           "Membership.Customers.Documents.DocumentsEdit"),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCustomers,
                                                        "[Entity-Customer.Vendors][Featue-Vendors View]",
                                                        "Membership.Customers.Vendors.VendorsView"),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleCustomers,
                                                           "[Entity-Customer.Vendors][Featue-Vendors Edit]",
                                                           "Membership.Customers.Vendors.VendorsEdit"),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCustomers,
                                                        "[Entity-Customer.Vendors][Featue-SMS Edit]",
                                                        "Membership.Customers.Vendors.SMSEdit"),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCustomers,
                                                        "[Entity-Customer.Vendors][Featue-SMS View]",
                                                        "Membership.Customers.Vendors.SMSView"),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCustomers,
                                                        "[Entity-Customer.Partnerships][Featue-Partnerships View]",
                                                        "Membership.Customers.Partnerships.PartnershipsView"),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModuleCustomers,
                                                           "[Entity-Customer.Partnerships][Featue-Partnerships Edit]",
                                                           "Membership.Customers.Partnerships.PartnershipsEdit"),

                       // Admin

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCompanyAdmin,
                                                        "[Entity-Admin][Feature-Manage User's Roles and Permissions]",
                                                        MembershipPermissions.PageUsers),

                       PermissionRegistrationModel.Page(MembershipPermissions.MembershipModuleCompanyAdmin,
                                                        "[Entity-Admin][Feature-Manage Roles]",
                                                        MembershipPermissions.PageRoles),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "[Entity-Admin][Feature-Activate or deactivate users]",
                                                           MembershipPermissions.ActivateDeactivateUsers),

                       PermissionRegistrationModel.Feature(MembershipPermissions.UndoDeletionOfUser,
                                                            "[Entity-Admin][Feature-Undo deletion of user]",
                                                            MembershipPermissions.UndoDeletionOfUser), 

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "[Entity-Admin][Feature-Impersonate other users]",
                                                           MembershipPermissions.Impersonation),

                       PermissionRegistrationModel.Feature(MembershipPermissions.MembershipModulePermission,
                                                           "[Entity-Templates][Feature-Ability to modify email templates]",
                                                           MembershipPermissions.CanEditTemplates),
                   };
        }
    }
}