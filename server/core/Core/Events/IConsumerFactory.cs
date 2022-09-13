using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Events
{
    public interface IConsumerFactory<T> : IDependency
    {
        IEnumerable<IConsumer<T>> GetConsumers();
    }

    public class DefaultConsumerFactory<T> : IConsumerFactory<T>
    {
        private readonly IEnumerable<IConsumer<T>> _consumers;

        public DefaultConsumerFactory(IEnumerable<IConsumer<T>> consumers)
        {
            _consumers = consumers;
        }

        public IEnumerable<IConsumer<T>> GetConsumers()
        {
            return _consumers;
        }
    }
}