using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaMarcenaria.API.Entities;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "varchar(50)")]
    public UserType UserType { get; set; }

    public string? Avatar { get; set; }

    public string? Document { get; set; } // CPF/CNPJ

    public bool Active { get; set; } = true;

    public double Rating { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<BudgetRequest> BudgetRequests { get; set; } = new List<BudgetRequest>();
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}

public enum UserType
{
    CLIENT,
    SELLER,
    CARPENTER
}

