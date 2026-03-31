using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public ProjectController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = 104857600)] // 100MB
    public async Task<IActionResult> CreateProject(
        [FromForm] string title,
        [FromForm] string description,
        [FromForm] string category,
        [FromForm] string budget,
        [FromForm] string deadline,
        [FromForm(Name = "address.street")] string street,
        [FromForm(Name = "address.number")] string number,
        [FromForm(Name = "address.complement")] string? complement,
        [FromForm(Name = "address.neighborhood")] string neighborhood,
        [FromForm(Name = "address.city")] string city,
        [FromForm(Name = "address.state")] string state,
        [FromForm(Name = "address.zipCode")] string zipCode,
        [FromForm] List<string> requirements,
        [FromForm] List<IFormFile> referenceImages)
    {
        // Save images
        var imageUrls = new List<string>();
        var uploadPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        foreach (var file in referenceImages)
        {
            if (file.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName.Replace(" ", "_")}";
                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                imageUrls.Add($"/uploads/{fileName}");
            }
        }

        // Aqui você pode persistir o projeto no banco de dados utilizando um Service/Repository.
        // Implementação de persistência foi deixada para uma etapa futura.

        return Ok(new
        {
            message = "Project created successfully!",
            project = new
            {
                title,
                description,
                category,
                budget,
                deadline,
                address = new { street, number, complement, neighborhood, city, state, zipCode },
                requirements,
                images = imageUrls
            }
        });
    }

    [HttpGet("dashboard")]
    public IActionResult GetDashboardSummary()
    {
        // Endpoint de dashboard ainda não implementado com dados reais.
        // Retorna um resumo padrão para evitar erros de compilação/execução
        // até que o módulo de projetos seja finalizado.

        return Ok(new
        {
            total = 0,
            completed = 0,
            in_progress = 0,
            invested = 0m
        });
    }
}
