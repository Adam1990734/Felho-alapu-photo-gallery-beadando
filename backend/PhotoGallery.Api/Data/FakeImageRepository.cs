using PhotoGallery.Api.Entities;
using PhotoGallery.Api.Models;

namespace PhotoGallery.Api.Data;

public static class FakeImageRepository
{
    //jelenleg ez csak mock:
    public static List<ImageEntity> Images =
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