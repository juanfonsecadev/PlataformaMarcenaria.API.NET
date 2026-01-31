using PlataformaMarcenaria.API.DTOs.Budget;
using PlataformaMarcenaria.API.DTOs.User;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.DTOs.Bid;

public class BidResponseDTO
{
    public long Id { get; set; }
    public UserResponseDTO Carpenter { get; set; } = null!;
    public BudgetRequestResponseDTO BudgetRequest { get; set; } = null!;
    public decimal Price { get; set; }
    public int ExecutionTimeInDays { get; set; }
    public string? Description { get; set; }
    public Entities.Bid.BidStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

