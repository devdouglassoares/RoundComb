using Core.ObjectMapping;
using Membership.Core.Dto;
using Membership.Core.Entities;
using System;
using System.Linq;

namespace Membership.Core.MappingRegistrations
{
    public class MembershipCoreMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {

            map.ConfigureMapping<Group, GroupModel>();
            map.ConfigureMapping<GroupModel, Group>()
               .ForMember(r => r.Users, m => m.Ignore());


            map.ConfigureMapping<User, UserPersonalInformation>()
               .ForMember(m => m.Roles, a => a.MapFrom(u => u.Roles.Select(r => r.Name).ToList()))
               .ForMember(m => m.RoleCodes, a => a.MapFrom(u => u.Roles.Select(r => r.Code).ToList()));

            map.ConfigureMapping<UserPersonalInformation, User>()
               .ForMember(m => m.Roles, a => a.Ignore())
               .ForMember(m => m.ClientCompanyId, a => a.Ignore());

            map.ConfigureMapping<User, UserBaseModel>()
               .ForMember(m => m.BizOwnerId, a => a.MapFrom(u => u.Company == null ? (long?)null : u.Company.Id))
               //.ForMember(m => m.Roles, a => a.MapFrom(u => u.Roles.Select(r => r.Description).ToList()))
               .ForMember(m => m.Roles, a => a.MapFrom(u => u.Roles.Select(r => r.Name).ToList()))
               .ForMember(m => m.CompanyName, a => a.MapFrom(u => u.ClientCompany != null ? u.ClientCompany.Name : ""))

               //.ForMember(m => m.RolesString, a => a.MapFrom(u => u.Roles/*.Select(r => r.Description).ToList()*/.Aggregate("", (user, role) => (user == "" ? "" : user + ",") + role.Description)))
               .ForMember(m => m.BizCompanyName,
                          a => a.MapFrom(u => u.Company != null ? (u.Company.Name) : string.Empty))
               .ForMember(m => m.Emails,
                          a => a.MapFrom(u => u.Contacts.Where(c => c.Type == MembershipConstant.ContactTypeEmail).Select(r => r.Value)))
               .ForMember(dto => dto.IsMainContactUser, a => a.MapFrom(entity =>
                                                                       entity.ClientCompany != null &&
                                                                       entity.ClientCompany.MainContactUser != null &&
                                                                       entity.ClientCompany.MainContactUser.Id ==
                                                                       entity.Id
                                                                 ))
               .ForMember(m => m.PhoneNumbers,
                          a => a.MapFrom(u => u.Contacts.Where(c => c.Type == MembershipConstant.ContactTypePhone).Select(r => r.Value)))
                ;

            map.ConfigureMapping<UserBaseModel, User>()
               .ForMember(u => u.Roles, a => a.Ignore())
               .ForMember(u => u.IsActive, a => a.Ignore());

            map.ConfigureMapping<Company, ClientCompanyDto>().ReverseMap();
            map.ConfigureMapping<Company, ClientCompanyWithOwnerDto>().ReverseMap();

            map.ConfigureMapping<CompanyShortDto, Company>();

            map.ConfigureMapping<Company, CompanyShortDto>()
                .ForMember(m => m.Status, a => a.MapFrom(c => c.StatusString))
                .ForMember(m => m.Status, a => a.MapFrom(c => (c.StatusValidDate.HasValue && c.StatusValidDate < DateTime.UtcNow) ? "Error [Expired]" : c.StatusString));




            map.ConfigureMapping<Company, BasicCompanyDto>()
                .ForMember(c => c.CompanyName, a => a.MapFrom(c => c.Name));

            map.ConfigureMapping<BasicCompanyDto, Company>()
                .ForMember(c => c.Name, a => a.MapFrom(c => c.CompanyName))
                .ForMember(c => c.Address, a => a.MapFrom(c => c.Address));

            map.ConfigureMapping<Contact, ContactDto>().ReverseMap();
        }
    }
}