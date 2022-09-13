using Core.ObjectMapping;
using LocationService.Library.Dtos;
using LocationService.Library.Entities;

namespace LocationService.Library.ObjectMappingRegistration
{
    public class LocationObjectMappingRegistration: IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<Location, LocationDto>();
            map.ConfigureMapping<LocationDto, Location>();
        }
    }
}
