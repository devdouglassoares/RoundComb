using Core;
using Core.SiteSettings;
using PaymentGateway.Stripe.Core.Settings;
using Stripe;
using System;

namespace PaymentGateway.Stripe.Core.Services
{
    public interface IStripeChargeService : IDependency
    {
        bool ProcessChargeCustomer(int amount, string currency, string description, string stripeCustomerId, out string error);

        bool ProcessChargeWithToken(int amount, string currency, string description, string token, out string error);
    }

    public class StripeChargeServiceImpl : IStripeChargeService
    {
        private readonly ISiteSettingService _siteSettingService;

        private StripeIntegrationSetting _stripeIntegrationSetting;

        private StripeIntegrationSetting StripeIntegrationSetting
        {
            get { return _stripeIntegrationSetting ?? (_stripeIntegrationSetting = _siteSettingService.GetSetting<StripeIntegrationSetting>()); }
        }

        private StripeChargeService _stripeChargeService;

        private StripeChargeService StripeChargeService => _stripeChargeService ?? (_stripeChargeService = new StripeChargeService(StripeIntegrationSetting.GetSecretKey()));

        public StripeChargeServiceImpl(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public bool ProcessChargeCustomer(int amount, string currency, string description, string stripeCustomerId, out string error)
        {
            if (string.IsNullOrEmpty(stripeCustomerId))
            {
                throw new ArgumentNullException("stripeCustomerId", "Stripe customer Id must not be null");
            }

            var options = new StripeChargeCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Description = description,
                CustomerId = stripeCustomerId
            };

            var result = StripeChargeService.Create(options);

            if (result.Captured != null && result.Captured.Value)
            {
                error = null;
                return true;
            }
            error = result.FailureMessage;
            return false;
        }

        public bool ProcessChargeWithToken(int amount, string currency, string description, string token, out string error)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token", "Token must not be null");
            }

            var options = new StripeChargeCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Description = description,
                SourceTokenOrExistingSourceId = token
            };

            var result = StripeChargeService.Create(options);

            if (result.Captured != null && result.Captured.Value)
            {
                error = null;
                return true;
            }
            error = result.FailureMessage;
            return false;
        }
    }
}
