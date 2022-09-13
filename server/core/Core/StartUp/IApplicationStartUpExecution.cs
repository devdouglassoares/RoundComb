using Core.BackgroundTasks;

namespace Core.StartUp
{
    public interface IApplicationStartUpExecution : IDependency
    {
        bool ShouldRun();

        void Execute();
    }

    public class DefaultApplicationStartUpExecution : IApplicationStartUpExecution
    {
        private readonly IBackgroundClockTicker _backgroundClockTicker;

        public DefaultApplicationStartUpExecution(IBackgroundClockTicker backgroundClockTicker)
        {
            _backgroundClockTicker = backgroundClockTicker;
        }

        public bool ShouldRun()
        {
            return true;
        }

        public void Execute()
        {
            _backgroundClockTicker.Activate();
        }
    }
}