using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Api.Data;
using PhotoGallery.Api.Database;
using PhotoGallery.Api.Dtos;
using PhotoGallery.Api.Entities;
using PhotoGallery.Api.Models;
using PhotoGallery.Api.Services;

namespace PhotoGallery.Api.Controllers;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IS3StorageService _s3StorageService;
    private readonly IConfiguration _configuration;

    public ImagesController(AppDbContext db, IS3StorageService iS3StorageService, IConfiguration configuration)
    {
        _db = db;
        _s3StorageService = iS3StorageService;
        _configuration = configuration;
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
    public async Task<IActionResult> Upload(
    [FromForm] ImageUploadDto request)
    {
        if (request.File is null ||
            request.File.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        var key = await _s3StorageService
            .UploadAsync(request.File);

        var bucketName = _configuration["AWS:BucketName"];

        var imageUrl =
            $"https://{bucketName}.s3.eu-central-1.amazonaws.com/{key}";

        var image = new ImageEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            FileName = key,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow
        };

        _db.Images.Add(image);

        await _db.SaveChangesAsync();

        return Ok(image);
    }
}