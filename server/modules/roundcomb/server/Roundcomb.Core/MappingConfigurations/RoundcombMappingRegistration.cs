using Core.ObjectMapping;
using Roundcomb.Core.Dtos;
using Roundcomb.Core.Entities;

namespace Roundcomb.Core.MappingConfigurations
{
    public class RoundcombMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<PropertyCustomerConsumingMapping, PropertyCustomerConsumingMappingDto>().ReverseMap();

            map.ConfigureMapping<PropertyFormConfigurationSetting, PropertyFormConfigurationSettingDto>().ReverseMap();

            map.ConfigureMapping<PropertyApplicationFormInstance, PropertyApplicationFormNoAnswerDto>();

            map.ConfigureMapping<PropertyApplicationFormInstance, PropertyApplicationFormInstanceDto>();


            map.ConfigureMapping<PropertyApplicationFormNoAnswerDto, PropertyApplicationFormInstance>()
                ;

            map.ConfigureMapping<PropertyApplicationFormInstanceDto, PropertyApplicationFormInstance>();

            map.ConfigureMapping<PropertyApplicationFormDocumentConfig, PropertyApplicationFormDocumentConfigDto>().ReverseMap();
        }
    }
}