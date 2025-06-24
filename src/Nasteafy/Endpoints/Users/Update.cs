using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Users.Commands.Update;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Users
{
    public sealed class UpdateUserProfileEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/users/profile", async ([FromForm] UpdateProfileCommand command, ISender sender, CancellationToken ct) =>
            {
                var response = await sender.Send(command, ct);

                if (response.IsFailed)
                    return response.ToApiError();

                return Results.Ok();
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}