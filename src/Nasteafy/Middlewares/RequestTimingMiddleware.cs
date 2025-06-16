using System.Diagnostics;

namespace Nasteafy.Middlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            await _next(context);
            watch.Stop();

            var elapsedMs = watch.ElapsedMilliseconds;
            _logger.LogInformation("Request [{method}] {url} executed in {duration}ms",
                context.Request.Method,
                context.Request.Path,
                elapsedMs);
        }
    }
}
