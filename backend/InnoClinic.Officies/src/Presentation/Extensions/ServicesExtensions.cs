using Business.Features.Commands.Offices.DeleteOffice;
using Business.Profiles;
using DataAccess.DbContexts;
using DataAccess.Repositories;
using DataAccess.Repositories.Abstractions;
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
