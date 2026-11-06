using Amazon.S3;
using Amazon.S3.Model;
using PhotoGallery.Api.Services;

public class S3StorageService : IS3StorageService
{
    private readonly IAmazonS3 _s3;
    private readonly IConfiguration _configuration;

    public S3StorageService(
        IAmazonS3 s3,
        IConfiguration configuration)
    {
        _s3 = s3;
        _configuration = configuration;
    }

    public async Task<string> UploadAsync(
        IFormFile file)
    {
        var bucketName =
            _configuration["AWS:BucketName"];

        var key =
            $"{Guid.NewGuid()}_{file.FileName}";

        using var stream =
            file.OpenReadStream();

        var request =
            new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType
            };

        await _s3.PutObjectAsync(request);

        return key;
    }
}