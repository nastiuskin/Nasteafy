using FluentValidation;
using Nasteafy.Application.Common.Models;
using Nasteafy.Extensions;

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
                _logger.LogError(ex, "Validation errors occurred: {ErrorMessage}", ex.Message);

                await WriteApiErrorAsync(context, StatusCodes.Status400BadRequest, $"{ex.ToErrorMessage()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred: {ErrorMessage}", ex.Message);

                await WriteApiErrorAsync(context, StatusCodes.Status500InternalServerError,
                     "Something went wrong. Please try again later.");
            }
        }

        private static async Task WriteApiErrorAsync(HttpContext context, int statusCode, string message)
        {
            var apiError = new ApiError
            {
                StatusCode = statusCode,
                ErrorMessage = message
            };

            await context.Response.WriteAsJsonAsync(apiError);
        }
    }
}

