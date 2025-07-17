using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using PhotoHUB.configs;

namespace PhotoHUB.Service;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsSettings _settings;
    private static readonly TimeSpan PresignedUrlValidity = TimeSpan.FromHours(6);

    public S3Service(IAmazonS3 s3Client, IOptions<AwsSettings> options)
    {
        _s3Client = s3Client;
        _settings = options.Value;
    }

    public async Task<string> UploadFileAsync(Stream stream, string key, string contentType)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            InputStream = stream,
            ContentType = contentType
        };

        var response = await _s3Client.PutObjectAsync(putRequest);
        return $"https://{_settings.BucketName}.s3.{_settings.Region}.amazonaws.com/{key}";
    }
    
    public async Task<string> GeneratePresignedUrl(string key)
    {
        var decodedKey = Uri.UnescapeDataString(key);
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = decodedKey,
            Expires = DateTime.UtcNow.Add(PresignedUrlValidity)
        };

        return await _s3Client.GetPreSignedURLAsync(request);
    }

    public async Task<Stream> GetFileAsync(string key)
    {
        var response = await _s3Client.GetObjectAsync(_settings.BucketName, key);
        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(string key)
    {
        await _s3Client.DeleteObjectAsync(_settings.BucketName, key);
    }
}
