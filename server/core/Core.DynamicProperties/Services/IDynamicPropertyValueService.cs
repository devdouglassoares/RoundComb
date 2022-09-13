using Core.DynamicProperties.Dtos;
using System.Collections.Generic;

namespace Core.DynamicProperties.Services
{
    public interface IDynamicPropertyValueService : IDependency
    {
        DynamicPropertyValuesModel GetExtendedPropertyValuesForEntity<T>(long entityId) where T : class;

        void DeleteExtendedPropertyValuesForEntity<T>(long entityId) where T : class;

        List<DynamicPropertyValuesModel> GetExtendedPropertyValuesForEntities<T>(long[] entityIds) where T : class;

        DynamicPropertyValuesModel GetAvailableExtendedPropertyValues<T>() where T : class;

        void UpdateEntityAdditionalFields<T>(long entity, DynamicPropertyValuesModel model) where T : class;

        IEnumerable<long> GetEntitiesHasValues<TTargetEntity>(Dictionary<long, DynamicPropertyFilterModel> dynamicPropertyFilters);
    }
}