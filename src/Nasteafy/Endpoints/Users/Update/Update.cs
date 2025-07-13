using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Common.Models;
using Nasteafy.Application.Users.Commands.Update;
using Nasteafy.Endpoints.Users.Update;
using Nasteafy.Extensions;

namespace Nasteafy.Endpoints.Users
{
    public sealed class UpdateUserProfileEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            routes.MapPut("api/users/profile", async ([FromForm] UpdateUserProfileRequest request, ISender sender, CancellationToken ct = default) =>
            {
                var command = new UpdateProfileCommand(request.Email, request.UserName, request.AvatarFile);
                var response = await sender.Send(command, ct);

                return response.IsSuccess
                 ? Results.Ok()
                 : response.ToApiError();

            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .Accepts<UpdateUserProfileRequest>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}