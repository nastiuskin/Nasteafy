using FluentValidation;
using Nasteafy.Application.Common.Models;

namespace Nasteafy.Middlewares
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
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation errors occurred.");

                var firstError = ex.Errors.FirstOrDefault();
                var message = firstError is null
                    ? "Validation failed"
                    : $"{firstError.ErrorMessage}";

                await WriteApiErrorAsync(context, StatusCodes.Status400BadRequest, $"{message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                await WriteApiErrorAsync(context, StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred: " + ex.Message);
            }
        }

        private static async Task WriteApiErrorAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var apiError = new ApiError
            {
                StatusCode = statusCode,
                ErrorMessage = message
            };

            await context.Response.WriteAsJsonAsync(apiError);
        }

    }
}

