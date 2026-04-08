using Azure.Storage.Blobs;
using Business.Features.Commands.Offices.DeleteOffice;
using Business.Profiles;
using DataAccess.BlobStorage;
using DataAccess.DbContexts;
using DataAccess.Email;
using DataAccess.Options;
using DataAccess.Repositories;
using DataAccess.Repositories.Abstractions;
using DataAccess.UsersService;
using FluentValidation;
using InnoClinic.Shared.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Presentation.Profiles;
using Presentation.Validators;

namespace Presentation.Extensions;

    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddValidatorsFromAssembly(typeof(OfficeCreateValidator).Assembly);
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.Configure<DbOptionClass>(configuration.GetSection("DatabaseOptions"));

            services.Configure<EmailOptions>(configuration.GetSection("EmailOptions"));
            services.Configure<UsersServiceOptions>(configuration.GetSection("UsersServiceOptions"));
            services.AddScoped<IEmailService, EmailService>();

            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMqOptions"));
            services.AddMassTransit(x =>
            {
                x.AddRequestClient<GetAdminEmailsRequest>();
                
                x.UsingRabbitMq((context, cfg) =>
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
            services.AddScoped<IUsersService, UsersService>();
            
            services.Configure<BlobStorageOptions>(
                configuration.GetSection("BlobStorage"));

            services.AddSingleton(_ => 
                new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));

            services.AddSingleton<IBlobService, BlobService>();
            
            services.AddDbContext<OfficesDbContext>((serviceProvider, optionsBuilder) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DbOptionClass>>().Value;

                var connectionString = options.ToConnectionString();
                Console.WriteLine($"{connectionString}");
                optionsBuilder.UseMongoDB(connectionString, options.DatabaseName);
            });
            
            services.AddAutoMapper(cfg => 
            {
                cfg.AddMaps(typeof(OfficeProfilePresentation).Assembly,
                    typeof(OfficeProfile).Assembly);
            });
            
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(typeof(DeleteOfficeCommand).Assembly);
            });
            
            return services;
        }
    }
