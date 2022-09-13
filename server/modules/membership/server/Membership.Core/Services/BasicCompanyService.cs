using AutoMapper;
using Core.Database;
using Core.ObjectMapping;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Dto;
using Membership.Core.Entities;

namespace Membership.Core.Services
{
    public class BasicCompanyService : BaseService<Company, BasicCompanyDto>, IBasicCompanyService
    {
        public BasicCompanyService(IMappingService mappingService,
            ICoreRepository membershipRepository)
            : base(mappingService, membershipRepository)
        {
        }  
    }
}
