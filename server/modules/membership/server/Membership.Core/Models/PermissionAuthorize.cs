namespace Membership.Core.Models
{
    public class PermissionAuthorize
    {
        internal string Value { get; set; }

        internal PermisionType PermisionType { get; set; }

        public static PermissionAuthorize Page(string pageUrl)
        {
            return new PermissionAuthorize
                   {
                       PermisionType = PermisionType.Page,
                       Value = pageUrl
                   };
        }

        public static PermissionAuthorize Feature(string featureCode)
        {
            return new PermissionAuthorize
                   {
                       PermisionType = PermisionType.Feature,
                       Value = featureCode
                   };
        }
    }
}