using Amazon.S3;

namespace PhotoGallery.Api.Services
{
    public interface IS3StorageService
    {
        Task<string> UploadAsync(IFormFile file);
        Task DeleteAsync(string key);
    }
}
