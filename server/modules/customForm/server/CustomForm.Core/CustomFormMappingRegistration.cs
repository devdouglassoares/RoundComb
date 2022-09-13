using Core.DynamicProperties.Dtos;
using Core.ObjectMapping;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using Membership.Core.Entities.Enums;

namespace CustomForm.Core
{
    public class CustomFormMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<FormField, FormFieldDto>().ReverseMap();

            map.ConfigureMapping<FormConfiguration, FormConfigurationDto>();
            map.ConfigureMapping<FormConfiguration, FormConfigurationNoField>();

            map.ConfigureMapping<FormConfigurationDto, FormConfiguration>()
               .ForMember(entity => entity.Fields, a => a.Ignore())
                ;

            map.ConfigureMapping<FormFieldFormConfiguration, FormFieldFormConfigurationDto>();
            map.ConfigureMapping<FormFieldFormConfigurationDto, FormFieldFormConfiguration>()
               .ForMember(entity => entity.FormField, a => a.Ignore())
               .ForMember(entity => entity.FormConfiguration, a => a.Ignore())
                ;


            map.ConfigureMapping<FormInstance, FormInstanceDto>()
                .ForMember(dto => dto.Answers, a => a.Ignore());

            map.ConfigureMapping<FormInstanceDto, FormInstance>()
               .ForMember(entity => entity.Answers, a => a.Ignore())
               .ForMember(entity => entity.FormConfiguration, a => a.Ignore());

            map.ConfigureMapping<FormFieldValue, FormFieldValueDto>()
                .ForMember(dto => dto.PropertyType, a => a.MapFrom(entity => entity.Field.PropertyType));

            map.ConfigureMapping<FormFieldValue, DynamicPropertyValueDto>()
                .ForMember(dto => dto.Property, a=> a.MapFrom(entity => entity.Field));


            map.ConfigureMapping<FormField, DynamicPropertyModel>().ReverseMap();

            map.ConfigureMapping<FormFieldValueDto, FormFieldValue>()
               .ForMember(entity => entity.FormInstance, a => a.Ignore())
               .ForMember(entity => entity.Field, a => a.Ignore())
                ;
        }
    }
}