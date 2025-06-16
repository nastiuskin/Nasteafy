using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Application.Artists.Queries.GetAll;

namespace Nasteafy.Application.Artists.Queries
{
    public record GetAllArtistsQuery : IRequest<Result<GetAllArtistsResponse>>;

    //public class GetAllArtistsQueryHandler(IUnitOfWork unitOfWork)
    //    : IRequestHandler<GetAllArtistsQuery, Result<GetAllArtistsResponse>>
    //{
    //    public async Task<Result<GetAllArtistsResponse>> Handle(GetAllArtistsQuery request, CancellationToken ct)
    //    {
    //        var artists = await unitOfWork.Artists
    //            .GetAllIncludeUsers(ct);

    //        var artistDtos = artists
    //            .Select(a => new ArtistDto(a.Id, a.User.AvatarUrl, a.Name))
    //            .ToList();

    //        return Result.Ok(new GetAllArtistsResponse{Artists = artistDtos });
    //    }
    //}
}
