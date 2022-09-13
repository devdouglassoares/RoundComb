using Membership.Core;
using Membership.Core.Contracts;
using Subscription.Core.Entities;
using Subscription.Core.Repositories;
using Subscription.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Subscription.Service.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IMembership _membership;
        private readonly IRepository _repository;
        private readonly IUserService _userService;

        public SubscriptionService(IRepository repository, IMembership membership, IUserService userService)
        {
            _repository = repository;
            _membership = membership;
            _userService = userService;
        }

        public IEnumerable<UserSubscription> GetCurrentUserSubscriptions()
        {
            var result = _repository.Fetch<UserSubscription>(userSubscription => userSubscription.UserId == _membership.UserId)
                                    .Select(userSubscription => userSubscription);

            return result;
        }

        public UserSubscription GetUserActiveSubscription(long userId)
        {
            var result = _repository.Fetch<UserSubscription>(userSubscription => userSubscription.UserId == userId && userSubscription.Status == "active")
                                    .FirstOrDefault(userSubscription => userSubscription.End == null || userSubscription.End > DateTime.UtcNow);
            return result;
        }

        public IEnumerable<long> GetSubscribedUserIdsByStartDateDescending()
        {
            var inativeUsers = _userService.QueryUsers()
                                             .Where(user => !user.IsActive || !user.IsApproved || user.IsDeleted)
                                             .Select(x => x.Id)
                                             .ToArray();

            var userIds = _repository.GetAll<UserSubscription>()
                                     .Where(userSubscription => !inativeUsers.Contains(userSubscription.UserId) && (userSubscription.End == null || userSubscription.End > DateTime.UtcNow) && userSubscription.Status == "active")
                                     .OrderByDescending(subscription => subscription.Start)
                                     .GroupBy(subscription => subscription.UserId)
                                     .Select(userSubscriptions => userSubscriptions.FirstOrDefault())
                                     .Select(x => x.UserId);

            return userIds;
        }
    }
}