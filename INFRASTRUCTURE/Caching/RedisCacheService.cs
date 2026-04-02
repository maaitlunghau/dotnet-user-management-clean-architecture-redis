using System.Text.Json;
using APPLICATION.Services; // Đảm bảo namespace này chứa ICacheService
using StackExchange.Redis;

namespace INFRASTRUCTURE.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _cache;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _cache = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _cache.StringGetAsync(key);
        if (!value.HasValue) return default;

        return JsonSerializer.Deserialize<T>(value.ToString()!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var json = JsonSerializer.Serialize(value);

        if (expiration.HasValue)
        {
            await _cache.StringSetAsync(key, json, expiration.Value);
        }
        else
        {
            await _cache.StringSetAsync(key, json);
        }
    }

    public async Task RemoveAsync(string key) => await _cache.KeyDeleteAsync(key);
}
