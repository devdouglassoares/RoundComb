using Core;
using Core.Database;
using Membership.Core.Dto;
using Membership.Core.Entities;

namespace Membership.Core.Contracts
{
    public interface IContactService : IBaseService<Contact, ContactDto>, IDependency
    {

    }
}