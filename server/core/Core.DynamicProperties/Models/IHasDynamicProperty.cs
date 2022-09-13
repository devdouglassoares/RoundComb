namespace Core.DynamicProperties.Models
{
    public interface IHasDynamicProperty : IDependency { }

    public class NullDynamicProperty : IHasDynamicProperty
    {
    }
}