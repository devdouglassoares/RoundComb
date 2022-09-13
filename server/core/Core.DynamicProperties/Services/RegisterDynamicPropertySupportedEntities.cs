using Core.DynamicProperties.Models;
using Core.DynamicProperties.Repositories;
using Core.StartUp;
using System.Collections.Generic;
using System.Linq;

namespace Core.DynamicProperties.Services
{
    public class RegisterDynamicPropertySupportedEntities : IApplicationStartUpExecution
    {
        private readonly IDynamicPropertyRepository _repository;
        private readonly IEnumerable<IHasDynamicProperty> _hasDynamicProperties;

        public RegisterDynamicPropertySupportedEntities(IDynamicPropertyRepository repository, IEnumerable<IHasDynamicProperty> hasDynamicProperties)
        {
            _repository = repository;
            _hasDynamicProperties = hasDynamicProperties.Where(x => x.GetType() != typeof(NullDynamicProperty));
        }

        public bool ShouldRun()
        {
            return true;
        }

        public void Execute()
        {
            foreach (var hasDynamicProperty in _hasDynamicProperties)
            {
                _repository.InsertOrUpdate<DynamicPropertySupportedEntityType>(x => x.EntityTypeFullName,
                                                                               new DynamicPropertySupportedEntityType
                                                                               {
                                                                                   EntityTypeFullName =
                                                                                       hasDynamicProperty.GetType()
                                                                                                         .FullName,
                                                                                   Name =
                                                                                       hasDynamicProperty.GetType().Name
                                                                               });
            }
            _repository.SaveChanges();
        }
    }
}