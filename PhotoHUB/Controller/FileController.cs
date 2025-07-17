using Microsoft.AspNetCore.Mvc;
using PhotoHUB.Service;

namespace PhotoHUB.controller;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IS3Service _s3Service;

    public FileController(IS3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Brak pliku");

        var stream = file.OpenReadStream();
        var key = $"uploads/{Guid.NewGuid()}_{file.FileName}";
        var url = await _s3Service.UploadFileAsync(stream, key, file.ContentType);

        return Ok(new { Url = url });
    }
    
    [HttpGet("get/{key}")]
    public async Task<IActionResult> GetFile(string key)
    {
        try
        {
            var url = await _s3Service.GeneratePresignedUrl(key);
            return Ok(new { Url = url });
        }
        catch (Exception ex)
        {
            return NotFound($"Plik o kluczu {key} nie został znaleziony: {ex.Message}");
        }
    }
    
}
