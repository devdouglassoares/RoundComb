using Core;
using Core.Database;
using Roundcomb.Core.Dtos;
using Roundcomb.Core.Entities;
using System.Collections.Generic;

namespace Roundcomb.Core.Services
{
    public interface IPropertyApplicationFormInstanceService : IBaseService<PropertyApplicationFormInstance, PropertyApplicationFormInstanceDto>, IDependency
    {
        IEnumerable<PropertyApplicationFormNoAnswerDto> GetPropertyApplications(params long[] propertyIds);

        IEnumerable<PropertyApplicationFormNoAnswerDto> GetApplicationRequestsForUser(long userId);

        PropertyApplicationFormInstance SavePropertyApplicationForm(PropertyApplicationFormInstanceDto model);

        bool CheckReceiveApplicationPermissionAbility(long propertyId);

        PropertyApplicationFormInstanceDto GetApplication(long applicationId);

        void MarkApplicationAsViewed(long applicationId);

        void ApproveApplication(long applicationId);

        void RejectApplication(long applicationId, PropertyApplicationRejectRequestModel cancelReason);

        void AcceptApplication(long applicationId);

        void DeclineApplication(long applicationId, PropertyApplicationRejectRequestModel cancelReason);

        void AssignFormConfigurationToPropertyCategory(long formConfigId, long propertyCategoryId);

        PropertyFormConfigurationSetting GetFormConfigurationSettingForProperty(long propertyId);

        PropertyFormConfigurationSetting GetFormConfigurationSettingForPropertyCategory(long propertyCategoryId);
        IEnumerable<PropertyApplicationFormNoAnswerDto> GetApplicationHistoryForUser(long userId);
    }
}