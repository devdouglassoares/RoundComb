using Core;
using Core.Database;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Services
{
    public interface IPropertySearchRecordService : IBaseService<PropertySearchRecord, PropertySearchRecordDto>, IDependency
    {

    }
}