using Core.Logging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Core.Events
{
    public interface IEventPublisher : ISingletonDependency
    {
        void Publish<T>(T eventMessage);
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly ConcurrentDictionary<object, Timer> _queue = new ConcurrentDictionary<object, Timer>();
        private readonly ILogger _logger = Logger.GetLogger<EventPublisher>();

        public void Publish<T>(T eventMessage)
        {
            if (eventMessage != null)
            {
                // Enable event throttling by allowing the very same event to be published only all 150 ms.
                Timer timer;
                if (_queue.TryGetValue(eventMessage, out timer))
                {
                    // do nothing. The same event was published a tick ago.
                    return;
                }

                _queue[eventMessage] = new Timer(RemoveFromQueue, eventMessage, 150, Timeout.Infinite);
            }

            var consumerFactory = ServiceLocator.Current.GetInstance<IConsumerFactory<T>>();
            if (consumerFactory == null)
                return;

            var consumers = consumerFactory.GetConsumers().OrderBy(c => c.Order);

            foreach (var consumer in consumers)
            {
                PublishEvent(consumer, eventMessage);
            }
        }

        private void PublishEvent<T>(IConsumer<T> x, T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (NotImplementedException exception)
            {
                _logger.Warn("Event handler not implemented.", exception);
            }
        }

        private void RemoveFromQueue(object eventMessage)
        {
            Timer timer;
            if (_queue.TryRemove(eventMessage, out timer))
            {
                timer.Dispose();
            }
        }
    }
}