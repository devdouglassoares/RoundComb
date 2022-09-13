using Core;
using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Implementations;
using Core.DynamicProperties.Repositories;
using Core.DynamicProperties.Services;
using Core.Exceptions;
using Core.ObjectMapping;
using Membership.Core.Entities;
using Membership.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Membership.Library.Services
{
    public interface IUserProfileDynamicPropertyService : IDynamicPropertyService, IDependency
    {
        IEnumerable<DynamicPropertyModel> GetAllProfilePropertiesForUser(long userId);
    }

    public class UserProfileDynamicPropertyService : DynamicPropertyService, IUserProfileDynamicPropertyService
    {
        private readonly IRepository _membershipRepository;

        public UserProfileDynamicPropertyService(IMappingService mappingService,
            IDynamicPropertyRepository repository,
            IRepository membershipRepository)
            : base(mappingService, repository)
        {
            _membershipRepository = membershipRepository;
        }

        public override IEnumerable<DynamicPropertyModel> GetDynamicPropertiesForConfig(long configId)
        {
            var result = GetDyamicPropertiesForEntity<UserProfile>();

            if (configId != 0)
            {
                var propertyIds =
                    _membershipRepository.Fetch<UserRoleProfileProperty>(x => x.UserRoleId == configId)
                                                .Select(x => x.PropertyId)
                                                .ToArray();

                result = result.Where(x => propertyIds.Contains(x.Id) || x.AvailableToAllEntities);
            }

            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(result);
        }

        public override void AssignToConfig(long propertyId, long configId)
        {
            var entity = GetEntity<UserProfile>(propertyId);

            var exists =
                _membershipRepository.Any<UserRoleProfileProperty>(
                    x => x.UserRoleId == configId && x.PropertyId == propertyId);

            if (exists)
                throw new InvalidOperationException("Selected dynamic property already assigned to selected role");


            _membershipRepository.Insert(new UserRoleProfileProperty
            {
                UserRoleId = configId,
                PropertyId = entity.Id
            });
            _membershipRepository.SaveChanges();
        }

        public override void RemoveFromConfig(long propertyId, long configId)
        {
            _membershipRepository.DeleteByCondition<UserRoleProfileProperty>(
                x => x.UserRoleId == configId && x.PropertyId == propertyId);
            _membershipRepository.SaveChanges();
        }

        public override IEnumerable<DynamicPropertyModel> GetFilterableProperties(long? configId)
        {
            var productDynamicProperties = GetFilterableProperties<UserProfile>();

            if (configId.HasValue)
            {
                var propertyIds =
                    _membershipRepository.Fetch<UserRoleProfileProperty>(x => x.UserRoleId == configId)
                                                .Select(x => x.PropertyId)
                                                .ToArray();

                productDynamicProperties =
                    productDynamicProperties.Where(x => propertyIds.Contains(x.Id) || x.AvailableToAllEntities);
            }

            return MappingService.Map<IEnumerable<DynamicPropertyModel>>(productDynamicProperties);
        }

        public IEnumerable<DynamicPropertyModel> GetAllProfilePropertiesForUser(long userId)
        {
            var user = _membershipRepository.Get<User>(userId);
            if (user == null)
                throw new BaseNotFoundException<User>();

            var properties = user.Roles.SelectMany(x => GetDynamicPropertiesForConfig(x.Id)).Distinct();

            return properties;
        }
    }
}