using Core.Database;
using Core.Extensions;
using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProductManagement.Services
{
    public class PropertyCategoryService : BaseService<PropertyCategory, PropertyCategoryDto>, IPropertyCategoryService
    {
        public PropertyCategoryService(IMappingService mappingService, IRepository repository)
            : base(mappingService, repository)
        {
        }

        public override IQueryable<PropertyCategory> GetAll()
        {
            return Fetch(x => !x.IsDeleted);
        }

        public override void Delete(params object[] keys)
        {
            var entity = GetEntity(keys);

            foreach (var propertyCategory in entity.Children)
            {
                propertyCategory.IsDeleted = true;
            }
            entity.IsDeleted = true;

            Repository.Update(entity);
            Repository.SaveChanges();
        }

        public PropertyCategory CreateOrUpdate(PropertyCategoryDto model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return null;

            var existingEntity = First(x => x.Name == model.Name);

            if (existingEntity != null)
                return existingEntity;

            return Create(model);
        }

        public void Update(IEnumerable<PropertyCategoryDto> models)
        {
            foreach (var propertyCategoryDto in models)
            {
                var entity = GetEntity(propertyCategoryDto.Id);
                entity.ParentCategoryId = null;
                PrepareForUpdating(entity, propertyCategoryDto);

                Repository.Update(entity);
            }
            Repository.SaveChanges();
        }

        public IEnumerable<PropertyCategory> GetAllAsTree()
        {
            return Fetch(x => !x.IsDeleted && x.ParentCategory == null)
                .Include(x => x.Children)
                .OrderBy(x => x.DisplayOrder)
                .ToArray();
        }

        public override PropertyCategory PrepareForInserting(PropertyCategory entity, PropertyCategoryDto model)
        {
            entity = base.PrepareForInserting(entity, model);

            return entity;
        }

        public override PropertyCategory PrepareForUpdating(PropertyCategory entity, PropertyCategoryDto model)
        {
            model.CopyTo(entity, false, dto => new
            {
                dto.Id,
                dto.Children
            });

            return entity;
        }
    }
}