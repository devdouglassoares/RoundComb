using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Implementations;
using Core.DynamicProperties.Repositories;
using Core.ObjectMapping;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Services
{
    public class PropertyPropertyService : DynamicPropertyService, IPropertyPropertyService
    {
        private readonly IRepository _propertyManagementRepository;

        public PropertyPropertyService(IMappingService mappingService,
                                      IDynamicPropertyRepository repository,
                                      IRepository propertyManagementRepository)
            : base(mappingService, repository)
        {
            _propertyManagementRepository = propertyManagementRepository;
        }

        public override IEnumerable<DynamicPropertyModel> GetDynamicPropertiesForConfig(long configId)
        {
            var result = GetDyamicPropertiesForEntity<Property>();

            if (configId != 0)
            {
                var propertyIds =
                    _propertyManagementRepository.Fetch<PropertyDynamicPropertyCategory>(x => x.CategoryId == configId)
                                                .Select(x => x.DynamicPropertyId)
                                                .ToArray();

                result = result.Where(x => propertyIds.Contains(x.Id) || x.AvailableToAllEntities);
            }

            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(result);
        }

        public override void AssignToConfig(long propertyId, long configId)
        {
            var entity = GetEntity<Property>(propertyId);

            var exists =
                _propertyManagementRepository.Any<PropertyDynamicPropertyCategory>(
                    x => x.CategoryId == configId && x.DynamicPropertyId == propertyId);

            if (exists)
                throw new InvalidOperationException("Selected dynamic property already assigned to selected category");


            _propertyManagementRepository.Insert(new PropertyDynamicPropertyCategory
            {
                CategoryId = configId,
                DynamicPropertyId = entity.Id
            });
            _propertyManagementRepository.SaveChanges();
        }

        public override void RemoveFromConfig(long propertyId, long configId)
        {
            _propertyManagementRepository.DeleteByCondition<PropertyDynamicPropertyCategory>(
                x => x.CategoryId == configId && x.DynamicPropertyId == propertyId);
            _propertyManagementRepository.SaveChanges();
        }

        public override IEnumerable<DynamicPropertyModel> GetFilterableProperties(long? configId)
        {
            var propertyDynamicProperties = GetFilterableProperties<Property>();

            if (configId.HasValue)
            {
                var propertyIds =
                    _propertyManagementRepository.Fetch<PropertyDynamicPropertyCategory>(x => x.CategoryId == configId)
                                                .Select(x => x.DynamicPropertyId)
                                                .ToArray();

                propertyDynamicProperties =
                    propertyDynamicProperties.Where(x => propertyIds.Contains(x.Id) || x.AvailableToAllEntities);
            }

            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(propertyDynamicProperties);
        }
    }
}