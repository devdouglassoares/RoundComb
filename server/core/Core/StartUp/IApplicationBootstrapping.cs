using System.Collections.Generic;
using System.Linq;

namespace Core.StartUp
{
    public interface IApplicationBootstrapping : IDependency
    {
        void StartUp();
    }

    public class DefaultApplicationBootstrapping : IApplicationBootstrapping
    {
        private readonly IEnumerable<IApplicationStartUpExecution> _applicationStartUpExecutions;

        public DefaultApplicationBootstrapping(IEnumerable<IApplicationStartUpExecution> applicationStartUpExecutions)
        {
            _applicationStartUpExecutions = applicationStartUpExecutions;
        }

        public void StartUp()
        {
            foreach (var execution in _applicationStartUpExecutions.Where(x => x.ShouldRun()))
            {
                execution.Execute();
            }
                
        }
    }
}