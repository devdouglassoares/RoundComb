namespace Membership.Core.Models
{
    public enum PermisionType
    {
        Page,
        Feature
    }

    public class PermissionRegistrationModel
    {
        public string ModuleName { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string ErrorMessage { get; set; }

        public PermisionType PermisionType { get; set; }

        public static PermissionRegistrationModel Page(string moduleName, string permissionDescription,
                                                       string permissionKey, string errorMessage = "")
        {
            return new PermissionRegistrationModel
            {
                PermisionType = PermisionType.Page,
                Name = permissionDescription,
                ModuleName = moduleName,
                Value = permissionKey,
                ErrorMessage = errorMessage
            };
        }

        public static PermissionRegistrationModel Feature(string moduleName, string permissionDescription,
                                                       string permissionKey, string errorMessage = "")
        {
            return new PermissionRegistrationModel
            {
                PermisionType = PermisionType.Feature,
                Name = permissionDescription,
                ModuleName = moduleName,
                Value = permissionKey,
                ErrorMessage = errorMessage
            };
        }
    }
}