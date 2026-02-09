using Application.Abstractions;
using Infrastructure.Authentication;
using Infrastructure.DbContexts;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DiExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options => { options.UseNpgsql(configuration["DB_CONNECTION_STRING"]); });
        
        services.AddScoped<IAuthDbContext>(provider => provider.GetRequiredService<AuthDbContext>()!);

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IHashProvider, HashProvider>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        
        return services;
    }
}