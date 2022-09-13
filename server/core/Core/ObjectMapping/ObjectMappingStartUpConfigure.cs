using Core.Logging;
using Core.StartUp;
using System.Collections.Generic;
using System.Linq;

namespace Core.ObjectMapping
{
    public class ObjectMappingStartUpConfigure : IObjectMappingStartUpConfigure, IApplicationStartUpExecution
    {
        private readonly IMappingService _mappingService;
        private readonly IEnumerable<IObjectMappingRegistration> _objectMappingRegistrations;
        private readonly ILogger _logger = Logger.GetLogger<ObjectMappingStartUpConfigure>();

        public ObjectMappingStartUpConfigure(IMappingService mappingService, IEnumerable<IObjectMappingRegistration> objectMappingRegistrations)
        {
            _mappingService = mappingService;
            _objectMappingRegistrations =
                objectMappingRegistrations.Where(x => x.GetType() != typeof(DefaultObjectMappingRegistration));
        }

        public void ConfigureMapping()
        {
            foreach (var register in _objectMappingRegistrations)
            {
                try
                {
                    register.ConfigureMapping(_mappingService);
                }
                catch (System.Exception exception)
                {
                    _logger.Error(
                        $"Error while registering mapping configuration. Failed class: {register.GetType().Name}",
                        exception);
                }
            }

            _mappingService.LockConfiguration();
        }

        public bool ShouldRun()
        {
            return true;
        }

        public void Execute()
        {
            ConfigureMapping();
        }
    }


}