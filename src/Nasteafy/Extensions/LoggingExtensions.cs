using Serilog;

namespace Nasteafy.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            return hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.WriteTo.Console();
                loggerConfiguration.ReadFrom.Configuration(configuration);
            });
        }
    }
}
