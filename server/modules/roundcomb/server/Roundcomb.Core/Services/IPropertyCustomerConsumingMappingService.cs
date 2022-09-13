using Core;
using Core.Database;
using ProductManagement.Core.Entities;
using Roundcomb.Core.Dtos;
using Roundcomb.Core.Entities;
using System.Collections.Generic;

namespace Roundcomb.Core.Services
{
    public interface IPropertyCustomerConsumingMappingService : IBaseService<PropertyCustomerConsumingMapping, PropertyCustomerConsumingMappingDto>, IDependency
    {
        void CreateConsumingMappingForProperty(PropertyApplicationFormInstance propertyApplication, PropertySellType propertySellType);

        void EndContractConsumingMapping(long propertyCustomerConsumingMappingId);

        IEnumerable<PropertyCustomerConsumingMappingDto> GetPropertyCustomerMappingForUser(long userId, bool pastCustomers);
    }
}