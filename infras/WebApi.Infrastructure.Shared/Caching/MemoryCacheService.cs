using Microsoft.Extensions.Caching.Memory;

namespace WebApi.Infrastructure.Shared.Caching;

public class MemoryCacheService : ICacheService
{
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(5);
    private readonly IMemoryCache cache;

    public MemoryCacheService(IMemoryCache cache) => this.cache = cache;

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null)
    {
        return await cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = absoluteExpiration ?? DefaultTtl;
            return await factory();
        });
    }

    public bool TryGet<T>(string key, out T? value) => cache.TryGetValue(key, out value);

    public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        => cache.Set(key, value, absoluteExpiration ?? DefaultTtl);

    public void Remove(string key) => cache.Remove(key);
}
