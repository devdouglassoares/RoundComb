using System.Collections.Generic;
using Core;
using Core.Exceptions;
using Membership.Core.Dto;

namespace Membership.Core.Contracts
{
    public interface IClientCompanyService : IDependency
    {
        IEnumerable<ClientCompanyDto> GetAll();

        /// <summary>
        /// Get Client Company with specified identifier
        /// </summary>
        /// <param name="clientCompanyId">Identifier of client company</param>
        /// <exception cref="BaseNotFoundException{T}"></exception>
        /// <returns></returns>
        ClientCompanyWithOwnerDto GetById(long clientCompanyId);

        void CreateClientCompany(ClientCompanyWithOwnerDto model);

        void Update(long clientCompanyId, ClientCompanyDto model);

        void Delete(long clientCompanyId);
    }
}