using Microsoft.EntityFrameworkCore;
using PhotoGallery.Api.Entities;

namespace PhotoGallery.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ImageEntity> Images => Set<ImageEntity>();
}
