using Application.Abstractions;
using Infrastructure.Authentication;
using Infrastructure.DbContexts;
using Infrastructure.Options;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DiExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AuthDbContext>((serviceProvider, options) =>
            options.UseNpgsql(serviceProvider.GetRequiredService<IOptions<DbConnectionOption>>()
                .Value
                .DbConnectionString));

        services.AddScoped<IAuthDbContext>(provider => provider.GetRequiredService<AuthDbContext>()!);

        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IHashProvider, HashProvider>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        return services;
    }
}
