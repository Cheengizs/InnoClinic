using Presentation.Middlewares;
using Serilog;

namespace Presentation.DiConfiguration;

public static class MiddlewareExtensions
{
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseSerilogRequestLogging(); 

        app.UseAuthentication(); 
        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.MapControllers();
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        return app;
    }
}
