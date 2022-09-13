using Core;
using Core.Database;
using Core.ObjectMapping;
using MembershipLocation.Api.Dto;
using MembershipLocation.Api.Models;
using MembershipLocation.Api.Repositories;

namespace MembershipLocation.Api.Services
{
    public interface ILocationTypeService: IBaseService<LocationType, LocationTypeDto>, IDependency
    {
         
    }

    public class LocationTypeService : BaseService<LocationType, LocationTypeDto>, ILocationTypeService
    {
        public LocationTypeService(IMappingService mappingService, IRepository repository) : base(mappingService, repository)
        {
        }
    }
}