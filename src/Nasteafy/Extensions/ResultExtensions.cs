using FluentResults;
using Nasteafy.Application.Common.Models;

namespace Nasteafy.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToApiError(this Result result)
        {
            var error = new ApiError
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = string.Join("; ", result.Errors.Select(e => e.Message))
            };

            return Results.Json(error, statusCode: StatusCodes.Status400BadRequest);
        }

        public static IResult ToApiError<T>(this Result<T> result)
        {
            if (result.IsSuccess && result.Value is not null)
                return Results.Ok(result.Value);

            var status = result.Value is null ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest;

            var error = new ApiError
            {
                StatusCode = status,
                ErrorMessage = string.Join("; ", result.Errors.Select(e => e.Message))
            };

            return Results.Json(error, statusCode: status);
        }
    }   
}
