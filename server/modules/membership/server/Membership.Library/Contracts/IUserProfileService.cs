using Core;
using Membership.Library.Dto;

namespace Membership.Library.Contracts
{
    public interface IUserProfileService : IDependency
    {
        UserProfileModel GetUserProfileForUser(long userId);

        void UpdateUserProfileForUser(long userId, UserProfileModel model);

        void FillUserAddresses();
    }
}