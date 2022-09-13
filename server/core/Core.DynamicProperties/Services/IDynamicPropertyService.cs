using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Models;
using System.Collections.Generic;
using System.Linq;

namespace Core.DynamicProperties.Services
{
    public interface IDynamicPropertyService: IDependency
    {
        IQueryable<DynamicPropertySupportedEntityType> GetHasDynamicPropertyEntityTypes();

        IEnumerable<string> GetAvailablePropertyType();

        DynamicProperty GetEntity<T>(long id) where T : class;

        DynamicProperty AssignDynamicPropertyToType<T>(long id) where T : class;

        void CreateDynamicProperty<T>(DynamicPropertyModel model) where T : class;

        void Update<T>(long id, DynamicPropertyModel model) where T : class;

        void Delete<T>(long id) where T : class;

        IQueryable<DynamicProperty> GetFilterableProperties<T>() where T : class;

        IQueryable<DynamicProperty> GetDyamicPropertiesForEntity<T>() where T : class;

        DynamicPropertyModel GetDto<T>(long id) where T : class;
        
        IEnumerable<DynamicPropertyModel> GetDynamicPropertiesForConfig(long configId);

        void AssignToConfig(long propertyId, long configId);

        void RemoveFromConfig(long propertyId, long configId);

        IEnumerable<DynamicPropertyModel> GetFilterableProperties(long? configId);
    }
}