using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Nasteafy.Application.Abstractions;
using Nasteafy.Infrastructure.Options;

//public class FirebaseStorageService : IFileStorageService
//{
//    private readonly StorageClient _storage;
//    private readonly FirebaseStorageOptions _options;

//    public FirebaseStorageService(StorageClient storage, IOptions<FirebaseStorageOptions> options)
//    {
//        _storage = storage;
//        _options = options.Value;
//    }

//    public async Task<Uri> UploadFile(string name, IFormFile file)
//    {
//        var randomGuid = Guid.NewGuid();
//        using var stream = new MemoryStream();
//        await file.CopyToAsync(stream);
//        var blob = await _storage.UploadObjectAsync(_options.BucketName,
//            $"{name}-{randomGuid}", file.ContentType, stream);
//        var photoUri = new Uri(blob.MediaLink);
//        return photoUri;
//    }
//}
