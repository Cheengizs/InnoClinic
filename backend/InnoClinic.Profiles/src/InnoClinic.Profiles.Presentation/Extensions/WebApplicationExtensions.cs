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
    
}
