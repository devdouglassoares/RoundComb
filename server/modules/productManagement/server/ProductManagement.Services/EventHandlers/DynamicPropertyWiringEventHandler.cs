using Core.DynamicProperties.Services;
using Core.Events;
using Core.UI;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using System.Linq;

namespace ProductManagement.Services.EventHandlers
{
    public class DynamicPropertyWiringEventHandler : IConsumer<DataTableDataSourceFiltered<PropertyDto>>
    {
        public int Order => 10;

        private readonly IDynamicPropertyValueService _dynamicPropertyValueService;

        public DynamicPropertyWiringEventHandler(IDynamicPropertyValueService dynamicPropertyValueService)
        {
            _dynamicPropertyValueService = dynamicPropertyValueService;
        }

        public void HandleEvent(DataTableDataSourceFiltered<PropertyDto> eventMessage)
        {
            var propertyIds = eventMessage.Datasource.Select(p => p.Id);

            var propertyDynamicPropertyValues =
                _dynamicPropertyValueService.GetExtendedPropertyValuesForEntities<Property>(propertyIds.ToArray());

            foreach (var dynamicModel in propertyDynamicPropertyValues)
            {
                var externalid = dynamicModel.ExternalEntityId;
                var propertyDto = eventMessage.Datasource.FirstOrDefault(x => x.Id == externalid);
                if (propertyDto != null)
                {
                    propertyDto.ExtendedProperties = dynamicModel;
                }
            }
        }
    }
}