using Serilog;

namespace Presentation.Extensions;

public static class LoggingExtension
{
    public static void AddLoggingExtension(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration) 
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
