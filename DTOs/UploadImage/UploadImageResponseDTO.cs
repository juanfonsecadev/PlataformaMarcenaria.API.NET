namespace PlataformaMarcenaria.API.DTOs.UploadImage;

public class UploadImageResponseDTO
{
    public long Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
