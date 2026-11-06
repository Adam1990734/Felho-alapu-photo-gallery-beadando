using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Api.Data;
using PhotoGallery.Api.Database;
using PhotoGallery.Api.Dtos;
using PhotoGallery.Api.Entities;
using PhotoGallery.Api.Models;

namespace PhotoGallery.Api.Controllers;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ImagesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            _db.Images
                .ProjectToType<ImageItemResponse>()
                .OrderByDescending(x => x.CreatedAt)
                .ToList());
    }

    //Ez csak mock-ra kellett:
    /*
    [HttpPost]
    public async Task<IActionResult> Create(ImageItemRequest image)
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
    */

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var image = _db.Images.FirstOrDefault(x => x.Id == id);

        if (image is null)
            return NotFound();

        _db.Images.Remove(image);

        _db.SaveChanges();

        return NoContent();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(ImageUploadDto request)
    {
        if (request.File is null ||
            request.File.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            return StatusCode(500);
        }

        var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName
        );

        using var stream = new FileStream(
            filePath,
            FileMode.Create
        );

        await request.File.CopyToAsync(stream);

        var image = new ImageEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            FileName = fileName,
            CreatedAt = DateTime.UtcNow
        };

        _db.Images.Add(image);

        await _db.SaveChangesAsync();

        return Ok(image);
    }
}