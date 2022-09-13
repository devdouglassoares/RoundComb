using Core.Database;
using Core.ObjectMapping;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Dto;
using Membership.Core.Entities;

namespace Membership.Core.Services
{
    public class ContactService : BaseService<Contact, ContactDto>, IContactService
    {
        public ContactService(IMappingService mappingService, ICoreRepository repository) : base(mappingService, repository)
        {
        }
    }
}