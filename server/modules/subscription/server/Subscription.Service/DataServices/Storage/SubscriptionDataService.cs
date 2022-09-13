using Core.Exceptions;
using Membership.Core.Contracts;
using Subscription.Core.Entities;
using Subscription.Core.Repositories;
using Subscription.Service.DataServices.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Service.DataServices.Storage
{
    /// <summary>
    ///     Implementation for CRUD related to subscriptions in the database.
    /// </summary>
    public class SubscriptionDataService : ISubscriptionDataService
    {
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly IRepository _repository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubscriptionDataService" /> class.
        /// </summary>
        public SubscriptionDataService(IRepository repository, IPermissionService permissionService, IRoleService roleService)
        {
            _repository = repository;
            _permissionService = permissionService;
            _roleService = roleService;
        }

        /// <summary>
        ///     Subscribes the user asynchronous.
        /// </summary>
        /// <param name="userId">The user.</param>
        /// <param name="planId">The plan identifier.</param>
        /// <param name="trialPeriodInDays">The trial period in days.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <param name="stripeId">The stripe identifier.</param>
        /// <returns>
        ///     The subscription
        /// </returns>
        /// <exception cref="BaseNotFoundException{T}"></exception>
        public UserSubscription SubscribeUserToSubscriptionPlan(long userId,
                                                                string planId,
                                                                int? trialPeriodInDays = null,
                                                                decimal taxPercent = 0,
                                                                string stripeId = null)
        {
            var plan = _repository.First<SubscriptionPlan>(x => x.Id.ToString() == planId);

            if (plan == null)
                throw new BaseNotFoundException<SubscriptionPlan>();


            var userSubscription = new UserSubscription
            {
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddMonths((int)plan.Interval),
                TrialStart = DateTime.UtcNow,
                TrialEnd = plan.IsTrialSubscription
                                       ? DateTime.UtcNow.AddMonths((int)plan.Interval)
                                       : DateTime.UtcNow.AddDays(trialPeriodInDays ?? plan.TrialPeriodInDays),
                UserId = userId,
                SubscriptionPlan = plan,
                Status = trialPeriodInDays == null ? "active" : "trialing",
                TaxPercent = taxPercent,
                ExternalId = stripeId
            };

            _repository.Insert(userSubscription);
            _repository.SaveChanges();

            if (plan.AssignRoleId != null)
            {
                _roleService.AssignToUser(userId, plan.AssignRoleId.Value);
            }
            else
            {
                foreach (var accessEntity in plan.AccessEntities)
                {
                    _permissionService.AssignToUser(userId, accessEntity.AccessEntityId);

                }
            }

            return userSubscription;
        }

        /// <summary>
        ///     Get the User's active subscription asynchronous. Only the first (valid if your customers can have only 1
        ///     subscription at a time).
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///     The subscription
        /// </returns>
        public UserSubscription GetCurrentActiveSubscriptionForUser(long userId)
        {
            return GetActiveSubscriptionsForUser(userId).FirstOrDefault();
        }

        /// <summary>
        ///     Get the User's active subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///     The list of Subscriptions
        /// </returns>
        public IQueryable<UserSubscription> GetActiveSubscriptionsForUser(long userId)
        {
            return _repository.Fetch<UserSubscription>(
                                                       s =>
                                                       s.UserId == userId && s.Status != "canceled" &&
                                                       s.Status != "unpaid")
                              .Where(s => s.End == null || s.End > DateTime.UtcNow);
        }

        /// <summary>
        ///     Ends the subscription asynchronous.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="subscriptionEnDateTime">The subscription en date time.</param>
        /// <param name="reasonToCancel">The reason to cancel.</param>
        /// <returns></returns>
        public void EndSubscription(int subscriptionId, DateTime subscriptionEnDateTime, string reasonToCancel)
        {
            var dbSub = _repository.Get<UserSubscription>(subscriptionId);
            dbSub.End = subscriptionEnDateTime;
            dbSub.ReasonToCancel = reasonToCancel;
            _repository.SaveChanges();

            // Remove permissions from user
            foreach (var accessEntity in dbSub.SubscriptionPlan.AccessEntities)
            {
                _permissionService.RemoveFromUser(dbSub.UserId, accessEntity.AccessEntityId);
            }
        }

        /// <summary>
        ///     Updates the subscription asynchronous.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns></returns>
        public void UpdateSubscription(UserSubscription subscription)
        {
            _repository.Update(subscription);
            _repository.SaveChanges();

            // TODO: Assign permissions to user
        }

        /// <summary>
        ///     Updates the subscription tax.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task UpdateSubscriptionTax(string subscriptionId, decimal taxPercent)
        {
            var subscription =
                await
                _repository.Fetch<UserSubscription>(s => s.ExternalId == subscriptionId)
                           .FirstOrDefaultAsync();
            subscription.TaxPercent = taxPercent;
            _repository.SaveChanges();
        }

        /// <summary>
        ///     Deletes the subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DeleteSubscriptionsAsync(long userId)
        {
            _repository.DeleteByCondition<UserSubscription>(x => x.UserId == userId);

            _repository.SaveChanges();
        }
    }
}