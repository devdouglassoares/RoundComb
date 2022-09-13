using Core.Database;
using Core.DynamicProperties.Dtos;
using Core.DynamicProperties.Models;
using Core.DynamicProperties.Services;
using Core.Extensions;
using Core.ObjectMapping;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using CustomForm.Data.Repositories;
using Membership.Core;
using Membership.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomForm.Services
{
    public class FormInstanceService : BaseService<FormInstance, FormInstanceDto>, IFormInstanceService
    {
        private readonly IMembership _membership;
        private readonly IUserService _userService;
        private readonly IDynamicPropertyValueService _dynamicPropertyValueService;

        public FormInstanceService(IMappingService mappingService,
                                   IRepository repository,
                                   IMembership membership,
                                   IUserService userService, IDynamicPropertyValueService dynamicPropertyValueService)
            : base(mappingService, repository)
        {
            _membership = membership;
            _userService = userService;
            _dynamicPropertyValueService = dynamicPropertyValueService;

            SynchronizeDeletion();
        }

        private void SynchronizeDeletion()
        {
            var inactiveUsers = GetInactiveUsers();

            var formInstances = Repository.Fetch<FormInstance>(fi => inactiveUsers.Contains(fi.UserId));

            if (!formInstances.Any())
                return;

            foreach (var formInstance in formInstances)
            {
                formInstance.IsDeleted = true;
                Repository.Update(formInstance);
            }

            Repository.SaveChanges();
        }

        private long[] GetInactiveUsers()
        {
            var inactiveUsers = _userService.QueryUsers()
                                            .Where(user => !user.IsActive || !user.IsApproved || user.IsDeleted)
                                            .Select(x => x.Id)
                                            .ToArray();
            return inactiveUsers;
        }

        public override FormInstanceDto ToDto(FormInstance entity)
        {
            var formInstanceDto = base.ToDto(entity);

            formInstanceDto.Answers = GetExtendedPropertyValuesForEntity(entity.Id);

            formInstanceDto.UserInformation = _userService.GetUserPersonalInformation(formInstanceDto.UserId);
			
	        formInstanceDto.ExtendedAnswers = _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<FormInstance>(formInstanceDto.Id);

			return formInstanceDto;
        }


        public IEnumerable<FormInstance> QueryFormByCode(string formCode, Dictionary<long, DynamicPropertyFilterModel> dynamicPropertyFilters)
        {
            var inactiveUsers = GetInactiveUsers();

            var results = Fetch(x => !x.IsDeleted && !inactiveUsers.Contains(x.UserId) && x.FormConfiguration.FormCode == formCode)
                .GroupBy(fi => fi.UserId)
                .Select(fiGroupped => fiGroupped.FirstOrDefault());

            if (dynamicPropertyFilters != null && dynamicPropertyFilters.Any() && dynamicPropertyFilters.Values.Any(filter => filter.HasFilter))
            {
                var propertyIdsFromDynamicProps = _dynamicPropertyValueService.GetEntitiesHasValues<FormInstance>(dynamicPropertyFilters).ToArray();

                results = results.Where(x => propertyIdsFromDynamicProps.Contains(x.Id));
            }

            return results;
        }

        public override FormInstance PrepareForInserting(FormInstance entity, FormInstanceDto model)
        {
            entity = base.PrepareForInserting(entity, model);

            entity.UserId = _membership.UserId;
            entity.CreatedBy = _membership.Name;

            return entity;
        }

        public override FormInstance Create(FormInstanceDto model)
        {
            var formInstance = base.Create(model);

            UpdateEntityAdditionalFields(formInstance.Id, model.Answers);
            UpdatePropertyAdditionalFields(formInstance, model);

            return formInstance;
        }

        public override FormInstance PrepareForUpdating(FormInstance entity, FormInstanceDto model)
        {
            if (entity.UserId != _membership.UserId)
                throw new UnauthorizedAccessException("Cannot modify form instance that does not belong to current user");

            UpdateEntityAdditionalFields(entity.Id, model.Answers);

            return entity;
        }

        public override FormInstance Update(FormInstanceDto model, params object[] keys)
        {
            var formInstance = base.Update(model, keys);

            UpdatePropertyAdditionalFields(formInstance, model);

            return formInstance;
        }


        private DynamicPropertyValuesModel GetExtendedPropertyValuesForEntity(long entityId)
        {
            var propertyPropertyValues = Repository.Fetch<FormFieldValue>(x => x.FormInstanceId == entityId);

            var dto = new DynamicPropertyValuesModel
            {
                ExternalEntityId = entityId
            };

            if (!propertyPropertyValues.Any())
            {
                return dto;
            }

            var dynamicPropertyValues = propertyPropertyValues.ToArray();
            foreach (var dynamicPropertyValue in dynamicPropertyValues)
            {
                dto[dynamicPropertyValue.Field.FieldName] = MappingService.Map<DynamicPropertyValueDto>(dynamicPropertyValue);
            }

            return dto;
        }

        private void UpdateEntityAdditionalFields(long entity, DynamicPropertyValuesModel model)
        {
            if (model == null) return;

            foreach (var extendedProperty in model.Keys)
            {
                var property =
                    Repository.First<FormField>(x => !x.IsDeleted && x.FieldName.Equals(extendedProperty.ToString()));
                if (property == null)
                    continue;

                var formFieldValue =
                    Repository.First<FormFieldValue>(x => x.FormInstanceId == entity && x.FieldId == property.Id);

                if (formFieldValue != null)
                {
                    model[extendedProperty].CopyTo(formFieldValue, true,
                                                   dto => new
                                                   {
                                                       dto.Property
                                                   });
                    Repository.Update(formFieldValue);
                }
                else
                {
                    formFieldValue = new FormFieldValue
                    {
                        FormInstanceId = entity,
                        FieldId = property.Id
                    };

                    model[extendedProperty].CopyTo(formFieldValue, true);
                    Repository.Insert(formFieldValue);
                }

                // validation SSN field.
                if (property.PropertyType == PropertyType.Ssn)
                {
                    var ssnValue = formFieldValue.StringValue;
                    if (string.IsNullOrEmpty(ssnValue))
                        continue;

                    var existingSsnFieldValue =
                        Repository.Any<FormFieldValue>(
                                                       x =>
                                                       x.FormInstance.UserId != _membership.UserId &&
                                                       x.StringValue == ssnValue);

                    if (existingSsnFieldValue)
                        throw new InvalidOperationException("The SSN already exists. Please enter unique SSN.");

                }
            }
            Repository.SaveChanges();
        }


        protected virtual void UpdatePropertyAdditionalFields(FormInstance entity, FormInstanceDto model)
        {
            _dynamicPropertyValueService.UpdateEntityAdditionalFields<FormInstance>(entity.Id, model.ExtendedAnswers);
        }

        public override IEnumerable<FormInstanceDto> ToDtos(IEnumerable<FormInstance> entities, bool wireup = true)
        {
            var formInstanceDtos = base.ToDtos(entities, wireup).ToArray();

            var formUsers = formInstanceDtos.Select(x => x.UserId).ToArray();

            var availableUsers = _userService.GetUsers()
                                             .Where(x => !x.IsDeleted && formUsers.Contains(x.Id))
                                             .Select(x => x.Id)
                                             .ToArray()
                                             .Select(id => _userService.GetUserPersonalInformation(id))
                                             .ToArray();

            foreach (var formInstanceDto in formInstanceDtos)
            {
                formInstanceDto.ExtendedAnswers = _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<FormInstance>(formInstanceDto.Id);
                formInstanceDto.UserInformation = availableUsers.FirstOrDefault(x => x.Id == formInstanceDto.UserId);
            }

            return formInstanceDtos.Where(x => x.UserInformation != null).AsQueryable();
        }
    }
}