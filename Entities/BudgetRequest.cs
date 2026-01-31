using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaMarcenaria.API.Entities;

[Table("budget_requests")]
public class BudgetRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [ForeignKey("Client")]
    public long ClientId { get; set; }

    [Required]
    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(max)")]
    public List<string> ReferenceImages { get; set; } = new List<string>();

    [Required]
    [Column(TypeName = "varchar(50)")]
    public BudgetStatus Status { get; set; } = BudgetStatus.OPEN;

    [Required]
    [ForeignKey("Location")]
    public long LocationId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedBudget { get; set; }

    public DateTime? DesiredDeadline { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User Client { get; set; } = null!;
    public Address Location { get; set; } = null!;
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public enum BudgetStatus
    {
        OPEN,
        WAITING_VISIT,
        WAITING_BIDS,
        CLOSED,
        CANCELLED
    }
}

