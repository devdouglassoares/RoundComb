using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Database;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;

namespace ProductManagement.Core.Services
{
    public interface IPropertyHistoryService : IBaseService<PropertyHistory, PropertyHistoryDto>, IDependency
    {
        IQueryable<PropertyUserHistoryDto> GetPropertyHistoryRecordsGroupByUser(long propertyId, PropertyHistoryType historyType);
    }
}