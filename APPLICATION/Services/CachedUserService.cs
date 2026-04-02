using APPLICATION.DTOs;
using APPLICATION.Interfaces.Services;

namespace APPLICATION.Services;

public class CachedUserService : IUserService
{
    private readonly IUserService _decorated;
    private readonly ICacheService _cacheService;
    public CachedUserService(IUserService decorated, ICacheService cacheService)
    {
        _decorated = decorated;
        _cacheService = cacheService;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        string key = $"user-{id}";

        var cachedUser = await _cacheService.GetAsync<UserDto>(key);
        if (cachedUser != null) return cachedUser;

        var user = await _decorated.GetUserByIdAsync(id);
        if (user != null)
            await _cacheService.SetAsync(key, user, TimeSpan.FromMinutes(10));

        return user;
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync()
        => _decorated.GetAllUsersAsync();

    public Task<UserDto> CreateUserAsync(CreateUserRequest request)
        => _decorated.CreateUserAsync(request);

    public async Task<bool> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        var result = await _decorated.UpdateUserAsync(id, request);
        if (result) await _cacheService.RemoveAsync($"user-{id}");

        return result;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var result = await _decorated.DeleteUserAsync(id);
        if (result) await _cacheService.RemoveAsync($"user-{id}");

        return result;
    }
}
