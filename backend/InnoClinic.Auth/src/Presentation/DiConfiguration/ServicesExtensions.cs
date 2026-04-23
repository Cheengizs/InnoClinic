using System.Text;
using System.Text.Json.Serialization;
using Application;
using Application.Abstractions;
using Application.Dto.Options;
using FluentValidation;
using Infrastructure;
using Infrastructure.Consumers;
using Infrastructure.EmailService;
using Infrastructure.Options;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Presentation.Validators;

namespace Presentation.DiConfiguration;

public static class ServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //json converter
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // msg brokers
        services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));
        services.AddMassTransit(x =>
        {
            x.AddConsumer<GetAdminEmailsConsumer>();
            x.AddConsumer<CreateAccountConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(rabbitOptions.Host, rabbitOptions.VirtualHost, h =>
                {
                    h.Username(rabbitOptions.Username);
                    h.Password(rabbitOptions.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        // jwt
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

        //validators
        services.AddValidatorsFromAssembly(typeof(RegisterUserRequestValidator).Assembly);

        // layers
        services.Configure<DbConnectionOption>(configuration.GetSection(nameof(DbConnectionOption)));
        services.AddInfrastructure();
        services.AddApplication();

        // swagger
        services.AddOpenApi();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Input only JWT token like this:{your_token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            });
        });

        // email
        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
