namespace WebApi.Infrastructure.Shared.Caching;

public interface ICacheService
{
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null);

    bool TryGet<T>(string key, out T? value);

    void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null);

    void Remove(string key);
}
