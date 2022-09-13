using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Services;
using Membership.Core.Entities;
using Membership.Library.Contracts;

namespace Membership.Library.Services
{
    public class UserProfileValueService : IUserProfileValueService
    {
        private readonly IDynamicPropertyValueService _dynamicPropertyValueService;

        public UserProfileValueService(IDynamicPropertyValueService dynamicPropertyValueService)
        {
            _dynamicPropertyValueService = dynamicPropertyValueService;
        }

        public DynamicPropertyValuesModel GetExtendedProfile(long profileId)
        {
            return _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<UserProfile>(profileId);
        }
    }
}