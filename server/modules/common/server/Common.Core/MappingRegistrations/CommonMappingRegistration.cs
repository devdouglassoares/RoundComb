using Common.Core.Entities;
using Core.ObjectMapping;

namespace Common.Core.MappingRegistrations
{
    public class CommonMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<ExceptionLogger, ExceptionLoggerDto>().ReverseMap();
        }
    }
}