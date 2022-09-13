using System.Collections.Generic;
using Core;
using Subscription.Core.Dtos;

namespace Subscription.Service.Contracts
{
    public interface IInvoiceServices: IDependency
    {
        IEnumerable<InvoiceDto> GetInvoicesForCurrentUser();
    }
}