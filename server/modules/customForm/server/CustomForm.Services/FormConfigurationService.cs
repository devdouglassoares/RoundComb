using System;
using Core.Database;
using Core.ObjectMapping;
using CustomForm.Core.Dtos;
using CustomForm.Core.Entities;
using CustomForm.Core.Services;
using CustomForm.Data.Repositories;

namespace CustomForm.Services
{
    public class FormConfigurationService : BaseService<FormConfiguration, FormConfigurationDto>, IFormConfigurationService
    {
        public FormConfigurationService(IMappingService mappingService, IRepository repository) : base(mappingService, repository)
        {
        }

        public override FormConfiguration Create(FormConfigurationDto model)
        {
            var existingEntity = Repository.Any<FormConfiguration>(form => form.FormCode == model.FormCode);
            if (existingEntity)
                throw new InvalidOperationException("Form configuration with the same code has already created");

            return base.Create(model);
        }

        public override FormConfiguration PrepareForInserting(FormConfiguration entity, FormConfigurationDto model)
        {
            entity = base.PrepareForInserting(entity, model);

            foreach (var field in model.Fields)
            {
                var formFieldFormConfiguration = MappingService.Map<FormFieldFormConfiguration>(field);
                formFieldFormConfiguration.FormConfiguration = entity;

                entity.Fields.Add(formFieldFormConfiguration);
            }

            return entity;
        }

        public override FormConfiguration PrepareForUpdating(FormConfiguration entity, FormConfigurationDto model)
        {
            entity = base.PrepareForUpdating(entity, model);

            Repository.DeleteByCondition<FormFieldFormConfiguration>(x => x.FormConfigurationId == entity.Id);

            foreach (var field in model.Fields)
            {
                var formFieldFormConfiguration = MappingService.Map<FormFieldFormConfiguration>(field);
                formFieldFormConfiguration.FormConfiguration = entity;

                entity.Fields.Add(formFieldFormConfiguration);
            }
            
            return entity;
        }
    }
}