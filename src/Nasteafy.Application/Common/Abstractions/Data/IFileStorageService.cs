using FluentResults;
using Nasteafy.Domain;

namespace Nasteafy.Application.Common.Abstractions.Data
{
    public interface IFileStorageService
    {
        Task<Result<string>> UploadFileAsync(Stream stream, string fileName, string contentType, FileType fileType, string? objectKey = null);
        Task<Result<string>> GetFileUrlAsync(FileType type, string? objectKey);
        Task<Result> DeleteFileAsync(FileType type, string objectKey);
    }
}


