using Serilog;

namespace Presentation.DiConfiguration;

public static class LoggerExtension
{
    public static ConfigureHostBuilder AddLogger(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });
        
        return host;
    }
}