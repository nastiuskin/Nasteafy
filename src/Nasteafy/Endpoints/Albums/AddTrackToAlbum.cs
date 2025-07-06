namespace Nasteafy.Endpoints.Albums
{
    //public sealed class AddTrackToAlbumEndpoint : IEndpoint
    //{
    //    public void MapEndpoint(IEndpointRouteBuilder routes)
    //    {
    //        routes.MapPost("api/albums/{albumId:guid}/tracks", async ([FromRoute] Guid albumId,
    //            [FromForm] AddTrackToAlbumCommand command, ISender sender, CancellationToken ct) =>
    //        {
    //            command.AlbumId = albumId;
    //            var result = await sender.Send(command, ct);

    //            if (result.IsFailed)
    //                return result.ToApiError();

    //            return Results.Ok(result.Value);
    //        })
    //        //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Artist" })
    //        .Accepts<IFormFile>("multipart/form-data")
    //        .DisableAntiforgery()
    //        .Produces<Guid>(StatusCodes.Status200OK)
    //        .Produces<ApiError>(StatusCodes.Status400BadRequest);
    //    }
    //}
}



