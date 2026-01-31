using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaMarcenaria.API.Entities;

[Table("bids")]
public class Bid
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [ForeignKey("Carpenter")]
    public long CarpenterId { get; set; }

    [Required]
    [ForeignKey("BudgetRequest")]
    public long BudgetRequestId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int ExecutionTimeInDays { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "varchar(50)")]
    public BidStatus Status { get; set; } = BidStatus.PENDING;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User Carpenter { get; set; } = null!;
    public BudgetRequest BudgetRequest { get; set; } = null!;

    public enum BidStatus
    {
        PENDING,
        ACCEPTED,
        REJECTED,
        CANCELLED
    }
}

