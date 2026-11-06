using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Api.Data;
using PhotoGallery.Api.Database;
using PhotoGallery.Api.Entities;
using PhotoGallery.Api.Models;
using Mapster;
using MapsterMapper;

namespace PhotoGallery.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ImagesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(
            _db.Images
                .ProjectToType<ImageItemResponse>()
                .OrderByDescending(x => x.CreatedAt)
                .ToList());
    }

    [HttpPost]
    public IActionResult Create(ImageItemRequest image)
    {
        var newImage = image.Adapt<ImageEntity>();
        newImage.Id = Guid.NewGuid();
        newImage.CreatedAt = DateTime.UtcNow;

        _db.Images.Add(newImage);

        _db.SaveChanges();

        return CreatedAtAction(
            nameof(GetAll),
            new { id = newImage.Id },
            newImage
        );
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var image = _db.Images.FirstOrDefault(x => x.Id == id);

        if (image is null)
            return NotFound();

        _db.Images.Remove(image);

        _db.SaveChanges();

        return NoContent();
    }
}