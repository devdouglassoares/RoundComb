using Core.DynamicProperties.Services;
using Core.Events;
using Core.Exceptions;
using Core.Extensions;
using Core.ObjectMapping;
using Core.SiteSettings;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Entities;
using Membership.Core.Events;
using Membership.Library.Contracts;
using Membership.Library.Data;
using Membership.Library.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Membership.Library.Services
{
    public class UserProfileService : IUserProfileService, IConsumer<OnUserRegisteredEvent>
    {
        private readonly IDynamicPropertyValueService _dynamicPropertyValueService;
        private readonly IMappingService _mappingService;
        private readonly IMembership _membership;
        private readonly IRepository _repository;
        private readonly ISiteSettingService _siteSettingService;
        private readonly IUserService _userService;

        public UserProfileService(IRepository repository,
                                  IMappingService mappingService,
                                  IUserService userService,
                                  IMembership membership,
                                  ISiteSettingService siteSettingService,
                                  IDynamicPropertyValueService dynamicPropertyValueService)
        {
            _repository = repository;
            _mappingService = mappingService;
            _userService = userService;
            _membership = membership;
            _siteSettingService = siteSettingService;
            _dynamicPropertyValueService = dynamicPropertyValueService;
        }

        public UserProfileModel GetUserProfileForUser(long userId)
        {
            var profile = TryGetUserProfile(userId);

            var profileModel = _mappingService.Map<UserProfileModel>(profile);

            profileModel.ExtendedProperties = _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<UserProfile>(profileModel.Id);

            return profileModel;
        }

        public void UpdateUserProfileForUser(long userId, UserProfileModel model)
        {
            var profile = TryGetUserProfile(userId);

            model.CopyTo(profile, ignoredProperties: profileModel => new
            {
                profileModel.Id
            });

            if (profile.User.FirstName != model.FirstName || profile.User.LastName != model.LastName)
            {
                profile.User.FirstName = model.FirstName;
                profile.User.LastName = model.LastName;
                profile.User.LastActivityDate = DateTime.UtcNow;
                profile.User.CellPhoneNumber = model.PhoneNumber;
            }
            if (profile.Birthday != model.Birthday)
            {
                profile.Birthday = model.Birthday;
            }

            _dynamicPropertyValueService.UpdateEntityAdditionalFields<UserProfile>(profile.Id, model.ExtendedProperties);
            profile.ModifiedDate = DateTimeOffset.Now;
            profile.ModifiedBy = _membership.Name;
            _repository.Update(profile);
            _repository.SaveChanges();
        }

        private UserProfile TryGetUserProfile(long userId)
        {
            var user = _userService.GetUser(userId);
            if (user == null || user.IsDeleted)
                throw new BaseNotFoundException<User>();

            var profile = _repository.Get<UserProfile>(userProfile => userProfile.UserId == userId);

            if (profile != null) return profile;

            profile = new UserProfile
            {
                UserId = userId,
                CreatedDate = DateTimeOffset.Now,
                CreatedBy = _membership.Name,
                ModifiedDate = DateTimeOffset.Now,
                ModifiedBy = _membership.Name
            };
            _repository.Insert(profile);
            _repository.SaveChanges();

            // reload profile
            return _repository.Get<UserProfile>(userProfile => userProfile.UserId == userId);
        }

        public int Order => 10;

        public void HandleEvent(OnUserRegisteredEvent eventMessage)
        {
            var profile = new UserProfile
            {
                UserId = eventMessage.User.Id
            };
            _repository.Insert(profile);
            _repository.SaveChanges();
        }

        public void FillUserAddresses()
        {
            var usersWithEmptyAddresses = _repository.GetAll<User>().Where(x => string.IsNullOrEmpty(x.Address)).ToList();
            foreach (var user in usersWithEmptyAddresses)
            {
                var userProfile = _repository.Get<UserProfile>(x => x.UserId == user.Id);
                var parameters = new List<string>();
                if (userProfile == null) continue;
                if (!string.IsNullOrEmpty(userProfile.ZipCode))
                {
                    parameters.Add(userProfile.ZipCode);
                }
                if (!string.IsNullOrEmpty(userProfile.Address))
                {
                    parameters.Add(userProfile.Address);
                }
                if (!string.IsNullOrEmpty(userProfile.City))
                {
                    parameters.Add(userProfile.City);
                }
                if (!string.IsNullOrEmpty(userProfile.State))
                {
                    parameters.Add(userProfile.State);
                }
                if (!string.IsNullOrEmpty(userProfile.Country))
                {
                    parameters.Add(userProfile.Country);
                }
                if (parameters.Count == 0) continue;
                user.Address = String.Join(",", parameters);
                if (!string.IsNullOrEmpty(user.Address))
                {
                    _repository.Update(user);
                }
            }
            _repository.SaveChanges();
        }
    }
}