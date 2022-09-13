using Core.ObjectMapping;
using Membership.Core;
using Subscription.Core.Dtos;
using Subscription.Core.Entities;
using Subscription.Core.Repositories;
using Subscription.Service.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Subscription.Service.Implementations
{
    public class InvoiceServices : IInvoiceServices
    {
        private readonly IMembership _membership;
        private readonly IRepository _repository;
        private readonly IMappingService _mappingService;

        public InvoiceServices(IMembership membership, IRepository repository, IMappingService mappingService)
        {
            _membership = membership;
            _repository = repository;
            _mappingService = mappingService;
        }

        public IEnumerable<InvoiceDto> GetInvoicesForCurrentUser()
        {
            var invoices = _repository.Fetch<SubscriptionInvoice>(x => x.CustomerId == _membership.UserId).ToArray();
            return _mappingService.Map<IEnumerable<InvoiceDto>>(invoices);
        }
    }
}