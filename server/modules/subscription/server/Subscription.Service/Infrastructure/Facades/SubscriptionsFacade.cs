using Membership.Core.Entities;
using PaymentGateway.Stripe.Core.Services;
using Subscription.Core.Entities;
using Subscription.Service.Contracts;
using Subscription.Service.DataServices.Interfaces;
using System;

namespace Subscription.Service.Infrastructure.Facades
{
    public class SubscriptionsFacade : ISubscriptionsFacade
    {
        private readonly IStripeChargeService _stripeChargeService;
        private readonly ISubscriptionDataService _subscriptionDataService;
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionsFacade(
            ISubscriptionDataService data,
            ISubscriptionPlanService subscriptionPlanService,
            IStripeChargeService stripeChargeService)
        {
            _subscriptionDataService = data;
            _subscriptionPlanService = subscriptionPlanService;
            _stripeChargeService = stripeChargeService;
        }

        public UserSubscription SubscribeUserAsync(User user, string planId, string creditCardToken = null)
        {
            // Save the subscription in the DB
            var subscription = _subscriptionDataService.SubscribeUserToSubscriptionPlan(user.Id, planId, null);

            if (string.IsNullOrEmpty(creditCardToken))
                return subscription;

            //// Create a new customer in Stripe and subscribe him to the plan
            //var stripeUser = _stripeCustomerProvider.CreateStripeCustomer(user, null, creditCardToken);
            //subscription.ExternalCustomerId = stripeUser.Id; // Add stripe user Id to the user

            //// Save Stripe Subscription Id in the DB
            ////subscription.StripeId = GetStripeSubscriptionIdForNewCustomer(stripeUser);
            //_subscriptionDataService.UpdateSubscription(subscription);

            string error;
            _stripeChargeService.ProcessChargeWithToken(
                Convert.ToInt32(subscription.SubscriptionPlan.Price * 100),
                subscription.SubscriptionPlan.Currency,
                subscription.SubscriptionPlan.Name,
                creditCardToken,
                out error);

            return subscription;
        }

        #region Helpers

        public int CalculateProRata(long planId)
        {
            var plan = _subscriptionPlanService.GetEntity(planId);

            var now = DateTime.UtcNow;
            var beginningMonth = new DateTime(now.Year, now.Month, 1);
            var endMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);

            var totalHoursMonth = (endMonth - beginningMonth).TotalHours;
            var hoursRemaining = (endMonth - now).TotalHours;

            var amountInCurrency = plan.Price * hoursRemaining / totalHoursMonth;

            switch (plan.Currency.ToLower())
            {
                case "usd":
                case "gbp":
                case "eur":
                    return (int)Math.Ceiling(amountInCurrency * 100);
                default:
                    return (int)Math.Ceiling(amountInCurrency);
            }
        }

        public DateTime? GetStartNextMonth()
        {
            var now = DateTime.UtcNow;
            var year = now.Month == 12 ? now.Year + 1 : now.Year;
            var month = now.Month == 12 ? 1 : now.Month + 1;

            return new DateTime(year, month, 1);
        }

        #endregion
    }
}