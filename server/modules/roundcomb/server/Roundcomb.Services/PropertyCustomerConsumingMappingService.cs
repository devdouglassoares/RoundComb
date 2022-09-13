using Core.Database;
using Core.ObjectMapping;
using ProductManagement.Core.Entities;
using Membership.Core.Contracts;
using Roundcomb.Core.Dtos;
using Roundcomb.Core.Entities;
using Roundcomb.Core.Repositories;
using Roundcomb.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roundcomb.Services
{
    public class PropertyCustomerConsumingMappingService : BaseService<PropertyCustomerConsumingMapping, PropertyCustomerConsumingMappingDto>, IPropertyCustomerConsumingMappingService
    {
        private readonly IUserService _userService;

        public PropertyCustomerConsumingMappingService(IMappingService mappingService, IRoundcombRepository repository, IUserService userService) : base(mappingService, repository)
        {
            _userService = userService;
        }

        public void CreateConsumingMappingForProperty(PropertyApplicationFormInstance propertyApplication, PropertySellType propertySellType)
        {
            if (propertySellType == PropertySellType.Unavailable)
                throw new InvalidOperationException("Invalid property sell type");

            var assignmentType = propertySellType == PropertySellType.ForRent
                                     ? PropertyAssignment.Rented
                                     : PropertyAssignment.Sold;

            var existingRecord =
                Repository.Get<PropertyCustomerConsumingMapping>(
                                                                mapping =>
                                                                mapping.PropertyId == propertyApplication.PropertyId &&
                                                                mapping.CustomerId == propertyApplication.UserId && !mapping.IsCompleted);

            if (existingRecord != null)
                throw new InvalidOperationException("The property assignment has already existed.");

            Repository.Insert(new PropertyCustomerConsumingMapping
            {
                PropertyId = propertyApplication.PropertyId,
                CustomerId = propertyApplication.UserId,
                PropertyApplicationFormInstanceId = propertyApplication.Id,
                Assignment = assignmentType,
                StartDate = DateTimeOffset.Now,
            });
            Repository.SaveChanges();
        }

        public void EndContractConsumingMapping(long propertyCustomerConsumingMappingId)
        {
            var propertyConsumingMapping = GetEntity(propertyCustomerConsumingMappingId);

            propertyConsumingMapping.Property.Status = PropertyStatus.AvailableForSell;

            propertyConsumingMapping.IsCompleted = true;

            Repository.Update(propertyConsumingMapping.Property);
            Repository.Update(propertyConsumingMapping);
        }

        public IEnumerable<PropertyCustomerConsumingMappingDto> GetPropertyCustomerMappingForUser(long userId, bool pastCustomers)
        {
            var result = Fetch(x => x.Property.OwnerId == userId && x.IsCompleted == pastCustomers);

            var dtos = ToDtos(result).ToArray();

            foreach (var propertyCustomerConsumingMappingDto in dtos)
            {
                propertyCustomerConsumingMappingDto.CustomerInformation =
                    _userService.GetUserPersonalInformation(propertyCustomerConsumingMappingDto.CustomerId);
            }

            return dtos;
        }
    }
}