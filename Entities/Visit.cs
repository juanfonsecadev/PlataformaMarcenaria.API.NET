using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaMarcenaria.API.Entities;

[Table("visits")]
public class Visit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [ForeignKey("Seller")]
    public long SellerId { get; set; }

    [Required]
    [ForeignKey("BudgetRequest")]
    public long BudgetRequestId { get; set; }

    [Required]
    public DateTime ScheduledDate { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public VisitStatus Status { get; set; } = VisitStatus.SCHEDULED;

    [Column(TypeName = "text")]
    public string? Notes { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public List<string> Photos { get; set; } = new List<string>();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User Seller { get; set; } = null!;
    public BudgetRequest BudgetRequest { get; set; } = null!;

    public enum VisitStatus
    {
        SCHEDULED,
        COMPLETED,
        CANCELLED,
        RESCHEDULED
    }
}

