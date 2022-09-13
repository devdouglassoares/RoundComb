using Core;

namespace Membership.Core.Contracts
{
    public interface ISmallCache : IDependency
    {
        object Check(string key);
        void Put(string key, object value);
    }
}
