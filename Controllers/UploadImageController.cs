using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaMarcenaria.API.Data;
using PlataformaMarcenaria.API.DTOs.UploadImage;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/upload-images")]
[Authorize]
public class UploadImageController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationDbContext _dbContext;

    public UploadImageController(IWebHostEnvironment environment, ApplicationDbContext dbContext)
    {
        _environment = environment;
        _dbContext = dbContext;
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
    public async Task<ActionResult<List<UploadImageResponseDTO>>> Upload([FromForm] List<IFormFile> images)
    {
        if (images == null || images.Count == 0)
        {
            return BadRequest(new { message = "Nenhuma imagem foi enviada." });
        }

        var rootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(rootPath))
        {
            rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var uploadFolder = Path.Combine(rootPath, "uploads");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var entities = new List<UploadImage>();
        foreach (var file in images)
        {
            if (file.Length <= 0) continue;

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            entities.Add(new UploadImage
            {
                Url = $"/uploads/{fileName}",
                UploadedAt = DateTime.UtcNow
            });
        }

        if (entities.Count == 0)
        {
            return BadRequest(new { message = "Os arquivos enviados são inválidos." });
        }

        _dbContext.AddRange(entities);
        await _dbContext.SaveChangesAsync();

        var response = entities
            .Select(img => new UploadImageResponseDTO
            {
                Id = img.Id,
                Url = img.Url,
                UploadedAt = img.UploadedAt
            })
            .ToList();

        return Ok(response);
    }
}
