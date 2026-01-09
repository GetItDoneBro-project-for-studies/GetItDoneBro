using Microsoft.Extensions.Caching.Memory;

namespace GetItDoneBro.Infrastructure.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default
        ) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task<T?> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default
        ) where T : class;
}

internal sealed class DistributedCacheService(IMemoryCache memoryCache) : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        memoryCache.TryGetValue(key: key, value: out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default
        ) where T : class
    {
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
        };

        memoryCache.Set(key: key, value: value, options: cacheOptions);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }

    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default
        ) where T : class
    {
        if (memoryCache.TryGetValue(key: key, value: out T? cached))
        {
            return cached;
        }

        var value = await factory(cancellationToken).ConfigureAwait(false);

        if (value != null)
        {
            await SetAsync(
                    key: key,
                    value: value,
                    expiration: expiration,
                    cancellationToken: cancellationToken
                )
                .ConfigureAwait(false);
        }

        return value;
    }
}
