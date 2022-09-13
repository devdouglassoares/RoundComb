using Stripe;
using Subscription.Core.Entities;

namespace Subscription.Service.Infrastructure
{
    /// <summary>
    /// Mapper for Stripe classes to SaasEcom classes
    /// </summary>
    public class Mappers
    {
        #region Stripe to SaasEcom Mapper
        public static SubscriptionInvoice Map(StripeInvoice stripeInvoice)
        {
            var invoice = new SubscriptionInvoice
            {
                AmountDue = stripeInvoice.AmountDue,
                ApplicationFee = stripeInvoice.ApplicationFee,
                AttemptCount = stripeInvoice.AttemptCount,
                Attempted = stripeInvoice.Attempted,
                Closed = stripeInvoice.Closed,
                Currency = stripeInvoice.Currency,
                Date = stripeInvoice.Date,
                Description = stripeInvoice.Description,
                // Discount = Map(stripeInvoice.StripeDiscount),
                EndingBalance = stripeInvoice.EndingBalance,
                Forgiven = stripeInvoice.Forgiven ?? false,
                NextPaymentAttempt = stripeInvoice.NextPaymentAttempt,
                Paid = stripeInvoice.Paid,
                PeriodStart = stripeInvoice.PeriodStart,
                PeriodEnd = stripeInvoice.PeriodEnd,
                ReceiptNumber = stripeInvoice.ReceiptNumber,
                StartingBalance = stripeInvoice.StartingBalance,
                ExternalCustomerId = stripeInvoice.CustomerId,
                StatementDescriptor = stripeInvoice.StatementDescriptor,
                Tax = stripeInvoice.Tax,
                TaxPercent = stripeInvoice.TaxPercent,
                ExternalId = stripeInvoice.Id,
                Subtotal = stripeInvoice.Subtotal,
                Total = stripeInvoice.Total
            };

            return invoice;
        }

        #endregion

        public static SubscriptionInvoice Map(StripeCharge stripeInvoice)
        {
            var invoice = new SubscriptionInvoice
            {
                AmountDue = stripeInvoice.Amount,
                Closed = stripeInvoice.Paid,
                Currency = stripeInvoice.Currency,
                Date = stripeInvoice.Created,
                Description = stripeInvoice.Description,
                Paid = stripeInvoice.Paid,
                ReceiptNumber = stripeInvoice.ReceiptNumber,
                ExternalCustomerId = stripeInvoice.CustomerId,

                StatementDescriptor = stripeInvoice.StatementDescriptor,
                ExternalId = stripeInvoice.Id,
                Total = stripeInvoice.Amount
            };

            return invoice;
        }
    }
}
