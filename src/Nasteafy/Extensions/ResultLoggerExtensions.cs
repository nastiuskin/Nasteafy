using FluentResults;

namespace Nasteafy.Extensions
{
    public static class ResultLoggerExtensions
    {
        public static IApplicationBuilder UseFluentResultsLogger(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<IResultLogger>();

            Result.Setup(settings =>
            {
                settings.Logger = logger;
            });

            return app;
        }
    }
}
