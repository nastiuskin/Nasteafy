using Nasteafy.Middlewares;

namespace Nasteafy.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDbTransaction(this IApplicationBuilder app) => app.UseMiddleware<TransactionMiddleware>();
    }
}
