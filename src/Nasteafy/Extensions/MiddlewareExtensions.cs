using Nasteafy.Application.Exceptions;
using Nasteafy.Middlewares;

namespace Nasteafy.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDbTransaction(this IApplicationBuilder app) => 
            app.UseMiddleware<TransactionMiddleware>();

        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app) =>
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

        public static IApplicationBuilder UseRequestTimingMiddleware(this IApplicationBuilder app) => 
            app.UseMiddleware<RequestTimingMiddleware>();
    }
}
