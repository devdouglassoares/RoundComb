using Core.IoC;
using Core.Logging;
using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Core.BackgroundTasks
{
    public interface IBackgroundClockTicker : ISingletonDependency
    {
        TimeSpan Interval { get; set; }
        void Activate();
        void Terminate();
    }

    public class BackgroundClockTicker : IBackgroundClockTicker, IDisposable
    {
        private readonly IServiceResolver _serviceResolver;
        private readonly Timer _timer;

        public BackgroundClockTicker(IServiceResolver serviceResolver)
        {
            _serviceResolver = serviceResolver;
            _timer = new Timer();
            _timer.Elapsed += Elapsed;

            Interval = TimeSpan.FromSeconds(30);
        }

        private readonly ILogger _logger = Logger.GetLogger<BackgroundClockTicker>();

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        public void Activate()
        {
            lock (_timer)
            {
                _timer.Start();
            }
        }

        public void Terminate()
        {
            lock (_timer)
            {
                _timer.Stop();
            }
        }

        void Elapsed(object sender, ElapsedEventArgs e)
        {
            // current implementation disallows re-entrancy
            if (!Monitor.TryEnter(_timer))
                return;

            try
            {
                if (_timer.Enabled)
                {
                    DoWork();
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(ex);
            }
            finally
            {
                Monitor.Exit(_timer);
            }
        }

        private void DoWork()
        {
            var thread = new Thread(StartThread);

            thread.Start(_serviceResolver);
        }

        private void StartThread(object o)
        {
            var serviceResolver = o as IServiceResolver;
            if (serviceResolver == null)
                return;

            var manager = serviceResolver.GetInstance<IBackgroundTaskRunner>();
            manager.DoWork();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}