namespace PhotoHUB.Service;

public interface IS3Service
{
    Task<string> UploadFileAsync(Stream stream, string key, string contentType);
    Task<Stream> GetFileAsync(string key);
    Task DeleteFileAsync(string key);
    Task<string> GeneratePresignedUrl(string key);
}
