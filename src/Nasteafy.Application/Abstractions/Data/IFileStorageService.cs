using Nasteafy.Domain;

namespace Nasteafy.Application.Abstractions.Data
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, FileType fileType, string? objectKey = null);
        Task<string?> GetFileUrlAsync(FileType type, string? objectKey);
        Task<bool> DeleteFileAsync(string bucket, string objectKey);
    }
}


