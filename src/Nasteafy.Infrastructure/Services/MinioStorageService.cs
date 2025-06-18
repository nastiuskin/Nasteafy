using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Nasteafy.Application.Common.Abstractions.Data;
using Nasteafy.Domain;
using Nasteafy.Infrastructure.Options;

public class MinioStorageService(
    IMinioClient minioClient,
    IOptions<MinioOptions> options,
    ILogger<MinioStorageService> logger) : IFileStorageService
{
    public async Task<Result<string>> UploadFileAsync(Stream stream, string fileName, string contentType, FileType fileType, string? objectKey = null)
    {
        var (bucketName, allowedExtensions, prefix) = GetStorageSettings(fileType);

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException($"Extension '{extension}' is not allowed for file type '{fileType}'.");

        objectKey ??= Guid.NewGuid() + extension;
        var fullKey = $"{prefix}/{objectKey}";

        try
        {
            var bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (!bucketExists)
            {
                await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            await minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fullKey)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType));

            return fullKey;

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Fail("Failed to upload file. Please try again later");
        }
    }

    private (string BucketName, string[] AllowedExtensions, string Prefix) GetStorageSettings(FileType fileType)
    {
        return fileType switch
        {
            FileType.Audio => (options.Value.Audio.Name, options.Value.Audio.AllowedExtensions, "audio"),
            FileType.UserAvatar => (options.Value.Image.Name, options.Value.Image.AllowedExtensions, "avatars"),
            FileType.TrackCover => (options.Value.Image.Name, options.Value.Image.AllowedExtensions, "tracks"),
            FileType.PlaylistCover => (options.Value.Image.Name, options.Value.Image.AllowedExtensions, "playlists"),
            FileType.AlbumCover => (options.Value.Image.Name, options.Value.Image.AllowedExtensions, "albums"),
            _ => throw new InvalidOperationException($"Unsupported file type: {fileType}")
        };
    }

    public async Task<Result<string>> GetFileUrlAsync(FileType type, string? objectKey)
    {
        if (string.IsNullOrEmpty(objectKey))
            return Result.Fail("Failed to get file");

        var bucketName = type == FileType.Audio ? options.Value.Audio.Name : options.Value.Image.Name;

        try
        {
            var fileUrl = await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectKey)
            .WithExpiry(3600));

            return Result.Ok(fileUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Fail("Failed to get file");
        }
    }

    public async Task<Result> DeleteFileAsync(FileType fileType, string objectKey)
    {
        var bucketName = GetStorageSettings(fileType).BucketName;
        try
        {
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey));

            return Result.Ok();
        }
        catch (Exception ex)
        {

            logger.LogError(ex.Message);
            return Result.Fail("Failed to delete file");
        }
    }
}
