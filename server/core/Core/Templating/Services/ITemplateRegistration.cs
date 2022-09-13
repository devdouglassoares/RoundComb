using Core.IoC;
using Core.Templating.Data.Entities;
using Core.Templating.Data.Repositories;
using System;
using System.Collections.Generic;

namespace Core.Templating.Services
{
    public interface ITemplateRegistration : IDependency
    {
        void RegisterTemplates();

        void RegisterTemplate<TTemplate>() where TTemplate : ITemplateModel;
    }

    public class TemplateRegistration : ITemplateRegistration
    {
        private readonly IEnumerable<ITemplateProvider> _providers;
        private readonly ITemplatingRepository _repository;

        public TemplateRegistration(IEnumerable<ITemplateProvider> providers, ITemplatingRepository repository)
        {
            _providers = providers;
            _repository = repository;
        }

        public void RegisterTemplates()
        {
            foreach (var templateProvider in _providers)
            {
                var assemblyQualifiedName = templateProvider.GetType().Assembly.GetName().Name;

                _repository.Update<TemplateModel>(
                    model => model.FromAssemblyName == assemblyQualifiedName,
                    model => new TemplateModel
                    {
                        IsDeleted = true
                    });

                _repository.SaveChanges();

                templateProvider.RegisterTemplates(this);
            }
        }

        public void RegisterTemplate<TTemplate>() where TTemplate : ITemplateModel
        {
            var type = typeof(TTemplate).FullName;

            var existing = _repository.First<TemplateModel>(x => x.TemplateTypeString == type);

            if (existing != null)
            {
                existing.IsDeleted = false;
                existing.FromAssemblyName = typeof(TTemplate).Assembly.GetName().Name;
                existing.Fields = GetTemplateFields<TTemplate>();
                _repository.Update(existing);
            }
            else
                _repository.Insert(new TemplateModel
                {
                    TemplateTypeString = type,
                    TemplateContent = "",
                    CreatedDate = DateTimeOffset.Now,
                    ModifiedDate = DateTimeOffset.Now,
                    Fields = GetTemplateFields<TTemplate>(),
                    FromAssemblyName = typeof(TTemplate).Assembly.GetName().Name
                });

            _repository.SaveChanges();
        }

        public List<string> GetTemplateFields<TTemplate>() where TTemplate : ITemplateModel
        {
            var type = typeof(TTemplate);

            var fieldNamesFromType = GetFieldNamesFromType(type);

            return fieldNamesFromType;
        }

        private List<string> GetFieldNamesFromType(Type type, string name = "", List<string> recursiveList = null, int level = 0)
        {
            if (recursiveList == null)
                recursiveList = new List<string>();

            if (level > 3)
                return recursiveList;

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                recursiveList.Add($"{name}.{property.Name}".Trim('.'));

                if (property.PropertyType.IsClass && property.PropertyType.IsNotSystemType())
                {
                    GetFieldNamesFromType(property.PropertyType, $"{name}.{property.Name}".Trim('.'), recursiveList, level + 1);
                }
            }

            return recursiveList;
        }
    }
}