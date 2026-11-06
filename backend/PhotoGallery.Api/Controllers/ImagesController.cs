using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Api.Data;
using PhotoGallery.Api.Models;

namespace PhotoGallery.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(FakeImageRepository.Images);
    }

    [HttpPost]
    public IActionResult Create(ImageItem image)
    {
        image.Id = Guid.NewGuid();
        image.CreatedAt = DateTime.UtcNow;

        FakeImageRepository.Images.Add(image);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = image.Id },
            image);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var image = FakeImageRepository.Images
            .FirstOrDefault(x => x.Id == id);

        if (image is null)
            return NotFound();

        FakeImageRepository.Images.Remove(image);

        return NoContent();
    }
}