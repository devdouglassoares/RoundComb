using Core;
using Subscription.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscription.Service.DataServices.Interfaces
{
    /// <summary>
    /// Interface for CRUD related to invoices in the database.
    /// </summary>
    public interface IInvoiceDataService : IDependency
    {
        /// <summary>
        /// Gets the User's invoices asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>List of invoices.</returns>
        Task<List<SubscriptionInvoice>> UserInvoicesAsync(long userId);

        /// <summary>
        /// Creates the or update asynchronous.
        /// </summary>
        /// <param name="subscriptionInvoice">The invoice.</param>
        /// <returns>int</returns>
        int CreateOrUpdateInvoice(SubscriptionInvoice subscriptionInvoice);
    }
}
