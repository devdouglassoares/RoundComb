using Core.IoC;
using Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.BackgroundTasks
{
    public interface IBackgroundTaskRunner : IDependency
    {
        void DoWork();
    }

    public class BackgroundTaskRunner : IBackgroundTaskRunner
    {
        private readonly IEnumerable<IBackgroundTask> _tasks;
        private readonly ILogger _logger = Logger.GetLogger<BackgroundTaskRunner>();
        private readonly IServiceResolver _serviceResolver;

        public BackgroundTaskRunner(IEnumerable<IBackgroundTask> tasks, IServiceResolver serviceResolver)
        {
            _tasks = tasks;
            _serviceResolver = serviceResolver;
        }

        public void DoWork()
        {
            foreach (var task in _tasks)
            {
                ExecuteTask(task);
            }
        }

        private void ExecuteTask(IBackgroundTask taskHandler)
        {
            //var thread = new Thread(StartThread);
            //thread.Start(taskHandler);

            try
            {
                taskHandler.Execute();
            }
            catch (Exception exception)
            {
                _logger.Error($"Error while running background task {taskHandler.GetType().FullName}", exception);
            }
        }

        private void StartThread(object o)
        {
            var taskHandler = o;

            var taskInstance = _serviceResolver.GetAllInstances<IBackgroundTask>()
            .FirstOrDefault(task => task.GetType() == taskHandler.GetType());

            if (taskInstance == null)
                return;

            try
            {
                taskInstance.Execute();
            }
            catch (Exception exception)
            {
                _logger.Error($"Error while running background task {taskHandler.GetType().FullName}", exception);
            }
        }
    }
}