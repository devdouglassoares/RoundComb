namespace Core.Events
{
    public interface IConsumer<T> : IDependency
    {
        int Order { get; }

        void HandleEvent(T eventMessage);
    }

    public class DefaultConsumer<T> : IConsumer<T>
    {
        public int Order => 0;

        public void HandleEvent(T eventMessage)
        {
            // Do nothing
        }
    }
}