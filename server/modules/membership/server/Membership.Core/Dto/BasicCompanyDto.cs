

namespace Membership.Core.Dto
{
    public class BasicCompanyDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Domain { get; set; }
        public string LogoUrl { get; set; }
        public string Code { get; set; }

        public string Alias { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }


        public UserBaseModel Owner { get; set; }
        public UserBaseModel MainContactUser { get; set; }
    }

    public class CompanyShortDto
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        public string Alias { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
    }
}
