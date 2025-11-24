using FluentResults;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;

namespace Nasteafy.Tests.Integration.Fixtures
{
    public class TestFileStorage : IFileStorageService
    {
        public Task<Result> DeleteFileAsync(FileType type, string objectKey)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> GetFileUrlAsync(FileType type, string fileName)
        {
            return Task.FromResult(Result.Ok("urlcik" + fileName));
        }

        public Task<Result<string>> UploadFileAsync(Stream stream, string fileName, string contentType, FileType fileType, string? objectKey = null)
        {
            throw new NotImplementedException();
        }
    }
}