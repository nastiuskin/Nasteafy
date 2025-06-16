using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nasteafy.Application.Abstractions;

public class LocalFileStorageService 
{
    private readonly string _storageRootPath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _storageRootPath = configuration.GetValue<string>("StorageRootPath") ?? "StorageRoot";
    }

    public async Task<Uri> UploadFileAsync(string folder, string fileName, IFormFile file)
    {
        var directoryPath = Path.Combine(_storageRootPath, folder);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var filePath = Path.Combine(directoryPath, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return new Uri(Path.GetFullPath(filePath));
    }
}
