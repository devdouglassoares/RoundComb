using System;
using System.Collections.Generic;
using System.Linq;
using Core.Exceptions;
using Core.ObjectMapping;
using Membership.Core.Contracts;
using Membership.Core.Data;
using Membership.Core.Dto;
using Membership.Core.Entities;
using Membership.Core.Exceptions;

namespace Membership.Core.Services
{
    public class ClientCompanyService : IClientCompanyService
    {
        private readonly ICoreRepository _repository;
        private readonly IMappingService _mappingService;
        private readonly IUserService _userService;
        private readonly IMembership _membership;

        public ClientCompanyService(ICoreRepository repository,
                                    IMappingService mappingService,
                                    IUserService userService,
                                    IMembership membership)
        {
            _repository = repository;
            _mappingService = mappingService;
            _userService = userService;
            _membership = membership;
        }

        public IEnumerable<ClientCompanyDto> GetAll()
        {
            var currentCompany = _membership.GetCurrentBizOwner();

            return _mappingService.Map<IEnumerable<ClientCompanyDto>>(_repository.GetAll<Company>()
                              .Where(x => x.Id != currentCompany.Id)
                              .Where(x => !x.IsDeleted));
        }

        public void CreateClientCompany(ClientCompanyWithOwnerDto model)
        {
            var existingCompany = _repository.Get<Company>(x => x.Name == model.Name);

            if (existingCompany != null)
                throw new CompanyNameInUsedException();

            var clientCompany = _mappingService.Map<Company>(model);
            model.MainContactUser.Roles = new List<string>() { MembershipConstant.C_ROLE_CODE_COMPANYCLIENT };
            var owner = _userService.Register(model.MainContactUser);

            var createdUser = _userService.GetUser(owner.Id);
            createdUser.ClientCompany = clientCompany;

            clientCompany.MainContactUser = createdUser;
            clientCompany.ModifiedDate = DateTimeOffset.Now;
            clientCompany.LastModifiedBy = _membership.Name;

            _repository.Insert(clientCompany);
            _repository.SaveChanges();
        }

        public void Update(long clientCompanyId, ClientCompanyDto model)
        {
            var clientCompany = _repository.Get<Company>(x => x.Id == clientCompanyId && !x.IsDeleted);
            if (clientCompany == null)
                throw new BaseNotFoundException<Company>();

            _mappingService.Map(model, clientCompany);
            clientCompany.ModifiedDate = DateTimeOffset.Now;
            clientCompany.LastModifiedBy = _membership.Name;

            _repository.Update(clientCompany);
            _repository.SaveChanges();
        }

        public void Delete(long clientCompanyId)
        {
            var clientCompany = _repository.Get<Company>(x => x.Id == clientCompanyId);
            if (clientCompany == null)
                throw new BaseNotFoundException<Company>();

            clientCompany.IsDeleted = true;
            _repository.Update(clientCompany);
            _repository.SaveChanges();
        }

        public ClientCompanyWithOwnerDto GetById(long clientCompanyId)
        {
            var clientCompany = _repository.Get<Company>(x => x.Id == clientCompanyId);
            if (clientCompany == null)
                throw new BaseNotFoundException<Company>();

            return _mappingService.Map<ClientCompanyWithOwnerDto>(clientCompany);
        }
    }
}