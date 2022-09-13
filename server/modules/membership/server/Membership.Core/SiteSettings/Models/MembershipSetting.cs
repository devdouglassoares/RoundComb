using Core.SiteSettings;

namespace Membership.Core.SiteSettings.Models
{
	public class MembershipSetting : ISiteSettingBase
	{
		public bool AllowNonActivatedUserLogIn { get; set; }

		public bool UserMustActivateRegistration { get; set; }

		public bool NotifyAdministratorForNewRegistrations { get; set; }

		public bool RegistrationApprovalMustBeDoneByAdmin { get; set; }

		public long DefaultRoleToAssignToNewRegistration { get; set; }

		public int ResetPasswordTokenExpiryIn { get; set; }

		public string ResetPasswordPageUrl { get; set; }

		public string UserActivationBaseUrl { get; set; }

		public bool UserMustAgreeToRegistrationConsentText { get; set; }

		public string RegistrationConsentText { get; set; }
		public string UserRegistrationConsentAlertMessage { get; set; }
	}
}