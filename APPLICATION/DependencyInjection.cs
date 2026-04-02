using APPLICATION.Interfaces.Services;
using APPLICATION.Services;
using Microsoft.Extensions.DependencyInjection;

namespace APPLICATION;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
