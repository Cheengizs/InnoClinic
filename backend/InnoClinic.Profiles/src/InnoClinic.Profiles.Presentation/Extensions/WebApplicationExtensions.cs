using InnoClinic.Profiles.Presentation.Controllers;

namespace InnoClinic.Profiles.Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseDependencies(this WebApplication app)
    
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        
        return app;
    }

    public static WebApplication AddControllers(this WebApplication app)
    {
        app.MapGroup("api/v1/doctor").MapDoctorControllersGroup();
        
        return app;
    }
}
