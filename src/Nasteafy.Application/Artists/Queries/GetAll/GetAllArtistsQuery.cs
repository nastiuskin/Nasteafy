using FluentResults;
using MediatR;
using Nasteafy.Application.Artists.Queries.GetAll;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Application.Common.Models;
using Nasteafy.Domain;

namespace Nasteafy.Application.Artists.Queries
{
    public record GetAllArtistsQuery(PagedRequest PagedRequest) : IRequest<Result<PagedResult<ArtistDto>>>;

    public class GetAllArtistsQueryHandler(IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService)
     : IRequestHandler<GetAllArtistsQuery, Result<PagedResult<ArtistDto>>>
    {
        public async Task<Result<PagedResult<ArtistDto>>> Handle(GetAllArtistsQuery request, CancellationToken ct)
        {
            var artistsPaged = await unitOfWork.Artists.GetPagedResultAsync(request.PagedRequest, ct);

            var artistDtos = new List<ArtistDto>(); 

            foreach (var a in artistsPaged.Items)
            {
                var avatarUrl = !string.IsNullOrEmpty(a.AvatarUrl)
                    ? await fileStorageService.GetFileUrlAsync(FileType.UserAvatar, a.AvatarUrl)
                    : null;

                artistDtos.Add(new ArtistDto(
                    a.Id,
                    avatarUrl?.Value,
                    a.Name,
                    a.Biography,
                    a.CreatedByAdmin));
            }

            var result = new PagedResult<ArtistDto>
            {
                Items = artistDtos,
                TotalItems = artistsPaged.TotalItems,
                PageNumber = artistsPaged.PageNumber,
                PageSize = artistsPaged.PageSize
            };

            return Result.Ok(result);
        }
    }
}
