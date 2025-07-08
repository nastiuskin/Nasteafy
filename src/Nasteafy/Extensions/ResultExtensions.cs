using FluentResults;
using Nasteafy.Application.Common.Models;

namespace Nasteafy.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToApiError(this Result result)
        {
            // Move to extension and reuse
            var errorMessage = result.Errors.Any()
                ? string.Join("; ", result.Errors.Select(e => e.Message))
                : "Unknown error occurred.";

            var error = new ApiError
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = errorMessage
            };

            // Do you need to use Json specifically? Asp.net serializes all responces into json already.
            // Is it always status code 400 when there is an error? No 401, 402, 402, 403, 404 etc.? You want to return more specific error codes.
            // You can adjust these methods to accept an error code from endpoints, keeping 400 as a default one.
            return Results.Json(error, statusCode: StatusCodes.Status400BadRequest);
        }

        public static IResult ToApiError<T>(this Result<T> result)
        {
            // Seems unexpected to return Ok with value when using ToApiError. It seems like a compile time rule that you have to follow so you can either remove this check or throw an InvalidOperationException 
            if (result.IsSuccess && result.Value is not null)
            {
                return Results.Ok(result.Value);
            }

            // Move to extension and reuse
            // Either put dot at the end of the message everywhere or don't put anywhere
            var errorMessage = result.Errors.Any()
                ? string.Join("; ", result.Errors.Select(e => e.Message))
                : "Unknown error occurred.";

            var error = new ApiError
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = errorMessage
            };

            return Results.Json(error, statusCode: StatusCodes.Status400BadRequest);
        }

        public static Result LogError(this Result result, ILogger logger)
        {
            if (result.IsFailed)
            {
                // Move to extension and reuse
                var message = result.Errors.Any()
                    ? string.Join("; ", result.Errors.Select(e => e.Message))
                    : "Unknown failure";

                logger.LogError("Result failed: {Error}", message);
            }

            return result;
        }
    }
}
