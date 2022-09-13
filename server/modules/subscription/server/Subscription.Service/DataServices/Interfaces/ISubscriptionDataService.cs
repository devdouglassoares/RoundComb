using Core;
using Subscription.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Service.DataServices.Interfaces
{
    /// <summary>
    ///     Interface for CRUD related to subscriptions in the database.
    /// </summary>
    public interface ISubscriptionDataService : IDependency
    {
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
        UserSubscription SubscribeUserToSubscriptionPlan(long userId, string planId, int? trialPeriodInDays = null,
                                                         decimal taxPercent = 0, string stripeId = null);

        /// <summary>
        ///     Get the User's active subscription asynchronous. Only the first (valid if your customers can have only 1
        ///     subscription at a time).
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The subscription</returns>
        UserSubscription GetCurrentActiveSubscriptionForUser(long userId);

        /// <summary>
        ///     Get the User's active subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The list of Subscriptions</returns>
        IQueryable<UserSubscription> GetActiveSubscriptionsForUser(long userId);

        /// <summary>
        ///     Ends the subscription asynchronous.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="subscriptionEnDateTime">The subscription en date time.</param>
        /// <param name="reasonToCancel">The reason to cancel.</param>
        /// <returns></returns>
        void EndSubscription(int subscriptionId, DateTime subscriptionEnDateTime, string reasonToCancel);

        /// <summary>
        ///     Updates the subscription asynchronous.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns></returns>
        void UpdateSubscription(UserSubscription subscription);

        /// <summary>
        ///     Updates the subscription tax.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="taxPercent">The tax percent.</param>
        /// <returns></returns>
        Task UpdateSubscriptionTax(string subscriptionId, decimal taxPercent);

        /// <summary>
        ///     Deletes the subscriptions asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        void DeleteSubscriptionsAsync(long userId);
    }
}