using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace OrchardCore.Infrastructure.Cache
{
    /// <summary>
    /// Provides a distributed cache service that can return existing references in the current scope.
    /// </summary>
    public interface IScopedDistributedCache
    {
        Task<T> GetAsync<T>(string key) where T : ScopedDistributedCacheable;
        Task<T> GetOrCreateAsync<T>(string key, DistributedCacheEntryOptions options, Func<Task<T>> factory) where T : ScopedDistributedCacheable;
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options) where T : ScopedDistributedCacheable;
        Task RemoveAsync(string key);
    }

    public class ScopedDistributedCacheable
    {
        public string CacheId { get; set; }
    }

    public static class ScopedDistributedCacheExtensions
    {
        public static Task<T> GetAsync<T>(this IScopedDistributedCache scopedDistributedCache) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.GetAsync<T>(typeof(T).FullName);
        }

        public static Task SetAsync<T>(this IScopedDistributedCache scopedDistributedCache, string key, T value) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.SetAsync(key, value, new DistributedCacheEntryOptions());
        }

        public static Task SetAsync<T>(this IScopedDistributedCache scopedDistributedCache, T value) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.SetAsync(typeof(T).FullName, value, new DistributedCacheEntryOptions());
        }

        public static Task SetAsync<T>(this IScopedDistributedCache scopedDistributedCache, T value, DistributedCacheEntryOptions options) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.SetAsync(typeof(T).FullName, value, options);
        }

        public static Task<T> GetOrCreateAsync<T>(this IScopedDistributedCache scopedDistributedCache, string key, Func<Task<T>> factory) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.GetOrCreateAsync(key, new DistributedCacheEntryOptions(), factory);
        }

        public static Task<T> GetOrCreateAsync<T>(this IScopedDistributedCache scopedDistributedCache, Func<Task<T>> factory) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.GetOrCreateAsync(typeof(T).FullName, new DistributedCacheEntryOptions(), factory);
        }

        public static Task<T> GetOrCreateAsync<T>(this IScopedDistributedCache scopedDistributedCache, DistributedCacheEntryOptions options, Func<Task<T>> factory) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.GetOrCreateAsync(typeof(T).FullName, options, factory);
        }

        public static Task RemoveAsync<T>(this IScopedDistributedCache scopedDistributedCache) where T : ScopedDistributedCacheable
        {
            return scopedDistributedCache.RemoveAsync(typeof(T).FullName);
        }
    }
}
