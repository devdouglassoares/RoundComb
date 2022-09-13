using Core.Database;
using Core.DynamicProperties.Services;
using Core.Events;
using Core.Exceptions;
using Core.ObjectMapping;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using Membership.Core;
using Membership.Core.Contracts;
using Membership.Core.Models;
using Roundcomb.Core.Dtos;
using Roundcomb.Core.Entities;
using Roundcomb.Core.Exceptions;
using Roundcomb.Core.Permissions;
using Roundcomb.Core.Repositories;
using Roundcomb.Core.Services;
using Roundcomb.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roundcomb.Services
{
    public class PropertyApplicationFormInstanceService :
        BaseService<PropertyApplicationFormInstance, PropertyApplicationFormInstanceDto>,
        IPropertyApplicationFormInstanceService
    {
        private readonly IMembership _membership;
        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDynamicPropertyValueService _dynamicPropertyValueService;
        private readonly IPropertyService _propertyService;
        private readonly IRepository _propertyManagementRepository;

        public PropertyApplicationFormInstanceService(IMappingService mappingService,
                                                     IRoundcombRepository repository,
                                                     IMembership membership,
                                                     IUserService userService,
                                                     IEventPublisher eventPublisher,
                                                     IDynamicPropertyValueService dynamicPropertyValueService,
                                                     IPropertyService propertyService, IRepository propertyManagementRepository)
            : base(mappingService, repository)
        {
            _membership = membership;
            _userService = userService;
            _eventPublisher = eventPublisher;
            _dynamicPropertyValueService = dynamicPropertyValueService;
            _propertyService = propertyService;
            _propertyManagementRepository = propertyManagementRepository;
        }

        public void AssignFormConfigurationToPropertyCategory(long formConfigId, long propertyCategoryId)
        {
            Repository.DeleteByCondition<PropertyFormConfigurationSetting>(x => x.PropertyCategoryId == propertyCategoryId);
            Repository.SaveChanges();

            Repository.Insert(new PropertyFormConfigurationSetting
            {
                FormConfigurationId = formConfigId,
                PropertyCategoryId = propertyCategoryId
            });
            Repository.SaveChanges();
        }

        public PropertyFormConfigurationSetting GetFormConfigurationSettingForProperty(long propertyId)
        {
            var property = _propertyManagementRepository.Get<Property>(p => p.Id == propertyId);
            if (property == null)
                throw new BaseNotFoundException<Property>();

            return GetFormConfigurationSettingForPropertyCategory(property.Category.Id);
        }

        public PropertyFormConfigurationSetting GetFormConfigurationSettingForPropertyCategory(long propertyCategoryId)
        {
            var setting = Repository.First<PropertyFormConfigurationSetting>(x => x.PropertyCategoryId == propertyCategoryId);
            if (setting == null)
                throw new BaseNotFoundException<PropertyFormConfigurationSetting>();

            return setting;
        }

        public IEnumerable<PropertyApplicationFormNoAnswerDto> GetApplicationHistoryForUser(long userId)
        {
            var formApplications = Repository.Fetch<PropertyApplicationFormInstance>(x => x.UserId == userId).ToArray();

            return BindingPropertiesToPropertyApplications(formApplications);
        }
        public IEnumerable<PropertyApplicationFormNoAnswerDto> GetPropertyApplications(params long[] propertyIds)
        {
            var formApplications = Repository.Fetch<PropertyApplicationFormInstance>(x => propertyIds.Contains(x.PropertyId)).ToArray();

            return BindingPropertiesToPropertyApplications(formApplications);
        }

        private IEnumerable<PropertyApplicationFormNoAnswerDto> BindingPropertiesToPropertyApplications(PropertyApplicationFormInstance[] formApplications)
        {
            var propertyApplicationFormNoAnswerDtos = MappingService.Map<PropertyApplicationFormNoAnswerDto[]>(formApplications);

            var propertyIds = formApplications.Select(x => x.PropertyId);
            var properties = _propertyService.GetDtos(propertyIds.ToArray()).ToArray();

            foreach (var applicationForm in propertyApplicationFormNoAnswerDtos)
            {
                var user = _userService.GetUserPersonalInformation(applicationForm.UserId);
                if (user != null)
                {
                    applicationForm.UserInformation = user;
                }
                applicationForm.Property = properties.FirstOrDefault(x => x.Id == applicationForm.PropertyId);

                if (applicationForm.Property != null)
                    applicationForm.Property.ExtendedProperties =
                        _dynamicPropertyValueService.GetExtendedPropertyValuesForEntity<Property>(applicationForm.Property.Id);
            }

            return propertyApplicationFormNoAnswerDtos;
        }


        public IEnumerable<PropertyApplicationFormNoAnswerDto> GetApplicationRequestsForUser(long userId)
        {
            var userPropertyIds = GetPropertyIdsBelongToUser(userId);

            return GetPropertyApplications(userPropertyIds);
        }

        public PropertyApplicationFormInstance SavePropertyApplicationForm(PropertyApplicationFormInstanceDto model)
        {
            var formInstance = MappingService.Map<PropertyApplicationFormInstance>(model);

            Repository.Insert(formInstance);
            Repository.SaveChanges();

            _eventPublisher.Publish(new PropertyApplicationSubmitted { ApplicationForm = formInstance });
            return formInstance;
        }

        public bool CheckReceiveApplicationPermissionAbility(long propertyId)
        {
            var property = _propertyService.GetEntity(propertyId);

            if (!property.OwnerId.HasValue)
                return false;


            var canReceiveApplication = _membership.IsAccessAllowed(PermissionAuthorize.Feature(RoundcombPermissions.CanReceiveApplications), property.OwnerId.Value);

            if (!canReceiveApplication)
            {
                _eventPublisher.Publish(new ApplicationSubmissionValidationFailed
                {
                    Property = _propertyService.ToDto(property)
                });
            }

            return canReceiveApplication;
        }

        public void ApproveApplication(long applicationId)
        {
            var formInstance = GetEntity(applicationId);

            var properties = GetPropertyIdsBelongToUser(_membership.UserId);

            if (!properties.Contains(formInstance.PropertyId))
                throw new PropertyApplicationInvalidOwnershipApprovalException();

            if (formInstance.UserDeclined)
                throw new InvalidOperationException("Cannot approve the application that has been already declined by its owner");

            formInstance.ApprovedDate = DateTimeOffset.Now;
            formInstance.IsApproved = true;
            Repository.Update(formInstance);
            Repository.SaveChanges();

            _eventPublisher.Publish(new PropertyApplicationApproved { ApplicationForm = formInstance });
        }

        public void AcceptApplication(long applicationId)
        {
            var formInstance = GetEntity(applicationId);

            if (formInstance.UserId != _membership.UserId)
                throw new UnauthorizedAccessException();

            if (!formInstance.IsApproved)
                throw new InvalidOperationException("Cannot accept the application that has not been yet approved");

            if (formInstance.IsRejected)
                throw new InvalidOperationException("Cannot accept the application that has been already rejected");

            formInstance.UserAccepted = true;
            formInstance.UserAcceptedDate = DateTimeOffset.Now;
            Repository.Update(formInstance);
            Repository.SaveChanges();

            _eventPublisher.Publish(new PropertyApplicationUserAccepted { ApplicationForm = formInstance });
        }

        public void DeclineApplication(long applicationId, PropertyApplicationRejectRequestModel cancelReason)
        {
            var formInstance = GetEntity(applicationId);

            if (formInstance.UserId != _membership.UserId)
                throw new UnauthorizedAccessException();

            if (formInstance.IsRejected)
                throw new InvalidOperationException("Cannot decline the application that has been already rejected");

            formInstance.UserDeclined = true;
            formInstance.UserDeclinedDate = DateTimeOffset.Now;
            formInstance.DeclinedReason = cancelReason.Reason;
            Repository.Update(formInstance);
            Repository.SaveChanges();

            _eventPublisher.Publish(new PropertyApplicationUserDeclined { ApplicationForm = formInstance });

        }

        public PropertyApplicationFormInstanceDto GetApplication(long applicationId)
        {
            var formInstance = GetEntity(applicationId);

            if (_membership.UserId != formInstance.UserId)
            {
                var properties = GetPropertyIdsBelongToUser(_membership.UserId);

                if (!properties.Contains(formInstance.PropertyId))
                    throw new PropertyApplicationInvalidOwnershipAccessException();
            }

            var applicationForm = ToDto(formInstance);

            var user = _userService.GetUserPersonalInformation(applicationForm.UserId);
            if (user != null)
            {
                applicationForm.UserInformation = user;
            }
            return applicationForm;
        }

        public void MarkApplicationAsViewed(long applicationId)
        {
            var formInstance = GetEntity(applicationId);

            if (formInstance.IsViewed) return;

            formInstance.IsViewed = true;
            formInstance.ViewedDate = DateTimeOffset.Now;
            Repository.Update(formInstance);
            Repository.SaveChanges();
        }

        public void RejectApplication(long applicationId, PropertyApplicationRejectRequestModel cancelReason)
        {
            var formInstance = GetEntity(applicationId);

            var properties = GetPropertyIdsBelongToUser(_membership.UserId);

            if (!properties.Contains(formInstance.PropertyId))
                throw new PropertyApplicationInvalidOwnershipApprovalException();

            if (formInstance.IsRejected) return;

            formInstance.IsRejected = true;
            formInstance.RejectedDate = DateTimeOffset.Now;
            formInstance.RejectReason = cancelReason.Reason;
            formInstance.RejectedBy = _membership.Name;
            Repository.Update(formInstance);
            Repository.SaveChanges();

            _eventPublisher.Publish(new PropertyApplicationRejected { ApplicationForm = formInstance });
        }

        private long[] GetPropertyIdsBelongToUser(long userId)
        {
            var userPropertyIds = Repository.Fetch<Property>(x => x.OwnerId == userId && !x.IsDeleted)
                                           .Select(x => x.Id)
                                           .ToArray();
            return userPropertyIds;
        }
    }
}