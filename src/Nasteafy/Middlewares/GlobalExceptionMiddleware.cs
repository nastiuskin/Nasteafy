using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Application.Common.Models;

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
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation errors occurred.");

                var message = string.Join("; ", ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

                await WriteApiErrorAsync(context,StatusCodes.Status400BadRequest,
                    $"Validation failed: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                await WriteApiErrorAsync(context,StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred: " + ex.Message);
            }
        }

        private static async Task WriteApiErrorAsync(HttpContext context,int statusCode,string message)
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

