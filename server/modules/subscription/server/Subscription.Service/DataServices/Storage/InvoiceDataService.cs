using Subscription.Core.Entities;
using Subscription.Core.Repositories;
using Subscription.Service.DataServices.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Subscription.Service.DataServices.Storage
{
    /// <summary>
    ///     Implementation for CRUD related to invoices in the database.
    /// </summary>
    public class InvoiceDataService : IInvoiceDataService
    {
        private readonly IRepository _repository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvoiceDataService" /> class.
        /// </summary>
        /// <param name="repository"></param>
        public InvoiceDataService(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Returns all the invoice given a user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>List of invoices</returns>
        public async Task<List<SubscriptionInvoice>> UserInvoicesAsync(long userId)
        {
            return
                await _repository.Fetch<SubscriptionInvoice>(i => i.CustomerId == userId).Select(s => s).ToListAsync();
        }

        /// <summary>
        ///     Creates the or update asynchronous.
        /// </summary>
        /// <param name="subscriptionInvoice">The invoice.</param>
        /// <returns>
        ///     int
        /// </returns>
        public int CreateOrUpdateInvoice(SubscriptionInvoice subscriptionInvoice)
        {
            var res = -1;

            var dbInvoice = _repository.First<SubscriptionInvoice>(i => i.ExternalId == subscriptionInvoice.ExternalId && i.CustomerId == subscriptionInvoice.CustomerId);

            if (dbInvoice == null)
            {
                // TODO: This method seems not working in Paypal payment
                //var subscription = await this._repository.Query<Library.Entities.Subscription.Subscription>().Where(u => u.ExternalCustomerId == invoice.ExternalCustomerId).FirstOrDefaultAsync();
                //if (subscription != null)
                //{
                //    invoice.Customer = subscription.User;
                //    invoice.StatementDescriptor = subscription.SubscriptionPlan.Name;
                //    this._repository.Insert(invoice);
                //    this._repository.SaveChanges();
                //    res = 0;
                //}
                _repository.Insert(subscriptionInvoice);
                _repository.SaveChanges();
                res = 0;
            }
            else
            {
                dbInvoice.Paid = subscriptionInvoice.Paid;
                dbInvoice.Attempted = subscriptionInvoice.Attempted;
                dbInvoice.AttemptCount = subscriptionInvoice.AttemptCount;
                dbInvoice.NextPaymentAttempt = subscriptionInvoice.NextPaymentAttempt;
                dbInvoice.Closed = subscriptionInvoice.Closed;
                _repository.SaveChanges();
                res = 0;
            }

            return res;
        }
    }
}