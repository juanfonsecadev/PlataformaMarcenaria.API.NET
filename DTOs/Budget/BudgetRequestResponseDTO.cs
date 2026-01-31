using PlataformaMarcenaria.API.DTOs.Address;
using PlataformaMarcenaria.API.DTOs.User;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.DTOs.Budget;

public class BudgetRequestResponseDTO
{
    public long Id { get; set; }
    public UserResponseDTO Client { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public List<string> ReferenceImages { get; set; } = new List<string>();
    public BudgetRequest.BudgetStatus Status { get; set; }
    public AddressResponseDTO Location { get; set; } = null!;
    public decimal? EstimatedBudget { get; set; }
    public DateTime? DesiredDeadline { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int TotalBids { get; set; }
    public decimal? LowestBid { get; set; }
    public decimal? HighestBid { get; set; }
}

