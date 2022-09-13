using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Models;
using Core.DynamicProperties.Repositories;
using Core.DynamicProperties.Services;
using Core.Extensions;
using Core.ObjectMapping;
using System.Collections.Generic;
using System.Linq;

namespace Core.DynamicProperties.Implementations
{
    public class DynamicPropertyValueService : IDynamicPropertyValueService
    {
        private readonly IDynamicPropertyRepository _repository;
        private readonly IMappingService _mappingService;

        public DynamicPropertyValueService(IDynamicPropertyRepository baseRepository, IMappingService mappingService)
        {
            _repository = baseRepository;
            _mappingService = mappingService;
        }

        public DynamicPropertyValuesModel GetExtendedPropertyValuesForEntity<T>(long entityId) where T : class
        {
            var propertyValues =
                _repository.Fetch<DynamicPropertyValue>(x => x.TargetEntityType == typeof(T).FullName && x.ExternalEntityId == entityId);

            var dto = new DynamicPropertyValuesModel
            {
                ExternalEntityId = entityId
            };

            if (!propertyValues.Any())
            {
                return dto;
            }

            var dynamicPropertyValues = propertyValues.ToArray();
            foreach (var dynamicPropertyValue in dynamicPropertyValues)
            {
                dto[dynamicPropertyValue.Property.PropertyName] = _mappingService.Map<DynamicPropertyValueDto>(dynamicPropertyValue);
            }

            return dto;
        }

        public void DeleteExtendedPropertyValuesForEntity<T>(long entityId) where T : class
        {
            _repository.DeleteByCondition<DynamicPropertyValue>(x => x.TargetEntityType == typeof(T).FullName && x.ExternalEntityId == entityId);
            _repository.SaveChanges();
        }

        public List<DynamicPropertyValuesModel> GetExtendedPropertyValuesForEntities<T>(long[] entityIds) where T : class
        {
            var propertyValues =
                _repository.Fetch<DynamicPropertyValue>(x => x.TargetEntityType == typeof(T).FullName && entityIds.Contains(x.ExternalEntityId));

            var dynamicValues = new List<DynamicPropertyValuesModel>();

            foreach (var dynamicPropertyValue in propertyValues)
            {
                var dynamicPropValues =
                    dynamicValues.FirstOrDefault(x => x.ExternalEntityId == dynamicPropertyValue.ExternalEntityId);
                if (dynamicPropValues == null)
                {
                    dynamicPropValues = new DynamicPropertyValuesModel();
                    dynamicPropValues.ExternalEntityId = dynamicPropertyValue.ExternalEntityId;
                    dynamicValues.Add(dynamicPropValues);
                }

                dynamicPropValues[dynamicPropertyValue.Property.PropertyName] = _mappingService.Map<DynamicPropertyValueDto>(dynamicPropertyValue);
            }

            return dynamicValues;
        }

        public DynamicPropertyValuesModel GetAvailableExtendedPropertyValues<T>() where T : class
        {
            var properties = _repository.Fetch<DynamicProperty>(x => !x.IsDeleted && x.DynamicPropertyEntityTypeMappings.Any(mapping => mapping.TargetEntityType == typeof(T).FullName));

            var dto = new DynamicPropertyValuesModel();

            if (!properties.Any())
            {
                return dto;
            }

            foreach (var property in properties)
            {
                dto[property.PropertyName] = null;
            }

            return dto;
        }

        public virtual void UpdateEntityAdditionalFields<T>(long entity, DynamicPropertyValuesModel model) where T : class
        {
            if (model == null) return;

            foreach (var extendedProperty in model.Keys)
            {
                var property =
                    _repository.First<DynamicProperty>(x => !x.IsDeleted && x.PropertyName.Equals(extendedProperty.ToString()) && x.DynamicPropertyEntityTypeMappings.Any(mapping => mapping.TargetEntityType == typeof(T).FullName));
                if (property == null)
                    continue;

                var existingValue =
                    _repository.First<DynamicPropertyValue>(x => x.ExternalEntityId == entity && x.PropertyId == property.Id && x.TargetEntityType == typeof(T).FullName);

                if (existingValue != null)
                {
                    model[extendedProperty].CopyTo(existingValue, true,
                                                   dto => new
                                                   {
                                                       dto.Property
                                                   });
                    _repository.Update(existingValue);
                }
                else
                {
                    var propertyValue = new DynamicPropertyValue
                    {
                        ExternalEntityId = entity,
                        PropertyId = property.Id,
                        TargetEntityType = typeof(T).FullName
                    };

                    model[extendedProperty].CopyTo(propertyValue, true,
                                                   dto => new
                                                   {
                                                       dto.Property
                                                   });
                    _repository.Insert(propertyValue);
                }
            }
            _repository.SaveChanges();
        }

