using Membership.Core.Contracts;
using Membership.Core.Models;
using System.Collections.Generic;

namespace ProductManagement.Core.Permissions
{
    public class PropertyManagementPermissions
    {
        public const string AccessManageCategories = "/propertyManagement/manageCategories";

        public const string AccessManageProperty = "/propertyManagement/manageProperties";

        public const string CanImportProperties = "ProductManagement.Permissions.ImportProducts";

        public const string CanUpdatePropertyStatus = "ProductManagement.Permissions.CanUpdateProductStatus";

        public const string CanCreateFeaturedProperties = "ProductManagement.Permissions.CanCreateFeaturedProducts";

        public const string CanManageSystemForms = "ProductManagement.Permissions.CanManageSystemForms";

        public const string CanManageAllUsersProperties = "ProductManagement.Permissions.CanManageAllUsersProducts";

        public const string CanContactPropertyOwner = "ProductManagement.Permissions.CanContactProductOwner";

        public const string CanManageVendors = "ProductManagement.Permissions.CanManageVendors";

        public const string CanExportProperties = "ProductManagement.Permissions.CanExportProducts";

        public const string CanPrintProperties = "ProductManagement.Permissions.CanPrintProducts";

        public const string UndoDeletionOfProperties = "ProductManagement.Permissions.UndoDeletionOfProducts";
    }

    public class PropertyManagementPermissionProvider : IPermissionProvider
    {
        public const string PropertyManagementModuleName = "ProductManagement";

        public static PermissionRegistrationModel AccessManageCategories =
            PermissionRegistrationModel.Page(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Manage Property Categories]",
                PropertyManagementPermissions.AccessManageCategories);

        public static PermissionRegistrationModel AccessManagementProperties =
            PermissionRegistrationModel.Page(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Manage Properties]",
                PropertyManagementPermissions.AccessManageProperty);



        public static PermissionRegistrationModel CanUpdatePropertyStatus =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Manage Property][Can Change Property's Status]",
                PropertyManagementPermissions.CanUpdatePropertyStatus);

        public static PermissionRegistrationModel CanCreateFeaturedProperties =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Create Featured Properties]",
                PropertyManagementPermissions.CanCreateFeaturedProperties);

        public static PermissionRegistrationModel CanManageSystemForms =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "Can manage system forms",
                PropertyManagementPermissions.CanManageSystemForms);

        public static PermissionRegistrationModel CanManageAllUsersProperties =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Manage Other Users' Properties]",
                PropertyManagementPermissions.CanManageAllUsersProperties);

        public static PermissionRegistrationModel CanContactPropertyOwner =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "Can contact property's owner",
                PropertyManagementPermissions.CanContactPropertyOwner);

        public static PermissionRegistrationModel CanImportProperties =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Import Properties]",
                PropertyManagementPermissions.CanImportProperties);

        public static PermissionRegistrationModel CanManageVendors =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Vendors Management]",
                PropertyManagementPermissions.CanManageVendors);

        public static PermissionRegistrationModel CanExportProperties =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Export Property]",
                PropertyManagementPermissions.CanExportProperties);

        public static PermissionRegistrationModel CanPrintProperties =
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Print Properties]",
                PropertyManagementPermissions.CanPrintProperties);

        public static PermissionRegistrationModel UndoDeletionOfProperties = 
            PermissionRegistrationModel.Feature(PropertyManagementModuleName,
                "[PropertyManagement][Feature-Undo Deletion of Properties]",
                PropertyManagementPermissions.UndoDeletionOfProperties);

        public IEnumerable<PermissionRegistrationModel> Register()
        {
            return new[]
                   {
                       AccessManageCategories,

                       AccessManagementProperties,

                       CanUpdatePropertyStatus,

                       CanCreateFeaturedProperties,

                       CanManageSystemForms,

                       CanManageAllUsersProperties,

                       CanContactPropertyOwner,

                       CanImportProperties,

                       CanManageVendors,

                       CanExportProperties,
                       
                       CanPrintProperties,

                       UndoDeletionOfProperties
                   };
        }
    }
}