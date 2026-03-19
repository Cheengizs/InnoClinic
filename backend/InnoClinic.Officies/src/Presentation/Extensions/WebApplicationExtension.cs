using Presentation.Middlewares;
using Presentation.MinimalApi;

namespace Presentation.Extensions;

public static class WebApplicationExtension
{
        public static void ConfigureApp(this WebApplication app)
        {
                if (app.Environment.IsDevelopment())
                {
                        app.MapOpenApi();
                        app.UseSwagger();
                        app.UseSwaggerUI();
                }

                app.UseMiddleware<ExceptionHandlerMiddleware>();
                app.UseMiddleware<LoggingHandlerMiddleware>();
                app.UseHttpsRedirection();

                app.MapGroup("/api/v1/offices")
                        .MapOffices();
                app.MapGroup("/api/v1/photos")
                        .MapPhotos();

        }
}
