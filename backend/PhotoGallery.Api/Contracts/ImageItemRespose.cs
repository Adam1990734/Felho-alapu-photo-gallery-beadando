namespace PhotoGallery.Api.Models;

public class ImageItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}