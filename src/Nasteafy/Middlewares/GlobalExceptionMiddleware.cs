using Microsoft.AspNetCore.Mvc;

namespace Nasteafy.Application.Exceptions
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError(ex, "Validation errors occurred.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var problemDetails = new ProblemDetails
                {
                    Title = "One or more validation errors occurred.",
                    Status = context.Response.StatusCode,
                    Instance = context.Request.Path
                };

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                problemDetails.Extensions["errors"] = errors;

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var error = ex.Message;
                var problemDetails = new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Status = context.Response.StatusCode,
                    Instance = context.Request.Path
                };
                problemDetails.Extensions["error"] = error;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
