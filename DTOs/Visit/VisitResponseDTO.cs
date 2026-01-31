using PlataformaMarcenaria.API.DTOs.Budget;
using PlataformaMarcenaria.API.DTOs.User;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.DTOs.Visit;

public class VisitResponseDTO
{
    public long Id { get; set; }
    public UserResponseDTO Seller { get; set; } = null!;
    public BudgetRequestResponseDTO BudgetRequest { get; set; } = null!;
    public DateTime ScheduledDate { get; set; }
    public Entities.Visit.VisitStatus Status { get; set; }
    public string? Notes { get; set; }
    public List<string> Photos { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

