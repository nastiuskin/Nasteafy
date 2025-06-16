using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nasteafy.Abstractions;
using Nasteafy.Application.Users.Commands.Update;

namespace Nasteafy.Endpoints.Users
{
    [Authorize]
    public sealed class UpdateUserProfileEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder routes)
        {
            //routes.MapPut("api/users/profile", async ([FromForm] string? email, [FromForm] IFormFile? file, ISender sender, CancellationToken ct) =>
            routes.MapPut("api/users/profile", async ([FromForm] UpdateProfileCommand command, ISender sender, CancellationToken ct) =>
            {
                //var command = new UpdateProfileCommand(email, file);
                var response = await sender.Send(command, ct);

                if (response.IsFailed)
                    return Results.BadRequest(response.Errors.Select(e => e.Message));

                return Results.Ok();

            })
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .DisableAntiforgery();
        }   
    }
}