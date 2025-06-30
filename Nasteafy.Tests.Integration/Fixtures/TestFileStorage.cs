using FluentResults;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Tests.Integration.Fixtures
{
    public class TestFileStorage : IFileStorageService
    {
        public Task<Result> DeleteFileAsync(FileType type, string objectKey)
        {
            return Task.FromResult(Result.Ok());
        }

        public Task<Result<string>> GetFileUrlAsync(FileType type, string fileName)
        {
            return Task.FromResult(Result.Ok("url/" + fileName));
        }

        public Task<Result<string>> UploadFileAsync(Stream stream, string fileName, string contentType, FileType fileType, string? objectKey = null)
        {
            var fakePath = $"uploads/{fileType}/{fileName}";
            return Task.FromResult(Result.Ok(fakePath));
        }
    }
}