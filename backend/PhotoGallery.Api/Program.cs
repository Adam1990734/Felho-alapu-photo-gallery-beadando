using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PhotoGallery.Api.Database;
using PhotoGallery.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<
    IS3StorageService,
    S3StorageService>();

//Contextusok:
var connectionString =
    Environment.GetEnvironmentVariable("DATABASE_CONNECTION")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Currently using: {connectionString}");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


//Végpont regisztrációk:
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}

app.UseCors("ReactPolicy");

var uploadsPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "uploads"
);

if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles();

app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider =
            new PhysicalFileProvider(uploadsPath),

        RequestPath = "/uploads"
    }
);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
