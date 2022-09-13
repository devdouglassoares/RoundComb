namespace Membership.Library.Dto
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }

        public string ResetPasswordToken { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordConfirm { get; set; }
    }

    public class ValidateResetPasswordTokenModel
    {
        public string ResetPasswordToken { get; set; }
    }
}