using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Nasteafy.Application.Abstractions.Data;
using Nasteafy.Domain;
using Nasteafy.Infrastructure.Options;

public class MinioStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _options;

    public MinioStorageService(IMinioClient minioClient, IOptions<MinioOptions> options)
    {
        _minioClient = minioClient;
        _options = options.Value;
    }
    public async Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, FileType fileType, string? objectKey = null)
    {
        var prefix = fileType switch
        {
            FileType.UserAvatar => "avatars",
            FileType.TrackCover => "tracks",
            FileType.PlaylistCover => "playlists",
            FileType.Audio => "audio",
            _ => "other"
        };

        var bucket = fileType == FileType.Audio ? _options.Buckets.Audio : _options.Buckets.Image;
        objectKey ??= Guid.NewGuid() + Path.GetExtension(fileName);
        var fullKey = $"{prefix}/{objectKey}";

        bool exists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
        if (!exists)
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));

        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(fullKey)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType));

        return fullKey;
    }

    public async Task<string?> GetFileUrlAsync(FileType type, string? objectKey)
    {
        if (string.IsNullOrEmpty(objectKey))
            return null;

        var bucket = type == FileType.Audio ? _options.Buckets.Audio : _options.Buckets.Image;
        return await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectKey)
            .WithExpiry(3600)); ;
    }

    public async Task<bool> DeleteFileAsync(string bucket, string objectKey)
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectKey));
            return true;
        }
        catch
        {
            return false;
        }
    }
}
