using Core;
using Core.SiteSettings;
using Membership.Core.Entities;
using PaymentGateway.Stripe.Core.Settings;
using Stripe;
using System;

namespace PaymentGateway.Stripe.Core.Services
{
    /// <summary>
    /// Interface for CRUD related to customers with Stripe
    /// </summary>
    public interface IStripeCustomerProvider : IDependency
    {
        /// <summary>
        /// Creates the customer asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="trialEnd">The trial end.</param>
        /// <param name="cardToken">The card token.</param>
        /// <returns></returns>
        StripeCustomer CreateStripeCustomer(User user, DateTime? trialEnd = null, string cardToken = null);

        /// <summary>
        /// Updates the customer.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cardToken">The card.</param>
        /// <param name="externalCustomerId">The externalCustomerId.</param>
        /// <returns></returns>
        StripeCustomer UpdateCustomer(User user, string cardToken, string externalCustomerId);

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        /// <param name="externalCustomerId">The user.</param>
        /// <returns></returns>
        void DeleteCustomer(string externalCustomerId);
    }


    /// <summary>
    /// Interface for CRUD related to customers with Stripe
    /// </summary>
    public class StripeCustomerProvider : IStripeCustomerProvider
    {
        // Stripe Dependencies
        private StripeCustomerService _customerService;

        private readonly ISiteSettingService _siteSettingService;

        private StripeIntegrationSetting _stripeIntegrationSetting;

        private StripeIntegrationSetting StripeIntegrationSetting
        {
            get { return _stripeIntegrationSetting ?? (_stripeIntegrationSetting = _siteSettingService.GetSetting<StripeIntegrationSetting>()); }
        }

        private StripeCustomerService CustomerService => _customerService ?? (_customerService = new StripeCustomerService(StripeIntegrationSetting.GetSecretKey()));

        /// <summary>
        /// Initializes a new instance of the <see cref="StripeCustomerProvider"/> class.
        /// </summary>
        public StripeCustomerProvider(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        /// <summary>
        /// Creates the customer asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="trialEnd">The trial end.</param>
        /// <param name="cardToken">The card token.</param>
        /// <returns></returns>
        public StripeCustomer CreateStripeCustomer(User user, DateTime? trialEnd = null, string cardToken = null)
        {
            var customer = new StripeCustomerCreateOptions
            {
                AccountBalance = 0,
                Email = user.Email
            };

            if (!string.IsNullOrEmpty(cardToken))
            {
                //customer.Source = new StripeSourceOptions
                //{
                //    TokenId = cardToken
                //};

                customer.SourceToken = cardToken;
            }

            var stripeUser = CustomerService.Create(customer);
            return stripeUser;
        }

        /// <summary>
        /// Updates the customer.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cardToken">The card.</param>
        /// <param name="externalCustomerId">The externalCustomerId.</param>
        /// <returns></returns>
        public StripeCustomer UpdateCustomer(User user, string cardToken, string externalCustomerId)
        {
            var customer = new StripeCustomerUpdateOptions
            {
                Email = user.Email,

                SourceToken = cardToken
                //// Card Details
                //Source = new StripeSourceOptions
                //    {
                //        TokenId = cardToken
                //    }
            };

            return _customerService.Update(externalCustomerId, customer);
        }

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        /// <param name="externalCustomerId">The user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DeleteCustomer(string externalCustomerId)
        {
            _customerService.Delete(externalCustomerId);
        }
    }
}
