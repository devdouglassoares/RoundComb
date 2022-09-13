using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Implementations;
using Core.DynamicProperties.Repositories;
using Core.ObjectMapping;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using CustomForm.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomForm.Services
{
    public class DynamicPropertyFormService : DynamicPropertyService, IDynamicPropertyFormService
    {
        private readonly IRepository _propertyManagementRepository;

        public DynamicPropertyFormService(IMappingService mappingService,
                                          IDynamicPropertyRepository repository,
                                          IRepository propertyManagementRepository)
            : base(mappingService, repository)
        {
            _propertyManagementRepository = propertyManagementRepository;
        }

        public override IEnumerable<DynamicPropertyModel> GetDynamicPropertiesForConfig(long configId)
        {
            var result = GetDyamicPropertiesForEntity<FormInstance>();

            if (configId != 0)
            {
                var propertyIds =
                    _propertyManagementRepository.Fetch<FormDynamicPropertyConfig>(x => x.FormConfigurationId == configId)
                                                .Select(x => x.DynamicPropertyId)
                                                .ToArray();

                result = result.Where(x => propertyIds.Contains(x.Id) || x.AvailableToAllEntities);
            }

            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(result);
        }

        public override void AssignToConfig(long propertyId, long formConfigId)
        {
            var entity = GetEntity<FormInstance>(propertyId);

            var exists =
                _propertyManagementRepository.Any<FormDynamicPropertyConfig>(
                    x => x.FormConfigurationId == formConfigId && x.DynamicPropertyId == propertyId);

            if (exists)
                throw new InvalidOperationException("Selected dynamic property already assigned to selected category");


            _propertyManagementRepository.Insert(new FormDynamicPropertyConfig
            {
                FormConfigurationId = formConfigId,
                DynamicPropertyId = entity.Id
            });
            _propertyManagementRepository.SaveChanges();
        }

        public override void RemoveFromConfig(long propertyId, long configId)
        {
            _propertyManagementRepository.DeleteByCondition<FormDynamicPropertyConfig>(
                x => x.FormConfigurationId == configId && x.DynamicPropertyId == propertyId);
            _propertyManagementRepository.SaveChanges();
        }

        public override IEnumerable<DynamicPropertyModel> GetFilterableProperties(long? configId)
        {
            var propertyDynamicProperties = GetFilterableProperties<FormInstance>();

            if (configId.HasValue)
            {
                var propertyIds =
                    _propertyManagementRepository.Fetch<FormDynamicPropertyConfig>(x => x.FormConfigurationId == configId)
                                                .Select(x => x.DynamicPropertyId)
                                                .ToArray();

                propertyDynamicProperties =
                    propertyDynamicProperties.Where(x => propertyIds.Contains(x.Id) || x.AvailableToAllEntities);
            }

            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(propertyDynamicProperties);
        }
    }
}