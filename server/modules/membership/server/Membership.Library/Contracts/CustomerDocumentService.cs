using Core;
using Core.Events;
using Core.ObjectMapping;
using DocumentsManagement.Library.Services;
using Membership.Library.Data;
using Membership.Library.Dto.Customer;
using Membership.Library.Entities;

namespace Membership.Library.Contracts
{
    public interface ICustomerDocumentService : IBaseDocumentsService<CompanyDocumentDto, CompanyDocument>, IDependency
    {

    }

    public class CustomerDocumentService : BaseDocumentsService<CompanyDocumentDto, CompanyDocument>,
                                           ICustomerDocumentService
    {
        public CustomerDocumentService(IRepository repository, IMappingService mappingService, IEventPublisher eventPublisher) :
            base(repository, mappingService, eventPublisher)
        {
        }
    }
}