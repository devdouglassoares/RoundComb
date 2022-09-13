using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using Core;
using Core.Database;

namespace ProductManagement.Core.Services
{
    public interface IPropertyImageService : IBaseService<PropertyImage, PropertyImageDto>, IDependency
    {

    }
}