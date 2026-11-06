using PhotoGallery.Api.Models;

namespace PhotoGallery.Api.Data;

public static class FakeImageRepository
{
    //jelenleg ez csak mock:
    public static List<ImageItem> Images =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Első kép",
            Description = "Demo kép",
            FileName = "demo.jpg",
            CreatedAt = DateTime.UtcNow
        }
    ];
}