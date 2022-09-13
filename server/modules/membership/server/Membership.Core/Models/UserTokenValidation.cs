namespace Membership.Core.Models
{
    public class UserTokenValidation
    {
        public string Email { get; set; }

        public string ValidationCode { get; set; }

        public long UserId { get; set; }
    }
}