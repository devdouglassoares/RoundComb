using Core.ObjectMapping;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Library.Contracts;
using Membership.Library.Dto;
using Membership.Library.Dto.Customer;
using Membership.Library.Entities;
using System;
using System.Linq;

namespace Membership.Library.MappingRegistrations
{
    public class MembershipMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<UserProfile, UserProfileModel>()
               .ForMember(x => x.FirstName, m => m.MapFrom(profile => profile.User.FirstName))
               .ForMember(x => x.LastName, m => m.MapFrom(profile => profile.User.LastName))
               .ForMember(x => x.Email, m => m.MapFrom(profile => profile.User.Email))
               .ReverseMap();

            map.ConfigureMapping<UserWithProfileModel, UserProfileModel>().ReverseMap();

            map.ConfigureMapping<Company, CompanyAutocompleteDto>();

            map.ConfigureMapping<CompanyAutocompleteDto, CompanyDto>();

            map.ConfigureMapping<AccessEntity, PermissionModel>();

            map.ConfigureMapping<Tuple<Role, AccessEntity>, PermissionModel>()
                  .ConvertUsing(t =>
                  {
                      var perm = map.Map<PermissionModel>(t.Item2);
                      perm.RoleId = t.Item1.Id;
                      perm.RoleName = t.Item1.Name;
                      return perm;
                  });

            map.ConfigureMapping<PermissionModel, AccessEntity>();

            map.ConfigureMapping<Role, RoleModel>()
               .ForMember(r => r.Users, m => m.Ignore())
               .ForMember(r => r.Permissions,
                          a => a.MapFrom(r => r.RoleAccessRights.SelectMany(ar => ar.AccessModule.AccessEntities)));

            map.ConfigureMapping<RoleModel, Role>()
               .ForMember(r => r.Users, m => m.Ignore());


            map.ConfigureMapping<Company, CompanyDto>()
               .ForMember(c => c.CompanyName, a => a.MapFrom(c => c.Name))
               .ForMember(c => c.MasterCompanyId,
                          a => a.MapFrom(c => c.MasterCompany != null ? c.MasterCompany.Id : (long?)null))
               .ForMember(c => c.Status, a => a.MapFrom(c => c.StatusString))
               .ForMember(m => m.IsStatusExpired,
                          a => a.MapFrom(c => c.StatusValidDate.HasValue && c.StatusValidDate < DateTime.UtcNow));

            map.ConfigureMapping<CompanyDto, Company>()
               .ForMember(c => c.Name, a => a.MapFrom(c => c.CompanyName))
               .ForMember(entity => entity.MainContactUser, a => a.Ignore())
               .ForMember(c => c.Address, a => a.MapFrom(c => c.Address));

            map.ConfigureMapping<CompanyDocument, CompanyDocumentDto>()
                .ForMember(d => d.FileName, a => a.MapFrom(e => e.FileRecord.FileName))
                .ForMember(d => d.FileUrl, a => a.MapFrom(e => e.FileRecord.FileUrl));
            map.ConfigureMapping<CompanyDocumentDto, CompanyDocument>();
        }
    }
}