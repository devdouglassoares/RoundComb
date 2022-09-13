using Core.ObjectMapping;
using MembershipLocation.Api.Dto;
using MembershipLocation.Api.Models;

namespace MembershipLocation.Api.Mappings
{
    public class MembershipLocationMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<LocationType, LocationTypeDto>().ReverseMap();
            map.ConfigureMapping<UserLocation, UserLocationDto>().ReverseMap();
        }
    }
}