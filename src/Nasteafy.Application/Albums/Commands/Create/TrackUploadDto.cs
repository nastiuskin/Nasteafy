using Microsoft.AspNetCore.Http;

namespace Nasteafy.Application.Albums.Commands.Create
{
    public record TrackUploadDto(string Title, IFormFile File);
}

