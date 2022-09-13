using Core.Database;
using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;

namespace ProductManagement.Services
{
    public class PropertyImportMappingService : BaseService<PropertyImportMappingSet, PropertyImportMappingSetDto>, IPropertyImportMappingService
    {
        public PropertyImportMappingService(IMappingService mappingService, IRepository repository) : base(mappingService, repository)
        {
        }
    }
}