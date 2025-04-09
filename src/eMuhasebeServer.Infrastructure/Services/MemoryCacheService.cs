using eMuhasebeServer.Application.Services;
using Microsoft.Extensions.Caching.Memory;

namespace eMuhasebeServer.Infrastructure.Services;
internal sealed class MemoryCacheService(
    IMemoryCache memoryCache) : ICacheService
{
    public T? Get<T>(string key)
    {
        memoryCache.TryGetValue<T>(key, out var value);
        return value;
    }

    public bool Remove(string key)
    {
        memoryCache.Remove(key);
        return true;
    }

    public void Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(1)
        };
        memoryCache.Set<T>(key, value, cacheEntryOptions);
    }

    public void RemoveAll()
    {
        List<string> keys = new()
        {
            "cashRegisters",
            "banks",
            "invoices",
            "products",
            "customers"
        };

        foreach (var key in keys)
        {
            memoryCache.Remove(key);
        }
    }


}
