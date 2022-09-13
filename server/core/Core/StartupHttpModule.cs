using System;
using System.Collections.Generic;
using System.Threading;

namespace Core
{
    public class StartupHttpModule
    {
        private static readonly object SynLock = new object();
        private static IList<Action> _awaiting = new List<Action>();

        /// <summary>
        /// Indicates whether the system is starting up
        /// </summary>
        /// <returns></returns>
        private static bool InWarmup()
        {
            lock (SynLock)
            {
                return _awaiting != null;
            }
        }

        /// <summary>
        /// Indicates the warm up process has started and all the pending requests will be added to _awaiting list
        /// </summary>
        public static void WarmingUp()
        {
            lock (SynLock)
            {
                if (_awaiting == null)
                {
                    _awaiting = new List<Action>();
                }
            }
        }

        /// <summary>
        /// Indicates the warm up process has completed and starts all the _awaiting actions
        /// </summary>
        public static void WarmupComplete()
        {
            IList<Action> temp;

            lock (SynLock)
            {
                temp = _awaiting;
                _awaiting = null;
            }

            if (temp != null)
            {
                foreach (var action in temp)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Enqueue or directly process action depending on current mode.
        /// </summary>
        private static void Await(Action action)
        {
            var temp = action;

            lock (SynLock)
            {
                if (_awaiting != null)
                {
                    temp = null;
                    _awaiting.Add(action);
                }
            }

            if (temp != null)
            {
                temp();
            }
        }

        public static IAsyncResult BeginBeginRequest(object sender, EventArgs e, AsyncCallback cb, object extraData)
        {
            if (!InWarmup())
            {
                var asyncResult = new StartingUpCompleteAsyncResult(extraData);
                cb(asyncResult);
                return asyncResult;
            }
            else
            {
                var asyncResult = new StartingUpAsyncResult(cb, extraData);
                Await(asyncResult.Completed);
                return asyncResult;
            }
        }

        public static void EndBeginRequest(IAsyncResult ar)
        {
        }

        /// <summary>
        /// AsyncResult for "on hold" request (resumes when "Completed()" is called)
        /// </summary>
        private class StartingUpAsyncResult : IAsyncResult
        {
            private readonly EventWaitHandle _eventWaitHandle = new AutoResetEvent(false/*initialState*/);
            private readonly AsyncCallback _cb;
            private readonly object _asyncState;
            private bool _isCompleted;

            public StartingUpAsyncResult(AsyncCallback cb, object asyncState)
            {
                _cb = cb;
                _asyncState = asyncState;
                _isCompleted = false;
            }

            public void Completed()
            {
                _isCompleted = true;
                _eventWaitHandle.Set();
                _cb(this);
            }

            bool IAsyncResult.CompletedSynchronously
            {
                get { return false; }
            }

            bool IAsyncResult.IsCompleted
            {
                get { return _isCompleted; }
            }

            object IAsyncResult.AsyncState
            {
                get { return _asyncState; }
            }

            WaitHandle IAsyncResult.AsyncWaitHandle
            {
                get { return _eventWaitHandle; }
            }
        }

        /// <summary>
        /// Async result for "ok to process now" requests
        /// </summary>
        private class StartingUpCompleteAsyncResult : IAsyncResult
        {
            private readonly object _asyncState;
            private static readonly WaitHandle WaitHandle = new ManualResetEvent(true/*initialState*/);

            public StartingUpCompleteAsyncResult(object asyncState)
            {
                _asyncState = asyncState;
            }

            bool IAsyncResult.CompletedSynchronously
            {
                get { return true; }
            }

            bool IAsyncResult.IsCompleted
            {
                get { return true; }
            }

            WaitHandle IAsyncResult.AsyncWaitHandle
            {
                get { return WaitHandle; }
            }

            object IAsyncResult.AsyncState
            {
                get { return _asyncState; }
            }
        }
    }
}