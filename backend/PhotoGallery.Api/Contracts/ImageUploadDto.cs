using Microsoft.AspNetCore.Http;

namespace PhotoGallery.Api.Dtos;

public class ImageUploadDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public IFormFile File { get; set; } = default!;
}