using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Exceptions;
using Core.DynamicProperties.Models;
using Core.DynamicProperties.Repositories;
using Core.DynamicProperties.Services;
using Core.Exceptions;
using Core.ObjectMapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DynamicProperties.Implementations
{
    public abstract class DynamicPropertyService : IDynamicPropertyService
    {
        protected readonly IMappingService MappingService;
        protected readonly IDynamicPropertyRepository Repository;

        protected DynamicPropertyService(IMappingService mappingService, IDynamicPropertyRepository repository)
        {
            Repository = repository;
            MappingService = mappingService;
        }

        public IQueryable<DynamicPropertySupportedEntityType> GetHasDynamicPropertyEntityTypes()
        {
            var dynamicPropertySupportedTypes = Repository.GetAll<DynamicPropertySupportedEntityType>();
            return dynamicPropertySupportedTypes;

        }

        public IEnumerable<string> GetAvailablePropertyType()
        {
            return Enum.GetNames(typeof(PropertyType));
        }

        public DynamicProperty GetEntity<T>(long id) where T : class
        {
            var entity = Repository.First<DynamicProperty>(x => x.Id == id && x.DynamicPropertyEntityTypeMappings.Any(mapping => mapping.TargetEntityType == typeof(T).FullName));

            if (entity == null || entity.IsDeleted)
                throw new BaseNotFoundException<DynamicProperty>();

            return entity;
        }

        public DynamicProperty AssignDynamicPropertyToType<T>(long id) where T : class
        {
            var entity = Repository.First<DynamicProperty>(x => x.Id == id);

            if (entity == null || entity.IsDeleted)
                throw new BaseNotFoundException<DynamicProperty>();

            if (entity.DynamicPropertyEntityTypeMappings.All(mapping => mapping.TargetEntityType != typeof(T).FullName))
            {
                entity.DynamicPropertyEntityTypeMappings.Add(new DynamicPropertyEntityTypeMapping
                {
                    TargetEntityType = typeof(T).FullName
                });
                Repository.Update(entity);
                Repository.SaveChanges();
            }

            return entity;
        }

        public void CreateDynamicProperty<T>(DynamicPropertyModel model) where T : class
        {
            var existingProp =
                Repository.First<DynamicProperty>(
                    x => x.PropertyName.Equals(model.PropertyName));

            if (existingProp != null && existingProp.DynamicPropertyEntityTypeMappings.All(mapping =>
                                                                                           mapping.TargetEntityType !=
                                                                                           typeof(T).FullName))
            {
                if (model.IsSame(existingProp))
                {
                    existingProp.DynamicPropertyEntityTypeMappings.Add(new DynamicPropertyEntityTypeMapping
                    {
                        TargetEntityType = typeof(T).FullName
                    });
                    Repository.Update(existingProp);
                    Repository.SaveChanges();
                    return;
                }
            }

            var entity = MappingService.Map(model, new DynamicProperty());
            entity.DynamicPropertyEntityTypeMappings.Add(new DynamicPropertyEntityTypeMapping
            {
                TargetEntityType = typeof(T).FullName
            });

            foreach (var targetEntityType in model.TargetEntityTypes.Where(name => name!=typeof(T).FullName))
            {
                entity.DynamicPropertyEntityTypeMappings.Add(new DynamicPropertyEntityTypeMapping
                {
                    TargetEntityType = targetEntityType
                });
            }

            Repository.Insert(entity);
            Repository.SaveChanges();
        }

        public void Update<T>(long id, DynamicPropertyModel model) where T : class
        {
            var existingProp =
                Repository.First<DynamicProperty>(
                    x =>
                        x.PropertyName.Equals(model.PropertyName) && x.Id != id &&
                        x.DynamicPropertyEntityTypeMappings.Any(mapping => mapping.TargetEntityType == typeof(T).FullName));

            if (existingProp != null)
                throw new DynamicPropertyAlreadyExistException<T>(
                    $"Another property for type {typeof(T).FullName} with same name already exists");

            var entity = Repository.GetById<DynamicProperty>(id);
            if (entity == null)
                throw new BaseNotFoundException<DynamicProperty>();

            entity.DynamicPropertyEntityTypeMappings.Clear();

            entity.DynamicPropertyEntityTypeMappings.Add(new DynamicPropertyEntityTypeMapping
            {
                TargetEntityType = typeof(T).FullName
            });
            Repository.Update(entity);
            Repository.SaveChanges();

            foreach (var targetEntityType in model.TargetEntityTypes.Where(name => name != typeof(T).FullName))
            {
                entity.DynamicPropertyEntityTypeMappings.Add(new DynamicPropertyEntityTypeMapping
                {
                    TargetEntityType = targetEntityType
                });
            }

            entity = MappingService.Map(model, entity);
            Repository.Update(entity);
            Repository.SaveChanges();
        }

        public void Delete<T>(long id) where T : class
        {
            var entity = Repository.First<DynamicProperty>(x => x.Id == id);

            if (entity == null)
                throw new BaseNotFoundException<DynamicProperty>();

            var entityMapping =
                entity.DynamicPropertyEntityTypeMappings.FirstOrDefault(x => x.TargetEntityType == typeof(T).FullName);

            if (entityMapping != null)
            {
                entity.DynamicPropertyEntityTypeMappings.Remove(entityMapping);
                Repository.Update(entity);
            }
            else if (entity.DynamicPropertyEntityTypeMappings.Count == 0)
            {
                Repository.Delete(entity);
            }
            Repository.SaveChanges();
        }

        public IQueryable<DynamicProperty> GetDyamicPropertiesForEntity<T>() where T : class
        {
            var dynamicProperties =
                Repository.Fetch<DynamicProperty>(x => !x.IsDeleted && x.DynamicPropertyEntityTypeMappings.Any(mapping => mapping.TargetEntityType == typeof(T).FullName));

            return dynamicProperties;
        }

        public DynamicPropertyModel GetDto<T>(long id) where T : class
        {
            var entity = Repository.First<DynamicProperty>(x => x.Id == id && x.DynamicPropertyEntityTypeMappings.Any(mapping => mapping.TargetEntityType == typeof(T).FullName));
            if (entity == null)
                throw new BaseNotFoundException<DynamicProperty>();

            return MappingService.Map<DynamicPropertyModel>(entity);
        }

        public abstract IEnumerable<DynamicPropertyModel> GetDynamicPropertiesForConfig(long configId);

        public abstract void AssignToConfig(long propertyId, long configId);

        public abstract void RemoveFromConfig(long propertyId, long configId);

        public abstract IEnumerable<DynamicPropertyModel> GetFilterableProperties(long? categoryId);

        public IQueryable<DynamicProperty> GetFilterableProperties<T>() where T : class
        {
            var dynamicProperties =
                Repository.Fetch<DynamicProperty>(
                    x => !x.IsDeleted &&
                         x.Searchable &&
                         x.DynamicPropertyEntityTypeMappings.Any(mapping => mapping.TargetEntityType == typeof(T).FullName));

            return dynamicProperties;
        }
    }
}