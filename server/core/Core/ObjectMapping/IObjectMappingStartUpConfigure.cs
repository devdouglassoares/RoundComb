namespace Core.ObjectMapping
{
    public interface IObjectMappingStartUpConfigure : IDependency
    {
        void ConfigureMapping();
    }
}