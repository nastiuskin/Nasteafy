using FluentResults;
using Nasteafy.Application.Common.Models;

namespace Nasteafy.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToApiError(this Result result, int statusCode = StatusCodes.Status400BadRequest)
        {
            var errorMessage = result.Errors.ToErrorMessage();

            var error = new ApiError
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };

            return Results.BadRequest(error);
        }

        public static IResult ToApiError<T>(this Result<T> result, int statusCode = StatusCodes.Status400BadRequest)
        {
            var errorMessage = result.Errors.ToErrorMessage();

            var error = new ApiError
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };

            return Results.BadRequest(error);
        }
    }
}
