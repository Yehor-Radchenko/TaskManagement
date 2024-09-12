namespace TaskManagement.DAL.Extentions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.DAL.Data;
using TaskManagement.DAL.UoW;

public static class DalServices
{
    public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TaskManagementDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
