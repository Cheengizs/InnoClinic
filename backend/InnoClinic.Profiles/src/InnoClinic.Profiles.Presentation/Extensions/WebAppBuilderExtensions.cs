using System.Text;
using Azure.Storage.Blobs;
using FluentValidation;
using InnoClinic.Profiles.Application.BlobStorage;
using InnoClinic.Profiles.Application.Features.Commands.Doctors.CreateDoctor;
using InnoClinic.Profiles.Application.Repositories;
using InnoClinic.Profiles.Infrastructure.BlobStorage;
using InnoClinic.Profiles.Infrastructure.DbConfiguring;
using InnoClinic.Profiles.Infrastructure.DbContexts;
using InnoClinic.Profiles.Infrastructure.MessageBroker;
using InnoClinic.Profiles.Infrastructure.Repositories;
using InnoClinic.Profiles.Presentation.Options;
using InnoClinic.Profiles.Presentation.Validators;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace InnoClinic.Profiles.Presentation.Extensions;

public static class WebAppBuilderExtensions
{
    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder)
    {
        // dev        
        builder.Services.AddOpenApi();

        // auth
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
        builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((options, jwtOptions) =>
            {
                var jwtSettings = jwtOptions.Value;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                    ClockSkew = TimeSpan.Zero,
                };
            });

        builder.Services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"ОШИБКА ВАЛИДАЦИИ ТОКЕНА: {context.Exception.Message}");
                        return Task.CompletedTask;
                    }
                };
            });
        builder.Services.AddAuthorization();

        // swagger
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("other-name", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("other-name", document)] = []
            });
        });

        // validation
        builder.Services.AddValidatorsFromAssembly(typeof(DoctorCreateRequestValidator).Assembly);
        
        //repositories
        builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
        
        // db
        builder.Services.Configure<ProfilesDbOptions>(builder.Configuration.GetSection(nameof(ProfilesDbOptions)));
        builder.Services.AddDbContext<ProfilesDbContext>((sp, options) =>
        {
            var dbConnOptions = sp.GetRequiredService<IOptions<ProfilesDbOptions>>().Value;
            options.UseSqlServer(dbConnOptions.ConnectionString);
        });
        
        //mediatr
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateDoctorCommand).Assembly);
        });
        
        //blob
        builder.Services.Configure<BlobStorageOptions>(
            builder.Configuration.GetSection("BlobService"));

        builder.Services.AddSingleton(_ => 
            new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));

        builder.Services.AddSingleton<IBlobService, BlobService>();
        
        // rabbitmq
        builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(nameof(RabbitMqOptions)));
        builder.Services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return builder;
    }
}
