using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Application;
using Application.Dto.Options;
using FluentValidation;
using Infrastructure;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presentation.Validators;

namespace Presentation.DiConfiguration;

public static class ServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((options, jwtOptionsWrapper) =>
            {
                var jwtSettings = jwtOptionsWrapper.Value;

                var secretKey = jwtSettings.SecretKey;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddOpenApi();
        services.AddValidatorsFromAssembly(typeof(RegisterUserRequestValidator).Assembly);
        services.Configure<DbConnectionOption>(configuration.GetSection(nameof(DbConnectionOption)));

        services.AddInfrastructure();
        services.AddApplication();

        return services;
    }
}
