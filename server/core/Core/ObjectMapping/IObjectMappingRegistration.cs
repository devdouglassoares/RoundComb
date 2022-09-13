namespace Core.ObjectMapping
{
    public interface IObjectMappingRegistration : IDependency
    {
        void ConfigureMapping(IMappingService map);
    }

    public class DefaultObjectMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {

        }
    }
}