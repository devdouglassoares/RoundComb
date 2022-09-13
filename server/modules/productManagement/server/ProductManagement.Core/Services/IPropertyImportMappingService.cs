using Core;
using Core.Database;
using Core.Database.Repositories;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Services
{
    public interface IPropertyImportMappingService : IBaseService<PropertyImportMappingSet, PropertyImportMappingSetDto>, IDependency
    {

    }
}