        public IEnumerable<long> GetEntitiesHasValues<TTargetEntity>(Dictionary<long, DynamicPropertyFilterModel> dynamicPropertyFilters)
        {
            long[] result = null;

            foreach (var filter in dynamicPropertyFilters.Values.Where(x => x.HasFilter))
            {
                var property =
                    _repository.Get<DynamicProperty>(
                                                     prop =>
                                                     prop.Id == filter.PropertyId &&
                                                     prop.PropertyTypeString == filter.PropertyType.ToString());

                if (property == null || !property.Searchable)
                    continue;

                switch (property.PropertyType)
                {
                    case PropertyType.CheckBoxes:
                        result = LoadCheckboxesFilteredEntities<TTargetEntity>(result, property, filter);
                        break;
                    case PropertyType.Number:
                        result = LoadNumberFilteredEntities<TTargetEntity>(result, property, filter);
                        break;
                    case PropertyType.Currency:
                        result = LoadCurrencyFilteredEntities<TTargetEntity>(result, property, filter);
                        break;
                    case PropertyType.DatePicker:
                        result = LoadDateFilteredEntities<TTargetEntity>(result, property, filter);
                        break;
                    default:
                        break;
                }
            }

            return result.Distinct();
        }

        private long[] LoadDateFilteredEntities<TTargetEntity>(long[] result, DynamicProperty property, DynamicPropertyFilterModel filter)
        {
            var list = _repository.Fetch<DynamicPropertyValue>(value => value.PropertyId == property.Id &&
                                                                        value.TargetEntityType ==
                                                                        typeof(TTargetEntity).FullName &&
                                                                        (filter.MinDateValue == null ||
                                                                         value.DateTimeValue >= filter.MinDateValue) &&
                                                                        (filter.MaxDateValue == null ||
                                                                         value.DateTimeValue <= filter.MaxDateValue))
                                  .Select(x => x.ExternalEntityId)
                                  .ToArray();

            var joinedElements = result?.Intersect(list) ?? list;

            return joinedElements.ToArray();
        }

        private long[] LoadCurrencyFilteredEntities<TTargetEntity>(long[] result, DynamicProperty property, DynamicPropertyFilterModel filter)
        {

            var list = _repository.Fetch<DynamicPropertyValue>(value => value.PropertyId == property.Id &&
                                                                        value.TargetEntityType ==
                                                                        typeof(TTargetEntity).FullName &&
                                                                        (filter.MinCurrencyValue == null ||
                                                                         value.DecimalValue >= filter.MinCurrencyValue) &&
                                                                        (filter.MaxCurrencyValue == null ||
                                                                         value.DecimalValue <= filter.MaxCurrencyValue))
                                  .Select(x => x.ExternalEntityId)
                                  .ToArray();

            var joinedElements = result?.Intersect(list) ?? list;

            return joinedElements.ToArray();
        }

        private long[] LoadNumberFilteredEntities<TTargetEntity>(long[] result, DynamicProperty property, DynamicPropertyFilterModel filter)
        {
            var list = _repository.Fetch<DynamicPropertyValue>(value => value.PropertyId == property.Id &&
                                                                        value.TargetEntityType ==
                                                                        typeof(TTargetEntity).FullName &&
                                                                        (filter.MinNumberValue == null ||
                                                                         value.IntValue >= filter.MinNumberValue) &&
                                                                        (filter.MaxNumberValue == null ||
                                                                         value.IntValue <= filter.MaxNumberValue))
                                  .Select(x => x.ExternalEntityId)
                                  .ToArray();

            var joinedElements = result?.Intersect(list) ?? list;

            return joinedElements.ToArray();
        }

        private long[] LoadCheckboxesFilteredEntities<TTargetEntity>(long[] result, DynamicProperty property, DynamicPropertyFilterModel filter)
        {
            var list = _repository.Fetch<DynamicPropertyValue>(value => value.TargetEntityType ==
                                                                        typeof(TTargetEntity).FullName &&
                                                                        value.PropertyId == property.Id).ToArray();

            var filteredResult = list.Where(
                propValue =>
                propValue.CheckboxValues != null && filter.CheckBoxesValues != null &&
                (!filter.CheckBoxesValues.Any() ||
                 propValue.CheckboxValues.Intersect(filter.CheckBoxesValues).Any()))
                                     .Select(x => x.ExternalEntityId)
                                  .ToArray();

            var joinedElements = result?.Intersect(filteredResult) ?? filteredResult;

            return joinedElements.ToArray();
        }
    }
}