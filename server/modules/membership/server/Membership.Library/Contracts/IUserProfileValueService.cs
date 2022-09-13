using Core;
using Core.DynamicProperties.Dtos;

namespace Membership.Library.Contracts
{
    public interface IUserProfileValueService : IDependency
    {
        DynamicPropertyValuesModel GetExtendedProfile(long profileId);
    }
}