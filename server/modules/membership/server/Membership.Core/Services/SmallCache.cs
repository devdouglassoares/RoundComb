using System;
using System.Runtime.Caching;
using Membership.Core.Contracts;

namespace Membership.Core.Services
{
    public class SmallCache : ISmallCache
    {
        private readonly MemoryCache cache;

        public SmallCache()
        {
            this.cache = MemoryCache.Default;

        }

        public object Check(string key)
        {
            var value = this.cache[key];
            return value;
        }

        public void Put(string key, object value)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1);
            cache.Set(key, value, policy);
        }
    }
}
