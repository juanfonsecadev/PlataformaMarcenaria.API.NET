using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaMarcenaria.API.Entities;

[Table("upload_images")]
public class UploadImage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Url { get; set; } = string.Empty;

    [Required]
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    // Optionally, relate to BudgetRequest or User, etc.
    // public long? BudgetRequestId { get; set; }
    // public BudgetRequest? BudgetRequest { get; set; }
}
