using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Services;
using ProductManagement.Core.Repositories;
using Core.Database;
using Core.ObjectMapping;

namespace ProductManagement.Services
{
    public class PropertyImageService : BaseService<PropertyImage, PropertyImageDto>, IPropertyImageService
    {
        public PropertyImageService(IMappingService mappingService, IRepository repository)
            : base(mappingService, repository)
        {
        }
    }
}