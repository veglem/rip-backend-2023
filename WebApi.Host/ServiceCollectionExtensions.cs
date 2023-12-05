using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess;

namespace WebApi;

public static class ServiceCollectionExtensions
{
    public static void AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString =
            configuration.GetSection("Database")["ConnectionString"];

        services.AddDbContext<RectorOrdersDatabaseContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
