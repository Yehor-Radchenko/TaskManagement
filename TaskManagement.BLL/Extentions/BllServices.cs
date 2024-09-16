namespace TaskManagement.BLL.Extentions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.BLL.Services;
using TaskManagement.BLL.Services.IService;
using TaskManagement.BLL.Services.Jwt;

public static class BllServices
{
    public static IServiceCollection AddBllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
