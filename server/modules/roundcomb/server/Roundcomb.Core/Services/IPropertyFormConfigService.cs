using Core;
using Roundcomb.Core.Dtos;

namespace Roundcomb.Core.Services
{
    public interface IPropertyFormConfigService : IDependency
    {
        void AssignFormToPropertyCategory(long formId, long propertyCategoryId);

        PropertyFormConfigurationSettingDto GetFormConfigurationSettingForPropertyCategory(long propertyCategoryId);

        PropertyApplicationFormDocumentConfigDto GetFormConfigurationSettingForProperty(long propertyId);

	    void SaveFormConfigurationSettingForProperty(long propertyId, PropertyApplicationFormDocumentConfigDto model);
    }
}