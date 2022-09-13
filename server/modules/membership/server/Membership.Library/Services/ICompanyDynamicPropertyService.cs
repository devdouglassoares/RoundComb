using Core;
using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Implementations;
using Core.DynamicProperties.Repositories;
using Core.DynamicProperties.Services;
using Core.ObjectMapping;
using Membership.Core.Entities;
using System;
using System.Collections.Generic;

namespace Membership.Library.Services
{
    public interface ICompanyDynamicPropertyService : IDynamicPropertyService, IDependency
    {

    }

    public class CompanyDynamicPropertyService : DynamicPropertyService, ICompanyDynamicPropertyService
    {
        public CompanyDynamicPropertyService(IMappingService mappingService,
                                             IDynamicPropertyRepository repository)
            : base(mappingService, repository)
        {
        }

        public override IEnumerable<DynamicPropertyModel> GetDynamicPropertiesForConfig(long configId)
        {
            var result = GetDyamicPropertiesForEntity<Company>();

            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(result);
        }

        public override void AssignToConfig(long propertyId, long configId)
        {
            throw new NotSupportedException();
        }

        public override void RemoveFromConfig(long propertyId, long configId)
        {
            throw new NotSupportedException();
        }

        public override IEnumerable<DynamicPropertyModel> GetFilterableProperties(long? configId)
        {
            var productDynamicProperties = GetFilterableProperties<UserProfile>();


            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(productDynamicProperties);
        }
    }
}