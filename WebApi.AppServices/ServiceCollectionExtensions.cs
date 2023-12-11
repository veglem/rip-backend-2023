using Microsoft.Extensions.DependencyInjection;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Handlers;
using WebApi.AppServices.Repositories;

namespace WebApi.AppServices;

public static class ServiceCollectionExtensions
{
    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddTransient<IUnitHandler, UnitHandler>();
        services.AddTransient<IOrdersHandler, OrdersHandler>();
        services.AddTransient<IUserHandler, UserHandler>();
    }
    
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IS3Repository, S3Repository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
