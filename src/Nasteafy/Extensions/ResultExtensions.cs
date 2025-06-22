using FluentResults;
using Nasteafy.Application.Common.Models;

namespace Nasteafy.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToApiError(this Result result)
        {
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

        public static IResult ToApiError<T>(this Result<T> result)
        {
            if (result.IsSuccess && result.Value is not null)
                return Results.Ok(result.Value);

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
                var message = result.Errors.Any()
                    ? string.Join("; ", result.Errors.Select(e => e.Message))
                    : "Unknown failure";

                logger.LogError("Result failed: {Error}", message);
            }

            return result;
        }
    }
}
