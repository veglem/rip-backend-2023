using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Minio;
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
    
    public static void AddS3(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetSection("Minio");
        
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(config["Host"])
            .WithCredentials(config["AccessKey"] ,config["SecretKey"])
            .WithSSL(false));
        
        services.AddScoped<S3Context>();
    }

    public static void AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults
            .AuthenticationScheme).AddCookie(options =>
            options.LoginPath = "/api/auth/login");
        services.AddAuthorization();
    }
}
