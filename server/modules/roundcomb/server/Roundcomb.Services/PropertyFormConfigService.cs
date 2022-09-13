using Core.ObjectMapping;
using ProductManagement.Core.Services;
using Roundcomb.Core.Dtos;
using Roundcomb.Core.Entities;
using Roundcomb.Core.Repositories;
using Roundcomb.Core.Services;

namespace Roundcomb.Services
{
    public class PropertyFormConfigService : IPropertyFormConfigService
    {
        private readonly IRoundcombRepository _repository;
        private readonly IMappingService _mappingService;
        private readonly IPropertyService _propertyService;

        public PropertyFormConfigService(IRoundcombRepository repository, IMappingService mappingService, IPropertyService propertyService)
        {
            _repository = repository;
            _mappingService = mappingService;
            _propertyService = propertyService;
        }

        public void AssignFormToPropertyCategory(long formId, long propertyCategoryId)
        {
            var entity =
                _repository.Get<PropertyFormConfigurationSetting>(setting =>
                                                                 setting.PropertyCategoryId == propertyCategoryId);
            if (entity == null)
            {
                _repository.Insert(new PropertyFormConfigurationSetting
                {
                    FormConfigurationId = formId,
                    PropertyCategoryId = propertyCategoryId
                });
            }

            else
            {
                _repository.Update<PropertyFormConfigurationSetting>(setting => setting.PropertyCategoryId == propertyCategoryId, setting => new PropertyFormConfigurationSetting
                {
                    FormConfigurationId = formId
                });
            }
            _repository.SaveChanges();
        }

        public PropertyFormConfigurationSettingDto GetFormConfigurationSettingForPropertyCategory(long propertyCategoryId)
        {
            var entity =
                _repository.Get<PropertyFormConfigurationSetting>(setting =>
                                                                 setting.PropertyCategoryId == propertyCategoryId);

            return _mappingService.Map<PropertyFormConfigurationSettingDto>(entity);
        }

        public PropertyApplicationFormDocumentConfigDto GetFormConfigurationSettingForProperty(long propertyId)
        {
            var property = _propertyService.GetEntity(propertyId);
			// old way, obsoleted
			// return GetFormConfigurationSettingForProductCategory(product.Category.Id);
			
			var existingFormConfig = _repository.First<PropertyApplicationFormDocumentConfig>(x => x.PropertyId == propertyId);

			return _mappingService.Map<PropertyApplicationFormDocumentConfigDto>(existingFormConfig);
        }

	    public void SaveFormConfigurationSettingForProperty(long propertyId, PropertyApplicationFormDocumentConfigDto model)
		{
			var property = _propertyService.GetEntity(propertyId);

			var existingFormConfig = _repository.First<PropertyApplicationFormDocumentConfig>(x => x.PropertyId == propertyId);

			if (existingFormConfig == null)
			{
				existingFormConfig = new PropertyApplicationFormDocumentConfig
				                     {
					                     PropertyId = propertyId,
					                     FileName = model.FileName,
					                     FileUrl = model.FileUrl,
					                     IsExternalSite = model.IsExternalSite,
										 ResultUrl = model.ResultUrl
				};
				_repository.Insert(existingFormConfig);
			}
			else
			{
				existingFormConfig.FileUrl = model.FileUrl;
				existingFormConfig.FileName = model.FileName;
				existingFormConfig.IsExternalSite = model.IsExternalSite;
				existingFormConfig.ResultUrl = model.ResultUrl;
				_repository.Update(existingFormConfig);
			}

		}
    }
}