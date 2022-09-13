using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Models;
using Core.ObjectMapping;
using System;
using System.Linq;

namespace Core.DynamicProperties.MappingConfig
{
    public class DynamicPropMappingConfig: IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {

            map.ConfigureMapping<DynamicProperty, DynamicPropertyModel>()
               .ForMember(dto => dto.AvailableOptions,
                          a => a.MapFrom(entity => entity.AvailableOptionsString.Split(new[]
                                                                                       {
                                                                                           ';'
                                                                                       },
                                                                                       StringSplitOptions
                                                                                           .RemoveEmptyEntries)))
               .ForMember(dto => dto.TargetEntityTypes,
                          a =>
                          a.MapFrom(
                                    entity =>
                                    entity.DynamicPropertyEntityTypeMappings.Select(x => x.TargetEntityType).ToArray()))
                ;

            map.ConfigureMapping<DynamicPropertyModel, DynamicProperty>()
               .ForMember(entity => entity.AvailableOptions, a => a.Ignore())
               .ForMember(entity => entity.AvailableOptionsString,
                   a =>
                       a.MapFrom(
                           dto =>
                               dto.AvailableOptions == null || !dto.AvailableOptions.Any()
                                   ? ""
                                   : string.Join(";", dto.AvailableOptions)))
                ;

            map.ConfigureMapping<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
        }
    }
}