    using Azure.Storage.Blobs;
    using Business.Features.Commands.Offices.DeleteOffice;
    using Business.Profiles;
    using DataAccess;
    using DataAccess.BlobStorage;
    using DataAccess.DbContexts;
    using DataAccess.Email;
    using DataAccess.Options;
    using DataAccess.Repositories;
    using DataAccess.Repositories.Abstractions;
    using DataAccess.UsersService;
    using FluentValidation;
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
            services.AddHttpClient<IUsersService, UsersService>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("UsersServiceOptions").GetValue<string>("BaseAddress")!);
            });

            services.AddScoped<IEmailService, EmailService>();
            
            services.Configure<BlobStorageOptions>(
                configuration.GetSection("BlobStorage"));

            services.AddSingleton(x => 
                new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage")));

            services.AddSingleton<IBlobService, BlobService>();
            
            services.AddDbContext<OfficesDbContext>((serviceProvider, optionsBuilder) =>
            {
                var options = serviceProvider.GetService<IOptions<DbOptionClass>>()?.Value;

                var connectionString = options.ToConnectionString();
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
