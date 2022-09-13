using System;
using System.Threading;
using System.Web;

namespace Core
{
    public class Startup<T> where T : class
    {
        private readonly Func<HttpApplication, T> _initialization;
        private readonly Action<HttpApplication, T> _beginRequest;
        private readonly Action<HttpApplication, T> _endRequest;
        private readonly Action<HttpApplication, EventArgs, T> _applicationError;
        private readonly object _synLock = new object();

        /// <summary>
        /// The result of the initialization queued work item.
        /// Set only when initialization has completed without errors.
        /// </summary>
        private volatile T _initializationResult;
        /// <summary>
        /// The (potential) error raised by the initialization thread. This is a "one-time"
        /// error signal, so that we can restart the initialization once another request
        /// comes in.
        /// </summary>
        private volatile Exception _error;
        /// <summary>
        /// The (potential) error from the previous initialization. We need to
        /// keep this error active until the next initialization is finished,
        /// so that we can keep reporting the error for all incoming requests.
        /// </summary>
        private volatile Exception _previousError;

        public Startup(Func<HttpApplication, T> initialization, Action<HttpApplication, T> beginRequest,
            Action<HttpApplication, T> endRequest,
            Action<HttpApplication, EventArgs, T> applicationError)
        {
            _initialization = initialization;
            _beginRequest = beginRequest;
            _endRequest = endRequest;
            _applicationError = applicationError;
        }

        public void OnApplicationStart(HttpApplication application)
        {
            LaunchStartupThread(application);
        }

        public void OnBeginRequest(HttpApplication application)
        {
            if (_error != null)
            {
                bool restartInitialization = false;

                lock (_synLock)
                {
                    if (_error != null)
                    {
                        _previousError = _error;
                        _error = null;
                        restartInitialization = true;
                    }
                }

                if (restartInitialization)
                {
                    LaunchStartupThread(application);
                }
            }

            // Previous initialization resulted in an error (and another initialization is running)
            if (_previousError != null)
            {
                throw new ApplicationException("Error during application initialization", _previousError);
            }

            // Only notify if the initialization has successfully completed
            if (_initializationResult != null)
            {
                _beginRequest(application, _initializationResult);
            }
        }

        public void OnEndRequest(HttpApplication application)
        {
            // Only notify if the initialization has successfully completed
            if (_initializationResult != null)
            {
                _endRequest(application, _initializationResult);
            }
        }

        /// <summary>
        /// Run the initialization delegate asynchronously in a queued work item
        /// </summary>
        public void LaunchStartupThread(HttpApplication application)
        {
            StartupHttpModule.WarmingUp();

            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    try
                    {
                        var result = _initialization(application);
                        _initializationResult = result;
                    }
                    catch (Exception e)
                    {
                        lock (_synLock)
                        {
                            _error = e;
                            _previousError = null;
                        }
                    }
                    finally
                    {
                        StartupHttpModule.WarmupComplete();
                    }
                });
        }

        public void OnApplicationError(HttpApplication application, EventArgs eventArgs)
        {
            _applicationError(application, eventArgs, _initializationResult);
        }
    }
}