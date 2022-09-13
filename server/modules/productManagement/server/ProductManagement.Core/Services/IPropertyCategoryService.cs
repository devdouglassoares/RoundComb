using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using Core;
using Core.Database;
using System.Collections.Generic;

namespace ProductManagement.Core.Services
{
    public interface IPropertyCategoryService : IBaseService<PropertyCategory, PropertyCategoryDto>, IDependency
    {
        PropertyCategory CreateOrUpdate(PropertyCategoryDto model);

        void Update(IEnumerable<PropertyCategoryDto> models);
        IEnumerable<PropertyCategory> GetAllAsTree();
    }
}